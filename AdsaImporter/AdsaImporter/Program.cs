using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdsaImporter.Models;

namespace AdsaImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Importer importer = new Importer();
            Exporter exporter = new Exporter();

            Console.WriteLine("Start importing customer");
            List<Customer> customers = importer.ImportCustomers();
            Console.WriteLine("Start exporting customer");
            exporter.ExportCustomers(customers);

            Console.WriteLine("Start importing products");
            List<Product> products = importer.ImportProducts();
            Console.WriteLine("Start exporing products");
            exporter.ExportProducts(products);

            Console.WriteLine("Start importing orders");
            List<Order> orders = importer.ImportOrders();

            Console.WriteLine("Start exporting orders");
            exporter.ExportOrders(orders);

        }
    }
}