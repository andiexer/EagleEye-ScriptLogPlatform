using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Infrastructure.Options;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace EESLP.Services.Logging.API.Services
{
    public class HostService : BaseService, IHostService
    {

        public HostService(IOptions<DatabaseOptions> options) : base(options)
        {
        }

        public IEnumerable<Host> GetAllHosts(string hostname, int skipNumber, int takeNumber)
        {
            hostname = hostname == null ? "" : hostname;
            using (var db = Connection)
            {
                db.Open();
                return db.Query<Host>($"SELECT * FROM Host WHERE hostname LIKE CONCAT(\"%\",@hostname,\"%\") LIMIT {skipNumber},{takeNumber}", new { hostname = hostname });
            }
        }

        public IEnumerable<int> GetAllHostIDs(string hostname)
        {
            hostname = hostname == null ? "" : hostname;
            using (var db = Connection)
            {
                db.Open();
                return db.Query<int>($"SELECT Id FROM Host WHERE hostname LIKE CONCAT(\"%\",@hostname,\"%\")", new { hostname = hostname });
            }
        }

        public int GetNumberOfAllHosts(string hostname)
        {
            hostname = hostname == null ? "" : hostname;
            using (var db = Connection)
            {
                db.Open();
                return db.Query<int>($"SELECT COUNT(*) FROM Host WHERE hostname LIKE CONCAT(\"%\",@hostname,\"%\")", new { hostname = hostname }).ToArray()[0];
            }
        }

        public Host GetHostById(int id)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Get<Host>(id);
            }
        }

        public Host GetHostByApiKey(string apiKey)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Query<Host>("SELECT * FROM Host WHERE ApiKey = @ApiKey",
                        new
                        {
                            ApiKey = apiKey
                        })
                    .FirstOrDefault();
            }
        }

        public bool Update(Host host)
        {
            using (var db = Connection)
            {
                db.Open();
                UpdateAuditableFields(host);
                return db.Update<Host>(host);
            }
        }

        public bool Delete(Host host)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Delete<Host>(host);
            }
        }

        public int Add(Host host)
        {
            using (var db = Connection)
            {
                db.Open();
                UpdateAuditableFields(host, true);
                return (int) db.Insert<Host>(host);
            }
        }
    }
}
