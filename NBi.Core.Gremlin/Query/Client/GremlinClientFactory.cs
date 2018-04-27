using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Gremlin.Query.Client
{
    public class GremlinClientFactory : Extensibility.Query.IClientFactory
    {

        public bool CanHandle(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            return connectionStringBuilder.ContainsKey(GremlinClient.HostnameToken)
                && connectionStringBuilder.ContainsKey(GremlinClient.UsernameToken)
                && connectionStringBuilder.ContainsKey(GremlinClient.PasswordToken)
                && HasApiSetTo(connectionStringBuilder, "gremlin");
        }

        protected bool HasApiSetTo(DbConnectionStringBuilder connectionStringBuilder, string api)
            => connectionStringBuilder.ContainsKey(GremlinClient.ApiToken) && ((string)connectionStringBuilder[GremlinClient.ApiToken]).ToLowerInvariant() == api.ToLowerInvariant();


        public Extensibility.Query.IClient Instantiate(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var hostname = (string)connectionStringBuilder[GremlinClient.HostnameToken];
            var port = Int32.Parse((string)connectionStringBuilder[GremlinClient.PortToken]);
            var enableSsl = Boolean.Parse((string)connectionStringBuilder[GremlinClient.EnableSslToken]);
            var username = (string)connectionStringBuilder[GremlinClient.UsernameToken];
            var password = (string)connectionStringBuilder[GremlinClient.PasswordToken];
            

            return new GremlinClient(hostname, port, enableSsl, username, password);
        }
    }
}
