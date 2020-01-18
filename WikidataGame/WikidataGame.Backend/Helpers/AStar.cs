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
        public static Path<TileNode> FindPath(
            TileNode start,
            TileNode destination,
            IEnumerable<Tile> map,
            int mapWidth,
            int mapHeight)
        {
            var closed = new HashSet<TileNode>();
            var queue = new PriorityQueue<double, Path<TileNode>>();
            queue.Enqueue(0, new Path<TileNode>(start));
            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();
                if (closed.Contains(path.LastStep))
                    continue;
                if (path.LastStep.Tile.Id == destination.Tile.Id)
                    return path;
                closed.Add(path.LastStep);
                foreach (TileNode n in path.LastStep.Neighbours(map, mapWidth, mapHeight).ToList())
                {
                    var newPath = path.AddStep(n, 1);
                    queue.Enqueue(newPath.TotalCost, newPath);
                }
            }
            return null;
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

    public class TileNode
    {
        public Tile Tile { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public IEnumerable<TileNode> Neighbours(IEnumerable<Tile> map, int mapWidth, int mapHeight)
        {
            return TileHelper.GetNeighbors(map, X, Y, mapWidth, mapHeight).Where(n => n.Value.IsAccessible).Select(n => new TileNode
            {
                Tile = n.Value,
                X = n.Key.Item1,
                Y = n.Key.Item2
            });
        }
    }

}
