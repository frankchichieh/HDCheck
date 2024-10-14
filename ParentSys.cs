using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HDCheck
{
    public abstract class ParentSys
    {
        /// <summary>
        /// 宣告讀寫INI文件的API函數 
        /// </summary>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private static string _SysFilename = "";
        //private static string _EncodeFileName = "";

        public static string AppIniFileName
        {
            get
            {
                if (_SysFilename == "")
                {
                    _SysFilename = Application.StartupPath.Trim() + "\\HDCheck.ini";
                }
                return _SysFilename;
            }
        }

        

        //寫INI文件 
        public static void IniWriteValue(string Section, string Key, string Value, string sIniFile)
        {
            WritePrivateProfileString(Section, Key, Value, sIniFile);
        }

        //讀取INI文件指定 
        public static string IniReadValue(string Section, string Key, string sIniFile)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, sIniFile);
            return temp.ToString();

        }
        /// <summary>
        /// 取出 IP 的路徑
        /// </summary>
        private static string _IP = "";
        public static string IPall
        {
            get
            {
                if (_IP == "")
                {
                    _IP = IniReadValue("Config", "IP", AppIniFileName);
                }
                return _IP;
            }
        }
        /// <summary>
        /// 取出 IsServer 的路徑
        /// </summary>
        private static string _IsServer = "";
        public static string IsServer
        {
            get
            {
                if (_IsServer == "")
                {
                    _IsServer = IniReadValue("Config", "IsServer", AppIniFileName);
                }
                return _IsServer;
            }
        }
        /// <summary>
        /// 取出 InsertDB 的路徑
        /// </summary>
        private static string _InsertDB = "";
        public static string InsertDB
        {
            get
            {
                if (_InsertDB == "")
                {
                    _InsertDB = IniReadValue("Config", "InsertDB", AppIniFileName);
                }
                return _InsertDB;
            }
        }
        /// <summary>
        /// 取出 InsertACCESS 的路徑
        /// </summary>
        private static string _InsertACCESS = "";
        public static string InsertACCESS
        {
            get
            {
                if (_InsertACCESS == "")
                {
                    _InsertACCESS = IniReadValue("Config", "InsertACCESS", AppIniFileName);
                }
                return _InsertACCESS;
            }
        }
        public static string _UserID = "";
        public static string UserID
        {
            get
            {
                if (_UserID == "")
                {
                    _UserID = IniReadValue("DataBase", "UserId", AppIniFileName);
                    //在這裡，@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = Database.mdb" 是連線字串
                }
                return _UserID;
            }
        }
        public static string _UserPassword = "";
        public static string UserPassword
        {
            get
            {
                if (_UserPassword == "")
                {
                    _UserPassword = IniReadValue("DataBase", "UserPassword", AppIniFileName);
                    //在這裡，@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = Database.mdb" 是連線字串
                }
                return _UserPassword;
            }
        }
        public static string _OLEDBPassword = "";
        public static string OLEDBPassword
        {
            get
            {
                if (_OLEDBPassword == "")
                {
                    _OLEDBPassword = IniReadValue("DataBase", "OLEDBPassword", AppIniFileName);
                    //在這裡，@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = Database.mdb" 是連線字串
                }
                return _OLEDBPassword;
            }
        }
        public static string _oledbconnectionstring = "";
        public static string OleDBConnectionString
        {
            get
            {
                if (_oledbconnectionstring == "")
                {
                    _oledbconnectionstring = IniReadValue("DataBase", "oledbconnectionstring", AppIniFileName);
                    if (UserID != "") _oledbconnectionstring += "User Id=" + UserID+";";
                    if (UserPassword != "") _oledbconnectionstring += "Password=" + UserPassword+";";
                    if (OLEDBPassword != "") _oledbconnectionstring += "Jet OLEDB:Database Password=" + OLEDBPassword;
                    //在這裡，@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = Database.mdb" 是連線字串
                }
                return _oledbconnectionstring;
            }
        }
        /// <summary>
        /// 取出 cmd 的路徑
        /// </summary>
        private static string _cmd = "";
        public static string cmd
        {
            get
            {
                if (_cmd == "")
                {
                    _cmd = IniReadValue("Config", "cmd", AppIniFileName);
                }
                return _cmd;
            }
        }
        /// <summary>
        /// 取出 Disk 的路徑
        /// </summary>
        private static string _Diskall = "";
        public static string Diskall
        {
            get
            {
                if (_Diskall == "")
                {
                    _Diskall = IniReadValue("Config", "Disk", AppIniFileName);
                }
                return _Diskall;
            }
        }
        /// <summary>
        /// 取出 Interval 的路徑
        /// </summary>
        private static int _Interval = int.MinValue;
        public static int Interval
        {
            get
            {
                if (_Interval == int.MinValue)
                {
                    _Interval = Convert.ToInt32(IniReadValue("Config", "Interval", AppIniFileName));
                }
                return _Interval;
            }
        }
        /// <summary>
        /// 取出 Abnormal 的路徑
        /// </summary>
        private static string _Abnormal = "";
        public static string Abnormal
        {
            get
            {
                if (_Abnormal == "")
                {
                    _Abnormal = IniReadValue("Config", "Abnormal", AppIniFileName);
                }
                return _Abnormal;
            }
        }
        /// <summary>
        /// 取出 musicaddress 的路徑
        /// </summary>
        private static string _musicaddress = "";
        public static string musicaddress
        {
            get
            {
                if (_musicaddress == "")
                {
                    _musicaddress = IniReadValue("Config", "musicaddress", AppIniFileName);
                }
                return _musicaddress;
            }
        }

        
        

        protected static bool isNumeric(char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected static string GetDateStr(DateTime date)
        {
            return date.Year.ToString() + "/" + date.Month.ToString().PadLeft(2, '0') + "/" +
                date.Day.ToString().PadLeft(2, '0');
        }

        protected static string GetTimeStr(DateTime time)
        {
            return time.Hour.ToString().PadLeft(2, '0') + ":" + time.Minute.ToString().PadLeft(2, '0') + ":" +
                time.Second.ToString().PadLeft(2, '0');
        }

        protected static short ToInt16(object Value)
        {
            return ToInt16(Value, 0);
        }

        protected static short ToInt16(object Value, short Default)
        {
            try
            {
                return Convert.ToInt16(Value);
            }
            catch
            {
                return Default;
            }
        }

        protected static int ToInt32(object Value)
        {
            return ToInt32(Value, 0);
        }

        protected static int ToInt32(object Value, int Default)
        {
            try
            {
                return Convert.ToInt32(Value);
            }
            catch
            {
                return Default;
            }
        }

        protected static long ToInt64(object Value)
        {
            return ToInt64(Value, 0);
        }

        protected static long ToInt64(object Value, long Default)
        {
            try
            {
                return Convert.ToInt64(Value);
            }
            catch
            {
                return Default;
            }
        }

        protected static string ToString(object Value)
        {
            return ToString(Value, "");
        }

        protected static string ToString(object Value, string Default)
        {
            try
            {
                return Value.ToString();
            }
            catch
            {
                return Default;
            }
        }

        protected static DateTime ToDateTime(object Value)
        {
            return ToDateTime(Value, DateTime.MinValue);
        }

        protected static DateTime ToDateTime(object Value, DateTime Default)
        {
            try
            {
                return Convert.ToDateTime(Value);
            }
            catch
            {
                return Default;
            }
        }

        protected static bool ToBool(object Value)
        {
            return ToBool(Value, false);
        }

        protected static bool ToBool(object Value, bool Default)
        {
            try
            {
                return Convert.ToBoolean(Value);
            }
            catch
            {
                return Default;
            }

        }

        protected static Single ToSingle(object Value)
        {
            return ToSingle(Value, Single.MinValue);
        }

        protected static Single ToSingle(object Value, Single Default)
        {
            try
            {
                return Convert.ToSingle(Value);
            }
            catch
            {
                return Default;
            }
        }

        protected static double ToDouble(object Value)
        {
            return ToDouble(Value, Double.MinValue);
        }

        protected static double ToDouble(object Value, double Default)
        {
            try
            {
                return Convert.ToDouble(Value);
            }
            catch
            {
                return Default;
            }
        }

        protected static Boolean StrToBool(string Value)
        {
            if (Value == null) return false;
            else return Value == "Y";
        }

        protected static string BoolToStr(bool Value)
        {
            if (Value) return "Y";
            else return "N";
        }
    }
}
