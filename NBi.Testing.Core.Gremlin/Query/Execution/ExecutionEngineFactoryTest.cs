using Moq;
using NBi.Core.Gremlin.Query.Command;
using NBi.Core.Gremlin.Query.Execution;
using NBi.Core.Gremlin.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using NBi.Core.Configuration;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Client;
using NBi.Core.Query.Command;

namespace NBi.Testing.Core.Gremlin.Unit.Query.Execution
{
    public class ExecutionEngineFactoryTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        private class GremlinConfig : IExtensionsConfiguration
        {
            public IReadOnlyDictionary<Type, IDictionary<string, string>> Extensions =>
                new Dictionary<Type, IDictionary<string, string>>()
                {
                    { typeof(GremlinClientFactory),   new Dictionary<string, string>() },
                    { typeof(GremlinCommandFactory),  new Dictionary<string, string>() },
                    { typeof(GremlinExecutionEngine), new Dictionary<string, string>() },
                };
        }

        [Test]
        public void Instantiate_GremlinConnectionString_GremlinExecutionEngine()
        {
            var config = new GremlinConfig();
            var clientProvider = new ClientProvider(config);
            var commandProvider = new CommandProvider(config);
            var factory = new ExecutionEngineFactory(clientProvider, commandProvider, config);

            var query = Mock.Of<IQuery>
                (
                    x => x.ConnectionString == $@"Hostname=your-endpoint.gremlin.cosmosdb.azure.com;port=443;EnableSsl=true;Username=/dbs/database/colls/;password=p@ssw0rd;api=gremlin"
                    && x.Statement == "g.V().Count()"
                );

            var engine = factory.Instantiate(query);
            Assert.That(engine, Is.Not.Null);
            Assert.That(engine, Is.TypeOf<GremlinExecutionEngine>());
        }

    }
}
