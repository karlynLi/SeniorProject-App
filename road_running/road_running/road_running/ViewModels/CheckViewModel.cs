using road_running.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using road_running.Providers;
using Xamarin.Forms;
using System.Collections.Specialized;

namespace road_running.ViewModels
{
    public class CheckViewModel : INotifyPropertyChanged
    {
        public CheckViewModel()
        {
            Getgift();
        }
        public string Searchtext = null;
        public event PropertyChangedEventHandler PropertyChanged;
        //public new event CollectionChangeEventHandler CollectionChanged;
        private static ObservableCollection<CheckIn> CheckGift { get; set; }
        //接收provider
        public static List<CheckIn> Initgift { get; set; }
        public async void Getgift()
        {
            var AppShellInstance = Xamarin.Forms.Shell.Current as AppShell;
            string id = AppShellInstance.Member_ID;
            Member member = new Member();
            member.Member_ID = id;
            Initgift = await GiftProvider.GiftAsync(member);
            Console.WriteLine("ViewModel裡面");
            Console.WriteLine(Initgift[0].Name);
            if (Initgift[0].Name == "noFile")
            {
                Text_Isvisible = true;
                List_Isvisible = false;
            }
            else
            {
                Text_Isvisible = false;
                List_Isvisible = true;
                ReturnCheck = AddGift();
            }
        }
        public ObservableCollection<CheckIn> ReturnCheck
        { //Property that will be used to get and set the item
            get { return CheckGift; }
            set
            {
                if (CheckGift != null)
                {
                    CheckGift = value;
                    OnPropertyChanged();
                    //OnCollectionChanged(NotifyCollectionChangedAction.Reset);
                }
            }
        }
        public ObservableCollection<CheckIn> AddGift()
        {
            CheckGift = new ObservableCollection<CheckIn>();
            for (int i = 0; i < Initgift.Count; i++)
            {
                CheckGift.Add(new CheckIn
                {
                    Name = Initgift[i].Name,
                    Registration_ID = Initgift[i].Registration_ID
                });
            }
            return CheckGift;
        }
        public string SearchText
        {
            get { return Searchtext; }
            set
            {
                if (Searchtext != value)
                {
                    Searchtext = value;
                }
                OnPropertyChanged();
            }
        }

        private static bool list_isvisible;
        public bool List_Isvisible
        {
            get { return list_isvisible; }
            set
            {
                list_isvisible = value;
                OnPropertyChanged();
            }
        }

        private static bool text_isvisible;
        public bool Text_Isvisible
        {
            get { return text_isvisible; }
            set
            {
                text_isvisible = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        //public static IList<CheckViewModel> All { private set; get; }
    }
}