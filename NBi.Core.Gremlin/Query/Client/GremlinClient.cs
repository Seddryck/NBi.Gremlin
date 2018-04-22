using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Gremlin.Query.Client
{
    class GremlinClient : NBi.Core.Query.Client.IClient
    {
        public const string HostnameToken = "hostname";
        public const string PortToken = "port";
        public const string EnableSslToken = "enablessl";
        public const string UsernameToken = "username";
        public const string PasswordToken = "password";
        public const string ApiToken = "api";

        protected string Hostname { get => (string)ConnectionStringTokens[HostnameToken]; }
        protected int Port { get => Int32.Parse((string)ConnectionStringTokens[PortToken]); }
        protected bool EnableSsl { get => Boolean.Parse((string)ConnectionStringTokens[EnableSslToken]); }
        protected string Username { get => (string)ConnectionStringTokens[UsernameToken]; }
        protected string Password { get => (string)ConnectionStringTokens[PasswordToken]; }
        
        protected DbConnectionStringBuilder ConnectionStringTokens => new DbConnectionStringBuilder() { ConnectionString = ConnectionString };

        public string ConnectionString { get; }
        public Type UnderlyingSessionType { get => typeof(GremlinClientOperation); }

        public virtual object CreateNew() => CreateClientOperation();
        private GremlinClientOperation CreateClientOperation()
            =>  new GremlinClientOperation(Hostname, Port, EnableSsl, Username, Password);

        internal GremlinClient(string hostname, int port, bool enableSsl, string username, string password)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder
            {
                { HostnameToken, hostname },
                { PortToken, port.ToString() },
                { EnableSslToken, enableSsl.ToString() },
                { UsernameToken, username },
                { PasswordToken, password}
            };
            ConnectionString = connectionStringBuilder.ToString();
        }
    }
}
