using Pt.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.BL.Repository
{//doya verştabanı dönüşüm ve kullanıcıda değer alınan işlemlerde try catch alınmalı
    public class RepositoryBase<T,TId> where T:class
    {
        protected internal static MyContext dbContext;

        public virtual List<T> GetAll()
        {
            try
            {
                dbContext =  new MyContext();
                return dbContext.Set<T>().ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
      
        }
        public virtual T GetById(TId id)
        {
            try
            {
                dbContext = new MyContext();
                return dbContext.Set<T>().Find(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }
        public virtual int Insert(T entity)   //override edip bunu tekrar kullanabilmek için     
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                dbContext.Set<T>().Add(entity);
                return dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual int Delete(T entity)
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                dbContext.Set<T>().Remove(entity);
                return dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public virtual int Update()
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                return dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
