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
    public class ContraindicationRepository:IDisposable
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        public ContraindicationRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            connection = connectionStringOptions;
        }
        public void Dispose()
        {

        }
        public Contraindication Find(int id)
        {
            return db.Contraindications.Find(id);
        }
        public Contraindication FindByHerb(int herbId)
        {
            return db.Contraindications.Where(s => s.HerbId == herbId).FirstOrDefault();
        }

        public async  Task<Contraindication> FindByHerbAsync(int herbId)
        {
            return await db.Contraindications.Where(s => s.HerbId == herbId).FirstOrDefaultAsync();
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(Contraindication item)
        {
            db.Contraindications.Add(item);
            Save();
        }
        public void Update(Contraindication item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            Contraindication item = Find(id);
            db.Contraindications.Remove(item);
            Save();
        }
    }
}
