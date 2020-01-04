using NBi.Core.Gremlin.Query.Client;
using NBi.Extensibility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Gremlin.Query.Command
{
    public class GremlinCommand : ICommand
    {
        public object Implementation { get; }
        public object Client { get; }

        internal GremlinCommand(GremlinClientOperation client, GremlinCommandOperation query)
        {
            Client = client;
            Implementation = query;
        }
    }
}
