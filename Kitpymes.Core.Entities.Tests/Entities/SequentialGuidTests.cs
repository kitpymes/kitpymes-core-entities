using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace Kitpymes.Core.Entities.Tests.Entities
{
    [TestClass]
    public class SequentialGuidTests
    {
        private sealed class Invoice : EntityBaseGuidSequencial { }

        // SqlGuid compara en el mismo orden que SQL Server ordena los uniqueidentifier.
        [TestMethod]
        public void Consecutive_Keys_Are_Ordered_As_In_Sql_Server()
        {
            var ids = Enumerable.Range(0, 10000).Select(_ => new SqlGuid(new Invoice().Id)).ToList();

            for (var i = 1; i < ids.Count; i++)
            {
                var ordered = ids[i - 1].CompareTo(ids[i]) < 0;

                Assert.IsTrue(ordered, $"La clave {i} ({ids[i]}) no es mayor que la anterior ({ids[i - 1]}).");
            }
        }

        [TestMethod]
        public void Keys_Created_In_Parallel_Are_Distinct_In_Sql_Server_Order()
        {
            var ids = new SqlGuid[8000];

            Parallel.For(0, ids.Length, i => ids[i] = new SqlGuid(new Invoice().Id));

            Assert.AreEqual(ids.Length, ids.Distinct().Count());
        }
    }
}
