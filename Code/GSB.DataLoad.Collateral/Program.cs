using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using Arsoft.Utility;
using Arsoft.Utility.FTP;
using Arsoft.Utility.MSSQL;


namespace GSB.DataLoad.Collateral
{
    class Program
    {

        static void Main(string[] args)
        {
            string _host = ConfigurationManager.AppSettings["ftp_host"].ToString();
            string _user = ConfigurationManager.AppSettings["ftp_user"].ToString();
            string _pw = ConfigurationManager.AppSettings["ftp_password"].ToString();
            string _protocol = ConfigurationManager.AppSettings["ftp_protocol"].ToString();
            string _hostkey = ConfigurationManager.AppSettings["ftp_hostkey"].ToString();
            string _data = ConfigurationManager.AppSettings["app_data"].ToString();
            string fname = ConfigurationManager.AppSettings["file_collateral"].ToString();
            string pathftp = ConfigurationManager.AppSettings["ftp_path_collateral"].ToString();
            string pathlocal = ConfigurationManager.AppSettings["app_data"].ToString();

            MsSqlConfiguration.ConfigurationConnectionString("GSBSQLServer");
            //string sourcefile="";
            string localfile = "";
            LogFiles.SetLogFolder = ConfigurationManager.AppSettings["Logfile"];

            FTPClient ftps;
            try
            {
                if (_protocol.ToUpper() == "SFTP")
                {
                    ftps = new FTPClient(_host, _user, _pw, _hostkey);
                }
                else
                {
                    ftps = new FTPClient(_host, _user, _pw);
                }

                if (ftps.FtpConnection())
                {
                    //sourcefile = ftps.AdjustDir(pathftp) + fname;
                    localfile = pathlocal + fname;
                    FileInfo fi = new FileInfo(localfile);
                    if (File.Exists(fi.FullName))
                    {
                        fi.Delete();
                    }

                    if (ftps.IsFileFileExists(fname, pathftp))
                    {
                        if (ftps.Download(fname,pathftp,pathlocal,true))
                        {
                            DataLoading(fi.FullName);
                        }                       
                    }
                    else
                    {
                        //UpdateLog(fname, DateTime.Now, 0, "DONE", "DONE :" + fi.FullName + " not found.", "002");
                        LogFiles.WriteToLog("GSB.DataLoad.Collateral", "Main", "DONE :" + fi.FullName + " not found.");
                    } 

                }
                else
                {
                    UpdateLog(fname, DateTime.Now, 0, "FAIL", "Could not connect ftp server.", "001");
                }

            }
            catch (Exception ex)
            {
                UpdateLog(fname, DateTime.Now, 0, "FAIL", ex.Message, "001");
                LogFiles.WriteToLog("GSB.DataLoad.Collateral", "Main", ex.Message);
                //throw ex;
            }

            //ftps.Upload(@"D:\1.jpg", null);
            //ftps.UploadAllFiles(@"C:\logs\*", null);
            //ftps.Download("*.txt", null, @"C:\logs\", true);
            //ftps.Download("sshkey.key", null, @"C:\logs\", true);
            //ftps.Download("sshkey.key", @"C:\logs\sshkey.key", false);
    
            //Console.WriteLine("Please any key..");
            //Console.ReadKey();
        }

        
        public static void DataLoading(string filename)
        {
            bool result = false;
            try
            {
                DataLoading ctrlLoading = new DataLoading();
                //Check Header

                string header = ctrlLoading.ReadFirstLine(filename); //Read Header file
                //string[] dataHeader = header.Split('|');
                if (header.Length== 17)
                {
                    result = true;
                }

                ////Check Footer
                //int numRow = 0;
                //if (result)
                //{
                //    string footer = ctrlLoading.ReadLastLine(filename);// Read Footer file
                //    numRow = ctrlLoading.CountLine(filename) - 1;
                //    //string[] dataFooter = footer.Split('|');
                    
                //}

                //Check Data
                if (result)
                {
                    string recordDetail = "";
                    string[] dataDetail;
                    string fname = ConfigurationManager.AppSettings["file_collateral"].ToString();
                    using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.Default))
                    {
                        int i = 0;
                        ctrlLoading.BulkCreateSourceData();

                        while (!sr.EndOfStream)
                        {
                            recordDetail = sr.ReadLine();
                            dataDetail = recordDetail.ToString().Split('|');
                            if (dataDetail.Length==5)
                            {
                                if (dataDetail[0].Trim().Length > 0)
                                {
                                    ctrlLoading.BulkAddData(recordDetail, header, i);
                                }
                            }
                            i++;
                        }

                        int total = ctrlLoading.CountData;
                        if (ctrlLoading.BulkSaveData())
                        {
                            UpdateLog(fname, DateTime.Now, total, "DONE", "Data import suessed.", "003");
                        }
                        else
                        {
                            UpdateLog(fname, DateTime.Now, 0, "FAIL", "Could not import data.", "003");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogFiles.WriteToLog("GSB.DataLoad.Collateral", "DataLoading", ex.Message);
                //throw ex;
            }
        }

        public static void UpdateLog(string fname,DateTime sdate,int total, string status, string msg, string groupid)
        {
            string sql = null;
            MsSqlAccess dbAccess = new MsSqlAccess();
            MsSqlParameter[] param = new MsSqlParameter[6];

            try
            {
                sql = "INSERT INTO s_log_import( "
                      + " resources,start_date,end_date,total,status,error_message,group_id) "
                      + "VALUES( "
                      + " @resources,@start_date,getdate(),@total,@status,@error_message,@group_id) ";

                param[0] = new MsSqlParameter("@resources", DbUtility.GetString(fname));
                param[1] = new MsSqlParameter("@start_date", DbUtility.GetDate(sdate));
                param[2] = new MsSqlParameter("@total", DbUtility.GetNumeric(total));
                param[3] = new MsSqlParameter("@status", DbUtility.GetString(status));
                param[4] = new MsSqlParameter("@error_message", DbUtility.GetString(msg));
                param[5] = new MsSqlParameter("@group_id", DbUtility.GetString(groupid));

                dbAccess.ExecuteNonQuery(sql, param);
            }
            catch (Exception ex)
            {
                LogFiles.WriteToLog("DataLoading", "UpdateLog()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (dbAccess != null)
                {
                    dbAccess.Dispose();
                    dbAccess = null;
                }
            }
        }

    }
}
