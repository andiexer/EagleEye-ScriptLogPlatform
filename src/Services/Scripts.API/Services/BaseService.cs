using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EESLP.Services.Scripts.API.Entities;
using EESLP.Services.Scripts.API.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace EESLP.Services.Scripts.API.Services
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
