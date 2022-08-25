using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.DataAccess.IRepository
{
    public interface IUnitOfWork
    {
        IBookRepository Book { get;}

        void Save();
    }
}
