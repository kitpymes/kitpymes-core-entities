using Kitpymes.Core.Entities.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Kitpymes.Core.Entities.Tests.Enumerations
{
    [TestClass]
    public class EnumerationValuesTests
    {
        [TestMethod]
        public void Card_Names_ShortNames_And_IsNone()
        {
            Assert.AreEqual("American Express", CardEnum.Amex.Name);
            Assert.AreEqual("Amex", CardEnum.Amex.ShortName);
            Assert.AreEqual("Master Card", CardEnum.MasterCard.Name);
            Assert.AreEqual("MasterCard", CardEnum.MasterCard.ShortName);
            Assert.IsTrue(CardEnum.None.IsNone);
            Assert.IsFalse(CardEnum.Visa.IsNone);
        }

        [TestMethod]
        public void Gender_ShortNames_And_IsNone()
        {
            Assert.AreEqual("M", GenderEnum.Male.ShortName);
            Assert.AreEqual("F", GenderEnum.Female.ShortName);
            Assert.IsTrue(GenderEnum.None.IsNone);
            Assert.IsFalse(GenderEnum.Male.IsNone);
        }

        [TestMethod]
        public void Log_And_Subscription_IsNone()
        {
            Assert.IsTrue(LogEnum.None.IsNone);
            Assert.IsFalse(LogEnum.Created.IsNone);
            Assert.IsTrue(SubscriptionEnum.None.IsNone);
            Assert.IsFalse(SubscriptionEnum.Gold.IsNone);
        }

        [TestMethod]
        public void Status_ToEnum_From_Bool()
        {
            Assert.AreSame(StatusEnum.Active, StatusEnum.ToEnum(true));
            Assert.AreSame(StatusEnum.Inactive, StatusEnum.ToEnum(false));
        }

        [TestMethod]
        [DataRow("Active", true)]
        [DataRow("Inactive", false)]
        public void Status_ToEnum_And_ToBool_From_Valid_String(string status, bool active)
        {
            Assert.AreSame(active ? StatusEnum.Active : StatusEnum.Inactive, StatusEnum.ToEnum(status));
            Assert.AreEqual(active, StatusEnum.ToBool(status));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("active")]
        [DataRow("Otro")]
        public void Status_ToEnum_And_ToBool_From_Invalid_String_Throw_ArgumentException(string? status)
        {
            var toEnum = Assert.ThrowsExactly<ArgumentException>(() => StatusEnum.ToEnum(status));
            var toBool = Assert.ThrowsExactly<ArgumentException>(() => StatusEnum.ToBool(status));

            Assert.AreEqual("status", toEnum.ParamName);
            Assert.AreEqual("status", toBool.ParamName);
            StringAssert.StartsWith(toEnum.Message, "status not found.");
            StringAssert.StartsWith(toBool.Message, "status not found.");
        }

        [TestMethod]
        public void Status_ToBool_From_Enumeration()
        {
            Assert.IsTrue(StatusEnum.ToBool(StatusEnum.Active));
            Assert.IsFalse(StatusEnum.ToBool(StatusEnum.Inactive));
        }

        [TestMethod]
        public void Status_ToBool_From_Null_Enumeration_Throws_ArgumentNullException()
        {
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() => StatusEnum.ToBool((StatusEnum)null!));

            Assert.AreEqual("status", exception.ParamName);
        }
    }
}
