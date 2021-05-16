using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LappyShop.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Order_ID { get; set; }

        [Required]
        public int laptop_id { get; set; }

        [Required]
        public string user_id { get; set; }

    }
}
