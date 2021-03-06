using System;
using System.Waf.Presentation.Converters;
using System.Waf.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Waf.Presentation.Converters
{
    [TestClass]
    public class StringFormatConverterTest
    {
        [TestMethod]
        public void StringFormatConverterBasicTest()
        {
            string book = "Star Wars - Heir to the Empire";
            string format = "Book: {0}";

            StringFormatConverter converter = StringFormatConverter.Default;
            Assert.AreEqual(string.Format(null, format, book), converter.Convert(book, null, format, null));
            Assert.AreEqual(string.Format(null, "{0}", book), converter.Convert(book, null, null, null));

            AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(null, (Type)null, null, null));
        }

        [TestMethod]
        public void StringFormatMultiConverterBasicTest()
        {
            string book = "Star Wars - Heir to the Empire";
            string author = "Timothy Zahn";
            string format = "Book: {0} by {1}";

            StringFormatConverter converter = StringFormatConverter.Default;
            Assert.AreEqual(string.Format(null, format, new[] { book, author }), converter.Convert(new[] { book, author }, null, format, null));
            Assert.AreEqual(string.Format(null, "{0}", new[] { book, author }), converter.Convert(new[] { book, author }, null, null, null));

            AssertHelper.ExpectedException<NotSupportedException>(() => converter.ConvertBack(null, (Type[])null, null, null));
        }
    }
}
