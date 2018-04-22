using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Gremlin.Query.Client;

namespace NBi.Testing.Core.Gremlin.Unit.Query.Client
{
    public class GremlinClientFactoryTest
    {
        [Test]
        public void CanHandle_GremlinWithApi_True()
        {
            var factory = new GremlinClientFactory();
            Assert.That(factory.CanHandle($@"Hostname=your-endpoint.gremlin.cosmosdb.azure.com;port=443;EnableSsl=true;Username=/dbs/database/colls/collection;password=p@ssw0rd;api=gremlin"), Is.True);
        }

        [Test]
        public void CanHandle_OleDbConnectionString_False()
        {
            var factory = new GremlinClientFactory();
            Assert.That(factory.CanHandle("data source=SERVER;initial catalog=DB;IntegratedSecurity=true;Provider=OLEDB.1"), Is.False);
        }

        [Test]
        public void Instantiate_GremlinConnectionString_GremlinClient()
        {
            var factory = new GremlinClientFactory();
            var session = factory.Instantiate($@"Hostname=your-endpoint.gremlin.cosmosdb.azure.com;port=443;EnableSsl=true;Username=/dbs/database/colls/collection;password=p@ssw0rd;api=gremlin");
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.TypeOf<GremlinClient>());
        }

        
    }
}
