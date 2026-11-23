using Kitpymes.Core.Entities.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kitpymes.Core.Entities.Tests.ValueObjects
{
    [TestClass]
    public class ValueObjectBaseTests
    {
        [TestMethod]
        public void Equals_Different_Type_Returns_False_Without_Throwing()
        {
            ValueObjectBase name = Name.Create("Ana");
            ValueObjectBase subdomain = Subdomain.Create("Ana");

            var equals = name.Equals(subdomain);
            var operatorEquals = name == subdomain;
            var operatorNotEquals = name != subdomain;

            Assert.IsFalse(equals);
            Assert.IsFalse(operatorEquals);
            Assert.IsTrue(operatorNotEquals);
        }

        [TestMethod]
        public void Equals_Null_Returns_False()
        {
            var name = Name.Create("Ana");

            var equals = name.Equals(null);
            var operatorEquals = name == null;
            var reverseEquals = null == name;

            Assert.IsFalse(equals);
            Assert.IsFalse(operatorEquals);
            Assert.IsFalse(reverseEquals);
        }

        [TestMethod]
        public void Operators_With_Both_Null_Return_Equal()
        {
            Name? left = null;
            Name? right = null;

            var operatorEquals = left == right;
            var operatorNotEquals = left != right;

            Assert.IsTrue(operatorEquals);
            Assert.IsFalse(operatorNotEquals);
        }

        [TestMethod]
        public void GetHashCode_Equal_Values_Are_Equal()
        {
            var left = Address.Create("Mayor", 1, "03580", "Busot", "Alicante", "España");
            var right = Address.Create("Mayor", 1, "03580", "Busot", "Alicante", "España");
            var other = Address.Create("Mayor", 2, "03580", "Busot", "Alicante", "España");

            Assert.AreEqual(left.GetHashCode(), right.GetHashCode());
            Assert.AreNotEqual(left.GetHashCode(), other.GetHashCode());
        }
    }
}
