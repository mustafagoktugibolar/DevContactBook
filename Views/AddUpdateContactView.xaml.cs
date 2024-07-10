
using DevContactBook.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;


namespace DevContactBook.Views
{
    /// <summary>
    /// Interaction logic for AddUpdateContactView.xaml
    /// </summary>
    public partial class AddUpdateContactView : UserControl
    {
        public AddUpdateContactView(AddUpdateContactViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
