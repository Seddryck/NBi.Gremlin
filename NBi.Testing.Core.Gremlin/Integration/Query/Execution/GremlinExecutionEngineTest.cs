using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Data;
using NBi.Core.Gremlin.Query.Client;
using NBi.Core.Gremlin.Query.Command;
using NBi.Core.Gremlin.Query.Execution;
using Moq;
using NBi.Extensibility.Query;

namespace NBi.Testing.Core.Gremlin.Integration.Query.Execution
{
    [TestFixture]
    public class GremlinExecutionEngineTest
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

        [Test]
        [Category("OnlyCosmosDb")]
        public void Execute_Vertex_DataSetFilled()
        {
            var client = new GremlinLikeClientFactory().Instantiate(ConnectionStringReader.GetGremlin());
            var statement = Mock.Of<IQuery>(x => x.Statement == "g.V()");
            GremlinCommandOperation gremlinQuery = new GremlinCommandFactory().Instantiate(client, statement).Implementation as GremlinCommandOperation;

            var engine = new GremlinExecutionEngine((GremlinClientOperation)(client.CreateNew()), gremlinQuery);

            var ds = engine.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(4));

            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("id"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("label"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("type"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("firstName"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("age"));

            var firstNames = new List<object>();
            var ages = new List<object>();
            foreach (DataRow row in ds.Tables[0].Rows)
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (row.Table.Columns[i].ColumnName == "firstName")
                        firstNames.Add(row.ItemArray[i]);
                    else if (row.Table.Columns[i].ColumnName == "age")
                        ages.Add(row.ItemArray[i]);
                }

            foreach (var expectedFirstName in new[] { "Thomas", "Mary", "Ben", "Robin" })
                Assert.That(firstNames, Has.Member(expectedFirstName));

            foreach (var expectedAge in new object[] { 44, DBNull.Value, 39 })
                Assert.That(ages, Has.Member(expectedAge));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Assert.That(row["id"], Is.Not.Null.Or.Empty);
                Assert.That(row["label"], Is.EqualTo("person"));
                Assert.That(row["type"], Is.EqualTo("vertex"));
            }

        }

        [Test]
        public void Execute_Edge_DataSetFilled()
        {
            var client = new GremlinLikeClientFactory().Instantiate(ConnectionStringReader.GetGremlin());
            var statement = Mock.Of<IQuery>(x => x.Statement == "g.E()");
            var gremlinQuery = new GremlinCommandFactory().Instantiate(client, statement).Implementation as GremlinCommandOperation;

            var engine = new GremlinExecutionEngine((GremlinClientOperation)(client.CreateNew()), gremlinQuery);

            var ds = engine.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(3));

            Assert.That(ds.Tables[0].Columns, Has.Count.EqualTo(7));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("id"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("label"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("type"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("inV"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("outV"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("inVLabel"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("outVLabel"));

            var outVs = new List<object>();
            var inVs = new List<object>();
            foreach (DataRow row in ds.Tables[0].Rows)
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (row.Table.Columns[i].ColumnName == "outV")
                        outVs.Add(row.ItemArray[i]);
                    else if (row.Table.Columns[i].ColumnName == "inV")
                        inVs.Add(row.ItemArray[i]);
                }

            Assert.That(inVs, Has.Count.EqualTo(3));
            Assert.That(outVs, Has.Count.EqualTo(3));
            Assert.That(outVs.Distinct().Count(), Is.EqualTo(2));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Assert.That(row["id"], Is.Not.Null.Or.Empty);
                Assert.That(row["inVLabel"], Is.EqualTo("person"));
                Assert.That(row["outVLabel"], Is.EqualTo("person"));
                Assert.That(row["label"], Is.EqualTo("knows"));
                Assert.That(row["type"], Is.EqualTo("edge"));
            }
        }

        [Test]
        public void Execute_ProjectionOfObjects_DataSetFilled()
        {
            var client = new GremlinLikeClientFactory().Instantiate(ConnectionStringReader.GetGremlin());
            var statement = Mock.Of<IQuery>(x => x.Statement == "g.V().project('FirstName','KnowsCount').by('firstName').by(out().count())");
            var gremlinQuery = new GremlinCommandFactory().Instantiate(client, statement).Implementation as GremlinCommandOperation;

            var engine = new GremlinExecutionEngine((GremlinClientOperation)(client.CreateNew()), gremlinQuery);

            var ds = engine.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(4));

            Assert.That(ds.Tables[0].Columns, Has.Count.EqualTo(2));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("FirstName"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("KnowsCount"));

            var firstNames = new List<object>();
            var knowsCount = new List<object>();
            foreach (DataRow row in ds.Tables[0].Rows)
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (row.Table.Columns[i].ColumnName == "FirstName")
                        firstNames.Add(row.ItemArray[i]);
                    else if (row.Table.Columns[i].ColumnName == "KnowsCount")
                        knowsCount.Add(row.ItemArray[i]);
                }

            foreach (var expectedFirstName in new[] { "Thomas", "Mary", "Ben", "Robin" })
                Assert.That(firstNames, Has.Member(expectedFirstName));

            foreach (var expectedKnowsCount in new object[] { 2, 1, 0 })
                Assert.That(knowsCount, Has.Member(expectedKnowsCount));
        }

        [Test]
        public void Execute_Integer_ScalarReturned()
        {
            var client = new GremlinLikeClientFactory().Instantiate(ConnectionStringReader.GetGremlin());
            var statement = Mock.Of<IQuery>(x => x.Statement == "g.V().count()");
            var gremlinQuery = new GremlinCommandFactory().Instantiate(client, statement).Implementation as GremlinCommandOperation;

            var engine = new GremlinExecutionEngine((GremlinClientOperation)(client.CreateNew()), gremlinQuery);

            var count = engine.ExecuteScalar();
            Assert.That(count, Is.EqualTo(4));
        }

        [Test]
        public void Execute_String_ScalarReturned()
        {
            var client = new GremlinLikeClientFactory().Instantiate(ConnectionStringReader.GetGremlin());
            var statement = Mock.Of<IQuery>(x => x.Statement == "g.V().has('firstName','Mary').values('lastName')");
            var gremlinQuery = new GremlinCommandFactory().Instantiate(client, statement).Implementation as GremlinCommandOperation;

            var engine = new GremlinExecutionEngine((GremlinClientOperation)(client.CreateNew()), gremlinQuery);

            var count = engine.ExecuteScalar();
            Assert.That(count, Is.EqualTo("Andersen"));
        }

        [Test]
        public void Execute_NullString_ScalarReturned()
        {
            var client = new GremlinLikeClientFactory().Instantiate(ConnectionStringReader.GetGremlin());
            var statement = Mock.Of<IQuery>(x => x.Statement == "g.V().has('firstName','Thomas').values('lastName')");
            GremlinCommandOperation commandOperation = new GremlinCommandFactory().Instantiate(client, statement).Implementation as GremlinCommandOperation;

            var engine = new GremlinExecutionEngine((GremlinClientOperation)(client.CreateNew()), commandOperation);

            var count = engine.ExecuteScalar();
            Assert.That(count, Is.Null);
        }

        [Test]
        public void Execute_ListOfString_ListReturned()
        {
            var client = new GremlinLikeClientFactory().Instantiate(ConnectionStringReader.GetGremlin());
            var statement = Mock.Of<IQuery>(x => x.Statement == "g.V().values('lastName')");
            var gremlinQuery = new GremlinCommandFactory().Instantiate(client, statement).Implementation as GremlinCommandOperation;

            var engine = new GremlinExecutionEngine((GremlinClientOperation)(client.CreateNew()), gremlinQuery);

            var count = engine.ExecuteList<string>();
            Assert.That(count, Has.Member("Andersen"));
            Assert.That(count, Has.Member("Miller"));
            Assert.That(count, Has.Member("Wakefield"));
        }

    }
}
