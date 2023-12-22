using Kitpymes.Core.Entities.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Kitpymes.Core.Entities.Tests.Enumerations;

[TestClass]
public class StatusEnumTests
{
    #region ToEnum

    [TestMethod]
    public void ToEnum_Passing_ValidName_Returns_Enumeracion()
    {
        var name = StatusEnum.Active.Name;
        var expected = StatusEnum.Active;

        var actual = StatusEnum.ToEnum<StatusEnum>(name);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidName_Returns_Null()
    {
        var name = Guid.NewGuid().ToString();

        var actual = StatusEnum.ToEnum<StatusEnum>(name);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidShortName_Returns_Enumeracion()
    {
        var shortName = StatusEnum.Inactive.ShortName;
        var expected = StatusEnum.Inactive;

        var actual = StatusEnum.ToEnum<StatusEnum>(shortName);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidShortName_Returns_Null()
    {
        var shortName = Guid.NewGuid().ToString();

        var actual = StatusEnum.ToEnum<StatusEnum>(shortName);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidId_Returns_Enumeracion()
    {
        var id = StatusEnum.Active.Id;
        var expected = StatusEnum.Active;

        var actual = StatusEnum.ToEnum<StatusEnum>(id);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidId_Returns_Null()
    {
        var id = new Random().Next();

        var actual = StatusEnum.ToEnum<StatusEnum>(id);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_Enum_Returns_Enumeracion()
    {
        var @enum = StatusEnum.Inactive;
        var expected = StatusEnum.Inactive;

        var actual = StatusEnum.ToEnum<StatusEnum>(@enum);

        Assert.AreEqual(expected, actual);
    }

    #endregion ToEnum 

    #region ToAll

    [TestMethod]
    public void ToAll_Returns_Enumeracion_List()
    {
        var expected = StatusEnum.ToAll<StatusEnum>().Count();

        var actual = StatusEnum.ToAll<StatusEnum>().ToList();

        Assert.AreEqual(expected, actual?.Count);
        CollectionAssert.Contains(actual, StatusEnum.Active);
        CollectionAssert.Contains(actual, StatusEnum.Inactive);
    }

    #endregion ToAll

    #region Is

    [TestMethod]
    public void IsActive_Returns_True()
    {
        var enumeracion = StatusEnum.Active;

        Assert.IsTrue(enumeracion.IsActive);
    }

    [TestMethod]
    public void IsInactive_Returns_True()
    {
        var enumeracion = StatusEnum.Inactive;

        Assert.IsTrue(enumeracion.IsInactive);
    }

    #endregion Is

    #region ToString

    [TestMethod]
    public void ToString_Passing_ValidName_Returns_Name()
    {
        var expected = StatusEnum.Active.Name;
        var actual = StatusEnum.Active.ToString();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToFullString_Passing_ValidName_Returns_Name()
    {
        var expected = $"{StatusEnum.Inactive.Id}: {StatusEnum.Inactive.Name}";
        var actual = StatusEnum.Inactive.ToFullString();

        Assert.AreEqual(expected, actual);
    }

    #endregion ToString
}
