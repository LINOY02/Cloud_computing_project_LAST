using Cloud_computing_project_LAST.Data;
using System;
using System.Collections.Generic;
namespace Cloud_computing_project_LAST.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public List<CartItem>? CartItem { get; set; }
    }

}
  





