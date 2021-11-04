using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using health.Data;
using health.Options;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace health.Models.Repository
{
    public class HerbRepository:IDisposable
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        public HerbRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            connection = connectionStringOptions;
        }
        public List<Herb> List()
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from Herbs order by Name";
                var list = db.Query<Herb>(sql);
                return list.ToList();
            }
        }

        public void Dispose()
        {

        }

        public async Task<Herb> FindAsync(string idname)
        {
            return await db.Herbs.AsNoTracking().Where(s => s.IdName == idname).FirstOrDefaultAsync();
        }

        public async Task<Herb> FindAsync(int id)
        {
            return await db.Herbs.Where(s => s.HerbId == id).FirstOrDefaultAsync();
        }

        public bool CheckForExist(string idName)
        {
            return db.Herbs.Where(s => s.IdName == idName).Count() > 0 ? true : false;
        }

        public async Task<List<Herb>> ListAsync()
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from Herbs order by Name";
                var list = await db.QueryAsync<Herb>(sql);
                return list.ToList();
            }
        }
        public async Task<int> Count()
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                int result = await db.ExecuteScalarAsync<int>("select count(*) from Herbs");
                return result;
            }
        }
        public async Task<int> CountByLetter(string letter)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                if (letter.Length == 1)
                {
                    int result = await db.ExecuteScalarAsync<int>($"select count(*) from Herbs where Name like concat('{letter}','%')");
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }
        public async Task<List<THerb>> ListAsync(int pageIndex, int pageSize, bool isAdmin)
        {
            int offset = (pageIndex - 1) * pageSize;
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = $@"select a.HerbId, a.Name, a.IdName, a.Description
                              from Herbs as a 
                              order by a.Name
                              offset {offset} rows fetch next {pageSize} rows only";
                var list = await db.QueryAsync(sql);
                List<THerb> result = new List<THerb>();
                foreach (var item in list)
                {
                    THerb model = new THerb();
                    model.IdName = item.IdName;
                    model.Description = item.Description;
                    model.HerbId = item.HerbId;
                    model.Name = item.Name;
                    result.Add(model);
                }
                return result;
            }
        }

        public async Task<List<THerb>> ListAsyncByLetter(int pageIndex, int pageSize, string letter, bool isAdmin)
        {
            int offset = (pageIndex - 1) * pageSize;
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = $@"select a.HerbId, a.Name, a.IdName, a.Description
                              from Herbs as a where a.Name like concat(@letter,'%')
                              order by a.Name
                              offset {offset} rows fetch next {pageSize} rows only";
                var list = await db.QueryAsync(sql,new { letter });
                List<THerb> result = new List<THerb>();
                foreach (var item in list)
                {
                    THerb model = new THerb();
                    model.IdName = item.IdName;
                    model.Description = item.Description;
                    model.HerbId = item.HerbId;
                    model.Name = item.Name;
                    result.Add(model);
                }
                return result;
            }
        }

        public Herb Find(int id)
        {
            return db.Herbs.Find(id);
        }

        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(Herb item)
        {
            db.Herbs.Add(item);
            Save();
        }
        public void Update(Herb item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            Herb item = Find(id);
            db.Herbs.Remove(item);
            Save();
        }
    }
}
