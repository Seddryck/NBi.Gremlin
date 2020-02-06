using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Gremlin
{
    [SetUpFixture]
    public class TestSuiteSetup
    {
        private string[] Statements
        {
            get => new[]
            {
                "g.V().drop()"
                , "g.addV('person').property('pk', 1).property('id', 'thomas').property('firstName', 'Thomas').property('age', 44)"
                , "g.addV('person').property('pk', 1).property('id', 'mary').property('firstName', 'Mary').property('lastName', 'Andersen').property('age', 39)"
                , "g.addV('person').property('pk', 1).property('id', 'ben').property('firstName', 'Ben').property('lastName', 'Miller')"
                , "g.addV('person').property('pk', 1).property('id', 'robin').property('firstName', 'Robin').property('lastName', 'Wakefield')"
                , "g.V().has('firstName','Thomas').addE('knows').to(g.V().has('firstName','Mary'))"
                , "g.V().has('firstName','Thomas').addE('knows').to(g.V().has('firstName','Ben'))"
                , "g.V().has('firstName','Ben').addE('knows').to(g.V().has('firstName','Robin'))"
            };
        }

        [SetUp]
        public virtual void Init()
        {
            if (!string.IsNullOrEmpty(ConnectionStringReader.GetCosmosDb()))
            {
                var cosmosdbConnectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = ConnectionStringReader.GetCosmosDb() };
                var endpoint = new Uri(cosmosdbConnectionStringBuilder["endpoint"].ToString());
                var authKey = cosmosdbConnectionStringBuilder["authkey"].ToString();
                var databaseId = cosmosdbConnectionStringBuilder["database"].ToString();
                var collectionId = cosmosdbConnectionStringBuilder["collection"].ToString();

                using (var client = new DocumentClient(endpoint, authKey))
                {
                    var database = new Database() { Id = databaseId };
                    var databaseResponse = client.CreateDatabaseIfNotExistsAsync(database).Result;
                    switch (databaseResponse.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            Console.WriteLine($"Database {databaseId} already exists.");
                            break;
                        case System.Net.HttpStatusCode.Created:
                            Console.WriteLine($"Database {databaseId} created.");
                            break;
                        default:
                            throw new ArgumentException($"Can't create database {databaseId}: {databaseResponse.StatusCode}");
                    }

                    var databaseUri = UriFactory.CreateDatabaseUri(databaseId);
                    var collectionResponse = client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection() { Id = collectionId }).Result;
                    switch (collectionResponse.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            Console.WriteLine($"Collection {collectionId} already exists.");
                            break;
                        case System.Net.HttpStatusCode.Created:
                            Console.WriteLine($"Database {collectionId} created.");
                            break;
                        default:
                            throw new ArgumentException($"Can't create database {collectionId}: {collectionResponse.StatusCode}");
                    }
                }
            }
            var gremlinConnectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = ConnectionStringReader.GetGremlin() };

            FillDatabase(
                gremlinConnectionStringBuilder["hostname"].ToString(),
                Int32.Parse(gremlinConnectionStringBuilder["port"].ToString()),
                Boolean.Parse(gremlinConnectionStringBuilder["enablessl"].ToString()),
                gremlinConnectionStringBuilder["username"].ToString(),
                gremlinConnectionStringBuilder["password"].ToString()
            );
        }

        private void FillDatabase(string hostname, int port, bool enableSsl, string username, string password)
        {
            var gremlinServer = new GremlinServer(hostname, port, enableSsl, username, password);

            using (var gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType))
            {
                var task = gremlinClient.SubmitWithSingleResultAsync<Int64>("g.V().count()");
                task.Wait();
                if (task.Result != 4)
                {
                    foreach (var statement in Statements)
                    {
                        var query = gremlinClient.SubmitAsync(statement);
                        Console.WriteLine($"Setup database: { statement }");
                        query.Wait();
                    }
                }
            }

            Console.WriteLine($"Tests will be run on '{hostname}:{port}'");
        }
    }
}
