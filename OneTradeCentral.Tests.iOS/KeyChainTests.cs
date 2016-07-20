using System;
using NUnit.Framework;

using OneTradeCentral.iOS;

namespace OneTradeCentral.Tests.iOS
{
	[TestFixture]
	public class KeyChainTests
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
//
//		[Test]
//		[Ignore ("another time")]
//		public void Ignore ()
//		{
//			Assert.True (false);
//		}

		[Test]
		public void TestSaveKeyChainEntry() {
			Identity.Reset();
			Assert.Null (Identity.UserName);
			Assert.Null (Identity.Password);
			var username = "hello";
			var password = "world";
			Assert.True(Identity.SaveKeyChainEntry (username, password));
			Assert.AreSame (username, Identity.UserName);
			Assert.AreSame (password, Identity.Password);
			Identity.Reset();
			Assert.Null (Identity.UserName);
			Assert.Null (Identity.Password);
			Assert.True (Identity.FindKeyChainEntry ());
			Assert.NotNull (Identity.UserName);
			Assert.NotNull (Identity.Password);
			Assert.True (username.Equals(Identity.UserName));
			Assert.True (password.Equals(Identity.Password));
		}

		[Test]
		public void TestCreateDeleteKeyChainEntry() {
			if (!Identity.CreateKeyChainEntry ("hello", "create")) {
				Assert.True (Identity.DeleteKeyChainEntry ());
				Assert.Null (Identity.UserName);
				Assert.Null (Identity.Password);
			}
			var username = "hello";
			var password = "createNew";
			Assert.True (Identity.CreateKeyChainEntry (username, password));
			Assert.NotNull (Identity.UserName);
			Assert.NotNull (Identity.Password);
			Assert.AreSame (username, Identity.UserName);
			Assert.AreSame (password, Identity.Password);
		}

		[Test]
		public void TestFindKeyChainEntry() {
			Identity.Reset();
			Assert.Null (Identity.UserName);
			Assert.Null (Identity.Password);
			if (Identity.FindKeyChainEntry()) {
				Assert.NotNull (Identity.UserName);
				Assert.NotNull (Identity.Password);
			} else {
				Assert.Null (Identity.UserName);
				Assert.Null (Identity.Password);
				var username = "hello";
				var password = "create";
				Identity.CreateKeyChainEntry (username, password);
				Assert.True (Identity.FindKeyChainEntry());
				Assert.NotNull (Identity.UserName);
				Assert.NotNull (Identity.Password);
				Assert.True (username.Equals (Identity.UserName));
				Assert.True (password.Equals (Identity.Password));
			}
		}

		[Test]
		public void TestDeleteKeyChainEntry() {
			var username = "hi";
			var password = "there";
			if (Identity.DeleteKeyChainEntry ()) {
				Assert.Null (Identity.UserName);
				Assert.Null (Identity.Password);
			} else {
				Assert.True (Identity.CreateKeyChainEntry (username, password));
				Assert.True (username.Equals (Identity.UserName));
				Assert.True (password.Equals (Identity.Password));
				Assert.True (Identity.DeleteKeyChainEntry ());
				Assert.Null (Identity.UserName);
				Assert.Null (Identity.Password);
			}
		}

	}
}
