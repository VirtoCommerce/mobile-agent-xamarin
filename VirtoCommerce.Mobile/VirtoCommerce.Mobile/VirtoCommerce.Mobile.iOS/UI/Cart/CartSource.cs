using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace VirtoCommerce.Mobile.iOS.UI.Cart
{
    public class CartSource : UITableViewSource
    {
        public Model.Cart Cart { set; get; }
        private readonly Action<Model.CartItem> _updateCartItem; 
        public CartSource(UITableView tableView, Model.Cart cart, Action<Model.CartItem> updateCartItem)
        {
            tableView.RegisterClassForCellReuse(typeof(CartCell), CartCell.CellId);
            _updateCartItem = updateCartItem;
            Cart = cart;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CartCell.CellId, indexPath) as CartCell;
            cell.UpdateCell(Cart.CartItems.ElementAt(indexPath.Row), _updateCartItem);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Cart.CartItems.Count;
        }
    }
}