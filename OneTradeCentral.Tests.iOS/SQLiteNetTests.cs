
using System;
using System.IO;
using NUnit.Framework;

using SQLite;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.Tests.iOS
{
	[TestFixture]
	public class SQLiteNetTests
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
		public void TestTableCreateInsert() {
			var db = new SQLiteConnection (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "OneTradeCentral.DB"));
//			SQLiteAsyncConnection db = new SQLiteAsyncConnection (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "todo.db"));
			Console.WriteLine("DB Path: " + Environment.SpecialFolder.Personal);
			db.CreateTable<Customer>();
			Assert.That(db.Table<Customer>().Count() == 0);
			Customer c = new Customer();
			c.BillToCity = "Taguig City";
			c.BillToCountry = "PH";
			c.BillToStreet1 = "32nd Avenue";
			c.BillToStreet2 = "Bonifacio Global City";
			int pk = db.Insert(c);
			Assert.That(db.Table<Customer>().Count() > 0);
			Assert.That (pk > 0);
			db.Insert(c);
			Assert.That(db.Table<Customer>().Count() > 1);
			db.DropTable<Customer>();
			db.Close();
		}

		[Test]
		public void TestCreateTables() {
			var db = new SQLiteConnection (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "OneTradeCentral.DB"));
			Console.WriteLine("DB Path: " + Environment.SpecialFolder.Personal);

			db.CreateTable<Customer>();
			Assert.That(db.Table<Customer>().Count() >= 0);

			db.CreateTable<Distributor>();
			Assert.That(db.Table<Distributor>().Count() >= 0);

			db.CreateTable<Order>();
			Assert.That(db.Table<Order>().Count() >= 0);

			db.CreateTable<OrderLine>();
			Assert.That(db.Table<OrderLine>().Count() >= 0);

			db.CreateTable<PaymentTerms>();
			Assert.That(db.Table<PaymentTerms>().Count() >= 0);

			db.CreateTable<Product>();
			Assert.That(db.Table<Product>().Count() >= 0);

			db.CreateTable<ProductCategory>();
			Assert.That(db.Table<ProductCategory>().Count() >= 0);
			db.Close();
		}
	}
}
