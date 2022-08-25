﻿using GetBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.DataAccess.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        void Update(Book item);
    }
}
