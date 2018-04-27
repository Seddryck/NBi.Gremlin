using NBi.Core.Gremlin.Query.Command;
using NBi.Core.Gremlin.Query.Client;
using NBi.Extensibility.Query;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using NBi.Extensibility;

namespace NBi.Core.Gremlin.Query.Execution
{
    [SupportedCommandType(typeof(GremlinCommandOperation))]
    internal class GremlinExecutionEngine : Extensibility.Query.IExecutionEngine
    {
        protected GremlinCommandOperation Command { get; }
        protected GremlinClientOperation Client { get; }

        private readonly Stopwatch stopWatch = new Stopwatch();

        protected internal GremlinExecutionEngine(GremlinClientOperation client, GremlinCommandOperation command)
        {
            Client = client;
            Command = command;
        }

        public DataSet Execute()
        {
            DataSet ds = null;
            StartWatch();
            ds = OnExecuteDataSet(Command.Client, Command.PreparedStatement);
            StopWatch();
            return ds;
        }

        protected DataSet OnExecuteDataSet(GremlinClientOperation client, string query)
        {
            var ds = new DataSet();
            var dt = new DataTable();
            ds.Tables.Add(dt);

            var task = client.Execute(query);
            task.Wait();

            foreach (var result in task.Result)
            {
                // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                var dico = (IDictionary<string, object>)result;
                if (dico.ContainsKey("type"))
                    AddVertexOrEdgeToDataTable(dico, ref dt);
                else
                    AddObjectToDataTable(dico, ref dt);
            }
            dt.AcceptChanges();

            return ds;
        }

        private void AddVertexOrEdgeToDataTable(IDictionary<string, object> dico, ref DataTable dt)
        {
            var dr = dt.NewRow();
            foreach (var attribute in dico.Keys)
            {
                if (attribute != "properties")
                {
                    if (!dt.Columns.Contains(attribute))
                        dt.Columns.Add(new DataColumn(attribute, typeof(object)) { DefaultValue = DBNull.Value });
                    dr[attribute] = dico[attribute];
                }
                else
                {
                    var properties = (IDictionary<string, object>)dico["properties"];
                    foreach (var property in properties.AsEnumerable())
                    {
                        if (!dt.Columns.Contains(property.Key))
                            dt.Columns.Add(new DataColumn(property.Key, typeof(object)));
                        var values = ((IEnumerable<object>)(property.Value)).FirstOrDefault();
                        var value = ((IDictionary<string, object>)values)["value"];
                        dr[property.Key] = value;
                    }
                }
            }
            dt.Rows.Add(dr);
        }

        private void AddObjectToDataTable(IDictionary<string, object> dico, ref DataTable dt)
        {
            var dr = dt.NewRow();
            foreach (var attribute in dico.Keys)
            {
                if (!dt.Columns.Contains(attribute))
                    dt.Columns.Add(new DataColumn(attribute, typeof(object)) { DefaultValue = DBNull.Value });
                dr[attribute] = dico[attribute];
            }
            dt.Rows.Add(dr);
        }

        public object ExecuteScalar()
        {
            object result = null;
            StartWatch();
            result = OnExecuteScalar(Command.Client, Command.PreparedStatement);
            StopWatch();
            return result;
        }

        public object OnExecuteScalar(GremlinClientOperation client, string query)
        {
            var ds = new DataSet();
            var dt = new DataTable();
            ds.Tables.Add(dt);

            var task = client.Execute(query);
            task.Wait();
            var result = task.Result;
            if (result!=null && result.Count>0)
                return result.First();
            return null;
        }

        public IEnumerable<T> ExecuteList<T>()
        {
            List<T> result = null;
            StartWatch();
            result = OnExecuteList<T>(Command.Client, Command.PreparedStatement);
            StopWatch();
            return result;
        }

        public List<T> OnExecuteList<T>(GremlinClientOperation client, string query)
        {
            var list = new List<T>();

            var task = client.Execute(query);
            task.Wait();

            foreach (var result in task.Result)
                list.Add(result);
            
            return list;
        }

        protected void StartWatch()
        {
            stopWatch.Restart();
        }

        protected void StopWatch()
        {
            stopWatch.Stop();
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Time needed to execute query [Gremlin]: {stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}");
        }

        protected internal TimeSpan Elapsed { get => stopWatch.Elapsed; }


    }
}
