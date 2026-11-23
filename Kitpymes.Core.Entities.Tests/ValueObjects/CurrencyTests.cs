using Kitpymes.Core.Entities.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;

namespace Kitpymes.Core.Entities.Tests.ValueObjects
{
    [TestClass]
    public class CurrencyTests
    {
        #region Create

        [TestMethod]
        [DataRow(Currency.CodeName.USD)]
        [DataRow(Currency.CodeName.EUR)]
        public void Create_Passing_Valid_Values_Returns_Currency(Currency.CodeName code)
        {
            var cultureInfo = Currency.GetCultureInfo(code);

            var actual = Currency.Create(code);

            Assert.AreEqual(cultureInfo!.NumberFormat.CurrencySymbol, actual.Symbol);
            Assert.AreEqual(code.ToString(), actual.Code);
            Assert.AreEqual(new RegionInfo(cultureInfo.Name).CurrencyNativeName, actual.Name);
        }

        [TestMethod]
        [DataRow("es-ES", "EUR")]
        [DataRow("en-US", "USD")]
        [DataRow("es-AR", "ARS")]
        public void Default_Returns_Currency_Of_Current_Culture_Country(string culture, string expected)
        {
            var previous = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(culture);

                Assert.AreEqual(expected, Currency.Default.Code);
                Assert.AreEqual(expected, Currency.DefaultCode().ToString());
            }
            finally
            {
                CultureInfo.CurrentCulture = previous;
            }
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("es")]
        public void Default_With_Culture_Without_Country_Throws_InvalidOperationException(string culture)
        {
            var previous = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = culture.Length == 0 ? CultureInfo.InvariantCulture : new CultureInfo(culture);

                var exception = Assert.ThrowsExactly<InvalidOperationException>(() => Currency.Default);

                StringAssert.Contains(exception.Message, $"({CultureInfo.CurrentCulture.Name})");
            }
            finally
            {
                CultureInfo.CurrentCulture = previous;
            }
        }

        [TestMethod]
        [DataRow(null)]
        public void Create_Passing_InvalidOrNull_Value_Throws_ArgumentException(Currency.CodeName code)
        {
            Assert.Throws<ArgumentException>(() => Currency.Create(code));
        }

        #endregion Create

        #region Change

        [TestMethod]
        [DataRow(Currency.CodeName.EUR)]
        public void Change_Passing_Valid_Values_Returns_Currency(Currency.CodeName code)
        {
            var expected = Currency.CodeName.USD;

            var actual = Currency.Create(code);
            actual.ChangeCurrency(expected);

            Assert.AreEqual(expected.ToString(), actual.Code);
        }

        #endregion Change

        #region ToString

        [TestMethod]
        [DataRow(Currency.CodeName.EUR)]
        [DataRow(Currency.CodeName.USD)]
        public void ToString_Passing_Valid_Values_Returns_Currency_String_Symbol(Currency.CodeName code)
        {
            var cultureInfo = Currency.GetCultureInfo(code);

            var result = Currency.Create(code);

            var actual = result.ToString();

            Assert.AreEqual(cultureInfo!.NumberFormat.CurrencySymbol, actual);
        }

        #endregion ToString

        #region Equals

        [TestMethod]
        [DataRow(Currency.CodeName.EUR)]
        [DataRow(Currency.CodeName.USD)]
        public void Equals_Passing_Valid_Values_Returns_True(Currency.CodeName code)
        {
            var left = Currency.Create(code);
            var right = Currency.Create(code);

            var isEqual = left.Equals(right);

            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        [DataRow(Currency.CodeName.EUR)]
        public void Equals_Passing_Valid_Distinct_Values_Returns_False(Currency.CodeName code)
        {
            var left = Currency.Create(code);
            var right = Currency.Create(Currency.CodeName.USD);

            var isEqual = left.Equals(right);

            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        [DataRow(Currency.CodeName.EUR)]
        [DataRow(Currency.CodeName.USD)]
        public void OperatorEqual_Passing_Valid_Values_Returns_True(Currency.CodeName code)
        {
            var left = Currency.Create(code);
            var right = Currency.Create(code);

            var isEqual = left == right;

            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        [DataRow(Currency.CodeName.EUR)]
        public void OperatorEqual_Passing_Valid_Distinct_Values_Returns_False(Currency.CodeName code)
        {
            var left = Currency.Create(code);
            var right = Currency.Create(Currency.CodeName.USD);

            var isEqual = left == right;

            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        [DataRow(Currency.CodeName.EUR)]
        [DataRow(Currency.CodeName.USD)]
        public void OperatorNotEqual_Passing_Valid_Values_Returns_False(Currency.CodeName code)
        {
            var left = Currency.Create(code);
            var right = Currency.Create(code);

            var isEqual = left != right;

            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        [DataRow(Currency.CodeName.EUR)]
        public void OperatorNotEqual_Passing_Valid_Values_Returns_True(Currency.CodeName code)
        {
            var left = Currency.Create(code);
            var right = Currency.Create(Currency.CodeName.USD);

            var isEqual = left != right;

            Assert.IsTrue(isEqual);
        }

        #endregion Equals
    }
}
