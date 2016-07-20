using System;
using OneTradeCentral.DAO;
using NUnit.Framework;
using OneTradeCentral.iOS;

namespace OneTradeCentral.Tests.iOS
{
	[TestFixture]
	public class DeviceIDTests
	{
//		[Test]
//		public void Pass ()
//		{
//			Assert.True (true);
//		}
//
//		[Test]
//		public void Fail ()
//		{
//			Assert.False (true);
//		}

		[Test]
		public void TestUniqueDeviceID() {
			var deviceID1 = Identity.DeviceID;
			Console.WriteLine ("Device ID 1:" + deviceID1);
			Assert.NotNull (deviceID1);
			var deviceID2 = Identity.DeviceID;
			Console.WriteLine ("Device ID 2:" + deviceID2);
			Assert.NotNull (deviceID2);
			// device ID's should have the same value but not necessarily be the same object
			Assert.AreEqual (deviceID1, deviceID2);
			Assert.AreNotSame (deviceID1, deviceID2);
		}

//		[Test]
//		[Ignore ("another time")]
//		public void Ignore ()
//		{
//			Assert.True (false);
//		}
	}
}
