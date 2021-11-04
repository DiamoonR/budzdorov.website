using health.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using health.Models;
using health.Data;
using Microsoft.Extensions.Options;
using health.Options;
using System.Text.RegularExpressions;
using health.Util;

namespace health.Areas.Admin.Util
{
    public static class AdminActions
    {
        public static void Sort<T>(List<T> itemList, SiteContext db, IOptions<ConnectionStringOptions> connection)
        {
            string type = typeof(T).Name;
            int rank = 0;
            switch (type)
            {
                case "Menu":
                    MenuRepository rep = new MenuRepository(db, connection);
                    foreach (var item in itemList)
                    {
                        rank++;
                        var model =(Menu)((object)item);
                        model.Rank = rank;
                        rep.Update(model);
                    }
                    break;
            }
        }

        public static async Task UpdateDiseaseIdentity(SiteContext db, IOptions<ConnectionStringOptions> connection)
        {
            DiseaseRepository rep = new DiseaseRepository(db, connection);
            List<Disease> list = rep.List();
            foreach(var item in list)
            {
                string idname = await TranslitAsync(item.Name, "Disease", db, connection);
                Disease model = new Disease();
                model.Content = item.Content;
                model.Description = item.Description;
                model.DiseaseId = item.DiseaseId;
                model.IdName = idname;
                model.Name = item.Name;
                rep.Update(model);
            }
        }

        public static async Task UpdateHerbIdentity(SiteContext db, IOptions<ConnectionStringOptions> connection)
        {
            HerbRepository rep = new HerbRepository(db, connection);
            List<Herb> list = rep.List();
            foreach (var item in list)
            {
                string idname = await TranslitAsync(item.Name, "Herb", db, connection);
                Herb model = new Herb();
                model.Content = item.Content;
                model.Description = item.Description;
                model.HerbId = item.HerbId;
                model.IdName = idname;
                model.Name = item.Name;                
                rep.Update(model);
            }
        }

        public static async Task UpdateArticleIdentity(SiteContext db, IOptions<ConnectionStringOptions> connection)
        {
            ArticleRepository rep = new ArticleRepository(db, connection);
            List<Article> list = rep.List();
            foreach (var item in list)
            {
                string idname = await TranslitAsync(item.Name, "Article", db, connection);
                Article model = new Article();
                model.Content = item.Content;
                model.Rank = item.Rank;
                model.Title = item.Title;
                model.Description = item.Description;
                model.ArticleId = item.ArticleId;
                model.IdName = idname;
                model.Name = item.Name;
                rep.Update(model);
            }
        }

        public static async Task<string> TranslitAsync(string name, string modelName, SiteContext db, IOptions<ConnectionStringOptions> connection)
        {            
            string pattern = @"[^a-z0-9а-я\s]";
            string idName = name.ToLower();
            idName = Regex.Replace(idName, pattern, String.Empty);
            idName = idName.Trim();
            idName = Regex.Replace(idName, @"\s", "-");
            idName = Transliteration.Front(idName).ToLower();
            idName = idName.Replace('.', '-');
            idName = idName.Replace(',', '-');
            idName = idName.Replace("!", "");
            idName = idName.Replace("\"", "");
            idName = idName.Replace("?", "");
            idName = await CheckIdNameAsync(idName, modelName,db,connection);
            return idName;
        }
        public static async Task<string> CheckIdNameAsync(string idName, string model, SiteContext db, IOptions<ConnectionStringOptions> connection)
        {
            int i;
            switch (model)
            {
                case "MyPage":
                    MyPageRepository mpRep = new MyPageRepository(db, connection);
                    i = 1;
                    if (mpRep.CheckForExist(idName))
                    {
                        i++;
                        idName = idName + i;
                        idName = await CheckIdNameAsync(idName, model, db, connection);
                    }
                    break;
                case "Article":
                    ArticleRepository articleRep = new ArticleRepository(db, connection);
                    i = 1;
                    if (articleRep.CheckForExist(idName))
                    {
                        i++;
                        idName = idName + i;
                        idName = await CheckIdNameAsync(idName, model, db, connection);
                    }
                    break;
                case "Disease":
                    DiseaseRepository disRep = new DiseaseRepository(db, connection);
                    i = 1;
                    if (disRep.CheckForExist(idName))
                    {
                        i++;
                        idName = idName + i;
                        idName = await CheckIdNameAsync(idName, model, db, connection);
                    }
                    break;
                case "Herb":
                    HerbRepository herbRep = new HerbRepository(db, connection);
                    i = 1;
                    if (herbRep.CheckForExist(idName))
                    {
                        i++;
                        idName = idName + i;
                        idName = await CheckIdNameAsync(idName, model, db, connection);
                    }
                    break;
            }
            return idName;
        }
    }
}
