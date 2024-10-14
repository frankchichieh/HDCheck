using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace HDCheck.AppCode.BusinessLayer.Schema
{
    public class Check
    {
        string _Disk = "";
        string _Desc = "";
        int _remark = 0;
        string _IP = "";
        /// <summary>
        /// IP
        /// </summary>
        public string IP
        {
            get { return this._IP; }
            set { this._IP = value; }
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
        /// act狀態 
        /// 0:未確認
        /// 1:確認
        /// </summary>
        public int remark
        {
            get { return this._remark; }
            set { this._remark = value; }
        }
    }
    public class CheckCollection : CollectionBase
    {
        public enum CheckFields
        {
            Disk,
            Desc,
            IP,
            None

        }
        public Check this[int index]
        {
            get { return (Check)List[index]; }
            set { List[index] = value; }
        }
        public int Add(Check value)
        {
            if (value == null) return -1;
            else return (List.Add(value));
        }
        public int IndexOf(Check value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, Check value)
        {
            List.Insert(index, value);
        }
        public void Remove(Check value)
        {
            List.Remove(value);
        }
        public bool Contains(Check value)
        {
            return (List.Contains(value));
        }
        public Check GetObject(string IP,string Disk)
        {
            Check obj = null;
            foreach (Check o in this)
            {
                if (o.Disk == Disk && o.IP==IP)
                {
                    return o;
                    //break;
                }
            }

            return obj;
        }
    }
}
