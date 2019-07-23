using ProductCatalog.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Data.Repository
{
    /*Interface generica de repositorio */
    public interface IRepository<T> where T : Base
    {
        T Create(T item);
        List<T> FindAll();
        T FindById(string Id);
        void Delete(string Id);
    }
}
