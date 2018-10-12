using System.Collections.Generic;

namespace AdsaImporter.Models
{
    class Product
    {
        public long ProductNumber { get; set; }
        public double Price { get; set; }
        public string Subcategory { get; set; }
        public string Category { get; set; }
        public List<Order> orders { get; set; }
    }
}