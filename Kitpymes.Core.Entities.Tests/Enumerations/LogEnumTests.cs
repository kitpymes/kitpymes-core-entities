using Kitpymes.Core.Entities.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Kitpymes.Core.Entities.Tests.Enumerations;

[TestClass]
public class LogEnumTests
{
    #region ToEnum

    [TestMethod]
    public void ToEnum_Passing_ValidName_Returns_Enumeracion()
    {
        var name = LogEnum.Created.Name;
        var expected = LogEnum.Created;

        var actual = LogEnum.ToEnum<LogEnum>(name);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidName_Returns_Null()
    {
        var name = Guid.NewGuid().ToString();

        var actual = LogEnum.ToEnum<LogEnum>(name);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidShortName_Returns_Enumeracion()
    {
        var shortName = LogEnum.Updated.ShortName;
        var expected = LogEnum.Updated;

        var actual = LogEnum.ToEnum<LogEnum>(shortName);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidShortName_Returns_Null()
    {
        var shortName = Guid.NewGuid().ToString();

        var actual = LogEnum.ToEnum<LogEnum>(shortName);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidId_Returns_Enumeracion()
    {
        var id = LogEnum.Deleted.Id;
        var expected = LogEnum.Deleted;

        var actual = LogEnum.ToEnum<LogEnum>(id);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidId_Returns_Null()
    {
        var id = new Random().Next();

        var actual = LogEnum.ToEnum<LogEnum>(id);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_Enum_Returns_Enumeracion()
    {
        var @enum = LogEnum.Deleted;
        var expected = LogEnum.Deleted;

        var actual = LogEnum.ToEnum<LogEnum>(@enum);

        Assert.AreEqual(expected, actual);
    }

    #endregion ToEnum 

    #region ToAll

    [TestMethod]
    public void ToAll_Returns_Enumeracion_List()
    {
        var expected = LogEnum.ToAll<LogEnum>().Count();

        var actual = LogEnum.ToAll<LogEnum>().ToList();

        Assert.AreEqual(expected, actual?.Count);
        CollectionAssert.Contains(actual, LogEnum.None);
        CollectionAssert.Contains(actual, LogEnum.Changed);
        CollectionAssert.Contains(actual, LogEnum.Created);
        CollectionAssert.Contains(actual, LogEnum.Deleted);
        CollectionAssert.Contains(actual, LogEnum.Updated);
    }

    #endregion ToAll

    #region Is

    [TestMethod]
    public void IsChanged_Returns_True()
    {
        var enumeracion = LogEnum.Changed;

        Assert.IsTrue(enumeracion.IsChanged);
    }

    [TestMethod]
    public void IsCreated_Returns_True()
    {
        var enumeracion = LogEnum.Created;

        Assert.IsTrue(enumeracion.IsCreated);
    }

    [TestMethod]
    public void IsDeleted_Returns_True()
    {
        var enumeracion = LogEnum.Deleted;

        Assert.IsTrue(enumeracion.IsDeleted);
    }

    [TestMethod]
    public void IsUpdated_Returns_True()
    {
        var enumeracion = LogEnum.Updated;

        Assert.IsTrue(enumeracion.IsUpdated);
    }

    #endregion Is

    #region ToString

    [TestMethod]
    public void ToString_Passing_ValidName_Returns_Name()
    {
        var expected = LogEnum.Updated.Name;
        var actual = LogEnum.Updated.ToString();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToFullString_Passing_ValidName_Returns_Name()
    {
        var expected = $"{LogEnum.Created.Id}: {LogEnum.Created.Name}";
        var actual = LogEnum.Created.ToFullString();

        Assert.AreEqual(expected, actual);
    }

    #endregion ToString
}
