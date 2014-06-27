using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using FISCA.Data;
using FISCA.Deployment;
using FISCA.Presentation;
using FISCA.UDT;
using KHJHCentralOffice;

namespace KHJHLog
{
    public partial class QueryLog : FISCA.Presentation.Controls.BaseForm
    {
        AccessHelper accesshelper = new AccessHelper();
        QueryHelper queryhelper = new QueryHelper();

        public QueryLog()
        {
            InitializeComponent();
        }

        private void labelX2_Click(object sender, EventArgs e)
        {

        }

        private string GetContentFormat(string Action,string Content)
        {
            return Content;

            StringBuilder strBuilder = new StringBuilder();
            XElement elmContent = XElement.Load(new StringReader(Content));

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
            else if (Action.Equals("轉班"))
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

                return strBuilder.ToString();
            }
            else if (Action.Equals("鎖定／解除鎖定班級"))
            {
                //鎖定／解除鎖定班級
                // <Content>
                //      <ClassName>資一忠</ClassName>
                //      <GradeYear>1</GradeYear>
                //      <Reason></Reason>
                // </Content>

                strBuilder.AppendLine(
                    "班級名稱「" + elmContent.ElementText("ClassName") + 
                    "」年級「" + elmContent.ElementText("GradeYear") +
                    "」原因「" + elmContent.ElementText("Reason") + "」");

                return strBuilder.ToString();
            }
            else
                return Content;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            grdLog.Rows.Clear();

            string strStartDate = dateStart.Value.ToShortDateString();
            string strEndDate = dateEnd.Value.ToShortDateString();
            string strSQL = "select uid,date_time,dsns,action,content,read,comment from $school_log where date_time>='" + strStartDate + " 00:00:00' and date_time<='" + strEndDate + " 23:59:59' order by date_time desc";

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
                    .Find(x=>x.DSNS.Equals(DSNS));

                string SchoolName = vSchool != null ? vSchool.Title : DSNS;

                grdLog.Rows.Add(
                    UID,
                    Date,
                    SchoolName,
                    Action, 
                    Content , 
                    Read,
                    Comment);
            }
        }

        private void QueryLog_Load(object sender, EventArgs e)
        {
            dateStart.Value = DateTime.Today.AddDays(-7);
            dateEnd.Value = DateTime.Today;
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
                    MessageBox.Show("更新註解失敗，錯誤訊息如下：" + System.Environment.NewLine  + ve.Message);
                }
            }
        }
    }
}