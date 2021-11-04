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
    public class CommentRepository
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        public CommentRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            connection = connectionStringOptions;
        }

        public async Task<List<Comment>> ListAsync(string razdel, int itemId, bool isAdmin)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string where = "";
                if (!isAdmin)
                {
                    where = "and Enable=1";
                }
                string sql = $"select * from Comments where Razdel=@razdel and ElementId=@itemId {where} order by DateAdd desc";
                var list = await db.QueryAsync<Comment>(sql,new { razdel, itemId });
                return list.ToList();
            }
        }

        public Comment Find(int id)
        {
            return db.Comments.Find(id);
        }

        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(Comment item)
        {
            db.Comments.Add(item);
            Save();
        }
        public void Update(Comment item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            Comment item = Find(id);
            db.Comments.Remove(item);
            Save();
        }

    }
}
