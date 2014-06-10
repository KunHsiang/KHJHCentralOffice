using System;
using DesktopLib;

namespace KHJHCentralOffice
{
    public partial class GraduateSurveyVagrant : DetailContentImproved
    {
        //private School SchoolData { get; set; }
        //private string PhysicalUrl { get; set; }

        public GraduateSurveyVagrant()
        {
            InitializeComponent();
            Group = "未升學未就業統計";
        }

        protected override void OnInitializeComplete(Exception error)
        {
            //WatchChange(new TextBoxSource(txtTitle));
            //WatchChange(new TextBoxSource(txtDSNS));
            //WatchChange(new TextBoxSource(txtGroup));
            //WatchChange(new TextBoxSource(txtComment));
        }

        protected override void OnSaveData()
        {
            //if (SchoolData != null)
            //{
            //    SchoolData.Title = txtTitle.Text;
            //    SchoolData.DSNS = txtDSNS.Text;
            //    SchoolData.Group = txtGroup.Text;
            //    SchoolData.Comment = txtComment.Text;
            //    SchoolData.Save();
            //    Program.RefreshFilteredSource();
            //    ConnectionHelper.ResetConnection(PrimaryKey);
            //}
            //ResetDirtyStatus();
        }

        protected override void OnPrimaryKeyChangedAsync()
        {
            //AccessHelper access = new AccessHelper();
            //List<School> schools = access.Select<School>(string.Format("uid='{0}'", PrimaryKey));

            //if (schools.Count > 0)
            //    SchoolData = schools[0];
            //else
            //    SchoolData = null;
        }

        private void ResolveUrl()
        {
            //PhysicalUrl = string.Empty;
            //if (SchoolData != null)
            //{
            //    AccessPoint ap;
            //    if (AccessPoint.TryParse(SchoolData.DSNS, out ap))
            //        PhysicalUrl = ap.Url;
            //}
        }

        protected override void OnPrimaryKeyChangedComplete(Exception error)
        {
            //if (SchoolData != null)
            //{
            //    BeginChangeControlData();
            //    txtTitle.Text = SchoolData.Title;
            //    txtDSNS.Text = SchoolData.DSNS;
            //    txtGroup.Text = SchoolData.Group;
            //    txtComment.Text = SchoolData.Comment;
            //    txtPhysicalUrl.Text = "解析中...";

            //    Task task = Task.Factory.StartNew(() =>
            //    {
            //        ResolveUrl();
            //    }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);

            //    task.ContinueWith(x =>
            //    {
            //        txtPhysicalUrl.Text = PhysicalUrl;
            //    }, TaskScheduler.FromCurrentSynchronizationContext());

            //    ResetDirtyStatus();
            //}
            //else
            //    throw new Exception("無查資料：" + PrimaryKey);
        }

        private void BasicInfoItem_Load(object sender, EventArgs e)
        {
            InitDetailContent();
        }
    }
}
