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
    public class SymptomRepository:IDisposable
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> connection;
        public SymptomRepository(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            connection = connectionStringOptions;
        }
        public Symptom Find(int id)
        {
            return db.Symptoms.Find(id);
        }
        public async Task<Symptom> FindByDiseaseAsync(int id)
        {
            return await db.Symptoms.Where(s => s.DiseaseId == id).FirstOrDefaultAsync();
        }
        public Symptom FindByDisease(int id)
        {
            return db.Symptoms.AsNoTracking().Where(s => s.DiseaseId == id).FirstOrDefault();
        }
        public void Dispose()
        {

        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(Symptom item)
        {
            db.Symptoms.Add(item);
            Save();
        }
        public void Update(Symptom item)
        {
            db.Entry(item).State = EntityState.Modified;
            Save();
        }
        public void Delete(int id)
        {
            Symptom item = Find(id);
            db.Symptoms.Remove(item);
            Save();
        }
    }
}
