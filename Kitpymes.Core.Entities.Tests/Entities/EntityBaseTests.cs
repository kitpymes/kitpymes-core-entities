using Kitpymes.Core.Mediator.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Kitpymes.Core.Entities.Tests.Entities
{
    [TestClass]
    public class EntityBaseTests
    {
        private sealed class Customer : EntityBaseInt
        {
            public Customer() { }

            public Customer(int id) : base(id) { }
        }

        private sealed class Supplier : EntityBaseInt
        {
            public Supplier(int id) : base(id) { }
        }

        private sealed class Product : EntityBase<string>
        {
            public Product(string id) : base(id) { }
        }

        private sealed class Document : EntityBase<Guid>
        {
            public Document(Guid id) : base(id) { }
        }

        private sealed class Order : EntityBaseGuid { }

        private sealed class Invoice : EntityBaseGuidSequencial { }

        private sealed class Note : EntityBaseString { }

        private sealed class Created : INotification { }

        private sealed class Paid : INotification { }

        #region Constructor

        [TestMethod]
        public void Constructor_With_Key_Sets_Id()
        {
            Assert.AreEqual(5, new Customer(5).Id);
            Assert.AreEqual("A-1", new Product("A-1").Id);
        }

        [TestMethod]
        public void Constructor_With_Null_Key_Throws_ArgumentNullException()
        {
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() => new Product(null!));

            Assert.AreEqual("key", exception.ParamName);
            StringAssert.StartsWith(exception.Message, "El campo key es requerido");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("   ")]
        public void Constructor_With_Empty_String_Key_Throws_ArgumentException(string key)
        {
            var exception = Assert.ThrowsExactly<ArgumentException>(() => new Product(key));

            Assert.AreEqual("key", exception.ParamName);
        }

        [TestMethod]
        public void Constructor_With_Empty_Guid_Throws_ArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new Document(Guid.Empty));
        }

        [TestMethod]
        public void Constructor_Without_Key_Leaves_Default_Id()
        {
            Assert.AreEqual(0, new Customer().Id);
        }

        [TestMethod]
        public void EntityBaseGuid_Creates_A_New_Key_Each_Time()
        {
            var first = new Order();
            var second = new Order();

            Assert.AreNotEqual(Guid.Empty, first.Id);
            Assert.AreNotEqual(first.Id, second.Id);
        }

        [TestMethod]
        public void EntityBaseString_Creates_A_Guid_Key()
        {
            var note = new Note();

            Assert.IsTrue(Guid.TryParse(note.Id, out var guid));
            Assert.AreNotEqual(Guid.Empty, guid);
            Assert.AreNotEqual(note.Id, new Note().Id);
        }

        [TestMethod]
        public void EntityBaseGuidSequencial_Creates_Distinct_Keys()
        {
            var ids = Enumerable.Range(0, 1000).Select(_ => new Invoice().Id).ToList();

            Assert.AreEqual(ids.Count, ids.Distinct().Count());
            Assert.IsFalse(ids.Contains(Guid.Empty));
        }

        #endregion Constructor

        #region Equals

        [TestMethod]
        public void Equals_Same_Type_And_Key_Returns_True()
        {
            var left = new Customer(1);
            var right = new Customer(1);

            Assert.IsTrue(left.Equals(right));
            Assert.IsTrue(left.Equals((object)right));
            Assert.IsTrue(left == right);
            Assert.IsFalse(left != right);
            Assert.AreEqual(left.GetHashCode(), right.GetHashCode());
        }

        [TestMethod]
        public void Equals_Same_Type_Different_Key_Returns_False()
        {
            var left = new Customer(1);
            var right = new Customer(2);

            Assert.IsFalse(left.Equals(right));
            Assert.IsFalse(left == right);
            Assert.IsTrue(left != right);
        }

        [TestMethod]
        public void Equals_Different_Type_Same_Key_Returns_False_Without_Throwing()
        {
            EntityBase<int> customer = new Customer(1);
            EntityBase<int> supplier = new Supplier(1);

            Assert.IsFalse(customer.Equals(supplier));
            Assert.IsFalse(customer.Equals((object)supplier));
            Assert.IsFalse(customer == supplier);
            Assert.IsTrue(customer != supplier);
        }

        [TestMethod]
        public void Equals_Object_Of_Other_Kind_Returns_False()
        {
            var customer = new Customer(1);

            Assert.IsFalse(customer.Equals("1"));
            Assert.IsFalse(customer.Equals((object)1));
        }

        [TestMethod]
        public void Equals_Null_Returns_False()
        {
            var customer = new Customer(1);

            Assert.IsFalse(customer.Equals(null));
            Assert.IsFalse(customer.Equals((object?)null));
            Assert.IsFalse(customer == null);
            Assert.IsFalse(null == customer);
            Assert.IsTrue(customer != null);
        }

        [TestMethod]
        public void Operators_With_Both_Null_Return_Equal()
        {
            Customer? left = null;
            Customer? right = null;

            Assert.IsTrue(left == right);
            Assert.IsFalse(left != right);
        }

        [TestMethod]
        public void Entities_Without_Key_Are_Only_Equal_To_Themselves()
        {
            var left = new Customer();
            var right = new Customer();
            var same = left;

            Assert.IsFalse(left.Equals(right));
            Assert.IsFalse(left == right);
            Assert.IsTrue(left.Equals(same));
            Assert.IsTrue(left == same);
        }

        [TestMethod]
        public void Entity_Without_Key_Is_Not_Equal_To_One_With_Key()
        {
            var withoutKey = new Customer();
            var withKey = new Customer(1);

            Assert.IsFalse(withoutKey.Equals(withKey));
            Assert.IsFalse(withKey.Equals(withoutKey));
        }

        [TestMethod]
        public void Equals_String_Keys_Are_Case_Sensitive()
        {
            var lower = new Product("abc");
            var upper = new Product("ABC");

            Assert.IsFalse(lower.Equals(upper));
        }

        [TestMethod]
        public void Implements_IEquatable_Of_Entity_And_Not_Of_Key()
        {
            var interfaces = typeof(EntityBase<int>).GetInterfaces();

            CollectionAssert.Contains(interfaces, typeof(IEquatable<EntityBase<int>>));
            CollectionAssert.DoesNotContain(interfaces, typeof(IEquatable<int>));
        }

        #endregion Equals

        #region GetHashCode

        [TestMethod]
        public void GetHashCode_Same_Key_Different_Type_Differs()
        {
            var customer = new Customer(1);
            var supplier = new Supplier(1);

            Assert.AreNotEqual(customer.GetHashCode(), supplier.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_Is_Type_And_Key()
        {
            var customer = new Customer(7);

            Assert.AreEqual(HashCode.Combine(typeof(Customer), 7), customer.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_Without_Key_Is_Per_Instance()
        {
            var customer = new Customer();

            Assert.AreEqual(System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(customer), customer.GetHashCode());
            Assert.AreEqual(customer.GetHashCode(), customer.GetHashCode());
        }

        [TestMethod]
        public void HashSet_Keeps_Distinct_Entities_Without_Key()
        {
            var set = new HashSet<Customer> { new Customer(), new Customer(), new Customer(1), new Customer(1) };

            Assert.AreEqual(3, set.Count);
        }

        [TestMethod]
        public void List_Contains_With_Mixed_Types_Does_Not_Throw()
        {
            var list = new List<IEntityBase> { new Customer(1), new Supplier(2) };

            Assert.IsTrue(list.Contains(new Supplier(2)));
            Assert.IsFalse(list.Contains(new Supplier(1)));
        }

        #endregion GetHashCode

        #region DomainEvents

        [TestMethod]
        public void DomainEvents_Starts_Empty()
        {
            Assert.AreEqual(0, new Customer(1).DomainEvents.Count);
        }

        [TestMethod]
        public void AddDomainEvent_Keeps_Order()
        {
            var customer = new Customer(1);
            var created = new Created();
            var paid = new Paid();

            customer.AddDomainEvent(created);
            customer.AddDomainEvent(paid);

            CollectionAssert.AreEqual(new INotification[] { created, paid }, customer.DomainEvents.ToArray());
        }

        [TestMethod]
        public void AddDomainEvent_Null_Throws_ArgumentNullException()
        {
            var customer = new Customer(1);

            var exception = Assert.ThrowsExactly<ArgumentNullException>(() => customer.AddDomainEvent(null!));

            Assert.AreEqual("domainEvent", exception.ParamName);
            Assert.AreEqual(0, customer.DomainEvents.Count);
        }

        [TestMethod]
        public void RemoveDomainEvent_Removes_Only_That_Event()
        {
            var customer = new Customer(1);
            var created = new Created();
            var paid = new Paid();
            customer.AddDomainEvent(created);
            customer.AddDomainEvent(paid);

            customer.RemoveDomainEvent(created);

            CollectionAssert.AreEqual(new INotification[] { paid }, customer.DomainEvents.ToArray());
        }

        [TestMethod]
        public void RemoveDomainEvent_Not_Added_Does_Nothing()
        {
            var customer = new Customer(1);
            var paid = new Paid();
            customer.AddDomainEvent(paid);

            customer.RemoveDomainEvent(new Created());

            Assert.AreEqual(1, customer.DomainEvents.Count);
        }

        [TestMethod]
        public void ClearDomainEvents_Removes_All()
        {
            var customer = new Customer(1);
            customer.AddDomainEvent(new Created());
            customer.AddDomainEvent(new Paid());

            customer.ClearDomainEvents();

            Assert.AreEqual(0, customer.DomainEvents.Count);
        }

        [TestMethod]
        public void DomainEvents_Cannot_Be_Modified_From_Outside()
        {
            var customer = new Customer(1);
            customer.AddDomainEvent(new Created());

            var events = customer.DomainEvents as ICollection<INotification>;

            Assert.IsNotNull(events);
            Assert.IsTrue(events.IsReadOnly);
            Assert.ThrowsExactly<NotSupportedException>(() => events.Add(new Paid()));
        }

        [TestMethod]
        public void DomainEvents_Through_Interface_Is_The_Same()
        {
            var customer = new Customer(1);
            var created = new Created();
            IEntityBase entity = customer;

            entity.AddDomainEvent(created);

            CollectionAssert.AreEqual(new INotification[] { created }, entity.DomainEvents.ToArray());

            entity.RemoveDomainEvent(created);
            Assert.AreEqual(0, entity.DomainEvents.Count);

            entity.AddDomainEvent(created);
            entity.ClearDomainEvents();
            Assert.AreEqual(0, entity.DomainEvents.Count);
        }

        [TestMethod]
        public void DomainEvents_Is_Not_Mapped()
        {
            var property = typeof(EntityBase<int>).GetProperty(nameof(IEntityBase.DomainEvents));

            Assert.IsNotNull(property);
            Assert.IsNotNull(property.GetCustomAttribute<NotMappedAttribute>());
        }

        [TestMethod]
        public void Domain_Events_Do_Not_Affect_Equality()
        {
            var left = new Customer(1);
            var right = new Customer(1);
            left.AddDomainEvent(new Created());

            Assert.IsTrue(left.Equals(right));
            Assert.AreEqual(left.GetHashCode(), right.GetHashCode());
        }

        #endregion DomainEvents
    }
}
