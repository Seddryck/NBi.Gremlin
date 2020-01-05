using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using NBi.Core.Gremlin.Query.Client;
using NBi.Core.Gremlin.Query.Command;
using NBi.Extensibility.Query;

namespace NBi.Testing.Core.Gremlin.Unit.Query.Command
{
    public class GremlinCommandFactoryTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        [Test]
        public void CanHandle_GremlinClient_True()
        {
            var client = new GremlinClient("host", 431, true, "user", "p@ssw0rd", string.Empty);
            var factory = new GremlinCommandFactory();
            Assert.That(factory.CanHandle(client), Is.True);
        }

        [Test]
        public void CanHandle_OtherKindOfClient_False()
        {
            var client = Mock.Of<IClient>();
            var factory = new GremlinCommandFactory();
            Assert.That(factory.CanHandle(client), Is.False);
        }

        [Test]
        public void Instantiate_GremlinClientAndQuery_CommandNotNull()
        {
            var client = new GremlinClient("host", 431, true, "user", "p@ssw0rd", string.Empty);
            var query = Mock.Of<IQuery>();
            var factory = new GremlinCommandFactory();
            var command = factory.Instantiate(client, query);
            Assert.That(command, Is.Not.Null);
        }

        [Test]
        public void Instantiate_GremlinClientAndQuery_CommandImplementationCorrectType()
        {
            var client = new GremlinClient("host", 431, true, "user", "p@ssw0rd", string.Empty);
            var query = Mock.Of<IQuery>();
            var factory = new GremlinCommandFactory();
            var command = factory.Instantiate(client, query);
            var impl = command.Implementation;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.TypeOf<GremlinCommandOperation>());
        }

        [Test]
        public void Instantiate_GremlinClientAndQuery_ClientCorrectType()
        {
            var client = new GremlinClient("host", 431, true, "user", "p@ssw0rd", string.Empty);
            var query = Mock.Of<IQuery>();
            var factory = new GremlinCommandFactory();
            var command = factory.Instantiate(client, query);
            var impl = command.Client;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.InstanceOf<GremlinClientOperation>());
        }
    }
}
