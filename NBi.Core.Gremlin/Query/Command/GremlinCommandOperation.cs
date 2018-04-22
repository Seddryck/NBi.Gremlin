using NBi.Core.Gremlin.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Gremlin.Query.Command
{
    class GremlinCommandOperation
    {
        public string PreparedStatement { get; }
        public GremlinClientOperation Client { get; }
        public GremlinCommandOperation(GremlinClientOperation client, string preparedStatement)
        {
            Client = client;
            PreparedStatement = preparedStatement;
        }
    }
}
