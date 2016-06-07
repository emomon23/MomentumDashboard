using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.Model.DTOs
{
    public class Order
    {
        private IList<CartProduct> products = new List<CartProduct>();

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool GiftWrap { get; set; }

        public IList<CartProduct> Products
        {
            get { return this.products; }
            set
            {
                this.products = value;
            }
        }
    }


    public class CartProduct
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}