using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using WinSCP;

namespace Arsoft.Utility.FTP
{
    public class FTPFileInfo
    {
        //Stores extended info about FTP file
        #region "Properties"
            private string _filename;
            private string _path;
            private DirectoryEntryTypes _fileType;
            private long _size;
            private DateTime _fileDateTime;
            private string _permission;

            public string FullName
            {
                get { return Path + Filename; }
            }

            public string Filename
            {
                get { return _filename; }
            }

            public string Path
            {
                get { return _path; }
            }

            public DirectoryEntryTypes FileType
            {
                get { return _fileType; }
            }

            public long Size
            {
                get { return _size; }
            }

            public DateTime FileDateTime
            {
                get { return _fileDateTime; }
            }

            public string Permission
            {
                get { return _permission; }
            }

            public string Extension
            {
                get
                {
                    int i = this.Filename.LastIndexOf(".");
                    if (i >= 0 && i < (this.Filename.Length - 1))
                    {
                        return this.Filename.Substring(i + 1);
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            public string NameOnly
            {
                get
                {
                    int i = this.Filename.LastIndexOf(".");
                    if (i > 0)
                    {
                        return this.Filename.Substring(0, i);
                    }
                    else
                    {
                        return this.Filename;
                    }
                }
            }

        #endregion

        /// <summary>
        /// Identifies entry as either File or Directory
        /// </summary>
        public enum DirectoryEntryTypes
        {
            File,
            Directory
        }

        /// <summary>
        /// Constructor taking a directory listing line and path
        /// </summary>
        /// <param name="line">The line returned from the detailed directory list</param>
        /// <param name="path">Path of the directory</param>
        /// <remarks></remarks>

        public FTPFileInfo(RemoteFileInfo finfo, string path)
        {
            //parse line
            if (finfo == null)
            {
                //failed
                throw (new ApplicationException("Unable to parse line: " + finfo));
            }
            else
            {
                _filename = finfo.Name;
                _path = path;

                //Int64.TryParse(m.Groups["size"].Value, out _size);
                _size = finfo.Length;

                _permission = finfo.FilePermissions.ToString();
                
                if (finfo.IsDirectory)
                {
                    _fileType = DirectoryEntryTypes.Directory;
                }
                else
                {
                    _fileType = DirectoryEntryTypes.File;
                }

                _fileDateTime = finfo.LastWriteTime;

            }
        }
           
    }
}
