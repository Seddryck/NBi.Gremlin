using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using NBi.Core.Gremlin.Query.Client;
using NBi.Extensibility.Query;
using NBi.Extensibility;

namespace NBi.Core.Gremlin.Query.Command
{
    class GremlinCommandFactory : ICommandFactory
    {
        public bool CanHandle(IClient client) => client is GremlinClient;

        public ICommand Instantiate(IClient client, IQuery query)
            => Instantiate(client, query, null);
        public ICommand Instantiate(IClient client, IQuery query, ITemplateEngine engine)
        {
            if (!CanHandle(client))
                throw new ArgumentException();
            var clientOperation = (GremlinClientOperation)client.CreateNew();
            var commandOperation = BuildCommandOperation(clientOperation, query, engine);
            return OnInstantiate(clientOperation, commandOperation);
        }

        protected ICommand OnInstantiate(GremlinClientOperation clientOperation, GremlinCommandOperation commandOperation)
            => new GremlinCommand(clientOperation, commandOperation);

        protected GremlinCommandOperation BuildCommandOperation(GremlinClientOperation clientOperation, IQuery query, ITemplateEngine engine)
        {
            var statementText = query.Statement;

            if (query.TemplateTokens != null && query.TemplateTokens.Count() > 0 && engine!=null)
                statementText = ApplyVariablesToTemplate(engine, query.Statement, query.TemplateTokens);

            return new GremlinCommandOperation(clientOperation, statementText);
        }

        private string ApplyVariablesToTemplate(ITemplateEngine engine, string template, IEnumerable<IQueryTemplateVariable> variables)
        {
            var valuePairs = new List<KeyValuePair<string, object>>();
            foreach (var variable in variables)
                valuePairs.Add(new KeyValuePair<string, object>(variable.Name, variable.Value));
            return engine.Render(template, valuePairs);
        }

    }
}
