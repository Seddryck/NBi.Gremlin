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
        protected string GremlinLikeConnectionString = @"
                Hostname=my-endpoint.documents.azure.com;
                Port=443;
                EnableSsl=true;
                Username=/dbs/myDatabase/colls/myCollection;
                Password=xLNOQPTaBpDrCxMoUTFVavrtnjkpSw5qaVbbxSazCtKNFopFjAZE9aeLKAdCuiQSqTUaRhffFGrJKJA89GTsdg==;
                Api=gremlin;
            ";

        protected string GremlinShortConnectionString = @"
                Hostname=localhost;
                Username=admin;
                Password=p@ssw0rd;
                Api=gremlin;
            ";

        [Test]
        public void CanHandle_GremlinLikeConnectionString_True()
        {
            var factory = new GremlinLikeClientFactory();
            Assert.That(factory.CanHandle(GremlinLikeConnectionString), Is.True);
        }

        [Test]
        public void CanHandle_OleDbConnectionString_False()
        {
            var factory = new GremlinLikeClientFactory();
            Assert.That(factory.CanHandle("data source=SERVER;initial catalog=DB;IntegratedSecurity=true;Provider=OLEDB.1"), Is.False);
        }

        [Test]
        public void Instantiate_GremlinConnectionString_GremlinClient()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate($@"Hostname=your-endpoint.gremlin.cosmosdb.azure.com;port=443;EnableSsl=true;Username=/dbs/database/colls/collection;password=p@ssw0rd;api=gremlin");
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.TypeOf<GremlinClient>());
        }

        [Test]
        public void Instantiate_GremlinLikeConnectionString_CorrectHostName()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinLikeConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Hostname, Is.EqualTo("my-endpoint.documents.azure.com"));
        }

        [Test]
        public void Instantiate_GremlinLikeConnectionString_CorrectPort()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinLikeConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Port, Is.EqualTo(443));
        }

        [Test]
        public void Instantiate_GremlinLikeConnectionString_CorrectEnableSsl()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinLikeConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.EnableSsl, Is.EqualTo(true));
        }

        [Test]
        public void Instantiate_GremlinLikeConnectionString_CorrectPassword()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinLikeConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Password, Is.EqualTo("xLNOQPTaBpDrCxMoUTFVavrtnjkpSw5qaVbbxSazCtKNFopFjAZE9aeLKAdCuiQSqTUaRhffFGrJKJA89GTsdg=="));
        }

        [Test]
        public void Instantiate_GremlinLikeConnectionString_CorrectUsername()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinLikeConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Username, Is.EqualTo("/dbs/myDatabase/colls/myCollection"));
        }

        [Test]
        public void CanHandle_GremlinShortConnectionString_True()
        {
            var factory = new GremlinLikeClientFactory();
            Assert.That(factory.CanHandle(GremlinShortConnectionString), Is.True);
        }

        [Test]
        public void Instantiate_GremlinShortConnectionString_CorrectHostName()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinShortConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Hostname, Is.EqualTo("localhost"));
        }

        [Test]
        public void Instantiate_GremlinShortConnectionString_CorrectPort()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinShortConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Port, Is.EqualTo(8182));
        }

        [Test]
        public void Instantiate_GremlinShortConnectionString_CorrectEnableSsl()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinShortConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.EnableSsl, Is.EqualTo(false));
        }

        [Test]
        public void Instantiate_GremlinShortConnectionString_CorrectPassword()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinShortConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Password, Is.EqualTo("p@ssw0rd"));
        }

        [Test]
        public void Instantiate_GremlinShortConnectionString_CorrectUsername()
        {
            var factory = new GremlinLikeClientFactory();
            var session = factory.Instantiate(GremlinShortConnectionString);
            var gremlinClient = session as GremlinClient;
            Assert.That(gremlinClient.Username, Is.EqualTo("admin"));
        }
    }
}
