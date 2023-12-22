using Kitpymes.Core.Entities.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Kitpymes.Core.Entities.Tests.Enumerations;

[TestClass]
public class GenderEnumTests
{
    #region ToEnum

    [TestMethod]
    public void ToEnum_Passing_ValidName_Returns_Enumeracion()
    {
        var name = GenderEnum.Female.Name;
        var expected = GenderEnum.Female;

        var actual = GenderEnum.ToEnum<GenderEnum>(name);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidName_Returns_Null()
    {
        var name = Guid.NewGuid().ToString();

        var actual = GenderEnum.ToEnum<GenderEnum>(name);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidShortName_Returns_Enumeracion()
    {
        var shortName = GenderEnum.Female.ShortName;
        var expected = GenderEnum.Female;

        var actual = GenderEnum.ToEnum<GenderEnum>(shortName);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidShortName_Returns_Null()
    {
        var shortName = Guid.NewGuid().ToString();

        var actual = GenderEnum.ToEnum<GenderEnum>(shortName);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_ValidId_Returns_Enumeracion()
    {
        var id = GenderEnum.Female.Id;
        var expected = GenderEnum.Female;

        var actual = GenderEnum.ToEnum<GenderEnum>(id);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToEnum_Passing_InvalidId_Returns_Null()
    {
        var id = new Random().Next();

        var actual = GenderEnum.ToEnum<GenderEnum>(id);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public void ToEnum_Passing_Enum_Returns_Enumeracion()
    {
        var @enum = GenderEnum.Female;
        var expected = GenderEnum.Female;

        var actual = GenderEnum.ToEnum<GenderEnum>(@enum);

        Assert.AreEqual(expected, actual);
    }

    #endregion ToEnum 

    #region ToAll

    [TestMethod]
    public void ToAll_Returns_Enumeracion_List()
    {
        var expected = GenderEnum.ToAll<GenderEnum>().Count();

        var actual = GenderEnum.ToAll<GenderEnum>().ToList();

        Assert.AreEqual(expected, actual?.Count);
        CollectionAssert.Contains(actual, GenderEnum.None);
        CollectionAssert.Contains(actual, GenderEnum.Female);
        CollectionAssert.Contains(actual, GenderEnum.Male);
    }

    #endregion ToAll

    #region Is

    [TestMethod]
    public void IsFemale_Returns_True()
    {
        var enumeracion = GenderEnum.Female;

        Assert.IsTrue(enumeracion.IsFemale);
    }

    [TestMethod]
    public void IsMale_Returns_True()
    {
        var enumeracion = GenderEnum.Male;

        Assert.IsTrue(enumeracion.IsMale);
    }

    #endregion Is

    #region ToString

    [TestMethod]
    public void ToString_Passing_ValidName_Returns_Name()
    {
        var expected = GenderEnum.Male.Name;
        var actual = GenderEnum.Male.ToString();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ToFullString_Passing_ValidName_Returns_Name()
    {
        var expected = $"{GenderEnum.Female.Id}: {GenderEnum.Female.Name}";
        var actual = GenderEnum.Female.ToFullString();

        Assert.AreEqual(expected, actual);
    }

    #endregion ToString
}
