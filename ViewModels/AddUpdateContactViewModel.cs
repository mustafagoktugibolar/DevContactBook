using System.Windows.Input;
using DevExpress.Mvvm;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using DevContactBook.Models;
using System.Collections.ObjectModel;


namespace DevContactBook.ViewModels
{
    public class AddUpdateContactViewModel : ViewModelBase
    {
        public AddUpdateContactViewModel(Contact contact)
        {
            SelectedContact = contact;

            SaveContactCommand = new DelegateCommand(async () => await SaveContactAsync());
            CancelCommand = new DelegateCommand(async () => await CancelAsync());
        }

        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                RaisePropertyChanged(nameof(SelectedContact));
            }
        }

        public ICommand SaveContactCommand { get; }
        public ICommand CancelCommand { get; }

        private async Task SaveContactAsync()
        {

            Messenger.Default.Send(new ContactMessage(SelectedContact));
            await CloseWindowAsync();
        }

        private async Task CancelAsync()
        {
            await CloseWindowAsync();
        }

        private async Task CloseWindowAsync()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            if (window != null)
            {
                await window.Dispatcher.InvokeAsync(window.Close);
            }
        }
    }
}
