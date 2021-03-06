﻿namespace VirtoCommerce.Mobile.Model
{
    public class CartItem
    {
        public string Id { set; get; }
        public Product Product { set; get; }
        public int Quantity { set; get; }
        public Currency Currency { set; get; }
        public decimal SubTotal { set; get; }
        public string FormattedSubTotal
        {
            get
            {
                return string.Format("{0}{1:#0.00}", Currency?.Symbol, SubTotal);
            }
        }

        public decimal Discount { get; set; }
    }
}
