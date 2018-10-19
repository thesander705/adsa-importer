using System;
using System.Collections.Generic;
using System.Linq;
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
                    "INSERT INTO `adsa`.`Product` (`ProductNumber`, `Price`, `SubCategory`, `Category`) VALUES (@ProductNumber, @Price, @SubCategory, @Category)";
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

        public void ExportOrders(List<Order> orders)
        {
            MySqlTransaction tr = null;

            string connStr =
                "server=sanderdeboer.me;user=adsa;database=adsa;port=3306;password=Adsa1337!;SSL Mode=None;";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand comm = conn.CreateCommand();
            conn.Open();

            tr = conn.BeginTransaction();
            comm.Transaction = tr;

            foreach (Order order in orders)
            {
                comm.CommandText =
                    "INSERT INTO `adsa`.`Order` (`OrderNumber`, `OrderDate`, `ExpectedDeliveryTime`, `ActualDelivertTime`, `ReasonOfReturn`, `Rating`, `customerCustomerNumber`) VALUES (@OrderNumber, @OrderDate, @ExpectedDeliveryTime, @ActualDelivertTime, @ReasonOfReturn, @Rating, @customerCustomerNumber)";
                comm.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                comm.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                comm.Parameters.AddWithValue("@ExpectedDeliveryTime", order.ExpectedDeliveryTime);
                comm.Parameters.AddWithValue("@ActualDelivertTime", order.ActualDeliveryTime);
                comm.Parameters.AddWithValue("@ReasonOfReturn", order.ReasonOfReturn);
                comm.Parameters.AddWithValue("@Rating", order.Rating);
                comm.Parameters.AddWithValue("@customerCustomerNumber", order.Customer);
                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
            }

            tr.Commit();
            conn.Close();
        }

        public void ExportOrderProductRelations(List<ProductOrder> productOrders)
        {
            MySqlTransaction tr = null;

            string connStr =
                "server=sanderdeboer.me;user=adsa;database=adsa;port=3306;password=Adsa1337!;SSL Mode=None;";
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand comm = conn.CreateCommand();
            conn.Open();

            tr = conn.BeginTransaction();
            comm.Transaction = tr;

            foreach (ProductOrder productOrder in productOrders)
            {


                comm.CommandText =
                    "INSERT INTO `adsa`.`Order_Product` (`OrderNumber`, `ProductNumber`, `Count`) VALUES (@OrderNumber, @ProductNumber, @Count)";
                comm.Parameters.AddWithValue("@OrderNumber", productOrder.OrderId);
                comm.Parameters.AddWithValue("@ProductNumber", productOrder.ProductId);
                comm.Parameters.AddWithValue("@Count", productOrder.Count);
                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
            }


            tr.Commit();
            conn.Close();
        }
    }
}