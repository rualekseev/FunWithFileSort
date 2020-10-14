using FunWithFileSort;
using NUnit.Framework;
using System;

namespace FunWithFileSortTests
{
    class LeftPartRowTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("0", "0", 0)]
        [TestCase("555", "555", 0)]
        [TestCase("2", "1", -1)]
        [TestCase("1", "2", 1)]
        [TestCase("123", "124", 1)]
        [TestCase("124", "123", -1)]
        [TestCase("500", "50", -1)]
        [TestCase("50", "500", 1)]
        [TestCase("987654321", "987654321", 0)]
        [TestCase("987654321", "987654320", -1)]
        [TestCase("987654320", "987654321", 1)]
        [TestCase("987654320", "98765432", -1)]
        [TestCase("00052", "52", -1)]
        [TestCase("52", "00052", 1)]
        [TestCase("00052", "00052", 0)]
        [TestCase("1", "415", 1)]
        [TestCase("415", "1", -1)]
        [TestCase("415", "1", -1)]
        [TestCase("1", "1", 0)]
        [TestCase("001", "1", -1)]
        public void Compare_Work_Correct(string firstValue,string secondValue,int expectedResult)
        {
            var row1 = new LeftPartRow(firstValue);
            var row2 = new LeftPartRow(secondValue);

            Assert.AreEqual(expectedResult, row1.CompareTo(row2));
        }

        [Test]
        public void Empty_Value_Correct()
        {
            var leftRowPart = LeftPartRow.Empty;

            Assert.AreEqual("0", leftRowPart.Value);
        }


        [Test]
        public void Create_With_Zero_Rank_Generate_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => LeftPartRow.GetRandom(0));
        }

        [Test]
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(5000)]
        public void Create_With_Rank_Correct(int rank)
        {
            var leftPart = LeftPartRow.GetRandom(rank);
            Assert.AreEqual(rank, leftPart.Value.Length);
        }

    }
}
