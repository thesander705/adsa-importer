using System;
using System.Collections.Generic;

namespace AdsaImporter.Models
{
    class Order
    {
        public long OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public long ExpectedDeliveryTime { get; set; }
        public long ActualDeliveryTime { get; set; }
        public string ReasonOfReturn { get; set; }
        public int? Rating { get; set; }
        public long Customer { get; set; }
        public List<long> Products { get; set; }
    }
}