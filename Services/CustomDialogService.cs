using DevExpress.Xpf.Core;
using DevExpress.Mvvm;
using System;
using System.Windows;
using DevContactBook.ViewModels;
using DevContactBook.Views;
using System.Windows.Controls;

namespace DevContactBook.Services
{
    public class CustomDialogService : ICustomDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomDialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool ShowDialog<TViewModel>(string title, TViewModel viewModel) where TViewModel : ViewModelBase
        {
            var dialogWindow = new DXWindow
            {
                Title = title,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInTaskbar = false,
                Topmost = true,
                DataContext = viewModel
            };

            if (typeof(TViewModel) == typeof(AddUpdateContactViewModel))
            {
                var view = new AddUpdateContactView(viewModel as AddUpdateContactViewModel);
                dialogWindow.Content = view;
            }
            else
            {
                dialogWindow.Content = new ContentControl { Content = viewModel };
            }

            var result = dialogWindow.ShowDialog();
            return result.HasValue && result.Value;
        }
    }
}
