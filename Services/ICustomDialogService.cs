using DevExpress.Mvvm;

namespace DevContactBook.Services
{
    public interface ICustomDialogService
    {
        bool ShowDialog<TViewModel>(string title, TViewModel viewModel) where TViewModel : ViewModelBase;
    }
}
