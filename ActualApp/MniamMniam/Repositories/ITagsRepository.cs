using MniamMniam.Data;
using MniamMniam.Models.CookBookModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Repositories
{
    public interface ITagsRepository
    {
        IEnumerable<Tag> GetAllTags();
    }

    public class TagsRepository : ITagsRepository
    {
        private readonly ApplicationDbContext db;

        public TagsRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Tag> GetAllTags() => db.Tags;
    }
}
