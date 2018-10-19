using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        //        public void ExportOrders(List<Order> orders)
        //        {
        //            MySqlTransaction tr = null;
        //
        //            string connStr =
        //                "server=sanderdeboer.me;user=adsa;database=adsa;port=3306;password=Adsa1337!;SSL Mode=None;";
        //            MySqlConnection conn = new MySqlConnection(connStr);
        //            MySqlCommand comm = conn.CreateCommand();
        //            conn.Open();
        //
        //            tr = conn.BeginTransaction();
        //            comm.Transaction = tr;
        //
        //            foreach (Order order in orders)
        //            {
        //                comm.CommandText =
        //                    "INSERT INTO `adsa`.`Order` (`OrderNumber`, `OrderDate`, `ExpectedDeliveryTime`, `ActualDelivertTime`, `ReasonOfReturn`, `Rating`, `customerCustomerNumber`) VALUES (@OrderNumber, @OrderDate, @ExpectedDeliveryTime, @ActualDelivertTime, @ReasonOfReturn, @Rating, @customerCustomerNumber)";
        //                comm.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
        //                comm.Parameters.AddWithValue("@OrderDate", order.OrderDate);
        //                comm.Parameters.AddWithValue("@ExpectedDeliveryTime", order.ExpectedDeliveryTime);
        //                comm.Parameters.AddWithValue("@ActualDelivertTime", order.ActualDeliveryTime);
        //                comm.Parameters.AddWithValue("@ReasonOfReturn", order.ReasonOfReturn);
        //                comm.Parameters.AddWithValue("@Rating", order.Rating);
        //                comm.Parameters.AddWithValue("@customerCustomerNumber", order.Customer);
        //                comm.ExecuteNonQuery();
        //                comm.Parameters.Clear();
        //            }
        //
        //            tr.Commit();
        //            conn.Close();
        //        }

        public void ExportOrders(List<Order> orders)
        {
            orders = orders
                .GroupBy(p => p.OrderNumber)
                .Select(g => g.First())
                .ToList();

            string ConnectionString =
                "server=localhost;user=adsa;database=adsa;port=3306;password=Adsa1337!;SSL Mode=None;Connection Timeout=400";
            StringBuilder sCommand =
                new StringBuilder(
                    "INSERT INTO `adsa`.`Order` (`OrderNumber`, `OrderDate`, `ExpectedDeliveryTime`, `ActualDelivertTime`, `ReasonOfReturn`, `Rating`, `customerCustomerNumber`) VALUES ");
            using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
            {
                int count = 0;
                List<string> Rows = new List<string>();

                foreach (Order order in orders)
                {
                    string reasonOfreturn = "null";
                    if (!string.IsNullOrEmpty(order.ReasonOfReturn))
                    {
                        reasonOfreturn = order.ReasonOfReturn;
                    }

                    string rating = "null";
                    if (!string.IsNullOrEmpty(order.Rating.ToString()))
                    {
                        rating = order.Rating.ToString();
                    }

                    Rows.Add(string.Format(
                        $"({order.OrderNumber}, \'{order.OrderDate.ToString("yyyy-MM-dd H:mm:ss")}\', {order.ExpectedDeliveryTime}, {order.ActualDeliveryTime}, \'{MySqlHelper.EscapeString(reasonOfreturn)}\', {rating}, {order.Customer})"));
                    count++;
                    if (count < 15000)
                    {
                        continue;
                    }

                    sCommand.Append(string.Join(",", Rows));
                    sCommand.Append(";");
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                    {
                        myCmd.CommandType = CommandType.Text;
                        myCmd.ExecuteNonQuery();
                    }
                    mConnection.Close();
                    sCommand =
                        new StringBuilder(
                            "INSERT INTO `adsa`.`Order` (`OrderNumber`, `OrderDate`, `ExpectedDeliveryTime`, `ActualDelivertTime`, `ReasonOfReturn`, `Rating`, `customerCustomerNumber`) VALUES ");

                    Rows = new List<string>();

                    count = 0;
                }

                sCommand.Append(string.Join(",", Rows));
                sCommand.Append(";");
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
                mConnection.Close();

            }
        }

        public void ExportOrderProductRelations(List<ProductOrder> productOrders)
        {
            string ConnectionString =
                "server=localhost;user=adsa;database=adsa;port=3306;password=Adsa1337!;SSL Mode=None;Connection Timeout=4000";
            StringBuilder sCommand =
                new StringBuilder(
                    "INSERT INTO `adsa`.`order_product` (`OrderNumber`, `ProductNumber`, `Count`) VALUES ");
            using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
            {
                int count = 0;
                List<string> Rows = new List<string>();

                foreach (ProductOrder productOrder in productOrders)
                {
                    Rows.Add(string.Format(
                        $"({productOrder.OrderId}, {productOrder.ProductId}, {productOrder.Count})"));
                    count++;
                    if (count < 10000)
                    {
                        continue;
                    }

                    sCommand.Append(string.Join(",", Rows));
                    sCommand.Append(";");
                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                    {
                        myCmd.CommandType = CommandType.Text;
                        myCmd.ExecuteNonQuery();
                    }
                    mConnection.Close();
                    sCommand =
                        new StringBuilder(
                            "INSERT INTO `adsa`.`order_product` (`OrderNumber`, `ProductNumber`, `Count`) VALUES ");

                    Rows = new List<string>();

                    count = 0;
                }

                sCommand.Append(string.Join(",", Rows));
                sCommand.Append(";");
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
                mConnection.Close();

            }
        }
    }
}