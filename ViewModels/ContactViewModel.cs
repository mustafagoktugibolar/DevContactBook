using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DevContactBook.Models;
using DevContactBook.Services;
using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace DevContactBook.ViewModels
{
    public class ContactViewModel : ViewModelBase
    {
        private const string DataFilePath = "C:\\Users\\mustafagoktugibolar\\source\\repos\\DevContactBook\\Data.json";
        private readonly ICustomDialogService _dialogService;

        public ContactViewModel(ICustomDialogService dialogService)
        {
            _dialogService = dialogService;
            Contacts = new ObservableCollection<Contact>();
            LoadContactsAsync();

            // Initialize Commands
            AddContactCommand = new DelegateCommand(AddContact);
            UpdateContactCommand = new DelegateCommand(UpdateContact, CanUpdateOrDeleteContact);
            DeleteContactCommand = new DelegateCommand(DeleteContact, CanUpdateOrDeleteContact);
            DarkThemeCommand = new DelegateCommand(SetDarkTheme);
            LightThemeCommand = new DelegateCommand(SetLightTheme);
            ExportToExcelCommand = new DelegateCommand(ExportToExcelAsync);

            // Register to receive messages from the AddUpdateContactViewModel
            Messenger.Default.Register<ContactMessage>(this, OnContactMessage);
            Debug.WriteLine("ContactViewModel created");
        }

        private ObservableCollection<Contact> contactsList;
        public ObservableCollection<Contact> Contacts
        {
            get => contactsList;
            set => SetProperty(ref contactsList, value, nameof(Contacts));
        }

        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (SetProperty(ref _selectedContact, value, nameof(SelectedContact)))
                {
                    UpdateContactCommand.RaiseCanExecuteChanged();
                    DeleteContactCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // Commands
        public DelegateCommand AddContactCommand { get; }
        public DelegateCommand UpdateContactCommand { get; }
        public DelegateCommand DeleteContactCommand { get; }
        public DelegateCommand DarkThemeCommand { get; }
        public DelegateCommand LightThemeCommand { get; }
        public DelegateCommand ExportToExcelCommand { get; }

        // Open the AddUpdateContactView to add a new contact
        private void AddContact()
        {
            OpenAddUpdateContactView(new Contact(), "Add Contact");
        }

        // Open the AddUpdateContactView to update a contact
        private void UpdateContact()
        {
            OpenAddUpdateContactView(SelectedContact, "Update Contact");
        }

        // Delete a contact
        private async void DeleteContact()
        {
            if (SelectedContact != null)
            {
                Contacts.Remove(SelectedContact);
                SelectedContact = null;
                await SaveContactsAsync();
            }
        }
        // Check if a contact is selected
        private bool CanUpdateOrDeleteContact()
        {
            return SelectedContact != null;
        }

        // themes
        private void SetDarkTheme()
        {
            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Win10DarkName;
        }

        private void SetLightTheme()
        {
            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Win10LightName;
        }

        // Load contacts from a json file
        private async Task LoadContactsAsync()
        {
            if (File.Exists(DataFilePath))
            {
                var json = await File.ReadAllTextAsync(DataFilePath);
                var contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(json);
                if (contacts != null)
                {
                    foreach (var contact in contacts)
                    {
                        Contacts.Add(contact);
                    }
                }
            }
        }
        // Save Contact to JSON file
        private async Task SaveContactsAsync()
        {
            var json = JsonConvert.SerializeObject(Contacts, Formatting.Indented);
            await File.WriteAllTextAsync(DataFilePath, json);
        }

        // Open the AddUpdateContactView
        private void OpenAddUpdateContactView(Contact contact, string title)
        {
            var manageContactViewModel = new AddUpdateContactViewModel(contact);
            var result = _dialogService.ShowDialog(
                title: title,
                viewModel: manageContactViewModel
            );
        }

        // Handle messages from the AddUpdateContactViewModel
        private async void OnContactMessage(ContactMessage message)
        {
            var contact = message.Contact;
            Debug.WriteLine($"Received message for {contact.FirstName} {contact.LastName}");
            if (!Contacts.Contains(contact))
            {
                Contacts.Add(contact);
            }
            await SaveContactsAsync();
        }
        // Export contacts to an Excel file
        private void ExportToExcelAsync()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Contacts");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "First Name";
            worksheet.Cell(currentRow, 2).Value = "Last Name";
            worksheet.Cell(currentRow, 3).Value = "Email";
            worksheet.Cell(currentRow, 4).Value = "Phone Number";
            worksheet.Cell(currentRow, 5).Value = "Address";
            worksheet.Cell(currentRow, 6).Value = "Favourite";

            foreach (var contact in Contacts)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = contact.FirstName;
                worksheet.Cell(currentRow, 2).Value = contact.LastName;
                worksheet.Cell(currentRow, 3).Value = contact.Email;
                worksheet.Cell(currentRow, 4).Value = contact.PhoneNumber;
                worksheet.Cell(currentRow, 5).Value = contact.Address;
                worksheet.Cell(currentRow, 6).Value = contact.IsFavourite;
            }

            worksheet.Columns().AdjustToContents();

            string filePath = "C:\\Users\\mustafagoktugibolar\\source\\repos\\DevContactBook\\Contacts.xlsx";

            workbook.SaveAs(filePath);

            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }

    // Message class to pass a Contact object
    public class ContactMessage
    {
        public ContactMessage(Contact contact)
        {
            Contact = contact;
        }

        public Contact Contact { get; }
    }
}
