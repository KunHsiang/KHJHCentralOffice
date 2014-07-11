
namespace KHJHLog
{
    class Permissions
    {
        public static string 記錄查詢 { get { return "44e329e1-ffb9-429f-9579-3d30dcb4f6b8"; } }

        public static bool 記錄查詢權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[記錄查詢].Executable;
            }
        }

        public static string 動作設定 { get { return "a348c6f6-a7d4-472b-aa60-752554d9dc77"; } }

        public static bool 動作設定權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[動作設定].Executable;
            }
        }
    }
}