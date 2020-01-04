using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NUnit.Framework;
using Moq;
using NBi.Core.Gremlin.Query.Client;
using NBi.Core.Gremlin.Query.Command;

namespace NBi.Testing.Core.Gremlin.Integration.Query.Command
{
    [TestFixture]
    public class GremlinCommandFactoryTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        //[Test]
        //public void Instantiate_NoParameter_CorrectResultSet()
        //{
        //    GremlinClient gremlinClient = new GremlinClientFactory().Instantiate(ConnectionStringReader.GetLocaleGremlin()) as GremlinClient;
        //    var query = Mock.Of<IQuery>(
        //        x => x.Statement == "g.V()"
        //        );
        //    var factory = new GremlinCommandFactory();
        //    var gremlinQuery = (factory.Instantiate(gremlinClient, query).Implementation) as GremlinCommandOperation;
        //    var statement = gremlinQuery.PreparedStatement;

        //    var client = gremlinClient.
        //    var results = client.Run(statement);
        //    Assert.That(results.Count, Is.EqualTo(4));
        //}
    }
}
