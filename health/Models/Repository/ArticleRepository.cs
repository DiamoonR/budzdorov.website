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
    public class ArticleRepository:IDisposable
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        public ArticleRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            connection = connectionStringOptions;
        }
        public void Dispose()
        {

        }
        public List<Article> List()
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from Articles order by Rank desc";
                var list = db.Query<Article>(sql);
                return list.ToList();
            }
        }

        public async Task<Article> FindAsync(string idname)
        {
            return await db.Articles.AsNoTracking().Where(s => s.IdName == idname).FirstOrDefaultAsync();
        }

        public async Task<Article> FindAsync(int id)
        {
            await Task.Delay(2000);
            return await db.Articles.Where(s => s.ArticleId == id).FirstOrDefaultAsync();
        }

        public bool CheckForExist(string idName)
        {
            return db.Articles.Where(s => s.IdName == idName).Count() > 0 ? true : false;
        }

        public async Task<List<Article>> ListAsync()
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from Articles order by Rank desc";
                var list = await db.QueryAsync<Article>(sql);
                return list.ToList();
            }
        }
        public async Task<int> Count()
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                int result = await db.ExecuteScalarAsync<int>("select count(*) from Articles");
                return result;
            }
        }
        public async Task<List<TArticle>> ListAsync(int pageIndex, int pageSize, bool isAdmin)
        {
            //await Task.Delay(2000);
            int offset = (pageIndex - 1) * pageSize;
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = $@"select a.ArticleId, a.Name, a.IdName, a.Description, a.Rank                              
                              from Articles as a 
                              order by Rank desc
                              offset {offset} rows fetch next {pageSize} rows only";
                var list = await db.QueryAsync(sql);
                List<TArticle> result = new List<TArticle>();
                foreach (var item in list)
                {
                    TArticle model = new TArticle();
                    model.IdName = item.IdName;
                    model.Description = item.Description;
                    model.ArticleId = item.ArticleId;
                    model.Name = item.Name;
                    model.Rank = item.Rank;
                    result.Add(model);
                }
                return result;
            }
        }

        public async Task<List<TArticle>> LastArticlesAsync(int num)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = $@"select top {num} a.ArticleId, a.Name, a.IdName, a.Description, a.Rank                              
                              from Articles as a 
                              order by Rank desc";
                var list = await db.QueryAsync(sql);
                List<TArticle> result = new List<TArticle>();
                foreach (var item in list)
                {
                    TArticle model = new TArticle();
                    model.IdName = item.IdName;
                    model.Description = item.Description;
                    model.ArticleId = item.ArticleId;
                    model.Name = item.Name;
                    model.Rank = item.Rank;
                    result.Add(model);
                }
                return result;
            }
        }

        public Article Find(int id)
        {
            return db.Articles.Find(id);
        }

        public int GetMaxRank()
        {
            return db.Articles.Count() > 0 ? db.Articles.Max(s => s.Rank) : 0;
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(Article item)
        {
            item.Rank = GetMaxRank() + 1;
            db.Articles.Add(item);
            Save();
        }
        public void Update(Article item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            Article item = Find(id);
            db.Articles.Remove(item);
            Save();
        }
        public void Move(int ArticleId, string direction)
        {
            Article Article = Find(ArticleId);
            if (Article != null)
            {
                Article Article2;
                switch (direction)
                {
                    case "up":
                        Article2 = db.Articles.Where(s => s.Rank > Article.Rank).OrderBy(s => s.Rank).FirstOrDefault();
                        break;
                    default:
                        Article2 = db.Articles.Where(s => s.Rank < Article.Rank).OrderByDescending(s => s.Rank).FirstOrDefault();
                        break;
                }
                if (Article2 != null)
                {
                    int oldRank = Article.Rank;
                    int newRank = Article2.Rank;
                    Article.Rank = newRank;
                    Article2.Rank = oldRank;
                    Update(Article);
                    Update(Article2);
                }
            }
        }

    }
}
