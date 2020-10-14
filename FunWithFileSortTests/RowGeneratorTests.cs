using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FunWithFileSort;
using NUnit.Framework;

namespace FunWithFileSortTests
{
    class RowGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Generate_Row_Correct()
        {
            var rowGeneratorEnumerator = new RowGenerator().GetEnumerator().GetEnumerator();
            Assert.IsTrue(rowGeneratorEnumerator.MoveNext());
            var row = rowGeneratorEnumerator.Current;

            Assert.AreEqual(2, row.GetRowAsString().Split('.').Length);
        }

        [Test]
        [TestCase(6)]
        [TestCase(8)]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(500)]
        [TestCase(5000)]
        [TestCase(50000)]
        public void Generate_Row_By_Size_Correct(long size)
        {
            var row = new RowGenerator().GenerateRow(size);

            Assert.AreEqual(size, row.GetSizeInBytes());
        }

        public void Generate_Exseption_When_try_to_Create_Row_Small_Size(long size)
        {
            Assert.Throws<ArgumentException>(() => new RowGenerator().GenerateRow(2));
        }

        public void Generate_Exseption_When_Try_To_Create_Row_Smalled_Size_Than_Can()
        {
            Assert.Throws<ArgumentException>(() => new RowGenerator().GenerateRow(2));
        }

        public void Generate_Exseption_When_Try_To_Create_Row_With_Odd_Size()
        {
            Assert.Throws<ArgumentException>(() => new RowGenerator().GenerateRow(111));
        }

    }
}
