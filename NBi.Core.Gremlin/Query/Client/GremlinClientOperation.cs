using Driver = Gremlin.Net.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace NBi.Core.Gremlin.Query.Client
{
    class GremlinClientOperation
    {
        private readonly Driver.GremlinClient gremlinClient;

        public GremlinClientOperation(string hostname, int port, bool enableSsl, string username, string password)
        {
            var gremlinServer = new Driver.GremlinServer(hostname, port, enableSsl, username, password);
            gremlinClient = new Driver.GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), Driver.GremlinClient.GraphSON2MimeType);
        }

        public Task<IReadOnlyCollection<dynamic>> Execute(string statement)
        {
            return gremlinClient.SubmitAsync<dynamic>(statement);           
        }

    }
}
    