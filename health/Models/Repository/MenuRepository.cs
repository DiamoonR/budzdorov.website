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
    public class MenuRepository
    {
        private SiteContext _db;
        private string _connection;        
        public MenuRepository(SiteContext context, IOptions<ConnectionStringOptions> connection)
        {
            _db = context;
            _connection = connection.Value.DefaultConnection;
        }
        public List<Menu> List()
        {
            using (IDbConnection db = new SqlConnection(_connection))
            {
                string sql = "select * from Menu order by Rank";
                var list = db.Query<Menu>(sql);
                return list.ToList();
            }
        }

        public async Task<List<Menu>> ListAsync()
        {
            using (IDbConnection db = new SqlConnection(_connection))
            {
                string sql = "select * from Menu order by Rank";
                var list = await db.QueryAsync<Menu>(sql);
                return list.ToList();
            }
        }

        public Menu Find(int id)
        {
            return _db.Menu.Find(id);
        }

        public int GetMaxRank()
        {
            return _db.Menu.Count() > 0 ? _db.Menu.Max(s => s.Rank) : 0;
        }
        public void Save()
        {
            _db.SaveChanges();
        }
        public void Add(Menu item)
        {
            item.Rank = GetMaxRank() + 1;
            _db.Menu.Add(item);
            Save();
        }
        public void Update(Menu item)
        {
            _db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            Menu item = Find(id);
            _db.Menu.Remove(item);
            Save();
        }
    }
}
