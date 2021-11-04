using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using health.Data;
using health.Options;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace health.Models.Repository
{
    public class PageInfoRepository:IDisposable
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        public PageInfoRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            connection = connectionStringOptions;
        }
        public PageInfo Find(int id)
        {
            return db.PageInfo.Find(id);
        }
        public bool CheckForExist(string url)
        {
            return db.PageInfo.Where(s => s.Url == url).Count() > 0 ? true : false;
        }
        public PageInfo Find(string url)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from PageInfo where url=@url";
                var list = db.Query<PageInfo>(sql, new { url });
                return list.FirstOrDefault();
            }
        }
        public async Task<PageInfo> FindAsync(string url)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from PageInfo where url=@url";
                var list = await db.QueryAsync<PageInfo>(sql,new { url });
                return list.FirstOrDefault();
            }
        }

        public void Dispose()
        {

        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(PageInfo item)
        {
            db.PageInfo.Add(item);
            Save();
        }
        public void Update(PageInfo item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            PageInfo item = Find(id);
            db.PageInfo.Remove(item);
            Save();
        }
    }
}
