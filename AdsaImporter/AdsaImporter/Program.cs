using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdsaImporter.Models;

namespace AdsaImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            WaitHandle[] waitHandles = {
                new AutoResetEvent(false),
                new AutoResetEvent(false)
            };

            ThreadPool.QueueUserWorkItem(DoCustomers, waitHandles[0]);
            ThreadPool.QueueUserWorkItem(DoProducts, waitHandles[1]);


            WaitHandle.WaitAll(waitHandles);

            Importer importer = new Importer();
            Exporter exporter = new Exporter();

            Console.WriteLine("Start importing orders");
            List<Order> orders = importer.ImportOrders();

            Console.WriteLine("Start exporting orders");
            exporter.ExportOrders(orders);

            Console.WriteLine("Finished");
            Console.ReadLine();

        }

        private static void DoCustomers(object state)
        {
            Importer importer = new Importer();
            Exporter exporter = new Exporter();

            Console.WriteLine("Start importing customers");
            List<Customer> customers = importer.ImportCustomers();
            Console.WriteLine("Start exporting customers");
            exporter.ExportCustomers(customers);
            Console.WriteLine("Exporting customers finished");

            AutoResetEvent waitHandle = (AutoResetEvent) state;
            waitHandle.Set();
        }

        private static void DoProducts(object state)
        {
            Importer importer = new Importer();
            Exporter exporter = new Exporter();

            Console.WriteLine("Start importing products");
            List<Product> products = importer.ImportProducts();
            Console.WriteLine("Start exporing products");
            exporter.ExportProducts(products);
            Console.WriteLine("finished exporing products");

            AutoResetEvent waitHandle = (AutoResetEvent)state;
            waitHandle.Set();
        }
    }
}