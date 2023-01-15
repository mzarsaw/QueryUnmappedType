using System.Data.Common;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace QueryUnmappedTypes.Extensions
{
    public static class DbContextSqlQueryExtension
    {
        private static readonly Dictionary<string, DbContext> _dbContexts = new();
        public static IList<T> FromSqlRaw<T>(this DbContext db, string sql, params object[] parameters) where T : class
        {
            if (!_dbContexts.ContainsKey(typeof(T).Name))
            {
                _dbContexts.Add(typeof(T).Name, new ContextForQueryType<T>(db.Database.GetDbConnection()));
            }
            var db2 = _dbContexts[typeof(T).Name];
            return db2.Set<T>().FromSqlRaw(sql, parameters).ToList();
        }

        public static IList<T> FromSqlRaw<T>(
        this DbContext db,
        string sql,
        IDictionary<string, object> parameters,
        Func<string, object, IDbDataParameter> getParameter) where T : class =>
            parameters == null ? throw new ArgumentNullException(nameof(parameters)) :
            getParameter == null ? throw new ArgumentNullException(nameof(getParameter)) :
            db.FromSqlRaw<T>(sql, parameters.Select(p => getParameter(p.Key, p.Value))
                    .Cast<object>()
                    .ToArray());

        public static async Task<IList<T>> FromSqlRawAsync<T>(this DbContext db, string sql, params object[] parameters) where T : class => await Task.FromResult(db.FromSqlRaw<T>(sql, parameters));
        public static async Task<IList<T>> FromSqlRawAsync<T>(this DbContext db, string sql, IDictionary<string, object> parameters, Func<string, object, IDbDataParameter> getParameter) where T : class => await Task.FromResult(db.FromSqlRaw<T>(sql, parameters, getParameter));

        private class ContextForQueryType<T> : DbContext where T : class
        {
            private readonly DbConnection _connnection;

            public ContextForQueryType(DbConnection connnection)
            {
                _connnection = connnection;
            }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                switch (_connnection.GetType().Name)
                {
                    case nameof(SqlConnection):
                        optionsBuilder.UseSqlServer(_connnection);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                base.OnConfiguring(optionsBuilder);
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<T>(entity => { entity.HasNoKey(); });
                base.OnModelCreating(modelBuilder);
            }
        }

    }
}

