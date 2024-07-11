using System.Collections.Generic;
using System.ComponentModel;

namespace DevContactBook.Models
{
    
    public class Contact : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _email;
        private string _address;
        private bool _isFavourite;

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value, nameof(FirstName));
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value, nameof(LastName));
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value, nameof(PhoneNumber));
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value, nameof(Email));
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value, nameof(Address));
        }

        public bool IsFavourite
        {
            get => _isFavourite;
            set => SetProperty(ref _isFavourite, value, nameof(IsFavourite));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
