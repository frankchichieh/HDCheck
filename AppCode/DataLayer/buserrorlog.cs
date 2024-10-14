using System;
using System.Collections.Generic;
using System.Text;
using Effect.DataAccess;
using HDCheck.AppCode.BusinessLayer.Schema;
using Effect;
using System.Collections;
using System.Data;
using Effect.Utility;

namespace HDCheck.AppCode.DataLayer
{
    public class buserrorlog : SqlDataAccessBase
    {
        public static ErrorLogCollection GetQryData()
        {
            try
            {
                SelectSqlBuilder Builder = new SelectSqlBuilder("ErrorLog with(nolock)");
                Builder.AddDataField("erroroid");
                Builder.AddDataField("IP");
                Builder.AddDataField("Disk");
                Builder.AddDataField("Descr");
                Builder.AddDataField("Alarmtime");
                Builder.AddDataField("Acttime");
                IDbCommand DbCmd = CreateCommand(Builder.GetSelectSqlText(""));
                ExtractCollectionFromReader Extractor = new ExtractCollectionFromReader(ExtractObjectCollectionFromReader);
                ErrorLogCollection results = (ErrorLogCollection)ExecuteReader(DbCmd, Extractor, DBControl.ConnectionString);
                return results;
            }
            catch (Exception exp)
            {
                utilLogs.WriterMessage("EMS", "buserrorlog.GetQryData _# exp : " + exp.Message);
                return null;
            }
        }
        private static CollectionBase ExtractObjectCollectionFromReader(IDataReader Reader)
        {
            ErrorLogCollection objs = new ErrorLogCollection();
            try
            {
                while (Reader.Read())
                {
                    try
                    {
                        ErrorLog obj = new ErrorLog(ToInt32(Reader["erroroid"]));
                        obj.Erroroid = ToInt32(Reader["erroroid"]);
                        obj.IP = ToString(Reader["IP"]);
                        obj.Disk = ToString(Reader["Disk"]);
                        obj.Desc = ToString(Reader["Descr"]);
                        obj.Alarmtime = ToString(Reader["Alarmtime"]);
                        obj.ActTime = ToString(Reader["ActTime"]);
                        

                        objs.Add(obj);
                    }
                    catch (Exception exp11)
                    {
                        utilLogs.WriterMessage("EMS", "buserrorlog.ExtractObjectCollectionFromReader _# exp11: " + exp11.Message);
                    }
                }
            }
            catch (Exception exp)
            {
                utilLogs.WriterMessage("EMS", "buserrorlog.ExtractObjectCollectionFromReader _# exp : " + exp.Message);
            }
            return objs;
        }
    }
}
