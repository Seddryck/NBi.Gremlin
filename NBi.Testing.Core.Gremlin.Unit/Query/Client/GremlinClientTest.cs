using NBi.Core;
using NBi.Core.Gremlin.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Gremlin.Unit.Query.Client
{
    public class GremlinClientTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        [Test]
        public void InstantiateUnderlyingSession_CosmosDbConnectionString_ISession()
        {
            var factory = new GremlinLikeClientFactory();
            var client = factory.Instantiate($@"Hostname=your-endpoint.gremlin.cosmosdb.azure.com;port=443;EnableSsl=true;Username=/dbs/database/colls/;password=p@ssw0rd;api=gremlin");
            Assert.That(client.UnderlyingSessionType, Is.EqualTo(typeof(GremlinClientOperation)));
        }

        [Test]
        public void InstantiateCreate_CosmosDbConnectionString_ISession()
        {
            var factory = new GremlinLikeClientFactory();
            var client = factory.Instantiate($@"Hostname=your-endpoint.gremlin.cosmosdb.azure.com;port=443;EnableSsl=true;Username=/dbs/database/colls/;password=p@ssw0rd;api=gremlin");
            var underlyingSession = client.CreateNew();
            Assert.That(underlyingSession, Is.Not.Null);
            Assert.That(underlyingSession, Is.AssignableTo<GremlinClientOperation>());
        }
    }
}
