using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FunWithFileSort;
using NUnit.Framework;


namespace FunWithFileSortTests
{
    class RandomTextGeneratorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Ctor_Generate_Argument_Null_Exception_Success()
        {
            Assert.Throws<ArgumentNullException>(delegate { new RandomTextGenerator(Row.Empty.GetSizeInBytes() + 1, null); });
        }


        class RowGeneratorWithConstantMinSize : IRowGenerator<Row>
        {
            public Row GenerateRow(long size)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Row> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public long GetRowMinSize()
            {
                return 5;
            }
        }


        [Test]
        public void Ctor_Generate_Argument_Exc_MinRowSize_Must_Be_More_Or_Equals_FileSize_Success()
        {
            var rowGenerator = new RowGeneratorWithConstantMinSize();
            var minRowSize = rowGenerator.GetRowMinSize();
            var fileSize = minRowSize - 1;
            var ex = Assert.Throws<ArgumentException>(delegate { new RandomTextGenerator(fileSize, rowGenerator); });

            Assert.That(ex.Message, Is.EqualTo($"file size must be more or equal {minRowSize}, but current value {fileSize}"));

        }

        [Test]
        [TestCase(300)]
        [TestCase(1000)]
        [TestCase(5000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000)]
        public void Generate_Text_With_Correct_Size(long filesize)
        {
            var rowGenerator = new RowGenerator();
            var filesizeGenerator = new RandomTextGenerator(filesize, rowGenerator);
            var sb = new StringBuilder();
            foreach(var row in filesizeGenerator)
            {
                sb.Append(row);
            }

            Assert.AreEqual(filesize, Encoding.Default.GetByteCount(sb.ToString()));
        }

    }
}
