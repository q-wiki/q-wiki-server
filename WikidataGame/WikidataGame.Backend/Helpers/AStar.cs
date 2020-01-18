using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Helpers
{
    /// <summary>
    /// Based on: https://docs.microsoft.com/en-us/archive/blogs/ericlippert/path-finding-using-a-in-c-3-0-part-four
    /// </summary>
    public static class AStar
    {
        public static Path<Tile> FindPath(
            Tile start,
            Tile destination,
            IEnumerable<Tile> map,
            int mapWidth,
            int mapHeight)
        {
            var closed = new HashSet<Tile>();
            var queue = new PriorityQueue<double, Path<Tile>>();
            queue.Enqueue(0, new Path<Tile>(start));
            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();
                if (closed.Contains(path.LastStep))
                    continue;
                if (path.LastStep.Id == destination.Id)
                    return path;
                closed.Add(path.LastStep);
                foreach (Tile n in GetNeighbours(path.LastStep, map, mapWidth, mapHeight))
                {
                    var newPath = path.AddStep(n, 1);
                    queue.Enqueue(newPath.TotalCost, newPath);
                }
            }
            return null;
        }

        private static IEnumerable<Tile> GetNeighbours(Tile tile, IEnumerable<Tile> map, int mapWidth, int mapHeight)
        {
            return TileHelper.GetNeighbors(map, tile.MapIndex % mapWidth, tile.MapIndex / mapWidth, mapWidth, mapHeight).Where(n => n.Value.IsAccessible).Select(n => n.Value);
        }
    }
    
    public class Path<Node> : IEnumerable<Node>
    {
        public Node LastStep { get; private set; }
        public Path<Node> PreviousSteps { get; private set; }
        public double TotalCost { get; private set; }
        private Path(Node lastStep, Path<Node> previousSteps, double totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }
        public Path(Node start) : this(start, null, 0) { }
        public Path<Node> AddStep(Node step, double stepCost)
        {
            return new Path<Node>(step, this, TotalCost + stepCost);
        }
        public IEnumerator<Node> GetEnumerator()
        {
            for (Path<Node> p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class PriorityQueue<P, V>
    {
        private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();
        public void Enqueue(P priority, V value)
        {
            Queue<V> q;
            if (!list.TryGetValue(priority, out q))
            {
                q = new Queue<V>();
                list.Add(priority, q);
            }
            q.Enqueue(value);
        }
        public V Dequeue()
        {
            // will throw if there isn’t any first element!
            var pair = list.First();
            var v = pair.Value.Dequeue();
            if (pair.Value.Count == 0) // nothing left of the top priority.
                list.Remove(pair.Key);
            return v;
        }
        public bool IsEmpty
        {
            get { return !list.Any(); }
        }
    }
}
