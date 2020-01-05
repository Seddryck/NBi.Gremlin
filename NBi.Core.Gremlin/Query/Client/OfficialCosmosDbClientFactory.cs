using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Gremlin.Query.Client
{
    public class OfficialCosmosDbClientFactory : Extensibility.Query.IClientFactory
    {
        public const string AccountEndpointToken = "accountendpoint";
        public const string AccountKeyToken = "accountkey";
        public const string DatabaseToken = "database";
        public const string CollectionToken = "collection";
        public const string ApiKindToken = "apikind";

        public bool CanHandle(string connectionString)
        
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            return connectionStringBuilder.ContainsKey(AccountEndpointToken)
                && connectionStringBuilder.ContainsKey(AccountKeyToken)
                && connectionStringBuilder.ContainsKey(DatabaseToken)
                && connectionStringBuilder.ContainsKey(CollectionToken)
                && HasApiSetTo(connectionStringBuilder, "gremlin");
        }


        protected bool HasApiSetTo(DbConnectionStringBuilder connectionStringBuilder, string api)
            => connectionStringBuilder.ContainsKey(ApiKindToken) && ((string)connectionStringBuilder[ApiKindToken]).ToLowerInvariant() == api.ToLowerInvariant();


        public Extensibility.Query.IClient Instantiate(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var uri = new Uri((string)connectionStringBuilder[AccountEndpointToken]);
            var hostname = uri.Host;
            var port = uri.Port;
            var enableSsl = uri.Scheme=="https" ;
            var username = $"/dbs/{(string)connectionStringBuilder[DatabaseToken]}/colls/{(string)connectionStringBuilder[CollectionToken]}";
            var password = (string)connectionStringBuilder[AccountKeyToken];
            
            return new GremlinClient(hostname, port, enableSsl, username, password, connectionString);
        }
    }
}
