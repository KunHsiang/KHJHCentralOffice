using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Words;
using FISCA.Presentation.Controls;

namespace KHJHCentralOffice
{
    public partial class Approach_Report : BaseForm
    {
        private byte[] template;
        private string title;
        
        public Approach_Report(string title,byte[] template)
        {
            InitializeComponent();

            this.Load += new EventHandler(Form_Load);
            this.template = template;
            this.title = title;
            this.TitleText = title;

            this.InitSchoolYear();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            this.circularProgress.Visible = false;
            this.circularProgress.IsRunning = false;
        }

        private void InitSchoolYear()
        {
            this.nudSchoolYear.Value = decimal.Parse((DateTime.Today.Year - 1912).ToString());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MailMerge_MergeField(object sender, Aspose.Words.Reporting.MergeFieldEventArgs e)
        {
            #region 科目成績
             
            #endregion
        }

        //報表產生完成後，儲存並且開啟
        private void Completed(string inputReportName, Document inputDoc)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "另存新檔";
            sd.FileName = inputReportName + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + ".doc";
            sd.Filter = "Word檔案 (*.doc)|*.doc|所有檔案 (*.*)|*.*";
            sd.AddExtension = true;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    inputDoc.Save(sd.FileName, Aspose.Words.SaveFormat.Doc);
                    System.Diagnostics.Process.Start(sd.FileName);
                }
                catch
                {
                    MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string survey_year = this.nudSchoolYear.Value + "";
            this.btnPrint.Enabled = false;
            this.circularProgress.Visible = true;
            this.circularProgress.IsRunning = true;

            Task<Document> task = Task<Document>.Factory.StartNew(() =>
            {
                MemoryStream template = new MemoryStream(this.template);
                Document doc = new Document();
                Document dataDoc = new Document(template, "", LoadFormat.Doc, "");
                dataDoc.MailMerge.MergeField += new Aspose.Words.Reporting.MergeFieldEventHandler(MailMerge_MergeField);
                dataDoc.MailMerge.RemoveEmptyParagraphs = true;
                doc.Sections.Clear();
                List<string> keys = new List<string>();
                List<object> values = new List<object>();
                Dictionary<string, object> mergeKeyValue = ApproachStatisticsCalculator
                    .Calculate(survey_year);
               
                foreach (string key in mergeKeyValue.Keys)
                {
                    keys.Add(key);
                    values.Add(mergeKeyValue[key]);
                }
                
                dataDoc.MailMerge.Execute(keys.ToArray(), values.ToArray());
                doc.Sections.Add(doc.ImportNode(dataDoc.Sections[0], true));
                return doc;
            });
            task.ContinueWith((x) =>
            {
                this.btnPrint.Enabled = true;
                this.circularProgress.Visible = false;
                this.circularProgress.IsRunning = false;

                if (x.Exception != null)
                    MessageBox.Show(x.Exception.InnerException.Message);
                else
                    Completed(this.TitleText, x.Result);
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}