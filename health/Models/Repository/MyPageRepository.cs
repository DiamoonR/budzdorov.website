using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using health.Data;
using health.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace health.Models.Repository
{
    public class MyPageRepository
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        public MyPageRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            connection = connectionStringOptions;
        }

        public List<MyPageShort> List()
        {
            var list = db.Pages.OrderBy(s => s.Name).Select(s => new { s.MyPageId, s.Name, s.IdName }).ToList();
            List<MyPageShort> result = new List<MyPageShort>();
            foreach (var item in list)
            {
                MyPageShort page = new MyPageShort();
                page.IdName = item.IdName;
                page.Name = item.Name;
                page.MyPageId = item.MyPageId;
                result.Add(page);
            }
            return result;
        }

        public async Task<IList<MyPage>> FullListAsync()
        {
            return await db.Pages.AsNoTracking().ToListAsync();
        }

        public List<string> GetIdNameList()
        {
            return db.Pages.Select(s => s.IdName).ToList();
        }
        public MyPage Find(int id)
        {
            if (id > 0)
                return db.Pages.Find(id);
            else
                return null;
        }

        public bool CheckForExist(string idName)
        {
            return db.Pages.Where(s => s.IdName == idName).Count() > 0 ? true : false;
        }

        public async Task<bool> CheckForExistAsync(string idName)
        {
            return await db.Pages.Where(s => s.IdName == idName).CountAsync() > 0 ? true : false;
        }
        public MyPage Find(string idname)
        {
            if (idname.Length > 0)
                return db.Pages.Where(s => s.IdName == idname).FirstOrDefault();
            else
                return null;
        }
        public async Task<MyPage> FindAsync(string idName)
        {
            return await db.Pages.Where(s => s.IdName == idName).FirstOrDefaultAsync();
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(MyPage item)
        {
            db.Pages.Add(item);
            Save();
        }
        public void Update(MyPage item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            MyPage item = Find(id);
            db.Pages.Remove(item);
            Save();
            //PhotoRepository rep = new PhotoRepository();
            //rep.DeleteAll(id, "page");
        }
    }
}
