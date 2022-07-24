using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models.Contexts
{
    public class UserRepository : IRepository<UserInfo>
    {
        private UserContext _context;

        public UserRepository()
        {
            _context = new UserContext();
        }

        public bool Contains(long id)
        {
            return _context.Contains(id);
        }

        public UserInfo FindById(long id)
        {
            return _context.Get(id);
        }

        public IList<UserInfo> GetAll()
        {
            return _context.GetAll();
        }

        public void Save(UserInfo value)
        {
            _context.Save(value);
        }
    }
}
