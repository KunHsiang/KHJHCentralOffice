using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopLib;
using DevComponents.DotNetBar.Controls;
using FISCA.UDT;
using System.Xml.Linq;
using System.IO;
using FISCA.DSAClient;
using System.Xml;

namespace KHJHCentralOffice
{
    public partial class GraduateSurveyApproach : DetailContentImproved
    {
        private List<ApproachStatistics> ApproachSats { get; set; }

        private string PhysicalUrl { get; set; }

        public GraduateSurveyApproach()
        {
            InitializeComponent();
            Group = "學生進路統計";
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
            AccessHelper access = new AccessHelper();
            ApproachSats = Utility.AccessHelper
                .Select<ApproachStatistics>(string.Format("ref_school_id={0}", PrimaryKey));

            if (ApproachSats.Count > 0)
            {
                XElement elm = XElement.Load(ApproachSats[0].Content);
            }
        }

        private void SetControl(int SchoolYear)
        {
            foreach (Control vControl in this.Controls)
            {
                if (vControl is TextBoxX)
                {
                    TextBoxX vTextBox = vControl as TextBoxX;
                    vTextBox.Text = string.Empty;
                }
            }

            for (int i = 0; i < cmbSurveyYear.Items.Count; i++)
            {
                if (cmbSurveyYear.Items[i].Equals(SchoolYear))
                {
                    cmbSurveyYear.SelectedIndex = i;
                    break;
                }
            }

            ApproachStatistics ApproachSat = ApproachSats
                .Find(x => x.SurveyYear.Equals(SchoolYear));

            if (ApproachSat != null)
            {

            }
        }

        protected override void OnPrimaryKeyChangedComplete(Exception error)
        {
            if (ApproachSats != null)
            {
                BeginChangeControlData();

                List<int> SurveyYears = ApproachSats
                    .Select(x => x.SurveyYear)
                    .ToList();

                SurveyYears.ForEach(x => cmbSurveyYear.Items.Add(x));

                Task task = Task.Factory.StartNew(() =>
                {
                    //ResolveUrl();
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);

                task.ContinueWith(x =>
                {
                    //txtPhysicalUrl.Text = PhysicalUrl;
                }, TaskScheduler.FromCurrentSynchronizationContext());

                ResetDirtyStatus();
            }
            else
                throw new Exception("無查資料：" + PrimaryKey);
        }

        private void BasicInfoItem_Load(object sender, EventArgs e)
        {
            InitDetailContent();
        }
    }
}
