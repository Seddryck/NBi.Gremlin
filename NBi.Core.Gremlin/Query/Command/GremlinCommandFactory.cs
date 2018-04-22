using NBi.Core.Query;
using NBi.Core.Query.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using NBi.Core.Gremlin.Query.Client;
using NBi.Core.Query.Client;

namespace NBi.Core.Gremlin.Query.Command
{
    class GremlinCommandFactory : ICommandFactory
    {
        public bool CanHandle(IClient client) => client is GremlinClient;

        public ICommand Instantiate(IClient client, IQuery query)
        {
            if (!CanHandle(client))
                throw new ArgumentException();
            var clientOperation = (GremlinClientOperation)client.CreateNew();
            var commandOperation = BuildCommandOperation(clientOperation, query);
            return OnInstantiate(clientOperation, commandOperation);
        }

        protected ICommand OnInstantiate(GremlinClientOperation clientOperation, GremlinCommandOperation commandOperation)
            => new GremlinCommand(clientOperation, commandOperation);

        protected GremlinCommandOperation BuildCommandOperation(GremlinClientOperation clientOperation, IQuery query)
        {
            var statementText = query.Statement;

            if (query.TemplateTokens != null && query.TemplateTokens.Count() > 0)
                statementText = ApplyVariablesToTemplate(query.Statement, query.TemplateTokens);

            return new GremlinCommandOperation(clientOperation, statementText);
        }

        private string ApplyVariablesToTemplate(string template, IEnumerable<IQueryTemplateVariable> variables)
        {
            var templateEngine = new StringTemplateEngine(template, variables);
            return templateEngine.Build();
        }

    }
}
