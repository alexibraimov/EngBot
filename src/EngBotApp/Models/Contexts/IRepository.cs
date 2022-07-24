using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models.Contexts
{
    public interface IRepository<T>
    {
        void Save(T value);
        IList<T> GetAll();
        T FindById(long id);
        bool Contains(long id);
    }
}
