using aiala.mobile.Actions;
using aiala.mobile.Models;
using aiala.mobile.ViewModels;
using Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aiala.mobile.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CurrentTaskViewCell : ViewCell
	{
        public CurrentTaskViewCell()
		{
			InitializeComponent();
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var homeViewModel = Parent.BindingContext as HomeViewModel;
            homeViewModel.NavigateToTaskCommand.Execute(null);
        }
    }
}