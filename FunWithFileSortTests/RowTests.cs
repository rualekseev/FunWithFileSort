using FunWithFileSort;
using NUnit.Framework;
using System;
using System.Text;

namespace FunWithFileSortTests
{
    class RowTests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void Empty_Value_Correct()
        {
            var row = Row.Empty;

            Assert.AreEqual($"0. {Environment.NewLine}", row.GetRowAsString());
        }

        [Test]
        public void GetRowAsString_Correct()
        {
            var row = new Row(new LeftPartRow("123654"),new RightPartRow("alskdjfhg"));

            Assert.AreEqual($"123654. alskdjfhg{Environment.NewLine}", row.GetRowAsString());
        }


        [Test]
        public void GetSizeInBytes_Correct()
        {
            var row = new Row(new LeftPartRow("123654"), new RightPartRow("alskdjfhg"));

            
            Assert.AreEqual(Encoding.Default.GetByteCount($"123654. alskdjfhg{Environment.NewLine}"), row.GetSizeInBytes());
        }


        [Test]
        public void Compare_Correct()
        {
            Assert.AreEqual(0, Row.Empty.CompareTo(Row.Empty));
            Assert.AreEqual(1, Row.Empty.CompareTo(Row1));
            Assert.AreEqual(1, Row.Empty.CompareTo(Row2));
            Assert.AreEqual(1, Row.Empty.CompareTo(Row3));
            Assert.AreEqual(1, Row.Empty.CompareTo(Row4));
            Assert.AreEqual(1, Row.Empty.CompareTo(Row5));

            Assert.AreEqual(-1, Row1.CompareTo(Row.Empty));
            Assert.AreEqual(0, Row1.CompareTo(Row1));
            Assert.AreEqual(1, Row1.CompareTo(Row2));
            Assert.AreEqual(1, Row1.CompareTo(Row3));
            Assert.AreEqual(1, Row1.CompareTo(Row4));
            Assert.AreEqual(1, Row1.CompareTo(Row5));

            Assert.AreEqual(-1, Row2.CompareTo(Row.Empty));
            Assert.AreEqual(-1, Row2.CompareTo(Row1));
            Assert.AreEqual(0, Row2.CompareTo(Row2));
            Assert.AreEqual(1, Row2.CompareTo(Row3));
            Assert.AreEqual(1, Row2.CompareTo(Row4));
            Assert.AreEqual(1, Row2.CompareTo(Row5));

            Assert.AreEqual(-1, Row3.CompareTo(Row.Empty));
            Assert.AreEqual(-1, Row3.CompareTo(Row1));
            Assert.AreEqual(-1, Row3.CompareTo(Row2));
            Assert.AreEqual(0, Row3.CompareTo(Row3));
            Assert.AreEqual(1, Row3.CompareTo(Row4));
            Assert.AreEqual(1, Row3.CompareTo(Row5));

            Assert.AreEqual(-1, Row4.CompareTo(Row.Empty));
            Assert.AreEqual(-1, Row4.CompareTo(Row1));
            Assert.AreEqual(-1, Row4.CompareTo(Row2));
            Assert.AreEqual(-1, Row4.CompareTo(Row3));
            Assert.AreEqual(0, Row4.CompareTo(Row4));
            Assert.AreEqual(1, Row4.CompareTo(Row5));

            Assert.AreEqual(-1, Row5.CompareTo(Row.Empty));
            Assert.AreEqual(-1, Row5.CompareTo(Row1));
            Assert.AreEqual(-1, Row5.CompareTo(Row2));
            Assert.AreEqual(-1, Row5.CompareTo(Row3));
            Assert.AreEqual(-1, Row5.CompareTo(Row4));
            Assert.AreEqual(0, Row5.CompareTo(Row5));
        }

        public static Row Row1 => new Row(new LeftPartRow("1"), new RightPartRow("Apple"));
        public static Row Row2 => new Row(new LeftPartRow("415"), new RightPartRow("Apple"));
        public static Row Row3 => new Row(new LeftPartRow("2"), new RightPartRow("Banana is yellow"));
        public static Row Row4 => new Row(new LeftPartRow("32"), new RightPartRow("Cherry is the best"));
        public static Row Row5 => new Row(new LeftPartRow("30432"), new RightPartRow("Something something something"));

    }
}
