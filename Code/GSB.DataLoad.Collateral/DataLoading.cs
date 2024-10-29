using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using Arsoft.Utility;
using Arsoft.Utility.MSSQL;

namespace GSB.DataLoad.Collateral
{
    public class DataLoading
    {

        #region "Attributes"
            DataTable sourceData;
        #endregion

        #region "Methods Check File"

        public string ReadFirstLine(string filename)
        {
            string returnValue = "";
            using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.Default))
            {
                returnValue = sr.ReadLine();
                sr.Close();

            }
            return returnValue;
        }

        public string ReadLastLine(string filename)
        {
            string returnValue = "";
            try
            {
                FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                if (stream.Length > 1024)
                {
                    stream.Seek(-1024, SeekOrigin.End);     // rewind enough for > 1 line 
                }

                StreamReader reader = new StreamReader(stream);
                reader.ReadLine();      // discard partial line 
                string nextLine;
                while (!reader.EndOfStream)
                {
                    nextLine = reader.ReadLine();
                    if (nextLine != null)
                    {
                        if (nextLine.Trim().Length > 3)
                        {
                            if (nextLine.Substring(0, 3) == "FT|")
                            {
                                returnValue = nextLine;
                            }
                        }
                    }
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                LogFiles.WriteToLog("DataValidateService", "ReadLastLine()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            return returnValue;
        }

        public int CountLine(string filename)
        {
            int result = 0;
            using (StreamReader fs = new StreamReader(filename))
            {
                while (fs.ReadLine() != null)
                {
                    result++;
                }
                fs.Close();
            }
            return result;
        }

        public int CountData 
        { 
            get
            {
                return sourceData.Rows.Count;            
            }        
        }

        #endregion

        #region "Bulk Copy"

        public void BulkCreateSourceData()
        {
            MsSqlAccess dbAccess = new MsSqlAccess();
            try
            {
                sourceData = dbAccess.GetTableStructure("cbs_ln_col");
            }
            catch (Exception ex)
            {
                LogFiles.WriteToLog("DataLoading", "BulkCreateSourceData()", ex.Message);
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

        public void BulkAddData(string dataDetail, string dt,int idx)
        {
            string[] data = null;
            DataRow row = null;
            try
            {
                row = sourceData.NewRow();
                data = dataDetail.Split('|');

                row["CBS_APP_NO"] = DbUtility.GetString(data[0]);
                row["CBS_COLLTYPE"] = DbUtility.GetString(data[1]);
                row["CBS_COLLSTYPE"] = DbUtility.GetString(data[2]);
                row["ID"] = DbUtility.GetString(data[3]);
                row["VALUE"] = DbUtility.GetNumeric(data[4]);
                row["BATCH_UPDATE_DTM"] = dt;

                //row["cid"] = idx;

                sourceData.Rows.Add(row);

            }
            catch (Exception ex)
            {
                LogFiles.WriteToLog("DataLoading", "BulkCopyAccount()", ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                //
            }
        }

        public bool BulkSaveData()
        {
            bool result = false;
            if (sourceData.Rows.Count > 0)
            {
                MsSqlAccess dbAccess = new MsSqlAccess();
                try
                {
                    dbAccess.BeginBulkCopy();
                    dbAccess.BulkCopyData(sourceData, "cbs_ln_col");
                    dbAccess.EndBulkCopy();
                    sourceData.Rows.Clear();
                    result = true;
                }
                catch (Exception ex)
                {
                    LogFiles.WriteToLog("DataLoading", "BulkSaveData()", ex.Message);
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
            return result;
        }
        
        #endregion
    }
}
