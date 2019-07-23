using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Data.Repository.Implementattions
{
    /*Classe generica de repositorio*/
    public class Repository<T> : IRepository<T> where T : Base
    {
        private readonly StoreDataContext _storeDataContext;
        private DbSet<T> dataset;
        private IConfiguration _config;

        public Repository(StoreDataContext storeDataContext, IConfiguration configuration)
        {
            _storeDataContext = storeDataContext;
            dataset = _storeDataContext.Set<T>();
            _config = configuration;
        }
        public T Create(T item)
        {
            try
            {
                dataset.Add(item);
                _storeDataContext.SaveChanges();
                return item;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Delete(string Id)
        {
            var result = FindById(Id);
            try
            {
                if (result != null)
                {
                    dataset.Remove(result);
                    _storeDataContext.SaveChanges(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<T> FindAll()
        {
            return dataset.ToList();
        }

        public T FindById(string Id)
        {
            return dataset.SingleOrDefault(x => x.Id.ToString().Equals(Id));
        }
    }
}
