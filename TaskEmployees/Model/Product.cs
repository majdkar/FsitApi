using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskEmployees.Model
{
    public class Product
    {
        public int Id { get; set; }
        public int IdCategory { get; set; }
        public string TitleProduct { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime? CreateOfDate  { get; set; }
        public string ImageUrlProduct { get; set; }
    }
}
