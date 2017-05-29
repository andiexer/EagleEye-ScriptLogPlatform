using EESLP.Services.Logging.API.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using EESLP.Services.Logging.API.Entities;

namespace EESLP.Services.Logging.API.Services
{
    public abstract class BaseService
    {
        private readonly string _connectionString;

        protected BaseService(IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        protected IDbConnection Connection => new MySqlConnection(_connectionString);

        protected void UpdateAuditableFields(IAuditableEntity entity, bool isCreated = false)
        {
            if (isCreated)
                entity.CreatedDateTime = DateTime.UtcNow;
            entity.LastModDateTime = DateTime.UtcNow;
        }
    }
}
