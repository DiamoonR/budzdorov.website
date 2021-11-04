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
    public class RecipeRepository:IDisposable
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        public RecipeRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            connection = connectionStringOptions;
        }
        public void Dispose()
        {

        }
        public async Task<List<Recipe>> ListAsync(int parentId)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from Recipes where DiseaseId=@parentId order by Rank";
                var list = await db.QueryAsync<Recipe>(sql,new { parentId });
                return list.ToList();
            }
        }
        public async Task<List<TRecipe>> LastRecipesAsync(int num)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = $@"select top {num} a.Content as Description, b.IdName, b.Name
                                from Recipes as a
                                inner join Disease as b on b.DiseaseId=a.DiseaseId
                                order by a.RecipeId desc";
                var list = await db.QueryAsync(sql);
                List<TRecipe> result = new List<TRecipe>();
                foreach (var item in list)
                {
                    TRecipe model = new TRecipe();
                    model.Url =$"{Constants.URL_DISEASE}/{item.IdName}";
                    model.Description = item.Description;
                    model.Name = item.Name;
                    result.Add(model);
                }
                return result;
            }
        }
        public Recipe Find(int id)
        {
            return db.Recipes.Find(id);
        }

        public async Task<Recipe> FindAsync(int id)
        {
            return await db.Recipes.Where(s => s.RecipeId == id).FirstOrDefaultAsync();
        }

        public int GetMaxRank(int parentId)
        {
            return db.Recipes.Where(s=>s.DiseaseId==parentId).Count() > 0 ? db.Recipes.Where(s=>s.DiseaseId==parentId).Max(s => s.Rank) : 0;
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(Recipe item)
        {
            item.Rank = GetMaxRank(item.DiseaseId) + 1;
            db.Recipes.Add(item);
            Save();
        }
        public void Update(Recipe item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            Recipe item = Find(id);
            db.Recipes.Remove(item);
            Save();
        }
        public void Move(int recipeId, string direction)
        {
            Recipe recipe = Find(recipeId);
            if (recipe != null)
            {
                Recipe recipe2;
                switch (direction)
                {
                    case "up":
                        recipe2 = db.Recipes.Where(s=>s.DiseaseId==recipe.DiseaseId).Where(s => s.Rank > recipe.Rank).OrderBy(s => s.Rank).FirstOrDefault();
                        break;
                    default:
                        recipe2 = db.Recipes.Where(s => s.DiseaseId == recipe.DiseaseId).Where(s => s.Rank < recipe.Rank).OrderByDescending(s => s.Rank).FirstOrDefault();
                        break;
                }
                if (recipe2 != null)
                {
                    int oldRank = recipe.Rank;
                    int newRank = recipe2.Rank;
                    recipe.Rank = newRank;
                    recipe2.Rank = oldRank;
                    Update(recipe);
                    Update(recipe2);
                }
            }
        }

    }
}
