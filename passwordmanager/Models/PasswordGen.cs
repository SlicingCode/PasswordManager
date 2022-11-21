using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passwordmanager.Models
{
    public class PasswordGen : INotifyPropertyChanged
    {
        private string? _generated;

        public string? Generated
        {
            get { return _generated; }
            set
            {
                var pwd = new Password(12);
                var password = pwd.Next();
                _generated = password;
                OnPropertyChanged("Generated");
            }
        }

        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
