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

                        Product product = new Product();
                        product.ProductNumber = CleanNumber(values[7]);
                        product.Category = values[9];
                        product.Subcategory = values[8];
                        product.Price = Convert.ToDouble(values[11]) / Convert.ToDouble(values[10]);

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

            return customers;
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
    }
}