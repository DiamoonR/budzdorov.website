using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using health.Data;
using health.Options;
using health.Util;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace health.Models.Repository
{
    public class UploadImageRepository
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        private readonly IWebHostEnvironment _env;
        public UploadImageRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions, IWebHostEnvironment env)
        {
            db = context;
            connection = connectionStringOptions;
            _env = env;
        }
        public UploadImage Find(int id)
        {
            return db.UploadImages.Find(id);
        }
        public bool CheckForExist(string fileName)
        {
            return db.UploadImages.Where(s => s.FileName == fileName).Count() > 0 ? true : false;
        }
        public UploadImage Find(string fileName)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from Images where FileName=@fileName";
                var list = db.Query<UploadImage>(sql, new { fileName });
                return list.FirstOrDefault();
            }
        }
        public async Task<UploadImage> FindAsync(string fileName)
        {
            using (IDbConnection db = new SqlConnection(connection.Value.DefaultConnection))
            {
                string sql = "select * from Images where FileName=@fileName";
                var list = await db.QueryAsync<UploadImage>(sql, new { fileName });
                return list.FirstOrDefault();
            }
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(UploadImage item)
        {
            db.UploadImages.Add(item);
            Save();
        }
        public void Update(UploadImage item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            UploadImage item = Find(id);
            string dir = System.IO.Path.Combine(_env.WebRootPath, Constants.PATH_UPLOAD_IMAGE);
            string file = System.IO.Path.Combine(dir, item.FileName + ".jpg");
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);
            db.UploadImages.Remove(item);
            Save();
        }
        public void DeleteByUrl(string url)
        {
            var list = db.UploadImages.Where(s => s.Url == url).ToList();
            foreach(var item in list)
            {
                string dir = System.IO.Path.Combine(_env.WebRootPath, Constants.PATH_UPLOAD_IMAGE);
                string file = System.IO.Path.Combine(dir, item.FileName + ".jpg");
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
                db.UploadImages.Remove(item);
            }
            Save();
        }
    }
}
