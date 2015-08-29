using System;
using NUnit.Framework;

namespace ConvertToArab
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void Test_Exception ()
		{
			var test = new ConvertToArabic();
			Assert.Throws<Exception>( () => test.Convert("IW"));
		}

		[Test()]
		public void Test_I ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(1,test.Convert("I") );
		}

		[Test()]
		public void Test_II ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(2,test.Convert("II") );
		}

		[Test()]
		public void Test_III ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(3,test.Convert("III") );
		}

		[Test()]
		public void Test_IV ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(4,test.Convert("IV") );
		}

		[Test()]
		public void Test_V ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(5,test.Convert("V") );
		}

		[Test()]
		public void Test_VI ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(6,test.Convert("VI") );
		}

		[Test()]
		public void Test_VII ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(7,test.Convert("VII") );
		}

		[Test()]
		public void Test_VIII ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(8,test.Convert("VIII") );
		}

		[Test()]
		public void Test_IX ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(9,test.Convert("IX") );
		}

		[Test()]
		public void Test_X ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(10,test.Convert("X") );
		}

		[Test()]
		public void Test_XI ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(11,test.Convert("XI") );
		}

		[Test()]
		public void Test_XII ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(12,test.Convert("XII") );
		}

		[Test()]
		public void Test_XIV ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(14,test.Convert("XIV") );
		}

		[Test()]
		public void Test_XIX ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(19,test.Convert("XIX") );
		}

		[Test()]
		public void Test_XX ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(20,test.Convert("XX") );
		}

		[Test()]
		public void Test_XVII ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(17,test.Convert("XVII") );
		}

		[Test()]
		public void Test_XLII ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(42,test.Convert("XLII") );
		}

		[Test()]
		public void Test_XXXI ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(31,test.Convert("XXXI") );
		}

		[Test()]
		public void Test_XLIV ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(44,test.Convert("XLIV") );
		}

		[Test()]
		public void Test_C ()
		{
			var test = new ConvertToArabic();
			Assert.AreEqual(100,test.Convert("C") );
		}
	}
}

