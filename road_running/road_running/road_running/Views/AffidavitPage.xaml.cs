using System;
using System.Collections.Generic;
using road_running.Models;
using Xamarin.Forms;
using road_running.Providers;
using System.Threading.Tasks;

namespace road_running.Views
{
    public partial class AffidavitPage : ContentPage
    {
        public AffidavitPage(Group group)
        {
            GroupInfo = group;
            InitializeComponent();
            //Task<string> text = AffidavitProvider.GetActivitysAsync(group.running_ID);
            UpdateToUI();
        }

        private Group GroupInfo;

        public async void UpdateToUI()
        {
            content.Text = $"您將報名{GroupInfo.running_name}的{GroupInfo.group_name}，本人已詳細閱讀過本活動之競賽規程且同意亦保證遵守大會於競賽規程中所約定之事項，保證本人身心健康，無不適合激烈運動之疾病及狀態，志願參加比賽方開始報名，競賽中若發生任何意外事件，本人及家屬願意承擔比賽期間所發生之個人意外風險責任，亦同意放棄對於非主辦方所造成的傷害、死亡或其他任何形式的損失提出任何形式的賠償索求，一切責任與主辦單位無關。亦明白此項比賽之錄影、攝影及成績於網絡上公開播送、公開傳輸及公開展示，本人亦同意本人肖像及成績由主辦或主辦單位授權之單位用於宣傳活動上。保證提供有效的身份證和資料用於核實本人身份，對以上論述予以確認並願意承擔相應的法律責任。一經報名後將不得以任何理由質疑競賽內容所例之事項。(若活動當天視身體狀況不佳之參賽選手，為考量自身安全，切勿勉強參賽。亦不得退回報名費。請審慎考慮後再決定是否參加)";
            loading.IsRunning = false;
            loading.IsVisible = false;
            frame.IsVisible = true;
            acept.IsVisible = true;
        }
        void agree(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ConfirmPage(GroupInfo));
        }
    }
}
