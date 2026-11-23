using Kitpymes.Core.Entities.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kitpymes.Core.Entities.Tests.Enumerations
{
    [TestClass]
    public class EnumerationBaseTests
    {
        private sealed class Color : EnumerationBase
        {
            public static readonly Color Red = new (1, "Red", "R");

            public static readonly Color Green = new (2, "Green", "G");

            public static readonly Color Blue = new (3, "Blue", "B");

            public Color(int id, string name, string? shortName)
                : base(id, name, shortName)
            {
            }
        }

        private sealed class Size : EnumerationBase
        {
            public static readonly Size Small = new (1, "Red", "R");

            public Size(int id, string name, string? shortName)
                : base(id, name, shortName)
            {
            }
        }

        [TestMethod]
        public void ToAll_Returns_The_Static_Fields()
        {
            CollectionAssert.AreEqual(new[] { Color.Red, Color.Green, Color.Blue }, EnumerationBase.ToAll<Color>().ToArray());
        }

        [TestMethod]
        public void ToEnum_By_Enumeration_Matches_By_Id_Only()
        {
            var actual = EnumerationBase.ToEnum<Color>(new Color(2, "x", "y"));

            Assert.AreSame(Color.Green, actual);
        }

        [TestMethod]
        public void ToEnum_By_Enumeration_Matches_By_Name_Only()
        {
            var actual = EnumerationBase.ToEnum<Color>(new Color(99, "Blue", "y"));

            Assert.AreSame(Color.Blue, actual);
        }

        [TestMethod]
        public void ToEnum_By_Enumeration_Matches_By_ShortName_Only()
        {
            var actual = EnumerationBase.ToEnum<Color>(new Color(99, "x", "G"));

            Assert.AreSame(Color.Green, actual);
        }

        [TestMethod]
        public void ToEnum_By_Enumeration_Without_Match_Returns_Null()
        {
            var actual = EnumerationBase.ToEnum<Color>(new Color(99, "x", "y"));

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ToEnum_By_Id_And_By_Name()
        {
            Assert.AreSame(Color.Blue, EnumerationBase.ToEnum<Color>(3));
            Assert.IsNull(EnumerationBase.ToEnum<Color>(99));
            Assert.AreSame(Color.Red, EnumerationBase.ToEnum<Color>("R"));
            Assert.AreSame(Color.Red, EnumerationBase.ToEnum<Color>("Red"));
            Assert.IsNull(EnumerationBase.ToEnum<Color>("x"));
        }

        [TestMethod]
        public void Equals_Same_Id_Same_Type_Is_True_Even_With_Other_Name()
        {
            var equals = Color.Red.Equals(new Color(1, "Otro", null));

            Assert.IsTrue(equals);
            Assert.AreEqual(Color.Red.GetHashCode(), new Color(1, "Otro", null).GetHashCode());
        }

        [TestMethod]
        public void Equals_Same_Id_Other_Type_Is_False()
        {
            var equals = Color.Red.Equals(Size.Small);
            var operatorEquals = Color.Red == Size.Small;

            Assert.IsFalse(equals);
            Assert.IsFalse(operatorEquals);
        }

        [TestMethod]
        public void Equals_Other_Id_Same_Type_Is_False()
        {
            var equals = Color.Red.Equals(Color.Green);

            Assert.IsFalse(equals);
        }

        [TestMethod]
        public void Equals_Not_An_Enumeration_Is_False()
        {
            var equals = Color.Red.Equals("Red");
            var nullEquals = Color.Red.Equals(null);

            Assert.IsFalse(equals);
            Assert.IsFalse(nullEquals);
        }

        [TestMethod]
        public void Operators_Equal_And_Not_Equal_With_Nulls()
        {
            Color? none = null;

            var bothNull = none == null;
            var leftNull = none == Color.Red;
            var rightNull = Color.Red == none;
            var same = Color.Red == new Color(1, "Red", "R");
            var notEqual = Color.Red != Color.Green;
            var notEqualSame = Color.Red != new Color(1, "Red", "R");
            var notEqualNull = none != Color.Red;

            Assert.IsTrue(bothNull);
            Assert.IsFalse(leftNull);
            Assert.IsFalse(rightNull);
            Assert.IsTrue(same);
            Assert.IsTrue(notEqual);
            Assert.IsFalse(notEqualSame);
            Assert.IsTrue(notEqualNull);
        }

        [TestMethod]
        public void Operators_Compare_By_Id()
        {
            var red = Color.Red;
            var green = Color.Green;
            var otherRed = new Color(1, "Red", "R");

            var results = new[]
            {
                red < green, !(green < red), !(red < otherRed),
                red <= green, red <= otherRed, !(green <= red),
                green > red, !(red > green), !(red > otherRed),
                green >= red, red >= otherRed, !(red >= green),
            };

            CollectionAssert.DoesNotContain(results, false);
        }

        [TestMethod]
        public void Operators_Compare_With_Nulls()
        {
            Color? none = null;
            Color? otherNone = null;
            var red = Color.Red;

            var results = new[]
            {
                none < red, !(none < otherNone), !(red < none),
                none <= red, none <= otherNone, !(red <= none),
                !(none > red), !(none > otherNone), red > none,
                !(none >= red), none >= otherNone, red >= none,
            };

            CollectionAssert.DoesNotContain(results, false);
        }

        [TestMethod]
        public void CompareTo_Null_Is_Greater_And_Other_By_Id()
        {
            Assert.AreEqual(1, Color.Red.CompareTo(null));
            Assert.IsTrue(Color.Red.CompareTo(Color.Blue) < 0);
            Assert.IsTrue(Color.Blue.CompareTo(Color.Red) > 0);
            Assert.AreEqual(0, Color.Red.CompareTo(new Color(1, "x", null)));
        }

        [TestMethod]
        public void ToString_And_ToFullString()
        {
            Assert.AreEqual("Green", Color.Green.ToString());
            Assert.AreEqual("2: Green", Color.Green.ToFullString());
        }
    }
}
