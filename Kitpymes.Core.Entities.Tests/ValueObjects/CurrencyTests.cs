using Kitpymes.Core.Entities.ValueObjects;
using Kitpymes.Core.Shared.Util;
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

        public void Default_Passing_Nothing_Returns_Default_Currency()
        {
            var expected = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;

            var result = Currency.Default;

            Assert.AreEqual(expected, result.Code);
        }

        [TestMethod]
        [DataRow(null)]
        public void Create_Passing_InvalidOrNull_Value_Returns_SharedException(Currency.CodeName code)
        {
            Assert.ThrowsExactly<SharedException>(() => Currency.Create(code));
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
