using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Configuration.Extension;
using NBi.Core.Gremlin.Query.Client;

namespace NBi.Testing.Core.Gremlin.Unit.Configuration.Extension
{
    public class ExtensionAnalyzerTest
    {
        [Test]
        public void Execute_CosmosDb_Six()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Gremlin");
            Assert.That(types.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Execute_GremlinApi_IClientFactory()
        {
            var analyzer = new ExtensionAnalyzer();
            var types = analyzer.Execute("NBi.Core.Gremlin");
            Assert.That(types, Has.Member(typeof(GremlinClientFactory)));
        }

        //[Test]
        //public void Execute_GremlinApi_ICommandFactory()
        //{
        //    var analyzer = new ExtensionAnalyzer();
        //    var types = analyzer.Execute("NBi.Core.Gremlin");
        //    Assert.That(types, Has.Member(typeof(GremlinCommandFactory)));
        //}

        //[Test]
        //public void Execute_GremlinApi_IExecutionEngine()
        //{
        //    var analyzer = new ExtensionAnalyzer();
        //    var types = analyzer.Execute("NBi.Core.Gremlin");
        //    Assert.That(types, Has.Member(typeof(GremlinExecutionEngine)));
        //}
    }
}
