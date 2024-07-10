using System.Windows.Input;
using DevExpress.Mvvm;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using DevContactBook.Models;


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

        // Create SelectedContact property
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
        // Create Commands
        public IDelegateCommand SaveContactCommand { get; }
        public IDelegateCommand CancelCommand { get; }


        // Send message to ContactViewModel for saving the contact
        private async Task SaveContactAsync()
        {

            Messenger.Default.Send(new ContactMessage(SelectedContact));
            await CloseWindowAsync();
        }

        // Close the window 
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
