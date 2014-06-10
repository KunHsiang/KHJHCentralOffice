using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DesktopLib;
using FISCA;
using FISCA.Authentication;
using FISCA.Presentation;
using FISCA.UDT;

namespace KHJHCentralOffice
{
    public static class Program
    {
        public static UserConfigManager User { get; private set; }

        public static ConfigurationManager App { get; private set; }

        public static ConfigurationManager Global { get; private set; }

        public static NLDPanel MainPanel { get; private set; }

        public static DynamicCache GlobalSchoolCache { get; private set; }

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [MainMethod]
        public static void Main()
        {
            DSAServices.AutoDisplayLoadingMessageOnMotherForm();

            GlobalSchoolCache = new DynamicCache(); //建立一個空的快取。

            InitAsposeLicense();
            InitStartMenu();
            InitConfigurationStorage();
            InitMainPanel();

            MainPanel.ListPaneContexMenu["執行 SQL 並匯出"].Click += delegate
            {
                new ExportQueryData().Export();
            };

            new FieldManager();
            new DetailItems();
            new RibbonButtons();
            new ImportExport();//匯入學校資料

            RefreshFilteredSource();

            FISCA.Presentation.MotherForm.Form.Text = GetTitleText();
        }

        private static void InitMainPanel()
        {
            MainPanel = new NLDPanel();
            MainPanel.Group = "學校";
            MainPanel.SetDescriptionPaneBulider<DetailItemDescription>();

            InitBasicSearch();

            MotherForm.AddPanel(MainPanel);
            MainPanel.AddView(new DefaultView());
        }

        private static void InitBasicSearch()
        {
            MainPanel.Search += delegate(object sender, SearchEventArgs args)
            {
                string cond = args.Condition;
                foreach (string each in GlobalSchoolCache.PrimaryKeys)
                {
                    string text = GlobalSchoolCache[each].Title;
                    if (text.IndexOf(cond) >= 0)
                    {
                        args.Result.Add(each);
                        continue;
                    }

                    text = GlobalSchoolCache[each].DSNS;
                    if (text.IndexOf(cond) >= 0)
                    {
                        args.Result.Add(each);
                        continue;
                    }

                    text = GlobalSchoolCache[each].Group;
                    if (text.IndexOf(cond) >= 0)
                    {
                        args.Result.Add(each);
                        continue;
                    }

                    text = GlobalSchoolCache[each].Comment;
                    if (text.IndexOf(cond) >= 0)
                    {
                        args.Result.Add(each);
                        continue;
                    }
                }
            };
        }

        internal static void RefreshFilteredSource()
        {
            RefreshFilteredSource(null);
        }

        internal static void RefreshFilteredSource(Action callback)
        {
            List<string> schoolids = new List<string>();
            Task task = Task.Factory.StartNew(() =>
            {
                AccessHelper access = new AccessHelper();

                List<School> schools = access.Select<School>();

                foreach (School school in schools)
                {
                    schoolids.Add(school.UID);
                    GlobalSchoolCache.FillProperty(school.UID, "Title", school.Title);
                    GlobalSchoolCache.FillProperty(school.UID, "DSNS", school.DSNS);
                    GlobalSchoolCache.FillProperty(school.UID, "Group", school.Group);
                    GlobalSchoolCache.FillProperty(school.UID, "Comment", school.Comment);
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);

            task.ContinueWith((x) =>
            {
                MainPanel.SetFilteredSource(schoolids);

                if (callback != null)
                    callback();
            }, TaskScheduler.Default);
        }

        private static void InitConfigurationStorage()
        {
            User = new UserConfigManager(new ConfigProvider_User(), DSAServices.UserAccount);
            App = new ConfigurationManager(new ConfigProvider_App());
            Global = new ConfigurationManager(new ConfigProvider_Global());
        }

        private static void InitStartMenu()
        {
            MotherForm.StartMenu["安全性"]["使用者管理"].Click += delegate
            {
                new FISCA.Permission.UI.UserManager().ShowDialog();
            };

            FISCA.Presentation.MotherForm.StartMenu["重新登入"].BeginGroup = true;
            FISCA.Presentation.MotherForm.StartMenu["重新登入"].Click += delegate
            {
                Application.Restart();
            };
        }

        private static void InitAsposeLicense()
        {
            //設定 ASPOSE 元件的 License。
            System.IO.Stream stream = new System.IO.MemoryStream(Properties.Resources.Aspose_Total);
            new Aspose.Cells.License().SetLicense(stream);

            stream.Seek(0, SeekOrigin.Begin);
            //new Aspose.Chart.License().SetLicense(stream);
        }

        /// <summary>
        /// 設定ischool標頭
        /// </summary>
        private static string GetTitleText()
        {
            string user = DSAServices.UserAccount;

            string version = "0.0.0.0";
            try
            {
                string path = Path.Combine(Application.StartupPath, "release.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                version = doc.DocumentElement.GetAttribute("Version");
            }
            catch (Exception) { }

            return string.Format("ischool 中央管理系統〈FISCA：{0}〉〈{1}〉", version, user);
        }

        internal static string TrimModuleServerUrl(string urlPart)
        {
            string part = urlPart.Replace("https://module.ischool.com.tw/module", "~");
            part = part.Replace("http://module.ischool.com.tw/module", "~");
            part = part.Replace("http://module.ischool.com.tw:8080/module", "~");
            part = part.Replace("https://module.ischool.com.tw:8080/module", "~");
            return part;
        }
    }
}
