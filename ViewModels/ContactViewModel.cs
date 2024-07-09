using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using ClosedXML.Excel;
using DevContactBook.Models;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace DevContactBook.ViewModels
{
    public class ContactViewModel : ViewModelBase
    {
        private const string DataFilePath = "C:\\Users\\mustafagoktugibolar\\source\\repos\\DevContactBook\\Data.json";
        protected IDialogService DialogService { get { return GetService<IDialogService>(); } }
        public ContactViewModel()
        {
            Contacts = new ObservableCollection<Contact>();
            LoadContactsAsync();

            //commands
            AddContactCommand = new DelegateCommand(AddContact);
            UpdateContactCommand = new DelegateCommand(UpdateContact, CanUpdateOrDeleteContact);
            DeleteContactCommand = new DelegateCommand(DeleteContact, CanUpdateOrDeleteContact);
            DarkThemeCommand = new DelegateCommand(SetDarkTheme);
            LightThemeCommand = new DelegateCommand(SetLightTheme);
            ExportToExcelCommand = new DelegateCommand(ExportToExcelAsync);

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
                    ((DelegateCommand)UpdateContactCommand).RaiseCanExecuteChanged();
                    ((DelegateCommand)DeleteContactCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand AddContactCommand { get; }
        public ICommand UpdateContactCommand { get; }
        public ICommand DeleteContactCommand { get; }
        public ICommand DarkThemeCommand { get; }
        public ICommand LightThemeCommand { get; }
        public ICommand ExportToExcelCommand { get; }

        private void AddContact()
        {
            OpenAddUpdateContactView(new Contact());
        }

        private void UpdateContact()
        {
            OpenAddUpdateContactView(SelectedContact);
        }

        private async void DeleteContact()
        {
            if (SelectedContact != null)
            {
                Contacts.Remove(SelectedContact);
                SelectedContact = null;
                await SaveContactsAsync();
            }
        }

        private bool CanUpdateOrDeleteContact()
        {
            return SelectedContact != null;
        }

        private void SetDarkTheme()
        {
            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Win10DarkName;
        }

        private void SetLightTheme()
        {
            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Win10LightName;
        }

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

        private async Task SaveContactsAsync()
        {
            var json = JsonConvert.SerializeObject(Contacts, Formatting.Indented);
            await File.WriteAllTextAsync(DataFilePath, json);
        }

        private void OpenAddUpdateContactView(Contact contact)
        {
            var manageContactViewModel = new AddUpdateContactViewModel(contact);
            var result = DialogService.ShowDialog(
                dialogButtons: MessageButton.OKCancel,
                title: "Edit Contact",
                viewModel: manageContactViewModel
            );
        }

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

            // Adjust columns to content
            worksheet.Columns().AdjustToContents();

            // Define a path to save the Excel file
            string filePath = "C:\\Users\\mustafagoktugibolar\\source\\repos\\DevContactBook\\Contacts.xlsx";

            // Save the workbook asynchronously
            workbook.SaveAs(filePath);

           // open the saved Excel file or notify the user that the export is complete
           System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });

        }
    }

    public class ContactMessage
    {
        public ContactMessage(Contact contact)
        {
            Contact = contact;
        }

        private Contact _contact;
        public Contact Contact
        {
            get => _contact;
            set => _contact = value;
        }
    }


}
