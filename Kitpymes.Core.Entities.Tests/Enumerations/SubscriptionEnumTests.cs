using Kitpymes.Core.Entities.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Kitpymes.Core.Entities.Tests.Enumerations;

[TestClass]
public class SubscriptionEnumTests
{
    #region ToEnum

    [TestMethod]
    public void ToEnum_Passing_ValidName_Returns_Enumeracion()
    {
        var name = SubscriptionEnum.Gold.Name;
        var expected = SubscriptionEnum.Gold;

        var actual = SubscriptionEnum.ToEnum<SubscriptionEnum>(name);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidName_Returns_Null()
    {
        var name = Guid.NewGuid().ToString();

        var actual = SubscriptionEnum.ToEnum<SubscriptionEnum>(name);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidShortName_Returns_Enumeracion()
    {
        var shortName = SubscriptionEnum.Silver.ShortName;
        var expected = SubscriptionEnum.Silver;

        var actual = SubscriptionEnum.ToEnum<SubscriptionEnum>(shortName);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidShortName_Returns_Null()
    {
        var shortName = Guid.NewGuid().ToString();

        var actual = SubscriptionEnum.ToEnum<SubscriptionEnum>(shortName);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidId_Returns_Enumeracion()
    {
        var id = SubscriptionEnum.Free.Id;
        var expected = SubscriptionEnum.Free;

        var actual = SubscriptionEnum.ToEnum<SubscriptionEnum>(id);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidId_Returns_Null()
    {
        var id = new Random().Next();

        var actual = SubscriptionEnum.ToEnum<SubscriptionEnum>(id);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_Enum_Returns_Enumeracion()
    {
        var @enum = SubscriptionEnum.Gold;
        var expected = SubscriptionEnum.Gold;

        var actual = SubscriptionEnum.ToEnum<SubscriptionEnum>(@enum);

        Assert.AreEqual(expected, actual);
    }

    #endregion ToEnum 

    #region ToAll

    [TestMethod]
    public void ToAll_Returns_Enumeracion_List()
    {
        var expected = SubscriptionEnum.ToAll<SubscriptionEnum>().Count();

        var actual = SubscriptionEnum.ToAll<SubscriptionEnum>().ToList();

        Assert.AreEqual(expected, actual?.Count);
        CollectionAssert.Contains(actual, SubscriptionEnum.None);
        CollectionAssert.Contains(actual, SubscriptionEnum.Free);
        CollectionAssert.Contains(actual, SubscriptionEnum.Silver);
        CollectionAssert.Contains(actual, SubscriptionEnum.Gold);
    }

    #endregion ToAll

    #region Is

    [TestMethod]
    public void IsFree_Returns_True()
    {
        var enumeracion = SubscriptionEnum.Free;

        Assert.IsTrue(enumeracion.IsFree);
    }

    [TestMethod]
    public void IsSilver_Returns_True()
    {
        var enumeracion = SubscriptionEnum.Silver;

        Assert.IsTrue(enumeracion.IsSilver);
    }

    [TestMethod]
    public void IsGold_Returns_True()
    {
        var enumeracion = SubscriptionEnum.Gold;

        Assert.IsTrue(enumeracion.IsGold);
    }

    #endregion Is

    #region ToString

    [TestMethod]
    public void ToString_Passing_ValidName_Returns_Name()
    {
        var expected = SubscriptionEnum.Gold.Name;
        var actual = SubscriptionEnum.Gold.ToString();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToFullString_Passing_ValidName_Returns_Name()
    {
        var expected = $"{SubscriptionEnum.Free.Id}: {SubscriptionEnum.Free.Name}";
        var actual = SubscriptionEnum.Free.ToFullString();

        Assert.AreEqual(expected, actual);
    }

    #endregion ToString
}
