using IMS.MobileApp.Models;
using IMS.MobileApp.PageModels;

namespace IMS.MobileApp.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}