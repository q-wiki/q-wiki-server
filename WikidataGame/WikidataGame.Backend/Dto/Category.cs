using System;

namespace WikidataGame.Backend.Dto
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public static Category FromModel(Models.Category category)
        {
            if (category == null)
                return null;

            return new Category
            {
                Id = category.Id,
                Title = category.Title
            };
        }
    }
}
