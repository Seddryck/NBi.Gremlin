using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Gremlin.Query.Client
{
    public class GremlinLikeClientFactory : Extensibility.Query.IClientFactory
    {
        public const string HostnameToken = "hostname";
        public const string PortToken = "port";
        public const string EnableSslToken = "enablessl";
        public const string UsernameToken = "username";
        public const string PasswordToken = "password";
        public const string ApiToken = "api";

        public bool CanHandle(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            return connectionStringBuilder.ContainsKey(HostnameToken)
                && connectionStringBuilder.ContainsKey(UsernameToken)
                && connectionStringBuilder.ContainsKey(PasswordToken)
                && HasApiSetTo(connectionStringBuilder, "gremlin");
        }

        protected bool HasApiSetTo(DbConnectionStringBuilder connectionStringBuilder, string api)
            => connectionStringBuilder.ContainsKey(ApiToken) && ((string)connectionStringBuilder[ApiToken]).ToLowerInvariant() == api.ToLowerInvariant();


        public Extensibility.Query.IClient Instantiate(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var hostname = (string)connectionStringBuilder[HostnameToken];
            var port = connectionStringBuilder.ContainsKey(PortToken) ? int.Parse((string)connectionStringBuilder[PortToken]) : 8182;
            var enableSsl = connectionStringBuilder.ContainsKey(EnableSslToken) ? bool.Parse((string)connectionStringBuilder[EnableSslToken]) : false;
            var username = (string)connectionStringBuilder[UsernameToken];
            var password = (string)connectionStringBuilder[PasswordToken];
            

            return new GremlinClient(hostname, port, enableSsl, username, password, connectionString);
        }
    }
}
