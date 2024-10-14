using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace HDCheck.AppCode.BusinessLayer.Schema
{

    public class Alarm : DBControl
    {
        int _OrgErroroid = int.MinValue;
        int _Erroroid = int.MinValue;
        string _Disk = "";
        string _IP = "";
        public Alarm()
        {
        }
        public Alarm(int Erroroid)
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
        /// 硬碟編號
        /// </summary>
        public string Disk
        {
            get { return this._Disk; }
            set { this._Disk = value; }
        }

    }
    public class AlarmCollection : CollectionBase
    {
        public enum AlarmFields
        {
            Erroroid,
            Disk,
            IP,
            None

        }
        public Alarm this[int index]
        {
            get { return (Alarm)List[index]; }
            set { List[index] = value; }
        }
        public int Add(Alarm value)
        {
            if (value == null) return -1;
            else return (List.Add(value));
        }
        public int IndexOf(Alarm value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, Alarm value)
        {
            List.Insert(index, value);
        }
        public void Remove(Alarm value)
        {
            List.Remove(value);
        }
        public bool Contains(Alarm value)
        {
            return (List.Contains(value));
        }
    }
}
