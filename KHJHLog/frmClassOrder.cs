using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using FISCA.DSAClient;

namespace KHJHLog
{
    public partial class frmClassOrder : FISCA.Presentation.Controls.BaseForm
    {
        public frmClassOrder()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Connection con = new Connection();

            //取得局端登入後Greening發的Passport，並登入指定的Contract
            con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "ischool.kh.central_office.user", FISCA.Authentication.DSAServices.PassportToken);

            //取得該Contract所發的Passport
            Envelope Response = con.SendRequest("DS.Base.GetPassportToken", new Envelope());
            PassportToken Passport = new PassportToken(Response.Body);

            //TODO：拿此Passport登入各校
            Connection conSchool = new Connection();
            conSchool.Connect("dev.jh_kh", "ischool.kh.central_office", Passport);

            Response = conSchool.SendRequest("_.GetClassStudentCount",new Envelope());

            //<Class>
            //  <ClassName>101</ClassName>
            //  <StudentCount>25</StudentCount>
            //  <Lock />
            //  <Comment />
            //  <NumberReduceSum />
            //  <ClassStudentCount>25</ClassStudentCount>
            //</Class>

            //班級名稱、實際人數、編班人數、編班順位、編班鎖定、鎖定備註

            XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));

            grdClassOrder.Rows.Clear();

            foreach (XElement elmClass in elmResponse.Elements("Class"))
            {
                grdClassOrder.Rows.Add(
                    "dev.jh_kh",
                    elmClass.ElementText("ClassName"),
                    elmClass.ElementText("StudentCount"),
                    elmClass.ElementText("ClassStudentCount"),
                    elmClass.ElementText("NumberReduceSum"),
                    elmClass.ElementText("Lock"),
                    elmClass.ElementText("Comment")
                    );
            }
        }
    }
}