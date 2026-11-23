using Kitpymes.Core.Entities.Enumerations;
using Kitpymes.Core.Entities.ValueObjects;
using Kitpymes.Core.Shared.Extensions;
using Kitpymes.Core.Shared.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Kitpymes.Core.Entities.Tests.Oracle
{
    // Entities 3.0.0 ya no depende de Kitpymes.Core.Shared: copia internamente lo que usaba.
    // Estos tests comparan con Shared 2.0.1 (la versión que usaba la 2.0.0): las mismas entradas se aceptan
    // o se rechazan igual y con el mismo mensaje. Sólo cambia la excepción (de SharedException a las de .NET).
    [TestClass]
    public class SharedOracleTests
    {
        private static string SharedMessage(Action action)
        {
            try
            {
                action();
            }
            catch (SharedException exception)
            {
                return exception.Message;
            }

            Assert.Fail("Shared 2.0.1 no lanzó SharedException.");
            return string.Empty;
        }

        private static void AssertSameRejection<TException>(Action entities, Action shared, string parameter)
            where TException : ArgumentException
        {
            var expected = SharedMessage(shared);

            var exception = Assert.ThrowsExactly<TException>(entities);

            Assert.AreEqual(parameter, exception.ParamName);
            Assert.AreEqual(new ArgumentException(expected, parameter).Message, exception.Message);
        }

        private static void AssertSameRejection(string? value, Action entities, Action shared, string parameter)
        {
            if (value is null)
            {
                AssertSameRejection<ArgumentNullException>(entities, shared, parameter);
            }
            else
            {
                AssertSameRejection<ArgumentException>(entities, shared, parameter);
            }
        }

        [TestMethod]
        [DataRow("ana@kitpymes.com")]
        [DataRow("a.b-c_1@d-e.es")]
        [DataRow("ana@kitpymes.co.uk")]
        [DataRow("ana@kitpymes")]
        [DataRow("ana kitpymes.com")]
        [DataRow("ana@kitpymes.comercial")]
        [DataRow("ñ@kitpymes.com")]
        [DataRow("")]
        [DataRow("   ")]
        [DataRow(null)]
        public void Email_Accepts_And_Rejects_Like_Shared(string? email)
        {
            if (email.ToIsEmail())
            {
                Assert.AreEqual(email, Email.Create(email).Value);
            }
            else
            {
                AssertSameRejection(email, () => Email.Create(email), () => email.ToThrowIfNotEmail("email"), "email");
            }
        }

        [TestMethod]
        [DataRow("Ana")]
        [DataRow("José María")]
        [DataRow("Ñandú Øster ħ")]
        [DataRow("Ana1")]
        [DataRow("O'Neil")]
        [DataRow("")]
        [DataRow(null)]
        public void Name_Accepts_And_Rejects_Like_Shared(string? name)
        {
            if (name.ToIsName())
            {
                Assert.AreEqual(name, Name.Create(name).Value);
                Assert.AreEqual(name, FullName.Create(name).FirstName);
            }
            else
            {
                AssertSameRejection(name, () => Name.Create(name), () => name.ToThrowIfNotName("name"), "name");
                AssertSameRejection(name, () => FullName.Create(name), () => name.ToThrowIfNotName("firstName"), "firstName");
            }
        }

        [TestMethod]
        [DataRow("kitpymes")]
        [DataRow("Mi-Sub.dominio_1")]
        [DataRow("con espacio")]
        [DataRow("ñ")]
        [DataRow("")]
        [DataRow(null)]
        public void Subdomain_Accepts_And_Rejects_Like_Shared(string? subdomain)
        {
            if (subdomain.ToIsSubdomain())
            {
                Assert.AreEqual(subdomain.ToLowerInvariant(), Subdomain.Create(subdomain).Value);
            }
            else
            {
                AssertSameRejection(subdomain, () => Subdomain.Create(subdomain), () => subdomain.ToThrowIfNotSubdomain("subdomain"), "subdomain");
            }
        }

        [TestMethod]
        [DataRow("José María")]
        [DataRow("  Ação Ñandú  ")]
        [DataRow("Çé ħ")]
        [DataRow("Ana")]
        public void ToNormalize_Is_The_Same_As_Shared(string value)
        {
            Assert.AreEqual(value.ToNormalize(), Name.Create(value).ToNormalize());
        }

        [TestMethod]
        [DataRow("josé.maría-1@kitpymes.com")]
        [DataRow("ana_ñ@kitpymes.es")]
        public void Email_ToNormalize_Is_The_Same_As_Shared(string value)
        {
            Assert.AreEqual(value.ToReplaceSpecialChars("@"), Email.Create(value).ToNormalize());
        }

        [TestMethod]
        [DataRow("kitpymes_01")]
        public void TenantName_ToNormalize_Is_The_Same_As_Shared(string value)
        {
            Assert.AreEqual(value.ToNormalize(), TenantName.Create(value).ToNormalize());
        }

        [TestMethod]
        [DataRow(null, "456")]
        [DataRow("", "456")]
        [DataRow(" ", "456")]
        public void Address_Required_Like_Shared(string? street, string postalCode)
        {
            AssertSameRejection(street, () => Address.Create(street!, 1, postalCode, "Busot", "Alicante", "España"), () => street.ToThrowIfNullOrEmpty("street"), "street");
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Address_Number_Less_Than_One_Like_Shared(int number)
        {
            AssertSameRejection<ArgumentOutOfRangeException>(() => Address.Create("Mayor", number, "03580", "Busot", "Alicante", "España"), () => number.ToThrowIfLess(1, "number"), "number");
        }

        [TestMethod]
        [DataRow(0L)]
        [DataRow(-5L)]
        public void Telephone_Number_Less_Than_One_Like_Shared(long number)
        {
            AssertSameRejection<ArgumentOutOfRangeException>(() => Telephone.Create("34", number), () => number.ToThrowIfLess(1, "number"), "number");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void Telephone_Prefix_Required_Like_Shared(string? prefix)
        {
            AssertSameRejection(prefix, () => Telephone.Create(prefix, 600000000), () => prefix.ToThrowIfNullOrEmpty("prefix"), "prefix");
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(-10)]
        public void Money_Decimals_Less_Than_Zero_Like_Shared(int numbeOfDecimals)
        {
            var currency = Currency.Create(Currency.CodeName.EUR);

            AssertSameRejection<ArgumentOutOfRangeException>(() => Money.Create(10, currency, numbeOfDecimals), () => numbeOfDecimals.ToThrowIfLess(0, "numbeOfDecimals"), "numbeOfDecimals");
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(4)]
        public void Enumeration_Id_Out_Of_Range_Like_Shared(int id)
        {
            AssertSameRejection<ArgumentOutOfRangeException>(() => _ = new CardEnum(id, "x", "x"), () => id.ToThrowIfOutRange(0, 3, "id"), "id");
            AssertSameRejection<ArgumentOutOfRangeException>(() => _ = new SubscriptionEnum(id, "x", "x"), () => id.ToThrowIfOutRange(0, 3, "id"), "id");
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(2)]
        public void StatusEnum_Id_Out_Of_Range_Like_Shared(int id)
        {
            AssertSameRejection<ArgumentOutOfRangeException>(() => _ = new StatusEnum(id, "x"), () => id.ToThrowIfOutRange(0, 1, "id"), "id");
        }

        [TestMethod]
        [DataRow(3)]
        public void GenderEnum_Id_Out_Of_Range_Like_Shared(int id)
        {
            AssertSameRejection<ArgumentOutOfRangeException>(() => _ = new GenderEnum(id, "x", "x"), () => id.ToThrowIfOutRange(0, 2, "id"), "id");
        }

        [TestMethod]
        [DataRow(5)]
        public void LogEnum_Id_Out_Of_Range_Like_Shared(int id)
        {
            AssertSameRejection<ArgumentOutOfRangeException>(() => _ = new LogEnum(id, "x", "x"), () => id.ToThrowIfOutRange(0, 4, "id"), "id");
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(3)]
        public void Enumeration_Id_In_Range_Is_Accepted(int id)
        {
            Assert.AreEqual(id, new CardEnum(id, "x", "x").Id);
        }
    }
}
