﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using UIKit;
using VirtoCommerce.Mobile.CustomControls;
using VirtoCommerce.Mobile.iOS.CustomRenderers;
using VirtoCommerce.Mobile.iOS.UI.Products;
using VirtoCommerce.Mobile.Model;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Themes;

[assembly:ExportRenderer(typeof(ProductsTableView),typeof(ProductsTableViewRenderer))]
namespace VirtoCommerce.Mobile.iOS.CustomRenderers
{
    public class ProductsTableViewRenderer:ViewRenderer<ProductsTableView, UIView>
    {
        private GridView _gridView { set; get; }

        protected override void OnElementChanged(ElementChangedEventArgs<ProductsTableView> e)
        {
            base.OnElementChanged(e);
            var rectangle = new RectangleF(0,0,1024,768);
            /*rectangle.X = (float)e.NewElement.Bounds.X;
            rectangle.Y = (float)e.NewElement.Bounds.Y;
            rectangle.Location = new PointF(rectangle.X, rectangle.Y);
            rectangle.Height = (float)e.NewElement.Bounds.Height;
            rectangle.Width = (float)e.NewElement.Bounds.Width;
            rectangle.Size = new SizeF(rectangle.Width, rectangle.Height);*/
            _gridView = new GridView(rectangle)
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
                BackgroundColor = UIColor.DarkGray
            };
            if (e.NewElement?.Items != null)
            {
                SetGridView(e.NewElement.Items);
            }
            SetNativeControl(_gridView);
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var view = (ProductsTableView)sender;
            SetGridView(view.Items);
        }

        private void SetGridView(ICollection<Product> products)
        {
            if (products == null)
                return;
            _gridView.TileWidth = 241;
            foreach (var product in products)
            {
                var cell = new ProductCell();
                cell.Bind(product);
                _gridView.AddTile(cell);
            }
        }
        protected override void UpdateNativeWidget()
        {
            base.UpdateNativeWidget();
        }
    }
}
