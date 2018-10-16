using System;
using System.Collections.Generic;
using AdsaImporter.Models;
using MySql.Data.MySqlClient;

namespace AdsaImporter
{
    class Exporter
    {
        public void ExportCustomers(List<Customer> customers)
        {
            MySqlTransaction tr = null;

            string connStr =
                "server=sanderdeboer.me;user=adsa;database=adsa;port=3306;password=Adsa1337!;SSL Mode=None;";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand comm = conn.CreateCommand();
            conn.Open();
            tr = conn.BeginTransaction();
            comm.Transaction = tr;
            foreach (Customer customer in customers)
            {
                comm.CommandText =
                    "INSERT INTO `adsa`.`customer` (`CustomerNumber`,`DateOfBirth`, `Gender`, `PlaceOfResidense`) VALUES (@cn, @dob, @gender, @por)";
                comm.Parameters.AddWithValue("@cn", customer.CustomerNumber);
                comm.Parameters.AddWithValue("@dob", customer.DateOfBirth);
                comm.Parameters.AddWithValue("@gender", customer.Gender.ToString());
                comm.Parameters.AddWithValue("@por", customer.PlaceOfResidense);
                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
            }
            tr.Commit();
            conn.Close();

        }

        public void ExportProducts(List<Product> products)
        {
            MySqlTransaction tr = null;

            string connStr =
                "server=sanderdeboer.me;user=adsa;database=adsa;port=3306;password=Adsa1337!;SSL Mode=None;";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand comm = conn.CreateCommand();
            conn.Open();

            tr = conn.BeginTransaction();
            comm.Transaction = tr;

            foreach (Product product in products)
            {
                comm.CommandText =
                    "INSERT INTO `adsa`.`customer` (`CustomerNumber`,`DateOfBirth`, `Gender`, `PlaceOfResidense`) VALUES (@ProductNumber, @Price, @SubCategory, @Category)";
                comm.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);
                comm.Parameters.AddWithValue("@Price", product.Price);
                comm.Parameters.AddWithValue("@SubCategory", product.Subcategory);
                comm.Parameters.AddWithValue("@Category", product.Category);
                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
            }

            tr.Commit();
            conn.Close();

        }
    }
}