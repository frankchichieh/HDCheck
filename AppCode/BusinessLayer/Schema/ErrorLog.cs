using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using HDCheck.AppCode.DataLayer.Schema;

namespace HDCheck.AppCode.BusinessLayer.Schema
{

    public class ErrorLog : DBControl
    {
        int _OrgErroroid = int.MinValue;
        int _Erroroid = int.MinValue;
        string _Alarmtime = "";
        string _Disk = "";
        string _Desc = "";
        string _ActTime = "";
        string _IP = "";
        public ErrorLog()
        {

        }
        public ErrorLog(int Erroroid)
        {
            _OrgErroroid = Erroroid;
        }
        /// <summary>
        /// OrgErroroid
        /// </summary>
        public int OrgErroroid
        {
            get { return this._OrgErroroid; }
            set { this._OrgErroroid = value; }
        }
        /// <summary>
        /// Erroroid
        /// </summary>
        public int Erroroid
        {
            get { return this._Erroroid; }
            set { this._Erroroid = value; }
        }
        /// <summary>
        /// IP
        /// </summary>
        public string IP
        {
            get { return this._IP; }
            set { this._IP = value; }
        }

        /// <summary>
        /// 警報觸發時間
        /// </summary>
        public string Alarmtime
        {
            get { return this._Alarmtime; }
            set { this._Alarmtime = value; }
        }
        /// <summary>
        /// 硬碟編號
        /// </summary>
        public string Disk
        {
            get { return this._Disk; }
            set { this._Disk = value; }
        }
        /// <summary>
        /// 硬碟狀態
        /// </summary>
        public string Desc
        {
            get { return this._Desc; }
            set { this._Desc = value; }
        }
        /// <summary>
        /// 警報確認時間
        /// </summary>
        public string ActTime
        {
            get { return this._ActTime; }
            set { this._ActTime = value; }
        }
        //單筆查詢
        public static ErrorLog GetObject(int Seqno)
        {
            return ErrorLogDataAccess.GetObject(Seqno, ConnectionString);
        }
        //多筆查詢
        public static ErrorLogCollection GetObjects(int Seqno)
        {
            return ErrorLogDataAccess.GetObjects(Seqno, ConnectionString);
        }
        //儲存
        public bool Save()
        {
            return ErrorLogDataAccess.AddObject(this, ConnectionString);
        }
        //刪除
        public static bool DeleteObject(int Seqno)
        {
            return ErrorLogDataAccess.DeleteObject(Seqno, ConnectionString);
        }
    }
    public class ErrorLogCollection : CollectionBase
    {
        public enum ErrorLogFields
        {
            Erroroid,
            Alarmtime,
            Disk,
            Desc,
            ActTime,
            IP,
            None

        }
        public ErrorLog this[int index]
        {
            get { return (ErrorLog)List[index]; }
            set { List[index] = value; }
        }
        public int Add(ErrorLog value)
        {
            if (value == null) return -1;
            else return (List.Add(value));
        }
        public int IndexOf(ErrorLog value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, ErrorLog value)
        {
            List.Insert(index, value);
        }
        public void Remove(ErrorLog value)
        {
            List.Remove(value);
        }
        public bool Contains(ErrorLog value)
        {
            return (List.Contains(value));
        }

        public ErrorLog GetObject(string Disk)
        {
            ErrorLog obj = null;
            foreach (ErrorLog o in this)
            {
                if (o.Disk == Disk)
                {
                    return o;
                    //break;
                }
            }

            return obj;
        }
    }
}
