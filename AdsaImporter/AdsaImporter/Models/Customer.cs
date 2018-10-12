using System;
using System.Collections.Generic;

namespace AdsaImporter.Models
{
    class Customer
    {

        public long CustomerNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PlaceOfResidense { get; set; }
        public List<Order> orders { get; set; }
    }
}