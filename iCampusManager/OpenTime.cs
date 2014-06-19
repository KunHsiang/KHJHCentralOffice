using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KHJHCentralOffice
{
    /// <summary>
    /// 設定開放時間
    /// </summary>
    public partial class OpenTime : FISCA.Presentation.Controls.BaseForm
    {
        private List<OpenTimeSetting> OpenTimeSettings = null;

        public OpenTime()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void OpenTime_Load(object sender, System.EventArgs e)
        {
            grdOpenDate.Rows.Clear();

             OpenTimeSettings = Utility.AccessHelper
                .Select<OpenTimeSetting>();

            foreach (OpenTimeSetting vSetting in OpenTimeSettings)
            {
                grdOpenDate.Rows.Add(
                    vSetting.SurveyYear, 
                    vSetting.StartDate.ToShortDateString(), 
                    vSetting.EndDate.ToShortDateString());
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            OpenTimeSettings.ForEach(x => x.Deleted = true);

            Utility.AccessHelper.DeletedValues(OpenTimeSettings);

            OpenTimeSettings.Clear();

            foreach(DataGridViewRow Row in grdOpenDate.Rows)
            {
                if (!Row.IsNewRow)
                {
                    string SurveyYear = "" + Row.Cells[0].Value;
                    string StartDateTime = "" + Row.Cells[1].Value;
                    string EndDateTime = "" + Row.Cells[2].Value;

                    OpenTimeSetting vSetting = new OpenTimeSetting();

                    vSetting.SurveyYear = int.Parse(SurveyYear);
                    vSetting.StartDate = DateTime.Parse(StartDateTime);
                    vSetting.EndDate = DateTime.Parse(EndDateTime);

                    OpenTimeSettings.Add(vSetting);
                }
            }

            Utility.AccessHelper.SaveAll(OpenTimeSettings);
        }
    }
}