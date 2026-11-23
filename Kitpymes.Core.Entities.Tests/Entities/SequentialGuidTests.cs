using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlTypes;
using System.Threading;
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

        // El contador va en los 8 últimos bytes: 10-15 los 6 más significativos y 8-9 los 2 menos.
        private static long Counter(Guid id)
        {
            var b = id.ToByteArray();
            var counter = new byte[] { b[9], b[8], b[15], b[14], b[13], b[12], b[11], b[10] };

            return BitConverter.ToInt64(counter, 0);
        }

        [TestMethod]
        public void Counter_Starts_At_The_Current_Time()
        {
            Thread.Sleep(50);
            var before = DateTime.UtcNow.Ticks;

            var counter = Counter(new Invoice().Id);

            Assert.IsTrue(counter >= before, $"El contador {counter} es anterior a {before}.");
            Assert.IsTrue(counter <= DateTime.UtcNow.Ticks + 1000000, $"El contador {counter} va muy por delante del reloj.");
        }

        [TestMethod]
        public void Counters_Created_In_Parallel_Are_Distinct()
        {
            var counters = new long[8000];

            Parallel.For(0, counters.Length, i => counters[i] = Counter(new Invoice().Id));

            Assert.AreEqual(counters.Length, counters.Distinct().Count());
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
