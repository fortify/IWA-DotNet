using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFocus.InsecureWebApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Discount { get; set; }
        public virtual ApplicationUser User { get; set; }
        public List<OrderDetail> LineItems { get; set; }

        [NotMapped]
        public bool IsAdmin { get; set; }
    }


    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Product Product { get; set; }
        public int Qty { get; set; }
    }
}
