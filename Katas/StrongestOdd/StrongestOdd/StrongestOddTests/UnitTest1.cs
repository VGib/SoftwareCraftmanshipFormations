using NUnit.Framework;
using System;

namespace StrongestOddTests
{
    public class StrongestOddTests
    {
        [Test]
        public void MinIntervalShouldNotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => StrongestOdd.FindStrongestOdd(-7, 5));
        }

        [Test]
        public void MaxIntervalShouldNotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => StrongestOdd.FindStrongestOdd(-7, -1));
        }

        [Test]
        public void MinValueShouldnotBeGreaterThanmaxValue()
        {
            Assert.Throws<ArgumentException>(() => StrongestOdd.FindStrongestOdd(7, 1));
        }

        [Test]
        public void ShouldWokWhenIntervalIsASingleton()
        {
            var result = StrongestOdd.FindStrongestOdd(7, 7);
            Assert.AreEqual(7, result);
        }

        [Test]
        public void ShouldWokWhenIntervalIsASingleton_2()
        {
            var result = StrongestOdd.FindStrongestOdd(0, 0);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ShouldWokWhenIntervalIsASingleton_3()
        {
            var result = StrongestOdd.FindStrongestOdd(1, 1);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldFindStrongestOdd_1()
        {
            var result = StrongestOdd.FindStrongestOdd(0, 1);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ShouldFindStrongestOdd_2()
        {
            var result = StrongestOdd.FindStrongestOdd(0, 3);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void ShouldFindStrongestOdd_3()
        {
            var result = StrongestOdd.FindStrongestOdd(3, 10);
            Assert.AreEqual(8, result);
        }

        [Test]
        public void ShouldFindStrongestOdd_4()
        {
            var result = StrongestOdd.FindStrongestOdd(9, 13);
            Assert.AreEqual(12, result);
        }

        [Test]
        public void ShouldFindStrongestOdd_5()
        {
            var value = 2048 * 5 * 7 * 9 * 11;
            var result = StrongestOdd.FindStrongestOdd(value - 3  , value + 3 );
            Assert.AreEqual(value, result);
        }


        [Test]
        public void ShouldFindStrongestOdd_6()
        {
            var value = 2048 * 7 * 7 ;
            var result = StrongestOdd.FindStrongestOdd(value - 3, value + 3);
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldFindStrongestOdd_7()
        {
            var value = 256 * 3;
            var result = StrongestOdd.FindStrongestOdd(value - 3, value + 3);
            Assert.AreEqual(value, result);
        }

        [Test]
        public void ShouldFindStrongestOdd_8()
        {
            var result = StrongestOdd.FindStrongestOdd(0, 1027);
            Assert.AreEqual(1024, result);
        }

        [Test]
        public void ShouldHandleMaxValue()
        {
            var result = StrongestOdd.FindStrongestOdd(int.MaxValue - 4, int.MaxValue);
            Assert.AreEqual(int.MaxValue - 3, result);
        }
    }
}