using Cima.AppContext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Cima.Repository.Shared
{
    public class SqlBaseRepository<T> where T : class
    {
        #region Chaines de connexion
        protected const string CONNECTION_STRING_SYSMAN = "SYSMAN_DS_CONNECTION";
        protected const string CONNECTION_STRING_DWH = "DWH_DS_CONNECTION_STRING";
        #endregion

        internal SysmanDbContext context;
        internal DbSet<T> dbSet;

        public SqlBaseRepository()
        {
           
        }

        public SqlBaseRepository(SysmanDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        protected IDbCommand GetCommand(string query, IDbConnection connection)
        {
            SqlCommand sqlcommand = new SqlCommand(query)
            {
                Connection = (SqlConnection)connection
            };

            return sqlcommand;
        }

        protected SqlConnection Connect(string dsConnection)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[dsConnection].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);

                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new System.ArgumentException("Echec de connexion à la BD, vérifier serveur de données !");
            }
        }

        public virtual IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual T GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}