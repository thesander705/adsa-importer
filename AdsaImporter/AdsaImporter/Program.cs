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
            Console.WriteLine("Start orders");
            List<Order> orders = importer.ImportOrders();

        }
    }
}