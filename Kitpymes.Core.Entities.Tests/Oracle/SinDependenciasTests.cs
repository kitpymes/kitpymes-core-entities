using Kitpymes.Core.Entities.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kitpymes.Core.Entities.Tests.Oracle
{
    [TestClass]
    public class SinDependenciasTests
    {
        // Entities es dominio: no puede depender de ASP.NET Core, de Shared (que lo trae) ni de MediatR.
        [TestMethod]
        [DataRow("Kitpymes.Core.Shared")]
        [DataRow("MediatR")]
        [DataRow("Microsoft.AspNetCore")]
        public void Entities_Does_Not_Reference(string prefix)
        {
            var references = typeof(EntityBaseInt).Assembly.GetReferencedAssemblies().Select(a => a.Name ?? string.Empty).ToList();

            Assert.IsFalse(references.Any(r => r.StartsWith(prefix, System.StringComparison.Ordinal)), string.Join(", ", references));
        }

        [TestMethod]
        public void Entities_References_Mediator_Abstractions()
        {
            var references = typeof(Name).Assembly.GetReferencedAssemblies().Select(a => a.Name).ToList();

            CollectionAssert.Contains(references, "Kitpymes.Core.Mediator.Abstractions");
        }
    }
}
