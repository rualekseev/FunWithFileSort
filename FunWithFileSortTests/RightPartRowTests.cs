using FunWithFileSort;
using NUnit.Framework;

namespace FunWithFileSortTests
{
    class RightPartRowTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("Apple", "Apple", 0)]
        [TestCase("Banana is yellow", "Banana is yellow", 0)]
        [TestCase("Cherry is the best", "Cherry is the best", 0)]
        [TestCase("Something something something", "Something something something", 0)]
        [TestCase("Apple", "Banana is yellow", 1)]
        [TestCase("Banana is yellow", "Apple", -1)]
        [TestCase("Apple", "Cherry is the best", 1)]
        [TestCase("Cherry is the best", "Apple", -1)]
        [TestCase("Apple", "Something something something", 1)]
        [TestCase("Something something something", "Apple", -1)]
        [TestCase("Banana is yellow", "Cherry is the best", 1)]
        [TestCase("Cherry is the best", "Banana is yellow", -1)]
        [TestCase("Banana is yellow", "Something something something", 1)]
        [TestCase("Something something something", "Banana is yellow", -1)]
        [TestCase("Cherry is the best", "Something something something", 1)]
        [TestCase("Something something something", "Cherry is the best", -1)]
        [TestCase("a", "a", 0)]
        [TestCase("A", "a", 0)]
        [TestCase("a", "A", 0)]
        [TestCase("abc", "abc", 0)]
        [TestCase("Abc", "abc", 0)]
        [TestCase("abc", "Abc", 0)]
        [TestCase("aBc", "abc", 0)]
        [TestCase("abc", "aBc", 0)]
        [TestCase("abC", "abc", 0)]
        [TestCase("abc", "abC", 0)]
        [TestCase("abcd", "abc", -1)]
        [TestCase("abc", "abcd", 1)]
        [TestCase("Abcd", "abc", -1)]
        [TestCase("abc", "Abcd", 1)]
        [TestCase("abcd", "Abc", -1)]
        [TestCase("Abc", "abcd", 1)]
        [TestCase("aBcd", "abc", -1)]
        [TestCase("abc", "aBcd", 1)]
        [TestCase("abcd", "aBc", -1)]
        [TestCase("aBc", "abcd", 1)]
        [TestCase("abcd", "abC", -1)]
        [TestCase("abC", "abcd", 1)]
        [TestCase("abCd", "abc", -1)]
        [TestCase("abc", "abCd", 1)]
        [TestCase("abcD", "abc", -1)]
        [TestCase("abc", "abcD", 1)]
        [TestCase("abCD", "abC", -1)]
        [TestCase("abC", "abCD", 1)]
        [TestCase("", "a", 1)]
        [TestCase("a", "", -1)]
        [TestCase("", "", 0)]
        [TestCase("a", "b", 1)]
        [TestCase("b", "a", -1)]
        public void Compare_Work_Correct(string firstValue, string secondValue, int result)
        {
            var row1 = new RightPartRow(firstValue);
            var row2 = new RightPartRow(secondValue);

            Assert.AreEqual(result, row1.CompareTo(row2));
        }


        [Test]
        public void Empty_Value_Correct()
        {
            var rowPart = RightPartRow.Empty;

            Assert.AreEqual(string.Empty, rowPart.Value);
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(500)]
        [TestCase(5000)]
        [TestCase(50000)]
        public void Created_Lenght_Correct(int lenght)
        {
            var rowPart = RightPartRow.GetRandom(lenght);

            Assert.AreEqual(lenght, rowPart.Value.Length);
        }

    }
}
