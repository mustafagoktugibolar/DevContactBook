using DevExpress.Xpf.Core;
using DevExpress.Mvvm;
using System.Windows;
using DevContactBook.ViewModels;
using DevContactBook.Views;
using System.Windows.Controls;

namespace DevContactBook.Services
{
    public class CustomDialogService : ICustomDialogService
    {
        // Show a dialog window
        public bool ShowDialog<TViewModel>(string title, TViewModel viewModel) where TViewModel : ViewModelBase
        {
            // Create a new dialog window
            var dialogWindow = new DXWindow
            {
                Title = title,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInTaskbar = false,
                Topmost = true,
                DataContext = viewModel
            };
            // Set the content of the dialog window based on the type of the view model
            if (typeof(TViewModel) == typeof(AddUpdateContactViewModel))
            {
                var view = new AddUpdateContactView(viewModel as AddUpdateContactViewModel);
                dialogWindow.Content = view;
            }
            else
            {
                dialogWindow.Content = new ContentControl { Content = viewModel };
            }
            // Show the dialog window
            var result = dialogWindow.ShowDialog();
            return result.HasValue && result.Value;
        }
    }
}
