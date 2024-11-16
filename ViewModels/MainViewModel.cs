using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace lifeManagementApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        public MainViewModel() {
            Items = new ObservableCollection<string>();
        }

        [ObservableProperty]
        ObservableCollection<string> items;

        [ObservableProperty]
        string text;

        [RelayCommand]
        void Add()
        {
            if (string.IsNullOrWhiteSpace(Text)) return;

            Items.Add(Text);
            // add task
            Text = string.Empty;
        }

        [RelayCommand]
        void Delete(string task)
        {
            if(Items.Contains(task)) Items.Remove(task);
        }
    }
}
