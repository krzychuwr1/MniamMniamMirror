using MniamMniam.Data;
using MniamMniam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam.Repositories
{
    public interface IUsersRepository
    {
        IEnumerable<ApplicationUser> GetAllUsers();
    }

    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext db;

        public UsersRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<ApplicationUser> GetAllUsers() => db.Users;
    }
}
