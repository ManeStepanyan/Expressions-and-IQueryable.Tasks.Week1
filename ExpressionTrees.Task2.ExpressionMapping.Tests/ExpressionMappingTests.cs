using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

        [TestMethod]
        public void TestMethod1()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Bar, Foo>();

            var res = mapper.Map(new Bar()
            {
                Bar1 = "foo1",
                Bar2 = 2
            });
            Assert.AreEqual(res.Foo1, "foo1");
            Assert.AreEqual(res.Foo2, "2");
        }
    }
}
