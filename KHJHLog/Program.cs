using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA;
using FISCA.Presentation;
using FISCA.UDT;

namespace KHJHLog
{
    public static class Program
    {
        public static NLDPanel MainPanel { get; private set; }

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [MainMethod]
        public static void Main()
        {
            InitMainPanel();

            SchemaManager Manager = new SchemaManager(FISCA.Authentication.DSAServices.DefaultConnection);

            Manager.SyncSchema(new SchoolLog());

            MainPanel.RibbonBarItems["自動編班"]["記錄查詢"].Image = Properties.Resources.admissions_search_128;
            MainPanel.RibbonBarItems["自動編班"]["記錄查詢"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MainPanel.RibbonBarItems["自動編班"]["記錄查詢"].Click += (sender, e) => new QueryLog().ShowDialog();
        }

        private static void InitMainPanel()
        {
            foreach(NLDPanel Panel in MotherForm.Panels)
                if (Panel.Group.Equals("學校"))
                    MainPanel = Panel;
        }
    }
}