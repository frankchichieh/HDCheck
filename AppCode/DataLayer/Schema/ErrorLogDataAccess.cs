using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Effect;
using HDCheck.AppCode.BusinessLayer.Schema;
using System.Collections;

namespace HDCheck.AppCode.DataLayer.Schema
{
    class ErrorLogDataAccess : Effect.DataAccess.SqlDataAccessBase
    {
        public static bool AddObject(ErrorLog obj, string ConnectionString)
        {
            if (CheckExists("ErrorLog", "Erroroid", DbType.Int32, obj.Erroroid, ConnectionString))
                throw new Exception("ErrorLog.Erroroid 重複");
            string cmd = "Insert into ErrorLog(";
            if (obj.IP.Length > 0) cmd += "IP";
            if (obj.ActTime.Length > 0) cmd += ",ActTime";
            if (obj.Disk.Length > 0) cmd += ",Disk";
            if (obj.Desc.Length > 0) cmd += ",Descr";
            if (obj.Alarmtime.Length > 0) cmd += ",Alarmtime";
            cmd += ",InsertDB";
            cmd += ") values(";

            if (obj.IP.Length > 0) cmd += "'" + obj.IP + "'";
            if (obj.ActTime.Length > 0) cmd += ",'" + obj.ActTime + "'";
            if (obj.Disk.Length > 0) cmd += ",'" + obj.Disk + "'";
            if (obj.Desc.Length > 0) cmd += ",'" + obj.Desc + "'";
            if (obj.Alarmtime.Length > 0) cmd += ",'" + obj.Alarmtime + "'";
            cmd += "," +"getdate()" + ")";

            IDbCommand DbCmd = CreateCommand(cmd);
            int result = (int)ExecuteNonQuery(DbCmd, ConnectionString);
            return result >= 1;
        }
        
        //update
        public static bool UpdateObject(ErrorLog obj, IDbTransaction Transaction, string ConnectionString)
        {
            if (obj.OrgErroroid != Int32.MinValue && !CheckExists("ErrorLog", "Erroroid", DbType.Int32, obj.Erroroid, ConnectionString))
                throw new Exception("ErrorLog.Erroroid 不存在");

            UpdateSqlBuilder Builder = new UpdateSqlBuilder("ErrorLog");
            if (obj.IP.Length > 0) Builder.AddDataField("IP");
            if (obj.ActTime.Length > 0) Builder.AddDataField("ActTime");
            if (obj.Disk.Length > 0) Builder.AddDataField("Disk");
            if (obj.Desc.Length > 0) Builder.AddDataField("Descr");
            if (obj.Alarmtime.Length > 0) Builder.AddDataField("Alarmtime");
            Builder.AddKeyField("Erroroid");

            IDbCommand DbCmd = CreateCommand(Builder.GetUpdateSqlText());
            if (obj.IP.Length > 0) SetParameter(DbCmd, "IP", DbType.String, obj.IP);
            if (obj.ActTime.Length > 0) SetParameter(DbCmd, "ActTime", DbType.String, obj.ActTime);
            if (obj.Disk.Length > 0) SetParameter(DbCmd, "Disk", DbType.String, obj.Disk);
            if (obj.Desc.Length > 0) SetParameter(DbCmd, "Descr", DbType.String, obj.Desc);
            if (obj.Alarmtime.Length > 0) SetParameter(DbCmd, "Alarmtime", DbType.String, obj.Alarmtime);
            SetParameter(DbCmd, "@old_Erroroid", DbType.Int32, obj.Erroroid);

            int result = (int)ExecuteNonQuery(DbCmd, Transaction, ConnectionString);
            return result >= 1;


        }
        //delete
        public static bool DeleteObject(int Erroroid, string ConnectionString)
        {
            if (!CheckExists("ErrorLog", "Erroroid", DbType.Int32, Erroroid, ConnectionString))
                throw new Exception("ErrorLog.Erroroid 不存在");

            UpdateSqlBuilder Builder = new UpdateSqlBuilder("ErrorLog");
            Builder.AddKeyField("Erroroid");

            IDbCommand Dbcmd = CreateCommand(Builder.GetDeleteSqlText());
            SetParameter(Dbcmd, "@old_Erroroid", DbType.Int32, Erroroid);

            int result = (int)ExecuteNonQuery(Dbcmd, ConnectionString);
            return result >= 1;
        }
        //取得全部資料
        public static ErrorLogCollection GetObjects(int Seqno, string ConnectionString)
        {
            SelectSqlBuilder Builder = new SelectSqlBuilder("HDstatus");
            if (Seqno != Int32.MinValue) Builder.AddCond("Erroroid=@Erroroid");

            IDbCommand DbCmd = CreateCommand(Builder.GetSelectSqlText(""));
            if (Seqno != Int32.MinValue) SetParameter(DbCmd, "@Erroroid", DbType.Int32, Seqno);

            ExtractCollectionFromReader Extractor = new ExtractCollectionFromReader(ExtractObjectCollectionFromReader);
            ErrorLogCollection results = (ErrorLogCollection)ExecuteReader(DbCmd, Extractor, ConnectionString);
            return results;
        }
        //取得一筆資料
        public static ErrorLog GetObject(int Erroroid, string ConnectionString)
        {
            SelectSqlBuilder Builder = new SelectSqlBuilder("ErrorLog");
            if (Erroroid != Int32.MinValue) Builder.AddCond("Erroroid=@Erroroid");

            IDbCommand DbCmd = CreateCommand(Builder.GetSelectSqlText(""));
            if (Erroroid != Int32.MinValue) SetParameter(DbCmd, "@Erroroid", DbType.Int32, Erroroid);

            ExtractCollectionFromReader Extractor = new ExtractCollectionFromReader(ExtractObjectCollectionFromReader);
            ErrorLogCollection results = (ErrorLogCollection)ExecuteReader(DbCmd, Extractor, ConnectionString);
            if (results.Count > 0) return results[0];
            else return null;
        }
        private static CollectionBase ExtractObjectCollectionFromReader(IDataReader Reader)
        {
            ErrorLogCollection objs = new ErrorLogCollection();
            while (Reader.Read())
            {
                ErrorLog obj = new ErrorLog(ToInt32(Reader["Erroroid"]));

                obj.Erroroid = ToInt32(Reader["Erroroid"]);
                obj.IP = ToString(Reader["IP"]);
                obj.Alarmtime = ToString(Reader["Alarmtime"]);
                obj.Disk = ToString(Reader["Disk"]);
                obj.Desc = ToString(Reader["Descr"]);
                obj.ActTime = ToString(Reader["ActTime"]);

                objs.Add(obj);
            }
            return objs;
        }
    }
}
