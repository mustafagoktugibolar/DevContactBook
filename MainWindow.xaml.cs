using DevContactBook.ViewModels;
using DevExpress.Xpf.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevContactBook
{
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = serviceProvider.GetRequiredService<ContactViewModel>();
        }
    }
}
