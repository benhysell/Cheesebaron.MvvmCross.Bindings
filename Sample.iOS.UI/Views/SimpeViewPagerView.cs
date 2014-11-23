// --------------------------------------------------------------------------------------------------------------------
// <summary>
//    Defines the DisclaimerView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using Sample.iOS.UI.Views;

namespace Sample.iOS.UI
{
    using Core.ViewModels;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    /// <summary>
    /// Defines the DisclaimerView type.
    /// </summary>
    [Register("SimpleViewPagerView")]
    public class SimpleViewPagerView : BaseView
    {

        private SimpleListViewModel viewModel;
        public new SimpleListViewModel ViewModel
        {
            get { return viewModel ?? (viewModel = base.ViewModel as SimpleListViewModel); }
        }

        /// <summary>
        /// The scroll view.
        /// </summary>
        private UIScrollView scrollView;

        /// <summary>
        /// The page control.
        /// </summary>
        private readonly UIPageControl pageControl = new UIPageControl
        {
            PageIndicatorTintColor = UIColor.LightGray,
        };

        /// <summary>
        /// Active Page
        /// </summary>
        private int page;
        public int Page
        {
            get { return page; }
            set
            {
                pageControl.CurrentPage = value;
                page = value;
                scrollView.SetContentOffset(new PointF((value * UIScreen.MainScreen.Bounds.Width), 0), true);
            }
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        /// <summary>
        /// Called when the View is first loaded
        /// </summary>
        public override void ViewWillAppear(bool animated)     
        {
            base.ViewDidAppear(animated);
            Title = "UIPageViewController";
            NavigationController.NavigationBarHidden = true;
            View = new UIView { BackgroundColor = UIColor.White };

            pageControl.ValueChanged += HandlePageControlValueChanged;
            scrollView = new UIScrollView()
            {
                ShowsHorizontalScrollIndicator = false,
                ShowsVerticalScrollIndicator = false,
                Bounces = true,
                PagingEnabled = true,
                Frame = UIScreen.MainScreen.Bounds
            };
            scrollView.Scrolled += HandleScrollViewDecelerationEnded;
            pageControl.Frame = new RectangleF(0, scrollView.Bounds.Bottom - 10, scrollView.Bounds.Width, 10);
            View.AddSubviews(scrollView, pageControl);

            int i;
            for (i = 0; i < ViewModel.Items.Count; i++)
            {
                var pageViewController = CreatePage(ViewModel.Items[i]);
                //note on this screen we are placing it absolutely at the bottom of the picker
                //thus, y == 0
                pageViewController.View.Frame = new RectangleF(UIScreen.MainScreen.Bounds.Width * i, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
                scrollView.AddSubview(pageViewController.View);
            }
            scrollView.ContentSize = new SizeF(UIScreen.MainScreen.Bounds.Width * (i == 0 ? 1 : i), UIScreen.MainScreen.Bounds.Height);              
            pageControl.Pages = i; 
        }

        
        /// <summary>
        /// Given a ViewModel create a UIViewController for the UIPageViewController
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private UIViewController CreatePage(IMvxViewModel viewModel)
        {
            var controller = new UINavigationController();           
            var screen = this.CreateViewControllerFor(viewModel) as UIViewController;              
            controller.PushViewController(screen, false);           
            return controller;
        }

        /// <summary>
        /// Handles when the user touches the pageControl
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void HandlePageControlValueChanged(object sender, EventArgs e)
        {
            Page = pageControl.CurrentPage;
        }

        /// <summary>
        /// Fires after the user has scrolled the scroll view
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void HandleScrollViewDecelerationEnded(object sender, EventArgs e)
        {
            var pageScrolledTo = (int)Math.Floor((scrollView.ContentOffset.X - scrollView.Frame.Width / 2) / scrollView.Frame.Width) + 1;
            page = pageScrolledTo;
            pageControl.CurrentPage = pageScrolledTo;
        }
    }
}
