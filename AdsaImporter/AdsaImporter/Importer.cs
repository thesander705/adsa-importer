using System;
using System.Collections.Generic;
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
                            Price = Convert.ToDouble(values[11]) / Convert.ToDouble(values[10])
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
                            DateOfBirth = DateTime.Parse(values[2]),
                            Gender = CleanGender(values[3]),
                            PlaceOfResidense = values[4]
                        };
                        customers.Add(customer);
                    }

                    startImport = true;
                }
            }

            List<Customer> customersCleaned = customers.GroupBy(x => x.CustomerNumber).Select(y => y.First()).ToList();
            return customersCleaned;
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
    }
}