using System;
using System.Collections.Generic;

namespace AdsaImporter.Models
{
    class Order
    {
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int ExpectedDeliveryTime { get; set; }
        public int ActualDeliveryTime { get; set; }
        public string ReasonOfReturn { get; set; }
        public int Rating { get; set; }
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; }
    }
}