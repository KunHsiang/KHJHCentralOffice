using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Campus.Configuration;
using FISCA.Data;
using FISCA.Presentation;
using FISCA.UDT;
using KHJHCentralOffice;

namespace KHJHLog
{
    public partial class QueryLog : FISCA.Presentation.Controls.BaseForm
    {
        private List<string> VerifyActions = new List<string>() { "匯入更新班級", "調整班級", "變更特殊身分", "鎖定班級" };
        private AccessHelper accesshelper = new AccessHelper();
        private QueryHelper queryhelper = new QueryHelper();
        private ConfigData config = Campus.Configuration.Config.User["Option"];

        public QueryLog()
        {
            InitializeComponent();
        }

        private void labelX2_Click(object sender, EventArgs e)
        {

        }

        private string GetContentFormat(string Action, string Content)
        {
            //return Content;

            StringBuilder strBuilder = new StringBuilder();
            XElement elmContent;

            try
            {
                elmContent = XElement.Load(new StringReader("<root>" + Content + "</root>"));
            }
            catch
            {
                return Content;
            }

            if (Action.Equals("特殊轉入"))
            {
                //特殊轉入
                // <Content>
                //     <IDNumber>   </IDNumber>
                //     <StudentNumber> </StudentNumber>
                //     <StudentName>   </StudentName>
                //     <ClassName> </ClassName>
                //     <SeatNo> </SeatNo>
                //     <ScheduleClassDate> </ScheduleClassDate>
                //     <Reason> </Reason>
                //  ...其他需要和異動相關的欄位
                //  </Content>

                strBuilder.AppendLine("學生姓名：" + elmContent.Element("StudentName").Value);

                return strBuilder.ToString();
            }
            else if (Action.Equals("班級調整"))
            {
                //轉班
                //  <Content>
                //      <IDNumber> </IDNumber>
                //      <StudentNumber> </StudentNumber>
                //      <StudentName> </StudentName>
                //       <GradeYear> </GradeYear>
                //       <ClassName>  </ClassName>
                //       <NewClassName>   </NewClassName>
                //       <Reason> </Reason>
                //  </Content>

                return string.Format("學生「{0}」從「{1}」調整班級到「{2}」",
                    elmContent.ElementText("StudentName"), elmContent.ElementText("ClassName"), elmContent.ElementText("NewClassName"));
            }
            else if (Action.Equals("鎖定班級") || Action.Equals("解鎖班級"))
            {
                //鎖定／解除鎖定班級
                // <Content>
                //      <ClassName>資一忠</ClassName>
                //      <GradeYear>1</GradeYear>
                //      <Reason></Reason>
                // </Content>

                //strBuilder.AppendLine(
                //    "班級名稱「" + elmContent.ElementText("ClassName") +
                //    "」年級「" + elmContent.ElementText("GradeYear") +
                //    "」原因「" + elmContent.ElementText("Reason") + "」");

                return string.Format("班級：{0}", elmContent.ElementText("ClassName"));
            }
            else
                return Content;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string StartDate = dateStart.Value.ToShortDateString();

            config["start_date"] = StartDate;

            config.SaveAsync();

            grdLog.Rows.Clear();

            List<string> SelectedActions = new List<string>();

            foreach (ListViewItem Item in lstAction.Items)
            {
                if (Item.Checked)
                {
                    SelectedActions.Add(Item.Name);
                }
            }           

            string strStartDate = dateStart.Value.ToShortDateString();
            string strEndDate = dateEnd.Value.ToShortDateString();

            StringBuilder strSQLBuilder = new StringBuilder();

            strSQLBuilder.Append("select uid,date_time,dsns,action,content,read,comment from $school_log where date_time>='" + strStartDate + " 00:00:00' and date_time<='" + strEndDate + " 23:59:59'");

            if (SelectedActions.Count > 0)
            {
                string strCondition = string.Join(",", SelectedActions.Select(x => "'" + x + "'").ToArray());
                strSQLBuilder.Append(" and action in (" + strCondition +")");
            }
             
            strSQLBuilder.Append(" order by date_time desc");

            string strSQL = strSQLBuilder.ToString();

            DataTable tblSchoolLog = queryhelper.Select(strSQL);
            List<School> Schools = accesshelper.Select<School>();

            foreach (DataRow row in tblSchoolLog.Rows)
            {
                string UID = row.Field<string>("uid");
                string Date = DateTime.Parse(row.Field<string>("date_time")).ToShortDateString();
                string DSNS = row.Field<string>("dsns");
                string Action = row.Field<string>("action");
                string Content = GetContentFormat(Action, row.Field<string>("content"));
                string Read = row.Field<string>("read");
                string Comment = row.Field<string>("comment");

                School vSchool = Schools
                    .Find(x => x.DSNS.Equals(DSNS));

                string SchoolName = vSchool != null ? vSchool.Title : DSNS;

                int RowIndex = grdLog.Rows.Add(
                    UID,
                    Date,
                    SchoolName,
                    Action,
                    Content,
                    Read,
                    Comment);
            }
        }

        private void QueryLog_Load(object sender, EventArgs e)
        {
            DateTime dteStart = DateTime.Today.AddDays(-7);
            dateStart.Value = dteStart;
            dateEnd.Value = DateTime.Today;

            string StartDate = config["start_date"];

            if (!string.IsNullOrWhiteSpace(StartDate))
            {
                DateTime.TryParse(StartDate, out dteStart);
                dateStart.Value = dteStart;
            }

            DataTable tblAction = queryhelper.Select("select distinct action from $school_log");

            List<string> Actions = new List<string>();

            lstAction.Items.Clear();

            foreach (DataRow row in tblAction.Rows)
            {
                string Action = row.Field<string>("action");

                ListViewItem vItem = new ListViewItem();
                vItem.Name = Action;
                vItem.Text = VerifyActions.Contains(Action)? Action + "（需審核）" : Action;
                lstAction.Items.Add(vItem);
            }
        }

        private void grdLog_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (grdLog.Columns[e.ColumnIndex].Name.Equals("colComment"))
            {
                try
                {
                    string Comment = "" + grdLog.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    string UID = "" + grdLog.Rows[e.RowIndex].Cells[0].Value;

                    List<SchoolLog> SchoolLog = accesshelper.Select<SchoolLog>("uid=" + UID);

                    if (SchoolLog.Count == 1)
                    {
                        SchoolLog[0].Comment = Comment;
                        SchoolLog.SaveAll();
                        MotherForm.SetStatusBarMessage("註解已成功更新為「" + Comment + "」");
                    }
                }
                catch (Exception ve)
                {
                    MessageBox.Show("更新註解失敗，錯誤訊息如下：" + System.Environment.NewLine + ve.Message);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            grdLog.Export("自動編班查詢記錄");
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem Item in lstAction.Items)
            {
                Item.Checked = chkSelectAll.Checked;
            }
        }

        private void grdLog_DoubleClick(object sender, EventArgs e)
        {
            if (grdLog.SelectedCells.Count == 1)
            {
                frmDetailLog DetailLog = new frmDetailLog(grdLog.Rows[grdLog.SelectedCells[0].RowIndex]);

                DetailLog.ShowDialog();
            }
        }
    }
}