using Kitpymes.Core.Entities.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Kitpymes.Core.Entities.Tests.ValueObjects
{
    [TestClass]
    public class MoneyTests
    {
        #region Create

        [TestMethod]
        [DataRow(100, Currency.CodeName.EUR, 2)]
        [DataRow(200, Currency.CodeName.EUR, 4)]
        [DataRow(400, Currency.CodeName.USD, 8)]
        public void Create_Passing_Valid_Values_Returns_Address(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var amountDecimal = Convert.ToDecimal(amount);
            var currency = Currency.Create(code);

            var actual = Money.Create(amountDecimal, currency, numbeOfDecimals);

            Assert.AreEqual(amountDecimal, actual.Amount);
            Assert.AreEqual(currency, actual.Currency);
            Assert.AreEqual(numbeOfDecimals, actual.NumbeOfDecimals);
        }

        [TestMethod]
        [DataRow(456, Currency.CodeName.USD, -3)]
        public void Create_Passing_InvalidOrNull_Value_Throws_ArgumentException(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var amountDecimal = Convert.ToDecimal(amount);
            var currency = Currency.Create(code);

            Assert.Throws<ArgumentException>(() => Money.Create(amountDecimal, currency, numbeOfDecimals));
        }

        #endregion Create

        #region Change

        [TestMethod]
        [DataRow(100, Currency.CodeName.EUR, 2)]
        [DataRow(200, Currency.CodeName.EUR, 4)]
        [DataRow(400, Currency.CodeName.EUR, 8)]
        public void Change_Passing_Valid_Values_Returns_Address(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var amountDecimal = Convert.ToDecimal(amount);
            decimal expectedAmount = 999;
            var expectedCurrency = Currency.Create(Currency.CodeName.USD);
            var expectedNumbeOfDecimals = 5;

            var actual = Money.Create(amountDecimal, Currency.Create(code), numbeOfDecimals);

            actual
                .ChangeAmount(expectedAmount)
                .ChangeCurrency(expectedCurrency)
                .ChangeNumbeOfDecimals(expectedNumbeOfDecimals);

            Assert.AreEqual(expectedAmount, actual.Amount);
            Assert.AreEqual(expectedCurrency, actual.Currency);
            Assert.AreEqual(expectedNumbeOfDecimals, actual.NumbeOfDecimals);
        }

        #endregion Change

        #region ToString

        [TestMethod]
        [DataRow(100, Currency.CodeName.EUR, 2)]
        [DataRow(200, Currency.CodeName.EUR, 4)]
        [DataRow(400, Currency.CodeName.USD, 8)]
        public void ToString_Passing_Valid_Values_Returns_Address_String_Format(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var currency = Currency.Create(code);
            var amountDecimal = Convert.ToDecimal(amount);
            var expected = $"{currency} {amount}";
            var result = Money.Create(amountDecimal, currency, numbeOfDecimals);

            var actual = result.ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion ToString

        #region Equals

        [TestMethod]
        [DataRow(100, Currency.CodeName.EUR, 2)]
        [DataRow(200, Currency.CodeName.EUR, 4)]
        [DataRow(400, Currency.CodeName.USD, 8)]
        public void Equals_Passing_Valid_Values_Returns_True(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var amountDecimal = Convert.ToDecimal(amount);
            var left = Money.Create(amountDecimal, Currency.Create(code), numbeOfDecimals);
            var right = Money.Create(amountDecimal, Currency.Create(code), numbeOfDecimals);

            var isEqual = left.Equals(right);

            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        [DataRow(100, Currency.CodeName.EUR, 2)]
        [DataRow(200, Currency.CodeName.EUR, 4)]
        [DataRow(400, Currency.CodeName.EUR, 8)]
        public void Equals_Passing_Valid_Distinct_Values_Returns_False(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var amountDecimal = Convert.ToDecimal(amount);
            var left = Money.Create(amountDecimal, Currency.Create(code), numbeOfDecimals);
            var right = Money.Create(999, Currency.Create(Currency.CodeName.USD), 0);

            var isEqual = left.Equals(right);

            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        [DataRow(100, Currency.CodeName.EUR, 2)]
        [DataRow(200, Currency.CodeName.EUR, 4)]
        [DataRow(400, Currency.CodeName.USD, 8)]
        public void OperatorEqual_Passing_Valid_Values_Returns_True(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var amountDecimal = Convert.ToDecimal(amount);
            var currency = Currency.Create(code);

            var left = Money.Create(amountDecimal, currency, numbeOfDecimals);
            var right = Money.Create(amountDecimal, currency, numbeOfDecimals);

            var isEqual = left == right;

            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        [DataRow(100, Currency.CodeName.EUR, 2)]
        [DataRow(200, Currency.CodeName.EUR, 4)]
        [DataRow(400, Currency.CodeName.EUR, 8)]
        public void OperatorEqual_Passing_Valid_Distinct_Values_Returns_False(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var amountDecimal = Convert.ToDecimal(amount);
            var currency = Currency.Create(code);

            var left = Money.Create(amountDecimal, currency, numbeOfDecimals);
            var right = Money.Create(999, Currency.Create(Currency.CodeName.USD), 0);

            var isEqual = left == right;

            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        [DataRow(100, Currency.CodeName.EUR, 2)]
        [DataRow(200, Currency.CodeName.EUR, 4)]
        [DataRow(400, Currency.CodeName.USD, 8)]
        public void OperatorNotEqual_Passing_Valid_Values_Returns_False(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var amountDecimal = Convert.ToDecimal(amount);
            var currency = Currency.Create(code);

            var left = Money.Create(amountDecimal, currency, numbeOfDecimals);
            var right = Money.Create(amountDecimal, currency, numbeOfDecimals);

            var isEqual = left != right;

            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        [DataRow(100, Currency.CodeName.EUR, 2)]
        [DataRow(200, Currency.CodeName.EUR, 4)]
        [DataRow(400, Currency.CodeName.EUR, 8)]
        public void OperatorNotEqual_Passing_Valid_Values_Returns_True(double amount, Currency.CodeName code, int numbeOfDecimals)
        {
            var amountDecimal = Convert.ToDecimal(amount);
            var currency = Currency.Create(code);

            var left = Money.Create(amountDecimal, currency, numbeOfDecimals);
            var right = Money.Create(999, Currency.Create(Currency.CodeName.USD), 0);

            var isEqual = left != right;

            Assert.IsTrue(isEqual);
        }

        #endregion Equals
    }
}
