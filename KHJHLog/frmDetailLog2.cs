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
using FISCA.Presentation.Controls;

namespace KHJHLog
{
    public partial class frmDetailLog2 : BaseForm
    {
        private DataGridViewRow Row;

        public frmDetailLog2(DataGridViewRow Row)
        {
            InitializeComponent();

            this.Row = Row;
        }

        private void frmDetailLog_Load(object sender, EventArgs e)
        {
            txtDate.Text = "" + Row.Cells["colDate"].Value;
            txtSchool.Text = "" + Row.Cells["colSchool"].Value;
            txtAction.Text = "" + Row.Cells["colAction"].Value;
            txtContent.Text = "" + Row.Cells["colContent"].Value;
            txtComment.Text = "" + Row.Cells["colComment"].Value;
            string UID = "" + Row.Cells["colID"].Value;

            QueryHelper query = new QueryHelper();

            DataTable table = query.Select("select detail from $school_log where uid=" + UID);

            if (table.Rows.Count > 0)
            {
                string Detail = table.Rows[0].Field<string>("detail");

                try
                {
                    XElement elmContent = XElement.Load(new StringReader("<root>" + Detail + "</root>"));

                    if (txtAction.Text.Equals("匯入新增學生"))
                    {
                        //<Detail>
                        //  <Student>
                        //    <IDNumber>   </IDNumber>
                        //    <StudentNumber> </StudentNumber>
                        //   <StudentName>   </StudentName>
                        //               <GradeYear></GradeYear>
                        //   <ClassName> </ClassName>
                        //   <SeatNo> </SeatNo>
                        //  </Student>
                        //</Detail>

                        grdDetail.Columns[6].Visible = false;
                        grdDetail.Columns[5].HeaderText = "座號";

                        foreach (XElement elmStudent in elmContent.Elements("Student"))
                        {
                            grdDetail.Rows.Add(elmStudent.ElementText("IDNumber"),
                                               elmStudent.ElementText("StudentNumber"),
                                               elmStudent.ElementText("StudentName"),
                                               elmStudent.ElementText("GradeYear"),
                                               elmStudent.ElementText("ClassName"),
                                               elmStudent.ElementText("SeatNo"));
                        }                   
                    }
                    else if (txtAction.Text.Equals("匯入更新班級"))
                    {
                        foreach (XElement elmStudent in elmContent.Elements("Student"))
                        {
                            grdDetail.Rows.Add(elmStudent.ElementText("IDNumber"),
                                               elmStudent.ElementText("StudentNumber"),
                                               elmStudent.ElementText("StudentName"),
                                               elmStudent.ElementText("GradeYear"),
                                               elmStudent.ElementText("ClassName"),
                                               elmStudent.ElementText("NewClassName"),
                                               elmStudent.ElementText("Reason"));
                        }
                    }
                }
                catch
                {
                    
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            grdDetail.Export(txtAction.Text + "明細");
        }
    }
}