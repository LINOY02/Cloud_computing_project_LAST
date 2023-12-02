using Cloud_computing_project_LAST.Data;
using Microsoft.EntityFrameworkCore.Storage;
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
        public List<CartItem>? CartItem { get; set; } = new List<CartItem>();

        public Cart()
        {
                
        }

        public Cart(int id, string UserId, int quantity, double totalPrice)
        {
            Id = id;
            this.userId=UserId;
            Quantity = quantity;
            TotalPrice = totalPrice;
            CartItem= new List<CartItem>();
        }
    }

}
  





