// --------------------------------------------------------------------------------------------------------------------
// <summary>
//    Defines the LengthView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.BindingContext;
using Sample.iOS.UI.Views;


namespace Sample.iOS.UI
{
    using Core.ViewModels;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    /// <summary>
    /// Defines the LengthView type.
    /// </summary>
    [Register("SimpleView")]
    public class SimpleView : BaseView
    {
        private SimpleViewModel viewModel;
        public new SimpleViewModel ViewModel
        {
            get { return viewModel ?? (viewModel = base.ViewModel as SimpleViewModel); }
        }

        public Rectangle LocalFrame;

        /// <summary>
        /// Views the did load.
        /// </summary>
        /// <summary>
        /// Called when the View is first loaded
        /// </summary>
        public override void ViewDidLoad()
        {
            View = new UIView { BackgroundColor = UIColor.Red };

            base.ViewDidLoad();
            NavigationController.NavigationBarHidden = true;
           
            var startMessageLabel = new UILabel(new RectangleF(75, 75, 200, 200));
           
            View.AddSubviews(startMessageLabel);

            var set = this.CreateBindingSet<SimpleView, SimpleViewModel>();
            
            set.Bind(startMessageLabel).To(vm => vm.Name);
           
            set.Apply();
        }
    }
}
