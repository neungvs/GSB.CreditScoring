using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSCP;

namespace Arsoft.Utility.FTP
{
    /// <summary>
    /// Stores a list of files and directories from an FTP result
    /// </summary>
    /// <remarks></remarks>
    public class FTPDirectory : List<FTPFileInfo>
    {
        //creates a blank directory listing
        public FTPDirectory()
        {
        }

        /// <summary>
        /// Constructor: create list from a (detailed) directory string
        /// </summary>
        /// <param name="dir">directory listing string</param>
        /// <param name="path"></param>
        /// <remarks></remarks>
        public FTPDirectory(RemoteDirectoryInfo dir, string path)
        {
            FTPFileInfo data;
            foreach (RemoteFileInfo fileInfo in dir.Files)
            {
                //Console.WriteLine("{0} with size {1}, permissions {2} and last modification at {3}",
                //    fileInfo.Name, fileInfo.Length, fileInfo.FilePermissions, fileInfo.LastWriteTime);
                data = new FTPFileInfo(fileInfo, path);
                this.Add(data);

            }
        }

        /// <summary>
        /// Filter out only files from directory listing
        /// </summary>
        /// <param name="ext">optional file extension filter</param>
        /// <returns>FTPdirectory listing</returns>
        public FTPDirectory GetFiles(string ext)
        {
            return this.GetFileOrDir(FTPFileInfo.DirectoryEntryTypes.File, ext);
        }

        /// <summary>
        /// Returns a list of only subdirectories
        /// </summary>
        /// <returns>FTPDirectory list</returns>
        /// <remarks></remarks>
        public FTPDirectory GetDirectories()
        {
            return this.GetFileOrDir(FTPFileInfo.DirectoryEntryTypes.Directory, "");
        }

        //internal: share use function for GetDirectories/Files
        private FTPDirectory GetFileOrDir(FTPFileInfo.DirectoryEntryTypes type, string ext)
        {
            FTPDirectory result = new FTPDirectory();
            foreach (FTPFileInfo fi in this)
            {
                if (fi.FileType == type)
                {
                    if (string.IsNullOrEmpty(ext))
                    {
                        result.Add(fi);
                    }
                    else if (ext == fi.Extension)
                    {
                        result.Add(fi);
                    }
                }
            }
            return result;

        }

        public bool FileExists(string filename)
        {
            foreach (FTPFileInfo ftpfile in this)
            {
                if (ftpfile.Filename == filename)
                {
                    return true;
                }
            }
            return false;
        }

        private const char slash = '/';
        public static string GetParentDirectory(string dir)
        {
            string tmp = dir.TrimEnd(slash);
            int i = tmp.LastIndexOf(slash);
            if (i > 0)
            {
                return tmp.Substring(0, i - 1);
            }
            else
            {
                throw (new ApplicationException("No parent for root"));
            }
        }
    
    }
}
