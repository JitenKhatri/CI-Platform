using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CiPlatformContext _db;
        internal DbSet<T> dbset;
        public Repository(CiPlatformContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = _db.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            return query.FirstOrDefault();

        }


    }
    public static class LinqHelper
    {
        public static IQueryable<T> Randomizer<T>(this IQueryable<T> pCol)
        {
            List<T> lResultado = new List<T>();
            List<T> lLista = pCol.ToList();
            Random lRandom = new Random();
            int lintPos = 0;

            while (lLista.Count > 0)
            {
                lintPos = lRandom.Next(lLista.Count);
                lResultado.Add(lLista[lintPos]);
                lLista.RemoveAt(lintPos);
            }
            return lResultado.AsQueryable();
        }

        //public static IQueryable<T> Randomizer<T>(this IQueryable<T> pCol)
        //{
        //    List<T> lLista = pCol.ToList();
        //    Random lRandom = new Random();

        //    IQueryable<T> lResultado = lLista
        //        .OrderBy(x => lRandom.Next())
        //        .AsQueryable();

        //    return lResultado;
        //}
    }
}
