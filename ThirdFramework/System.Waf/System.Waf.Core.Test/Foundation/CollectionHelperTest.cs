using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using System.Waf.Foundation;
using System.Collections.Generic;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class CollectionHelperTest
    {
        [TestMethod]
        public void EmptyTest()
        {
            var emptyStringList1 = CollectionHelper.Empty<string>();
            Assert.AreEqual(0, emptyStringList1.Count);

            // Check that the list is cached
            var emptyStringList2 = CollectionHelper.Empty<string>();
            Assert.AreSame(emptyStringList1, emptyStringList2);

            var emptyIntList1 = CollectionHelper.Empty<int>();
            Assert.AreEqual(0, emptyIntList1.Count);
            var emptyIntList2 = CollectionHelper.Empty<int>();
            Assert.AreSame(emptyIntList1, emptyIntList2);
        }

        [TestMethod]
        public void IndexOfTest()
        {
            IReadOnlyList<string> collection1 = new[] { "Zero", "One", "Two" };
            Assert.AreEqual(0, collection1.IndexOf("Zero"));
            Assert.AreEqual(1, collection1.IndexOf("One"));
            Assert.AreEqual(2, collection1.IndexOf("Two"));
            Assert.AreEqual(-1, collection1.IndexOf("Nine"));
            Assert.AreEqual(-1, collection1.IndexOf(null));

            var collection2 = new Queue<string>(new[] { "Zero", "One", "Two" });
            Assert.AreEqual(0, collection2.IndexOf("Zero"));
            Assert.AreEqual(1, collection2.IndexOf("One"));
            Assert.AreEqual(2, collection2.IndexOf("Two"));
            Assert.AreEqual(-1, collection2.IndexOf("Nine"));
            Assert.AreEqual(-1, collection2.IndexOf(null));
        }

        [TestMethod]
        public void GetNextElementOrDefaultTest()
        {
            AssertHelper.ExpectedException<ArgumentNullException>(() => CollectionHelper.GetNextElementOrDefault(null, 5));

            int[] collection1 = new[] { 1, 2, 3, 4, 5 };
            Assert.AreEqual(4, CollectionHelper.GetNextElementOrDefault(collection1, 3));
            Assert.AreEqual(2, CollectionHelper.GetNextElementOrDefault(collection1, 1));
            Assert.AreEqual(0, CollectionHelper.GetNextElementOrDefault(collection1, 5));

            int[] collection2 = new[] { 1 };
            Assert.AreEqual(0, CollectionHelper.GetNextElementOrDefault(collection2, 1));

            int[] collection3 = new int[] { };
            AssertHelper.ExpectedException<ArgumentException>(() => CollectionHelper.GetNextElementOrDefault(collection3, 9));
        }
    }
}
