using aiala.mobile.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aiala.mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictureDetailView : ContentPage
    {
        public PictureDetailView()
        {
            InitializeComponent();
        }

        private async void DeleteButtonClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert(UiTexts.Communicate_DeleteTitle, UiTexts.Communicate_DeleteQuestion, UiTexts.General_Yes, UiTexts.General_No);
            if (result)
            {
                var viewModel = BindingContext as ViewModels.PictureDetailViewModel;
                viewModel.DeletePictureCommand.Execute(null);
            }
        }
    }
}