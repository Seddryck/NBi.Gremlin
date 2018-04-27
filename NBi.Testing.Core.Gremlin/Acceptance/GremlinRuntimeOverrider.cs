using NUnit.Framework;
using NBi.Testing.Acceptance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Gremlin.Acceptance
{
    public class CosmosDbRuntimeOverrider : BaseRuntimeOverrider
    {
        [SetUp]
        public void Setup()
        {
            CopyConnectionStringUserConfig();
        }

        public void CopyConnectionStringUserConfig()
        {
            var fileName = "ConnectionString.user.config";
            if (System.IO.Directory.Exists($@"Acceptance\Resources\Positive\"))
                System.IO.File.Copy(fileName, $@"Acceptance\Resources\Positive\{fileName}", true);
        }


        [Test]
        [TestCase(@"ResultSetEqualToResultSet.nbits")]
        public override void RunPositiveTestSuiteWithConfig(string filename) => base.RunPositiveTestSuiteWithConfig(filename);

        public override void RunIgnoredTests(string filename) => throw new NotImplementedException();

        public override void RunNegativeTestSuite(string filename) => throw new NotImplementedException();

        public override void RunNegativeTestSuiteWithConfig(string filename) => throw new NotImplementedException();

        public override void RunPositiveTestSuite(string filename) => throw new NotImplementedException();
    }
}
