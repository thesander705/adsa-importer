using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AdsaImporter.Models;

namespace AdsaImporter
{
    class Importer
    {
        private string csvFile = @"C:\Users\Sander\Desktop\out.csv";

        public List<Product> ImportProducts()
        {
            List<Product> products = new List<Product>();
            List<Product> productsCleaned;
            using (StreamReader reader = new StreamReader(csvFile))
            {
                bool startImport = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (startImport)
                    {
                        string[] values = line.Split(',');

                        Product product = new Product
                        {
                            ProductNumber = CleanNumber(values[7]),
                            Category = values[9],
                            Subcategory = values[8],
                            Price = Convert.ToDouble(values[11]) / Convert.ToDouble(values[10]),
                            orders = new List<Order>()
                        };

                        products.Add(product);
                    }

                    startImport = true;
                }

                productsCleaned = products.GroupBy(x => x.ProductNumber).Select(y => y.First()).ToList();
            }

            return productsCleaned;
        }

        public List<Customer> ImportCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (StreamReader reader = new StreamReader(csvFile))
            {
                bool startImport = false;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (startImport)
                    {
                        string[] values = line.Split(',');

                        Customer customer = new Customer
                        {
                            CustomerNumber = Convert.ToInt64(CleanNumber(values[1])),
                            DateOfBirth = Convert.ToDateTime(values[2]),
                            Gender = CleanGender(values[3]),
                            PlaceOfResidense = values[4],
                            orders = new List<Order>()
                        };
                        customers.Add(customer);
                    }

                    startImport = true;
                }
            }

            List<Customer> customersCleaned = customers.GroupBy(x => x.CustomerNumber).Select(y => y.First()).ToList();
            return customersCleaned;
        }

        public List<Order> ImportOrders()
        {
            List<Order> orders = new List<Order>();
            List<Customer> customers = ImportCustomers();
            List<Product> products = ImportProducts();

            using (StreamReader reader = new StreamReader(csvFile))
            {
                bool startImport = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (startImport)
                    {
                        string[] values = line.Split(',');

                        Order order = new Order()
                        {
                            OrderNumber = CleanNumber(values[5]),
                            OrderDate = CleanDateTime(values[6]),
                            ExpectedDeliveryTime = CleanNumber(values[12]),
                            ActualDeliveryTime = CleanNumber(values[13]),
                            Rating = cleanRating(values[15]),
                            ReasonOfReturn = values[14],
                            Customer = customers.FirstOrDefault(x => x.CustomerNumber == CleanNumber(values[1])),
                            Products = new List<Product>()
                        };
                        order.Customer.orders.Add(order);
                        order.Products.Add(products.FirstOrDefault(x => x.ProductNumber == CleanNumber(values[7])));
                        orders.Add(order);
                    }

                    startImport = true;
                }
            }

            foreach (Order order in orders)
            {
                foreach (Order orderSameId in orders.Where(x => x.OrderNumber == order.OrderNumber))
                {
                    foreach (Product product in orderSameId.Products)
                    {
                        product.orders.Add(order);
                        order.Products.Add(product);
                    }
                }
            }

            List<Order> ordersCleaned = orders.GroupBy(x => x.OrderNumber).Select(y => y.First()).ToList();

            return ordersCleaned;
        }

        private long CleanNumber(string productNumber)
        {
            string toReturn = productNumber;
            toReturn = toReturn.ToLower();
            toReturn = toReturn.Replace('o', '0');
            toReturn = toReturn.Replace('i', '1');
            toReturn = toReturn.Replace('l', '1');

            return Convert.ToInt64(toReturn);
        }

        private Gender CleanGender(string gender)
        {
            if (gender == "Man")
            {
                return Gender.Male;
            }

            if (gender == "Woman")
            {
                return Gender.Female;
            }

            return Gender.Other;
        }

        private int? cleanRating(string rating)
        {
            if (string.IsNullOrEmpty(rating))
            {
                return null;
            }

            int cleanedRating = Convert.ToInt32(rating);
            if (cleanedRating < 0)
            {
                return null;
            }

            return cleanedRating;
        }

        private DateTime CleanDateTime(string date)
        {
            date = date.Replace(' ', '0');
            DateTime dateCleaned;
            if (date.Contains('-'))
            {
                dateCleaned = DateTime.Parse(date);
            }
            else
            {
                dateCleaned = DateTime.ParseExact(date, "MM/dd/yy", null);
            }


            return dateCleaned;
        }
    }
}