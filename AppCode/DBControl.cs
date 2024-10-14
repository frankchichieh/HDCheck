using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.OleDb;

namespace HDCheck.AppCode
{
    /// <summary>
    /// DBControl 的摘要描述
    /// </summary>
    public class DBControl : HDCheck.ParentSys
    {
        public DBControl()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }    

        private static string _ConnectionString = "";

        public static string ConnectionString
        {
            get
            {
                if (_ConnectionString == "")
                {
                    _ConnectionString = IniReadValue("DataBase", "ConnectionString", AppIniFileName);
                }
                return _ConnectionString;
            }
        }
        

        

        
        
    }
}
