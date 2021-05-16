using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LappyShop.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public string  description { get; set; }
        public string image { get; set; }
        public int price { get; set; }
    }
}
