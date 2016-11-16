using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MvvmCross.iOS.Views;
using VirtoCommerce.Mobile.Model;
using VirtoCommerce.Mobile.ViewModels;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using System.Drawing;
using VirtoCommerce.Mobile.iOS.UI;
using VirtoCommerce.Mobile.iOS.UI.ProductDetail;
using VirtoCommerce.Mobile.iOS.NativConvertors;
using Xamarin.Themes;
using System.IO;

namespace VirtoCommerce.Mobile.iOS.Views
{

    public class DetailProductView : MvxViewController
    {

        public DetailProductViewModel DetailViewModel { get { return (DetailProductViewModel)ViewModel; } }

        public const int Padding = 10;
        public DetailProductView() : base(null, null)
        {

            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            CreateView();
            var set = this.CreateBindingSet<DetailProductView, DetailProductViewModel>();
            set.Bind(_titleLabel).To(x => x.Product.Name);
            set.Bind(_descriptionLabel).To(x => x.ProductDescription);
            set.Bind(_salePriceLable).To(x => x.Product.Price.FormattedSalePriceFull);
            set.Bind(NavigationItem).For(x => x.Title).To(x => x.Product.Name);
            set.Bind(_listPriceLable).For(x => x.Text).To(x => x.Product.Price.FormattedListPriceFull);
            set.Bind(_manufactureLabel).For(x => x.Text).To(x => x.Product.Manufacture);
            set.Bind(_profitPriceLabel).For(x => x.Text).To(x => x.Product.Price).WithConversion(new ProfitConvertor());
            set.Bind(_descriptionLabel).For(x => x.Hidden).To(x => x.HideDescription);
            set.Bind(_propertiesTable).For(x => x.Hidden).To(x => x.HideProperties);
            set.Bind(_cartButton).To(x => x.AddToCartCommand);
            set.Bind(_cartOpenButton).For(x => x.Title).To(x => x.CountInCart);
            set.Apply();
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBarHidden = false;
            _segmentControl.SelectedSegment = 0;
        }
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            /*var mainFrame = View.Frame;
            mainFrame.Y = NavigationController.NavigationBar.Frame.Height + UIApplication.SharedApplication.StatusBarFrame.Height;
            mainFrame.Height = mainFrame.Height - mainFrame.Y;
            View.Frame = mainFrame;*/
            PrepareMainInfo();
            //
            PrepareDetailInfo();
        }

        #region View

        // private UIImageView _productImage { get; set; }
        private UILabel _manufactureLabel { set; get; }
        private UIView _mainInfo { set; get; }
        private UIView _detailView { set; get; }
        private UILabel _titleLabel { get; set; }
        private UILabel _descriptionLabel { set; get; }
        private UIView _priceView { set; get; }
        private UILabel _salePriceLable { set; get; }
        private UILabel _listPriceLable { set; get; }
        private UILabel _profitPriceLabel { set; get; }
        private UIButton _cartButton { set; get; }
        private UIView _profitView { set; get; }
        private UIView _saleView { set; get; }
        private ImageSlider _imageSlider { set; get; }
        private UITableView _propertiesTable { set; get; }
        private UISegmentedControl _segmentControl { set; get; }
        private UIView _borderView { set; get; }
        private UIBarButtonItem _cartOpenButton { set; get; }

        private void CreateView()
        {
            View = new UIView(new CGRect(0, 0, 600, 600))
            {
                BackgroundColor = Consts.ColorMainBg,
                ContentMode = UIViewContentMode.ScaleToFill,
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
                AutosizesSubviews = true
            };

            #region main info
            _mainInfo = new UIView();
            //manufacture
            _manufactureLabel = new UILabel()
            {
                TextColor = Consts.ColorGray,
                Font = UIFont.FromName(Consts.FontNameRegular, 20),
                TextAlignment = UITextAlignment.Left,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
            };
            _mainInfo.Add(_manufactureLabel);
            //title
            _titleLabel = new UILabel(new CGRect(0, 5, 295, 40))
            {
                TextColor = Consts.ColorBlack,
                Font = UIFont.FromName(Consts.FontNameRegular, 30),
                TextAlignment = UITextAlignment.Center,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
            };
            _mainInfo.AddSubview(_titleLabel);
            //slider
            _imageSlider = new ImageSlider()
            {
                BackgroundColor = Consts.ColorMainBg
            };
            foreach (var img in DetailViewModel.Product.Images)
            {
                _imageSlider.AddImage(UIImage.FromFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), img)));
            }
            _mainInfo.Add(_imageSlider);
            //price
            _priceView = new UIView();
            _listPriceLable = new UILabel(new RectangleF(0, 5, 50, 30))
            {
                TextColor = Consts.ColorBlack,
                Font = UIFont.FromName(Consts.FontNameRegular, 25),
                TextAlignment = UITextAlignment.Center,
            };
            _priceView.Add(_listPriceLable);
            _priceView.Add(new UILabel(new CGRect(0, 0, 100, 45))
            {
                Text = "Price:",
                TextColor = Consts.ColorBlack,
                Font = UIFont.FromName(Consts.FontNameRegular, 25),
            });
            _mainInfo.AddSubview(_priceView);
            //profit price
            _profitView = new UIView();
            _profitPriceLabel = new UILabel(new RectangleF(0, 5, 50, 30))
            {
                TextColor = Consts.ColorBlack,
                Font = UIFont.FromName(Consts.FontNameRegular, 25),
                TextAlignment = UITextAlignment.Center,
            };
            _profitView.Add(_profitPriceLabel);
            _profitView.Add(new UILabel(new CGRect(0, 0, 300, 45))
            {
                Text = "Instant Saving:",
                TextColor = Consts.ColorBlack,
                Font = UIFont.FromName(Consts.FontNameRegular, 25),
            });
            _mainInfo.AddSubview(_profitView);
            //sale price
            _saleView = new UIView();
            _salePriceLable = new UILabel(new RectangleF(0, 5, 50, 30))
            {
                TextColor = UIColor.FromRGB(189, 13, 13),
                Font = UIFont.FromName(Consts.FontNameRegular, 35),
                TextAlignment = UITextAlignment.Center,
            };

            _saleView.Add(_salePriceLable);
            _saleView.Add(new UILabel(new CGRect(0, 0, 300, 45))
            {
                Text = "You Pay:",
                TextColor = UIColor.FromRGB(189, 13, 13),
                Font = UIFont.FromName(Consts.FontNameRegular, 35),
            });
            _mainInfo.AddSubview(_saleView);
            //
            #endregion

            #region Detail info
            _detailView = new UIView();

            //description
            _descriptionLabel = new UILabel(new RectangleF(0, 0, 295, 200))
            {
                Font = UIFont.FromName(Consts.FontNameRegular, 17),
                TextAlignment = UITextAlignment.Left,
                TextColor = Consts.ColorBlack,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
            };
            _detailView.AddSubview(_descriptionLabel);
            //tabbed
            _segmentControl = new UISegmentedControl(new CGRect(0, 0, 250, 50));
            _segmentControl.InsertSegment("Properties", 0, true);
            _segmentControl.InsertSegment("Description", 1, true);
            _segmentControl.ValueChanged += ChangeSegment;
            _detailView.AddSubviews(_segmentControl);
            //properties
            _propertiesTable = new UITableView();
            _propertiesTable.BackgroundColor = Consts.ColorMainBg;
            _propertiesTable.ScrollEnabled = true;
            _propertiesTable.Source = new PropertiesSource(DetailViewModel.Product.Properties.Select(x => new KeyValuePair<string, string>(x.DisplayName, x.Value)).ToList());
            _propertiesTable.RowHeight = 30;
            _propertiesTable.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            _propertiesTable.ReloadData();
            _detailView.AddSubview(_propertiesTable);
            //add to cart item
            _cartButton = Helpers.UICreator.CreateImageButtonWithText("Add to cart", "cart.png");
            _detailView.AddSubview(_cartButton);
            //border view
            _borderView = new UIView
            {
                BackgroundColor = Consts.ColorGray
            };
            Add(_borderView);
            #endregion
            //cart button
            _cartOpenButton = new UIBarButtonItem("Cart", UIBarButtonItemStyle.Plain, (s, e) => { DetailViewModel.OpenCartCommad.Execute(); });
            NavigationItem.RightBarButtonItem = _cartOpenButton;
            View.AddSubviews(_mainInfo, _detailView);
        }

        private void ChangeSegment(object o, EventArgs e)
        {
            ((DetailProductViewModel)ViewModel).SetSegment((int)_segmentControl.SelectedSegment);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _segmentControl.ValueChanged -= ChangeSegment;
            DetailViewModel.UnSubscribeEventor();
        }

        private void PrepareMainInfo()
        {
            var mainViewFrame = _mainInfo.Frame;
            mainViewFrame.Width = (View.Frame.Width / 2) - Padding;
            mainViewFrame.Height = View.Frame.Height;
            mainViewFrame.X = Padding;
            _mainInfo.Frame = mainViewFrame;
            //prepare manufacture
            _manufactureLabel.SizeToFit();
            var manufFrame = _manufactureLabel.Frame;
            manufFrame.Width = _mainInfo.Frame.Width;
            manufFrame.X = 0;
            manufFrame.Y = Padding;
            _manufactureLabel.Frame = manufFrame;
            //prepare title
            _titleLabel.SizeToFit();
            var titleFrame = _titleLabel.Frame;
            titleFrame.Width = _mainInfo.Frame.Width;
            titleFrame.X = Padding;
            titleFrame.Y = Padding + manufFrame.Height;
            _titleLabel.Frame = titleFrame;
            //prepare slider
            var sliderFrame = _imageSlider.Frame;
            sliderFrame.Width = mainViewFrame.Width;
            sliderFrame.Y = titleFrame.Y + titleFrame.Height + Padding;
            sliderFrame.Height = 400;
            _imageSlider.Frame = sliderFrame;
            _imageSlider.Draw(_imageSlider.Frame);
            //prepare Price
            //view
            _listPriceLable.SizeToFit();
            var priceViewFrame = _priceView.Frame;
            priceViewFrame.Width = mainViewFrame.Width;
            priceViewFrame.Height = _listPriceLable.Frame.Height == 0?30: _listPriceLable.Frame.Height;
            priceViewFrame.Y = sliderFrame.Y + sliderFrame.Height + Padding;
            _priceView.Frame = priceViewFrame;
            //price
            var listPriceFrame = _listPriceLable.Frame;
            listPriceFrame.X = mainViewFrame.Width - _listPriceLable.Frame.Width - Padding;
            listPriceFrame.Y = 0;
            _listPriceLable.Frame = listPriceFrame;
            //prepare profit
            //view
            _profitPriceLabel.SizeToFit();
            var profitViewFrame = _profitView.Frame;
            profitViewFrame.Width = mainViewFrame.Width;
            profitViewFrame.Height = _listPriceLable.Frame.Height == 0? 30: _listPriceLable.Frame.Height;
            profitViewFrame.Y = priceViewFrame.Y + priceViewFrame.Height + Padding;
            _profitView.Frame = profitViewFrame;
            //profit
            var profitPriceFrame = _profitPriceLabel.Frame;
            profitPriceFrame.X = mainViewFrame.Width - _profitPriceLabel.Frame.Width - Padding;
            profitPriceFrame.Y = 0;
            _profitPriceLabel.Frame = profitPriceFrame;
            //prepare profit
            //view
            _salePriceLable.SizeToFit();
            var saleViewFrame = _saleView.Frame;
            saleViewFrame.Width = mainViewFrame.Width;
            saleViewFrame.Height = _salePriceLable.Frame.Height;
            saleViewFrame.Y = profitViewFrame.Y + profitViewFrame.Height + Padding;
            _saleView.Frame = saleViewFrame;
            //profit
            var salePriceFrame = _salePriceLable.Frame;
            salePriceFrame.X = mainViewFrame.Width - _salePriceLable.Frame.Width - Padding;
            salePriceFrame.Y = 0;
            _salePriceLable.Frame = salePriceFrame;
            
        }

        private void PrepareDetailInfo()
        {
            var detailViewFrame = _detailView.Frame;
            detailViewFrame.Width = (View.Frame.Width / 2) - Padding;
            detailViewFrame.Height = View.Frame.Height;
            detailViewFrame.X = (View.Frame.Width / 2) + Padding;
            _detailView.Frame = detailViewFrame;

            //border
            var borderFrame = _borderView.Frame;
            borderFrame.X = detailViewFrame.X;
            borderFrame.Height = detailViewFrame.Height;
            borderFrame.Width = 1;
            _borderView.Frame = borderFrame;
            //segment control
            _segmentControl.SizeToFit();
            var segmentFrame = _segmentControl.Frame;
            segmentFrame.X = detailViewFrame.Width / 2 - segmentFrame.Width / 2;
            segmentFrame.Y = Padding;
            _segmentControl.Frame = segmentFrame;
            //description
            _descriptionLabel.SizeToFit();
            var descriptionFrame = _descriptionLabel.Frame;
            descriptionFrame.Width = detailViewFrame.Width - Padding;
            descriptionFrame.X = Padding;
            descriptionFrame.Y = _segmentControl.Frame.Y + _segmentControl.Frame.Height + Padding;
            _descriptionLabel.Frame = descriptionFrame;
            //properties
            var propertiesTableFrame = _propertiesTable.Frame;
            propertiesTableFrame.Width = detailViewFrame.Width - Padding;
            propertiesTableFrame.X = Padding;
            propertiesTableFrame.Y = segmentFrame.Y + segmentFrame.Height + Padding;
            propertiesTableFrame.Height = detailViewFrame.Height - (segmentFrame.Y + segmentFrame.Height + Padding + 80);
            _propertiesTable.Frame = propertiesTableFrame;
            _propertiesTable.ReloadData();
            //add to cart
            var addCartFrame = _cartButton.Frame;
            addCartFrame.X = detailViewFrame.Width - addCartFrame.Width - Padding;
            addCartFrame.Y = propertiesTableFrame.Height + propertiesTableFrame.Y + Padding;
            addCartFrame.Width = 200;
            addCartFrame.Height = 60;
            _cartButton.Frame = addCartFrame;
        }

        #endregion
    }
}