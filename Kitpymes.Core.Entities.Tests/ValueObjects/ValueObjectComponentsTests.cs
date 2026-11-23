using Kitpymes.Core.Entities.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Reflection;

namespace Kitpymes.Core.Entities.Tests.ValueObjects
{
    // Cada componente de la igualdad cuenta: dos objetos que sólo se diferencian en uno no son iguales.
    [TestClass]
    public class ValueObjectComponentsTests
    {
        private static Address Address(string street = "Mayor", int number = 1, string postalCode = "03580", string city = "Busot", string state = "Alicante", string country = "España")
            => ValueObjects.Address.Create(street, number, postalCode, city, state, country);

        private static void AssertDistinct(ValueObjectBase left, ValueObjectBase right)
        {
            var equals = left.Equals(right);

            Assert.IsFalse(equals, $"{left} y {right} no deberían ser iguales.");
        }

        // El ORM rellena las propiedades por reflexión: así se simula un registro con columnas a nulo.
        private static T WithProperties<T>(T value, params (string Name, object? Value)[] properties)
        {
            foreach (var (name, propertyValue) in properties)
            {
                typeof(T).GetProperty(name, BindingFlags.Public | BindingFlags.Instance)!.SetValue(value, propertyValue);
            }

            return value;
        }

        [TestMethod]
        public void Address_Each_Component_Counts()
        {
            var address = Address();

            Assert.AreEqual(address, Address());
            AssertDistinct(address, Address(street: "Otra"));
            AssertDistinct(address, Address(number: 2));
            AssertDistinct(address, Address(postalCode: "03000"));
            AssertDistinct(address, Address(city: "Alicante"));
            AssertDistinct(address, Address(state: "Valencia"));
            AssertDistinct(address, Address(country: "Italia"));
        }

        [TestMethod]
        [DataRow(nameof(ValueObjects.Address.Street))]
        [DataRow(nameof(ValueObjects.Address.PostalCode))]
        [DataRow(nameof(ValueObjects.Address.City))]
        [DataRow(nameof(ValueObjects.Address.State))]
        [DataRow(nameof(ValueObjects.Address.Country))]
        public void Address_IsNull_When_Any_Text_Is_Missing(string missing)
        {
            var address = WithProperties(
                ValueObjects.Address.Null,
                ("Street", "Mayor"),
                ("Number", 1),
                ("PostalCode", "03580"),
                ("City", "Busot"),
                ("State", "Alicante"),
                ("Country", "España"),
                (missing, null));

            Assert.IsTrue(address.IsNull);
        }

        [TestMethod]
        public void Address_IsNull_False_When_Complete_And_ToString()
        {
            var address = Address();

            Assert.IsFalse(address.IsNull);
            Assert.IsTrue(ValueObjects.Address.Null.IsNull);
            Assert.AreEqual("Mayor 1, 03580 - Busot Alicante, España", address.ToString());
        }

        [TestMethod]
        public void FullName_Each_Component_Counts()
        {
            var name = FullName.Create("Ana", "Pérez", "María");

            Assert.AreEqual(name, FullName.Create("Ana", "Pérez", "María"));
            AssertDistinct(name, FullName.Create("Eva", "Pérez", "María"));
            AssertDistinct(name, FullName.Create("Ana", "Gómez", "María"));
            AssertDistinct(name, FullName.Create("Ana", "Pérez", "Luisa"));
            Assert.AreEqual("Ana Pérez", name.ToString());
            Assert.AreEqual("Ana María, Pérez", name.ToFullString());
        }

        [TestMethod]
        public void FullName_Optional_Parts_And_IsNull()
        {
            var name = FullName.Create("Ana");

            Assert.IsNull(name.LastName);
            Assert.IsNull(name.MiddleName);
            Assert.IsFalse(name.IsNull);
            Assert.IsTrue(FullName.Null.IsNull);
        }

        [TestMethod]
        public void Telephone_Each_Component_Counts()
        {
            var telephone = Telephone.Create("34", 600000000);

            AssertDistinct(telephone, Telephone.Create("33", 600000000));
            AssertDistinct(telephone, Telephone.Create("34", 600000001));
        }

        [TestMethod]
        [DataRow("Prefix")]
        [DataRow("Number")]
        public void Telephone_IsNull_When_Any_Part_Is_Missing(string missing)
        {
            var telephone = WithProperties(Telephone.Null, ("Prefix", "34"), ("Number", 600000000L), (missing, null));

            Assert.IsTrue(telephone.IsNull);
        }

        [TestMethod]
        public void Money_Each_Component_Counts_And_Rounds_To_Its_Decimals()
        {
            var eur = Currency.Create(Currency.CodeName.EUR);
            var usd = Currency.Create(Currency.CodeName.USD);
            var money = Money.Create(10.5m, eur);

            AssertDistinct(money, Money.Create(10.5m, usd));
            AssertDistinct(money, Money.Create(11m, eur));
            Assert.AreEqual(1.235m, Money.Create(1.23456m, eur, 3).Amount);
            Assert.AreEqual(1.23m, Money.Create(1.23456m, eur).Amount);
        }

        [TestMethod]
        public void Money_ChangeCurrency_Replaces_It()
        {
            var eur = Currency.Create(Currency.CodeName.EUR);
            var usd = Currency.Create(Currency.CodeName.USD);

            var money = Money.Create(1, eur).ChangeCurrency(usd);

            Assert.AreSame(usd, money.Currency);
        }

        [TestMethod]
        public void Money_Default_Uses_Current_Culture()
        {
            var previous = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("es-ES");

                var money = Money.Default;

                Assert.AreEqual(0m, money.Amount);
                Assert.AreEqual(2, money.NumbeOfDecimals);
                Assert.AreEqual("EUR", money.Currency!.Code);
            }
            finally
            {
                CultureInfo.CurrentCulture = previous;
            }
        }

        [TestMethod]
        public void Currency_Equality_Is_By_Code_And_ToFullString()
        {
            var eur = Currency.Create(Currency.CodeName.EUR);

            Assert.AreEqual(eur, Currency.Create(Currency.CodeName.EUR));
            AssertDistinct(eur, Currency.Create(Currency.CodeName.USD));
            Assert.AreEqual($"EUR: {eur.Symbol}", eur.ToFullString());
            Assert.AreEqual(eur.Symbol, eur.ToString());
        }

        [TestMethod]
        public void Currency_Default_With_Country_Currency_Not_In_The_List_Throws_InvalidOperationException()
        {
            var previous = CultureInfo.CurrentCulture;

            try
            {
                // Venezuela usa VES desde 2018; la lista sólo tiene VEF.
                CultureInfo.CurrentCulture = new CultureInfo("es-VE");
                var symbol = new RegionInfo("es-VE").ISOCurrencySymbol;

                if (Enum.TryParse<Currency.CodeName>(symbol, out _))
                {
                    Assert.Inconclusive($"La cultura es-VE de esta máquina usa {symbol}, que sí está en la lista.");
                }

                Assert.ThrowsExactly<InvalidOperationException>(() => Currency.DefaultCode());
            }
            finally
            {
                CultureInfo.CurrentCulture = previous;
            }
        }

        [TestMethod]
        public void Single_Value_Objects_ToString_Equality_And_IsNull()
        {
            Assert.AreEqual("ana@kitpymes.com", Email.Create("ana@kitpymes.com").ToString());
            AssertDistinct(Email.Create("ana@kitpymes.com"), Email.Create("eva@kitpymes.com"));
            Assert.IsTrue(Email.Null.IsNull);
            Assert.IsFalse(Email.Create("ana@kitpymes.com").IsNull);

            Assert.AreEqual("Ana", Name.Create("Ana").ToString());
            AssertDistinct(Name.Create("Ana"), Name.Create("Eva"));
            Assert.IsTrue(Name.Null.IsNull);
            Assert.IsNull(Name.Null.ToNormalize());

            Assert.AreEqual("kitpymes", Subdomain.Create("Kitpymes").ToString());
            AssertDistinct(Subdomain.Create("a"), Subdomain.Create("b"));
            Assert.IsTrue(Subdomain.Null.IsNull);

            Assert.AreEqual("kit_01", TenantName.Create("kit_01").ToString());
            AssertDistinct(TenantName.Create("a"), TenantName.Create("b"));
            Assert.IsTrue(TenantName.Null.IsNull);
            Assert.IsFalse(TenantName.Create("a").IsNull);
        }

        [TestMethod]
        public void Subdomain_Change_Keeps_Case()
        {
            var subdomain = Subdomain.Create("a");

            subdomain.Change("Mi-Sub");

            Assert.AreEqual("Mi-Sub", subdomain.Value);
        }

        [TestMethod]
        [DataRow("kit-01")]
        [DataRow("kit 01")]
        [DataRow("kitñ")]
        [DataRow("")]
        public void TenantName_Invalid_Throws_ArgumentException(string tenantName)
        {
            var exception = Assert.ThrowsExactly<ArgumentException>(() => TenantName.Create(tenantName));

            Assert.AreEqual("tenantName", exception.ParamName);
            StringAssert.StartsWith(exception.Message, "Only numbers, letters and underscore are allowed");
        }

        [TestMethod]
        public void TenantName_Null_Throws_ArgumentNullException()
        {
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() => TenantName.Create(null));

            Assert.AreEqual("tenantName", exception.ParamName);
            StringAssert.StartsWith(exception.Message, "Only numbers, letters and underscore are allowed");
        }
    }
}
