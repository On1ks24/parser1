using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teacher;

namespace DataAccessLayer
{
    public interface IRepository
    {
        IEnumerable<teacher> GetAll();
        void Add(teacher item);
        teacher GetById(int Id);

    }
}
