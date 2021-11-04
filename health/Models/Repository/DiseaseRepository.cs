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
using health.Util;

namespace health.Models.Repository
{
    public class DiseaseRepository:IDisposable
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        public DiseaseRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            connection = connectionStringOptions;
        }
        public List<Disease> List()
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from Disease order by Name";
                var list = db.Query<Disease>(sql);
                return list.ToList();
            }
        }
        public void Dispose()
        {

        }
        public async Task<Disease> FindAsync(string idname)
        {
            return await db.Diseases.AsNoTracking().Where(s => s.IdName == idname).FirstOrDefaultAsync();
        }

        public async Task<Disease> FindAsync(int id)
        {
            return await db.Diseases.Where(s => s.DiseaseId == id).FirstOrDefaultAsync();
        }

        public bool CheckForExist(string idName)
        {
            return db.Diseases.Where(s => s.IdName == idName).Count() > 0 ? true : false;
        }

        public async Task<List<Disease>> ListAsync()
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from Disease order by Name";
                var list = await db.QueryAsync<Disease>(sql);
                return list.ToList();
            }
        }
        public async Task<int> Count()
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                int result = await db.ExecuteScalarAsync<int>("select count(*) from Disease");
                return result;
            }
        }
        public async Task<int> CountByLetter(string letter)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                if (letter.Length == 1)
                {
                    int result = await db.ExecuteScalarAsync<int>($"select count(*) from Disease where Name like concat('{letter}','%')");
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }

        public async Task<List<health.ViewModels.SearchItem>> GetDiseaseSearchList(string name)
        {            
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = @"select Name, IdName from Disease
                               where Name like concat('%',@name,'%')";
                var list = await db.QueryAsync(sql, new { name });
                List<health.ViewModels.SearchItem> resultList = new List<ViewModels.SearchItem>();
                foreach (var item in list)
                {
                    health.ViewModels.SearchItem model = new ViewModels.SearchItem();
                    model.Name = item.Name;
                    model.Url = $"{Constants.URL_DISEASE}/{item.IdName}";
                    resultList.Add(model);
                }
                return resultList;
            }
        }

        public async Task<List<TDisease>> ListAsync(int pageIndex, int pageSize, bool isAdmin)
        {
            int offset = (pageIndex - 1) * pageSize;
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = $@"select a.DiseaseId, a.Name, a.IdName, a.Description, (select count(*) from Recipes where Recipes.DiseaseId=a.DiseaseId) as RecipeCount
                              from Disease as a 
                              order by a.Name
                              offset {offset} rows fetch next {pageSize} rows only";
                var list = await db.QueryAsync(sql);
                List<TDisease> result = new List<TDisease>();
                foreach (var item in list)
                {
                    TDisease model = new TDisease();
                    model.IdName = item.IdName;
                    model.Description = item.Description;
                    model.DiseaseId = item.DiseaseId;
                    model.Name = item.Name;
                    model.RecipeCount = item.RecipeCount;
                    result.Add(model);
                }
                return result;
            }
        }

        public async Task<List<TDisease>> ListAsyncByLetter(int pageIndex, int pageSize, string letter, bool isAdmin)
        {
            int offset = (pageIndex - 1) * pageSize;
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = $@"select a.DiseaseId, a.Name, a.IdName, a.Description, (select count(*) from Recipes where Recipes.DiseaseId=a.DiseaseId) as RecipeCount
                              from Disease as a where a.Name like concat(@letter,'%')
                              order by a.Name
                              offset {offset} rows fetch next {pageSize} rows only";
                var list = await db.QueryAsync(sql, new { letter });
                List<TDisease> result = new List<TDisease>();
                foreach (var item in list)
                {
                    TDisease model = new TDisease();
                    model.IdName = item.IdName;
                    model.Description = item.Description;
                    model.DiseaseId = item.DiseaseId;
                    model.Name = item.Name;
                    model.RecipeCount = item.RecipeCount;
                    result.Add(model);
                }
                return result;
            }
        }

        public Disease Find(int id)
        {
            return db.Diseases.Find(id);
        }

        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(Disease item)
        {
            db.Diseases.Add(item);
            Save();
        }
        public void Update(Disease item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            Disease item = Find(id);
            db.Diseases.Remove(item);
            Save();
        }
    }
}
