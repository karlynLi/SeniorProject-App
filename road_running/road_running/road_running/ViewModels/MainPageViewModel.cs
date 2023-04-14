using System;
using Xamarin.Forms;
using road_running.Models;
using System.ComponentModel;
using System.Collections.Generic;
using road_running.Views;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using road_running.Providers;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Threading;
using Xamarin.Essentials;

namespace road_running.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged // INotifyPropertyChanged:讓UI可以變動
    {
        public MainPageViewModel()
        {
            //_ = InitGet();
            initGet();
        }
        

        // INotifyPropertyChanged實作介面
        public event PropertyChangedEventHandler PropertyChanged;
        public string searchtext = null; // 搜尋字串
        public static bool issigningup = false; // switch切換
        public static ObservableCollection<activity> activitys { get; set; } // 顯示在view的list(List)
        public static List<activity> initGetList { get; set; } // 用於接收ActivityProvider.cs
        public static List<city> CityList { get; set; } // 存縣市清單
        public static List<month> MonthList { get; set; } // 存月份清單

        Thread Get_Thread;  // 創建thread
        private void initGet()
        {
            Get_Thread = new Thread(async () =>
            {
                try
                {
                    List_IsVisible = false;
                    IsBusy = true;
                    // Call your web service here
                    initGetList = await ActivityProvider.GetActivitysAsync();
                    Activitys = AddListView();
                    GetCities = AddCityList();
                    GetMonths = AddMonthList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    TextContent = "伺服器無回應，請檢查網路狀態";
                    TextIsVisible = true;
                    List_IsVisible = false;
                    // Handle exception
                }
                finally
                {
                    IsBusy = false;
                    List_IsVisible = true;
                }
                
            });
            Get_Thread.Start();
        }

        //async Task InitGet() // 從ActivityProvider.cs取得活動清單
        //{
        //    try
        //    {
        //        List_IsVisible = false;
        //        IsBusy = true;

        //        // Call your web service here
        //        initGetList = await ActivityProvider.GetActivitysAsync();
        //        Activitys = AddListView();
        //        GetCities = AddCityList();
        //        GetMonths = AddMonthList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        TextContent = "伺服器無回應，請檢查網路狀態";
        //        TextIsVisible = true;
        //        List_IsVisible = false;
        //        // Handle exception
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //        List_IsVisible = true;
        //    }
        //}

        // 搜尋字串
        public string SearchText
        {
            get { return searchtext; }
            set
            {
                searchtext = value;
                OnPropertyChanged();
            }
        }
        // 是否顯示報名中
        public bool IsSigningUp
        {
            get { return issigningup; }
            set
            {
                if (issigningup != value)
                {
                    issigningup = value;
                    SelectedMonth = null;
                    SelectedCity = null;
                    SelectedKm = null;
                    List_IsVisible = false;
                    IsBusy = true;
                    Activitys = AddListView();
                    IsBusy = false;
                    List_IsVisible = true;
                }
            }
        }
        // Listview是否有改
        public ObservableCollection<activity> Activitys
        { //Property that will be used to get and set the item
            get
            {
                return activitys;
            }
            set
            {
                activitys = value;
                if (activitys.Count == 0)
                {
                    TextContent = "查無活動";
                    TextIsVisible = true;
                }
                else
                {
                    TextIsVisible = false;
                }
                OnPropertyChanged(nameof(Activitys));
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        // 更新List
        public ObservableCollection<activity> AddListView()
        {
            activitys = new ObservableCollection<activity>();
            if (issigningup == false) // 如果switch關閉，顯示所有活動
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    activitys.Add(new activity
                    {
                        Id = initGetList[i].Id,
                        Name = initGetList[i].Name,
                        Location = initGetList[i].Location,
                        Date = initGetList[i].Date,
                        Startdate = initGetList[i].Startdate,
                        Enddate = initGetList[i].Enddate,
                        Group = initGetList[i].Group,
                        ImageUrl = initGetList[i].ImageUrl,
                        ImageName = initGetList[i].ImageName
                    });
                }
            }
            else // 顯示報名中活動
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    // 判斷現在日期是否介於報名日期中間
                    if (initGetList[i].Startdate <= DateTime.Now && DateTime.Now <= initGetList[i].Enddate)
                    {
                        activitys.Add(new activity
                        {
                            Id = initGetList[i].Id,
                            Name = initGetList[i].Name,
                            Location = initGetList[i].Location,
                            Date = initGetList[i].Date,
                            Startdate = initGetList[i].Startdate,
                            Enddate = initGetList[i].Enddate,
                            Group = initGetList[i].Group,
                            ImageUrl = initGetList[i].ImageUrl,
                            ImageName = initGetList[i].ImageName
                        });
                    }
                }
            }
            return activitys;
        }

        // 將縣市加入list
        public List<city> AddCityList()
        {
            CityList = new List<city>();
            for (int i = 0; i < initGetList.Count; i++)
            {
                // 如果list裡面沒有此縣市才加入，若已有則不加入
                if (!CityList.Exists(t => t.Name == initGetList[i].Location.Substring(0, 3)))
                {
                    CityList.Add(new city
                    {
                        Name = initGetList[i].Location.Substring(0, 3)
                    });
                }
            }
            return CityList;
        }
        // CityPicker Binding
        public List<city> GetCities
        {
            get { return CityList; }
            set
            {
                CityList = value;
                OnPropertyChanged();
            }
        }
        city selectedCity = null; // 存放選擇到的城市
        public city SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged();
            }
        }

        // 將月份加入list
        public List<month> AddMonthList()
        {
            MonthList = new List<month>();
            for (int i = 0; i < initGetList.Count; i++)
            {
                //var result = MonthList.Exists(t => t.Month == initGetList[i].Date.Month);
                if (!MonthList.Exists(t => t.Month == initGetList[i].Date.Month.ToString()))
                {
                    MonthList.Add(new month
                    {
                        Month = initGetList[i].Date.Month.ToString()
                    });
                }
            }
            return MonthList;
        }
        // MonthPicker Binding
        public List<month> GetMonths
        {
            get { return MonthList; }
            set
            {
                MonthList = value;
                OnPropertyChanged();
            }
        }

        month selectedMonth = null; // 存放選擇到的月份
        public month SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                OnPropertyChanged();
            }
        }

        string selectedKm = null; // 存放選擇到的里程
        public string SelectedKm
        {
            get { return selectedKm; }
            set
            {
                selectedKm = value;
                OnPropertyChanged();
            }
        }

        // piCKER 確認、清除按鈕的command
        public ICommand PickerCommand
        {
            get
            {
                return new Command<string>((x) => ComfirmSelect(x));
            }
        }

        // 確認篩選
        public void ComfirmSelect(string x)
        {
            if (x == "confirm")
            {
                issigningup = false;
                OnPropertyChanged(nameof(IsSigningUp));
                List_IsVisible = false;
                IsBusy = true;
                Activitys =  UpdateListView(selectedMonth, selectedCity, selectedKm);
                IsBusy = false;
                List_IsVisible = true;
            }
            // 清除篩選結果
            if (x == "clear")
            {
                SelectedMonth = null;
                SelectedCity = null;
                SelectedKm = null;
            }
        }

        public ObservableCollection<activity> UpdateListView(month m,city c,string k)
        {
            activitys = new ObservableCollection<activity>();
            if (k == null && c == null && m == null)
            {
                activitys = AddListView();
                return activitys;
            }
            else if (k == null && c == null && m != null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].Date.Month.ToString() == m.Month)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == null && c != null && m == null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].Location.Substring(0, 3) == c.Name)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == null && c != null && m != null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].Location.Substring(0, 3) == c.Name && initGetList[i].Date.Month.ToString() == m.Month)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "10K以下" && c == null && m == null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].LessThenTen() == true)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "10K以下" && c == null && m != null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].LessThenTen() == true && initGetList[i].Date.Month.ToString() == m.Month)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "10K以下" && c != null && m == null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].LessThenTen() == true && initGetList[i].Location.Substring(0, 3) == c.Name)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "10K以下" && c != null && m != null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].LessThenTen() == true && initGetList[i].Location.Substring(0, 3) == c.Name && initGetList[i].Date.Month.ToString() == m.Month)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "10K~20K" && c == null && m == null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].BetweenTenAndTewnty() == true)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "10K~20K" && c == null && m != null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].BetweenTenAndTewnty() == true && initGetList[i].Date.Month.ToString() == m.Month)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "10K~20K" && c != null && m == null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].BetweenTenAndTewnty() == true && initGetList[i].Location.Substring(0, 3) == c.Name)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "10K~20K" && c != null && m != null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].LessThenTen() == true && initGetList[i].Location.Substring(0, 3) == c.Name && initGetList[i].Date.Month.ToString() == m.Month)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "20K以上" && c == null && m == null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].GreaterTewnty() == true)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "20K以上" && c == null && m != null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].GreaterTewnty() == true && initGetList[i].Date.Month.ToString() == m.Month)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else if (k == "20K以上" && c != null && m == null)
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].GreaterTewnty() == true && initGetList[i].Location.Substring(0, 3) == c.Name)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
            else
            {
                for (int i = 0; i < initGetList.Count; i++)
                {
                    if (initGetList[i].GreaterTewnty() == true && initGetList[i].Location.Substring(0, 3) == c.Name && initGetList[i].Date.Month.ToString() == m.Month)
                    {
                        activitys = Add(activitys, i);
                    }
                }
                return activitys;
            }
        }
        public ObservableCollection<activity> Add(ObservableCollection<activity> activitys, int i)
        {
            activitys.Add(new activity
            {
                Id = initGetList[i].Id,
                Name = initGetList[i].Name,
                Location = initGetList[i].Location,
                Date = initGetList[i].Date,
                Startdate = initGetList[i].Startdate,
                Enddate = initGetList[i].Enddate,
                Group = initGetList[i].Group,
                ImageUrl = initGetList[i].ImageUrl
            });
            return activitys;
        }

        // ListView點選項目
        private activity _ItemSelected;
        public activity objItemSelected
        {
            get
            {
                return _ItemSelected;
            }
            set
            {
                GoToActivityDetailPage(value);
                _ItemSelected = null;
                OnPropertyChanged();
            }
        }

        // 前往活動詳細頁面
        public async void GoToActivityDetailPage(activity Info)
        {
            string mail = Preferences.Get("mail", "");
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            // 會員的活動詳細頁面
            if (mail == "" || SShellInstance == null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ActivityDetailPage(Info));
            }
            else
            {
                // 工作人員的活動詳細頁面
                await Application.Current.MainPage.Navigation.PushAsync(new S_ActivityDetailPage(Info));
            }
        }

        public async void ReCallAPI()
        {
            Console.WriteLine("ReCallAPIReCallAPIReCallAPIReCallAPI");
            initGetList = await ActivityProvider.GetActivitysAsync();
            RefreshListView();
        }

        // 更新LIST
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //IsBusy = true;

                    //initGetList = await ActivityProvider.GetActivitysAsync();
                    //Activitys = AddListView();
                    //GetCities = AddCityList();
                    //GetMonths = AddMonthList();

                    //IsBusy = false;
                    initGet();
                });
            }
        }

        public void RefreshListView()
        {
            List_IsVisible = false;
            IsBusy = true;
            IsSigningUp = false;
            SelectedMonth = null;
            SelectedCity = null;
            SelectedKm = null;
            if (!string.IsNullOrEmpty(searchtext))
            {
                // || x.Name.Contains(searchtext.First().ToString().ToUpper())
                var matches = initGetList.Where(x => x.Name.Contains(searchtext));
                activitys = new ObservableCollection<activity>();
                foreach (var item in matches)
                {
                    activitys.Add(item);
                }
                Activitys = activitys;
                //Activitys = (ObservableCollection<activity>)Activitys.Where(i => i.Name.Contains(searchtext));
            }
            else
            {
                Activitys = AddListView();
            }
            IsBusy = false;
            List_IsVisible = true;
        }

        bool isbusy;
        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                isbusy = value;
                OnPropertyChanged();
            }
        }

        bool list_isvisible;
        public bool List_IsVisible
        {
            get { return list_isvisible; }
            set
            {
                list_isvisible = value;
                OnPropertyChanged();
            }
        }

        bool textisvisible;
        public bool TextIsVisible
        {
            get { return textisvisible; }
            set
            {
                textisvisible = value;
                OnPropertyChanged();
            }
        }
        string textcontent;
        public string TextContent
        {
            get { return textcontent; }
            set
            {
                textcontent = value;
                OnPropertyChanged();
            }
        }
    }
}
