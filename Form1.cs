using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using SnmpSharpNet;
using Effect.Utility;
using HDCheck.AppCode.BusinessLayer.Schema;
using System.Media;
using System.IO;
using System.Data.OleDb;
using System.Net.Sockets;
using HDCheck.AppCode.DataLayer;

namespace HDCheck
{

    public partial class Form1 : Form
    {
        int count = 0;
        ErrorLogCollection objs = new ErrorLogCollection();
        CheckCollection objcs = new CheckCollection();
        AlarmCollection objalarms = new AlarmCollection();
        ErrorLog obj = new ErrorLog();
        Alarm objalarm = new Alarm();
        OleDbConnection oleDb = new OleDbConnection(@ParentSys.OleDBConnectionString);
        public Form1()
        {
            InitializeComponent();
        }
        //-----------------------------------------------------------------------------//
        //                        伺服器控制項事件處理函數                             //
        //-----------------------------------------------------------------------------//
        private void btnQuery_Click(object sender, EventArgs e)
        {
            
            timer1.Enabled = true;
            timer1.Interval = ParentSys.Interval;
            exec();
            btnQuery.Enabled = false;
        }
        private void stop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            objs = new ErrorLogCollection();
            btnQuery.Enabled = true;

        }
        //-----------------------------------------------------------------------------//
        //                             自訂功能函數                                    //
        //-----------------------------------------------------------------------------//


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ParentSys.IsServer == "Y") objs = new ErrorLogCollection();
            if (ParentSys.IsServer == "Y") objalarms = new AlarmCollection();
            exec();
        }

        private void exec()
        {
            try
            {
                btnstop.Enabled = false;
                //string cmd = ".1.3.6.1.4.1.674.10893.1.20.130.4.1.4";//抓取物理磁碟的指令碼
                string cmd = ParentSys.cmd;
                string index = ParentSys.Diskall;//硬碟序號
                string IP = ParentSys.IPall;//伺服器IP
                string[] IPNO = IP.Split(',');
                //以/區隔不同ip的disk編號,並且每個ip硬碟個數都要寫
                string[] spindex = index.Split('/');
                string result = "";
                string code = "";
                string abnormal = ParentSys.Abnormal;
                string[] abn = abnormal.Split(',');
                DialogResult answer;
                for (int k = 0; k < IPNO.Length; k++)
                {
                    string[] disknumber = spindex[k].Split(',');
                    for (int i = 0; i < disknumber.Length; i++)
                    {
                        ErrorLog obj = new ErrorLog();

                        Check objc = objcs.GetObject(IPNO[k], disknumber[i]);
                        if (objc == null)
                        {
                            objc = new Check();
                            objcs.Add(objc);
                        }
                        #region 測試
                        ////測試
                        //if (k == 0)
                        //{
                        //    code = "1";
                        //    if (i == 2)
                        //    {
                        //        code = "11";
                        //    }
                        //}
                        //if (k == 1)
                        //{
                        //    code = "11";

                        //}
                        #endregion
                        #region 真實狀態
                        //讀取硬碟的狀態
                        code = snmpGet(IPNO[k], cmd, disknumber[i]);
                        
                        switch (code) //以下列舉該指令回傳值含意
                        {
                            case "0": result = "未知"; break;
                            case "1": result = "就緒 - 可以使用，但尚未分配RAID配置。"; break;
                            case "2": result = "失敗 - 無法運行。"; break;
                            case "3": result = "在線 - 已分配Operational.RAID配置。"; break;
                            case "4": result = "離線 - 該驅動器不可用於RAID控制器。"; break;
                            case "6": result = "降級 - 表示故障 - 容錯的陣列 / 虛擬磁盤的磁盤出現故障。"; break;
                            case "7": result = "恢復 - 指從磁盤上的壞塊恢復的狀態。"; break;
                            case "11": result = "已刪除 - 表示已刪除陣列磁盤。"; break;
                            case "13": result = "非RAID - 表示陣列磁盤不是支持RAID的磁盤。"; break;
                            case "14": result = "未就緒 - 適用於PCIeSSD設備，指示該設備處於鎖定狀態。"; break;
                            case "15": result = "重新同步 - 表示以下磁盤操作類型之一：轉換類型，重新配置和檢查一致性。"; break;
                            case "22": result = "正在更換 - 表示正在進行回寫操作。"; break;
                            case "24": result = "重建"; break;
                            case "25": result = "無介質 - CD - ROM或可移動磁槃無介質。"; break;
                            case "26": result = "格式化 - 在格式化過程中。"; break;
                            case "28": result = "診斷 - 診斷正在運行。"; break;
                            case "34": result = "預測性失敗"; break;
                            case "35": result = "初始化：僅適用於PERC，PERC 2 / SC和PERC 2 / DC控制器上的虛擬磁盤。"; break;
                            case "39": result = "外國"; break;
                            case "40": result = "清除"; break;
                            case "41": result = "不支持"; break;
                            case "53": result = "不兼容"; break;
                            case "56": result = "只讀 - 適用於PCIeSSD設備。表示設備已達到只讀狀態。"; break;
                            default: result = code; break;
                        }
                        #endregion
                        //是否是server
                        if (ParentSys.IsServer != "Y")
                        {
                            obj.IP = objc.IP = IPNO[k];
                            obj.Disk = objc.Disk = disknumber[i];
                            obj.Desc = objc.Desc = result;
                            for (int j = 0; j < abn.Length; j++)
                            {
                                if (code == abn[j] && objc.remark == 0)
                                {
                                    if (obj.Alarmtime.Trim().Length == 0)
                                    {
                                        obj.Alarmtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                                    }

                                    //answer = MessageBox.Show("Disc NO" + disknumber[i] + "磁碟異常，請確認", "異常訊息", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                    //if (answer == DialogResult.OK)
                                    //{
                                    //    objc.remark = 1;
                                    //    obj.ActTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                                    //}
                                    objs.Add(obj);
                                    //是否要存入資料庫(是:Y)
                                    if (ParentSys.InsertDB == "Y") obj.Save();
                                    //是否存入office Access(是:Y)
                                    if (ParentSys.InsertACCESS == "Y") Add(obj);
                                }
                            }
                        }
                        else
                        {
                            objc.IP = IPNO[k];
                            objc.Disk = disknumber[i];
                            objc.Desc = result;

                            if (i == 0 && k == 0)
                            {
                                if (ParentSys.InsertACCESS == "Y")
                                {
                                    oleDb.Open();
                                    string sql = "SELECT * FROM [資料表1]";
                                    OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDb);
                                    OleDbDataReader Reader = oleDbCommand.ExecuteReader();


                                    while (Reader.Read())
                                    {
                                        obj = new ErrorLog(Convert.ToInt32(Reader["Erroroid"]));

                                        obj.Erroroid = Convert.ToInt32(Reader["Erroroid"]);
                                        obj.IP = Convert.ToString(Reader["IP"]);
                                        obj.Alarmtime = Convert.ToString(Reader["Alarmtime"]);
                                        obj.Disk = Convert.ToString(Reader["Disk"]);
                                        obj.Desc = Convert.ToString(Reader["Descr"]);
                                        obj.ActTime = Convert.ToString(Reader["ActTime"]);

                                        objs.Add(obj);
                                        if (objs.Count > count && count != 0)
                                        {
                                            objalarm = new Alarm(Convert.ToInt32(Reader["Erroroid"]));
                                            objalarm.Erroroid = obj.Erroroid;
                                            objalarm.IP = obj.IP;
                                            objalarm.Disk = obj.Disk;

                                            objalarms.Add(objalarm);
                                        }
                                    }
                                    oleDb.Close();
                                    

                                    //假如警報資訊有新增發警報,剛開始若有警報資訊會發警報
                                    if (objs.Count > count && count != 0)
                                    {
                                        string message = "";
                                        count = objs.Count;
                                        SoundPlayer player = new SoundPlayer();
                                        player.SoundLocation = @ParentSys.musicaddress;
                                        player.Load(); //同步載入聲音
                                        player.Play(); //啟用新執行緒播放
                                        for (int m = 0; m < objalarms.Count; m++)
                                        {
                                            message += "IP=" + objalarms[m].IP + Environment.NewLine + "Disk N O" + objalarms[m].Disk + Environment.NewLine;
                                        }
                                        answer = MessageBox.Show(message + "磁碟異常，請確認", "異常訊息", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                        if (answer == DialogResult.OK)
                                        {
                                            for (int m = 0; m < objalarms.Count; m++)
                                            {
                                                obj.Erroroid = objalarms[m].Erroroid;
                                                obj.ActTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

                                                //是否要存入資料庫(是:Y)
                                                if (ParentSys.InsertDB == "Y") obj.Save();
                                                //是否存入office Access(是:Y)
                                                if (ParentSys.InsertACCESS == "Y") Update(obj);

                                            }
                                        }
                                        
                                    }
                                    if (count == 0)
                                    {
                                        count = objs.Count;
                                    }
                                }
                                else if (ParentSys.InsertDB == "Y"&&ParentSys.InsertACCESS=="N")
                                {
                                    objs = buserrorlog.GetQryData();

                                    //假如警報資訊有新增發警報,剛開始若有警報資訊會發警報
                                    if (objs.Count > count && count != 0)
                                    {
                                        string message = "";
                                        SoundPlayer player = new SoundPlayer();
                                        player.SoundLocation = @ParentSys.musicaddress;
                                        player.Load(); //同步載入聲音
                                        player.Play(); //啟用新執行緒播放
                                        for (int m = count; m < objs.Count; m++)
                                        {
                                            message += "IP=" + objs[m].IP + Environment.NewLine + "Disk N O" + objs[m].Disk + Environment.NewLine;
                                        }
                                        answer = MessageBox.Show(message + "磁碟異常，請確認", "異常訊息", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                            
                                        if (answer == DialogResult.OK)
                                        {
                                            for (int m = 0; m < objalarms.Count; m++)
                                            {
                                                obj.Erroroid = objalarms[m].Erroroid;
                                                obj.ActTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                                                //是否要存入資料庫(是:Y)
                                                if (ParentSys.InsertDB == "Y") obj.Save();
                                            }
                                        }
                                        count = objs.Count;
                                    }
                                    if (count == 0)
                                    {
                                        count = objs.Count;
                                    }
                                }
                            }
                        }
                        //硬碟狀態列表
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        DataTable dt = new DataTable();
                        dt.Columns.Add("IP", typeof(string));
                        dt.Columns.Add("Disc NO", typeof(string));
                        dt.Columns.Add("Desc", typeof(string));
                        for (int j = 0; j < objcs.Count; j++)
                        {
                            DataRow rw = dt.NewRow();
                            rw[0] = objcs[j].IP.ToString();
                            rw[1] = objcs[j].Disk.ToString();
                            rw[2] = objcs[j].Desc.ToString();


                            dt.Rows.Add(rw);
                        }
                        dataGridView1.DataSource = dt;
                        btnstop.Enabled = true;

                        //警報時間列表
                        if ((ParentSys.IsServer == "Y" && k == 0) || ParentSys.IsServer != "Y")
                        {
                            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            dt = new DataTable();
                            dt.Columns.Add("IP", typeof(string));
                            dt.Columns.Add("Alarmtime", typeof(string));
                            dt.Columns.Add("Disc NO", typeof(string));
                            dt.Columns.Add("Desc", typeof(string));
                            dt.Columns.Add("Ack Time", typeof(string));
                            for (int n = 0; n < objs.Count; n++)
                            {
                                DataRow rw = dt.NewRow();
                                rw[0] = objs[n].IP.ToString();
                                rw[1] = objs[n].Alarmtime.ToString();
                                rw[2] = objs[n].Disk.ToString();
                                rw[3] = objs[n].Desc.ToString();
                                rw[4] = objs[n].ActTime.ToString();

                                dt.Rows.Add(rw);
                            }
                            dataGridView2.DataSource = dt;
                        }

                        btnstop.Enabled = true;
                    }
                }
            }
            catch (Exception exp)
            {
                Effect.Utility.utilLogs.WriterMessage("HDCheck", "HDCheck.Form1.exec.ConnectServer _#exp:" + exp.Message.ToString());
            }
        }

        public bool Add(ErrorLog obj)
        {
            try
            {
                oleDb.Open();
                string sql = "insert into 資料表1 (IP,Alarmtime,Disk,Descr)values ('" + obj.IP + "','" + obj.Alarmtime + "','" + obj.Disk + "','" + obj.Desc+"')";
                OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDb);
                int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的數目
                oleDb.Close();
                return i > 0;
            }
            catch (Exception exp)
            {
                Effect.Utility.utilLogs.WriterMessage("HDCheck", "HDCheck.Form1.exec.Add _#exp:" + exp.Message.ToString());
                return false;
            }


        }
        public bool Update(ErrorLog obj)
        {
            try
            {
            oleDb.Open();
            string sql = "UPDATE 資料表1 set Acttime='" + obj.ActTime + "' WHERE Erroroid="+obj.Erroroid;
            OleDbCommand oleDbCommand = new OleDbCommand(sql, oleDb);
            int i = oleDbCommand.ExecuteNonQuery(); //返回被修改的數目
            oleDb.Close();
            return i > 0;
            }
            catch (Exception exp)
            {
                Effect.Utility.utilLogs.WriterMessage("HDCheck", "HDCheck.Form1.exec.Update _#exp:" + exp.Message.ToString());
                return false;
            }


        }

        public string snmpGet(string IP, string Cmd, string arrayIndex)//snmp協定發送接收器
        {
            string result = "";
            try
            {
                utilLogs.WriterMessage("HDCheck", "snmpGet step1");
                string community = "public";
                SimpleSnmp snmp = new SimpleSnmp(IP, community);
                utilLogs.WriterMessage("HDCheck", "snmpGet step2");
                if (snmp == null)
                    return "snmp is null";
                if (!snmp.Valid)//檢測伺服器是否存在
                {
                    utilLogs.WriterMessage("HDCheck", "snmpGet step3");
                    result = "無法連接設備.";
                }
                else
                {
                    utilLogs.WriterMessage("HDCheck", "snmpGet step4");
                    var code = snmp.Get(SnmpVersion.Ver2, new[] { string.Format("{0}.{1}", Cmd, arrayIndex) })
                                                          [new Oid(string.Format("{0}.{1}", Cmd, arrayIndex))].ToString(); //組合指令並發送與接收
                    utilLogs.WriterMessage("HDCheck", "snmpGet step5");
                    if (code == null || code == "")
                        return "code is null";
                    utilLogs.WriterMessage("HDCheck", "snmpGet step6 code=" + code);
                    switch (code) //以下列舉該指令回傳值含意
                    {
                        case "0": result = "未知"; break;
                        case "1": result = "就緒 - 可以使用，但尚未分配RAID配置。"; break;
                        case "2": result = "失敗 - 無法運行。"; break;
                        case "3": result = "在線 - 已分配Operational.RAID配置。"; break;
                        case "4": result = "離線 - 該驅動器不可用於RAID控制器。"; break;
                        case "6": result = "降級 - 表示故障 - 容錯的陣列 / 虛擬磁盤的磁盤出現故障。"; break;
                        case "7": result = "恢復 - 指從磁盤上的壞塊恢復的狀態。"; break;
                        case "11": result = "已刪除 - 表示已刪除陣列磁盤。"; break;
                        case "13": result = "非RAID - 表示陣列磁盤不是支持RAID的磁盤。"; break;
                        case "14": result = "未就緒 - 適用於PCIeSSD設備，指示該設備處於鎖定狀態。"; break;
                        case "15": result = "重新同步 - 表示以下磁盤操作類型之一：轉換類型，重新配置和檢查一致性。"; break;
                        case "22": result = "正在更換 - 表示正在進行回寫操作。"; break;
                        case "24": result = "重建"; break;
                        case "25": result = "無介質 - CD - ROM或可移動磁槃無介質。"; break;
                        case "26": result = "格式化 - 在格式化過程中。"; break;
                        case "28": result = "診斷 - 診斷正在運行。"; break;
                        case "34": result = "預測性失敗"; break;
                        case "35": result = "初始化：僅適用於PERC，PERC 2 / SC和PERC 2 / DC控制器上的虛擬磁盤。"; break;
                        case "39": result = "外國"; break;
                        case "40": result = "清除"; break;
                        case "41": result = "不支持"; break;
                        case "53": result = "不兼容"; break;
                        case "56": result = "只讀 - 適用於PCIeSSD設備。表示設備已達到只讀狀態。"; break;
                        default: result = code; break;
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("程式例外了:" + ex.Message);
                result = "程式例外了:" + ex.Message;
            }
            return result;
        }






    }

}
