using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IpAddressesv3
{
    public partial class MainPage : ContentPage
    {
        private GenViewModel GenVM = new GenViewModel();
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = GenVM;
        }

        private void Calculate(object sender, TextChangedEventArgs e)
        {
            GenVM.GetResults();
        }
    }
}
