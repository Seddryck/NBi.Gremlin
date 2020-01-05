using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Gremlin.Query.Client;

namespace NBi.Testing.Core.Gremlin.Unit.Query.Client
{
    public class OfficialCosmosDbClientFactoryTest
    {
        protected string OfficialConnectionString = @"
                AccountEndpoint=https://my-endpoint.documents.azure.com:443/;
                Database=myDatabase;
                Collection=myCollection;
                AccountKey=xLNOQPTaBpDrCxMoUTFVavrtnjkpSw5qaVbbxSazCtKNFopFjAZE9aeLKAdCuiQSqTUaRhffFGrJKJA89GTsdg==;
                ApiKind=Gremlin;
            ";

        [Test]
        public void CanHandle_OfficialConnectionString_True()
        {
            var factory = new OfficialCosmosDbClientFactory();
            Assert.That(factory.CanHandle(OfficialConnectionString), Is.True);
        }

        [Test]
        public void CanHandle_OleDbConnectionString_False()
        {
            var factory = new OfficialCosmosDbClientFactory();
            Assert.That(factory.CanHandle("data source=SERVER;initial catalog=DB;IntegratedSecurity=true;Provider=OLEDB.1"), Is.False);
        }

        [Test]
        public void Instantiate_OfficialConnectionString_GremlinClient()
        {
            var factory = new OfficialCosmosDbClientFactory();
            var session = factory.Instantiate(OfficialConnectionString);
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.TypeOf<GremlinClient>());
        }

        [Test]
        public void Instantiate_OfficialConnectionString_CorrectHostName()
        {
            var factory = new OfficialCosmosDbClientFactory();
            var session = factory.Instantiate(OfficialConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Hostname, Is.EqualTo("my-endpoint.documents.azure.com"));
        }

        [Test]
        public void Instantiate_OfficialConnectionString_CorrectPort()
        {
            var factory = new OfficialCosmosDbClientFactory();
            var session = factory.Instantiate(OfficialConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Port, Is.EqualTo(443));
        }

        [Test]
        public void Instantiate_OfficialConnectionString_CorrectEnableSsl()
        {
            var factory = new OfficialCosmosDbClientFactory();
            var session = factory.Instantiate(OfficialConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.EnableSsl, Is.EqualTo(true));
        }

        [Test]
        public void Instantiate_OfficialConnectionString_CorrectPassword()
        {
            var factory = new OfficialCosmosDbClientFactory();
            var session = factory.Instantiate(OfficialConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Password, Is.EqualTo("xLNOQPTaBpDrCxMoUTFVavrtnjkpSw5qaVbbxSazCtKNFopFjAZE9aeLKAdCuiQSqTUaRhffFGrJKJA89GTsdg=="));
        }

        [Test]
        public void Instantiate_OfficialConnectionString_CorrectUsername()
        {
            var factory = new OfficialCosmosDbClientFactory();
            var session = factory.Instantiate(OfficialConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Username, Is.EqualTo("/dbs/myDatabase/colls/myCollection"));
        }


    }
}
