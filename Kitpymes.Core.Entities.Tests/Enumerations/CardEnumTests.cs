using Kitpymes.Core.Entities.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Kitpymes.Core.Entities.Tests.Enumerations;

[TestClass]
public class CardEnumTests
{
    #region ToEnum

    [TestMethod]
    public void ToEnum_Passing_ValidName_Returns_Enumeracion()
    {
        var name = CardEnum.Amex.Name;
        var expected = CardEnum.Amex;

        var actual = CardEnum.ToEnum<CardEnum>(name);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidName_Returns_Null()
    {
        var name = Guid.NewGuid().ToString();

        var actual = CardEnum.ToEnum<CardEnum>(name);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidShortName_Returns_Enumeracion()
    {
        var shortName = CardEnum.Amex.ShortName;
        var expected = CardEnum.Amex;

        var actual = CardEnum.ToEnum<CardEnum>(shortName);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidShortName_Returns_Null()
    {
        var shortName = Guid.NewGuid().ToString();

        var actual = CardEnum.ToEnum<CardEnum>(shortName);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidId_Returns_Enumeracion()
    {
        var id = CardEnum.Amex.Id;
        var expected = CardEnum.Amex;

        var actual = CardEnum.ToEnum<CardEnum>(id);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidId_Returns_Null()
    {
        var id = new Random().Next();

        var actual = CardEnum.ToEnum<CardEnum>(id);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_Enum_Returns_Enumeracion()
    {
        var @enum = CardEnum.Amex;
        var expected = CardEnum.Amex;

        var actual = CardEnum.ToEnum<CardEnum>(@enum);

        Assert.AreEqual(expected, actual);
    }

    #endregion ToEnum 

    #region ToAll

    [TestMethod]
    public void ToAll_Returns_Enumeracion_List()
    {
        var expected = CardEnum.ToAll<CardEnum>().Count();

        var actual = CardEnum.ToAll<CardEnum>().ToList();

        Assert.AreEqual(expected, actual?.Count);
        CollectionAssert.Contains(actual, CardEnum.None);
        CollectionAssert.Contains(actual, CardEnum.Amex);
        CollectionAssert.Contains(actual, CardEnum.MasterCard);
        CollectionAssert.Contains(actual, CardEnum.Visa);
    }

    #endregion ToAll

    #region Is

    [TestMethod]
    public void IsVisa_Returns_True()
    {
        var enumeracion = CardEnum.Visa;

        Assert.IsTrue(enumeracion.IsVisa);
    }

    [TestMethod]
    public void IsAmex_Returns_True()
    {
        var enumeracion = CardEnum.Amex;

        Assert.IsTrue(enumeracion.IsAmex);
    }

    [TestMethod]
    public void IsMasterCard_Returns_True()
    {
        var enumeracion = CardEnum.MasterCard;

        Assert.IsTrue(enumeracion.IsMasterCard);
    }

    #endregion Is

    #region ToString

    [TestMethod]
    public void ToString_Passing_ValidName_Returns_Name()
    {
        var expected = CardEnum.Visa.Name;
        var actual = CardEnum.Visa.ToString();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToFullString_Passing_ValidName_Returns_Name()
    {
        var expected = $"{CardEnum.Visa.Id}: {CardEnum.Visa.Name}";
        var actual = CardEnum.Visa.ToFullString();

        Assert.AreEqual(expected, actual);
    }

    #endregion ToString
}
