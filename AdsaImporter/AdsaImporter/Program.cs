﻿using System;
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
            List<Product> products = importer.ImportProducts();
            importer.ImportProducts();
        }
    }
}