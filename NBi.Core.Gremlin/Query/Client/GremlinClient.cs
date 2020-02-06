using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Gremlin.Query.Client
{
    class GremlinClient : Extensibility.Query.IClient
    {
        public string Hostname { get; }
        public int Port { get; }
        public bool EnableSsl { get; }
        public string Username { get; }
        public string Password { get; }
        public string ConnectionString { get; }

        public Type UnderlyingSessionType { get => typeof(GremlinClientOperation); }

        public virtual object CreateNew() => CreateClientOperation();
        private GremlinClientOperation CreateClientOperation()
            => new GremlinClientOperation(Hostname, Port, EnableSsl, Username, Password);

        internal GremlinClient(string hostname, int port, bool enableSsl, string username, string password, string connectionString)
            => (Hostname, Port, EnableSsl, Username, Password, ConnectionString)
                = (hostname, port, enableSsl, username, password, connectionString);
    }
}
