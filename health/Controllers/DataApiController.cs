using Dapper;
using health.Data;
using health.Options;
using health.Util;
using health.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace health.Controllers
{
    [Produces("application/json")]
    [Route("api/dataapi/[action]/{id?}")]
    [ApiController]
    public class DataApiController : ControllerBase
    {
        private SiteContext db;
        private readonly IOptions<ConnectionStringOptions> _connectionStringOptions;

        public DataApiController(SiteContext context, IOptions<ConnectionStringOptions> connectionStringOptions)
        {
            db = context;
            _connectionStringOptions = connectionStringOptions;
        }
        [HttpGet]
        public async Task<IActionResult> GetDiseaseList()
        {            
            string name = HttpContext.Request.Query["name"].ToString();

            using (IDbConnection db = new SqlConnection(_connectionStringOptions.Value.DefaultConnection))
            {
                string sql = @"select Name, IdName from Disease
                               where Name like concat('%',@name,'%')";
                var list = await db.QueryAsync(sql, new { name });
                List<SearchItem> resultList = new List<SearchItem>();
                foreach (var item in list)
                {
                    SearchItem model = new SearchItem();
                    model.Name = item.Name;
                    model.Url = $"{Constants.URL_DISEASE}/{item.IdName}";
                    resultList.Add(model);
                }
                return Ok(resultList);
            }
        }

        public async Task<IActionResult> GetHerbsList()
        {
            string name = HttpContext.Request.Query["name"].ToString();

            using (IDbConnection db = new SqlConnection(_connectionStringOptions.Value.DefaultConnection))
            {
                string sql = @"select Name, IdName from Herbs
                               where Name like concat('%',@name,'%')";
                var list = await db.QueryAsync(sql, new { name });
                List<SearchItem> resultList = new List<SearchItem>();
                foreach (var item in list)
                {
                    SearchItem model = new SearchItem();
                    model.Name = item.Name;
                    model.Url = $"{Constants.URL_HERB}/{item.IdName}";
                    resultList.Add(model);
                }
                return Ok(resultList);
            }
        }
    }
}
