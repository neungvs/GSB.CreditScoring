using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using WinSCP;

namespace Arsoft.Utility.FTP
{
    public class FTPClient
    {
        #region "Attributes"
            private WinSCP.Protocol _protocal = WinSCP.Protocol.Ftp;
            private string _hostname;
            private string _username;
            private string _password;
            private string _keyfingerprint;
            SessionOptions sessionOptions;
            Session session;
        #endregion

        #region "Constructors"
        /// <summary>
        /// Blank constructor
        /// </summary>
        /// <remarks>Hostname, username and password must be set manually</remarks>
        public FTPClient()
        {
        }

        /// <summary>
        /// Constructor just taking the hostname
        /// </summary>
        /// <param name="hostName">in either ftp://ftp.host.com or ftp.host.com form</param>
        /// <remarks></remarks>
        public FTPClient(string hostName)
        {
            _hostname = hostName;
        }

        /// <summary>
        /// Constructor taking hostname, username and password
        /// </summary>
        /// <param name="hostName">in either ftp://ftp.host.com or ftp.host.com form</param>
        /// <param name="userName">Leave blank to use 'anonymous' but set password to your email</param>
        /// <param name="password"></param>
        /// <remarks></remarks>
        public FTPClient(string hostName, string userName, string password)
        {
            _hostname = hostName;
            _username = userName;
            _password = password;
        }

        public FTPClient(string hostName, string userName, string password, string keyfingerprint)
        {
            _hostname = hostName;
            _username = userName;
            _password = password;
            _keyfingerprint = keyfingerprint;
            _protocal = WinSCP.Protocol.Sftp;
        }

        #endregion

        #region "Properties"

        public WinSCP.Protocol Protocal
        {
            get { return _protocal; }
            set { _protocal = value; }        
        }
        
        /// <summary>
        /// Hostname
        /// </summary>
        /// <value></value>
        /// <remarks>Hostname can be in either the full URL format
        /// ftp://ftp.myhost.com or just ftp.myhost.com
        /// </remarks>
        public string Hostname
        {
            get { return _hostname; }
            set { _hostname = value; }
        }
        
        /// <summary>
        /// Username property
        /// </summary>
        /// <value></value>
        /// <remarks>Can be left blank, in which case 'anonymous' is returned</remarks>
        public string Username
        {
            get { return (string.IsNullOrEmpty(_username) ? "anonymous" : _username); }
            set { _username = value; }
        }
        
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string HostKeyFingerprint 
        {
            get { return _keyfingerprint; }
            set { _keyfingerprint = value; }
        }
       
        /// <summary>
        /// The CurrentDirectory value
        /// </summary>
        /// <remarks>Defaults to the root '/'</remarks>
        private string _currentDirectory = "/";
        public string CurrentDirectory
        {
            //return directory, ensure it ends with /
            get { return _currentDirectory + ((_currentDirectory.EndsWith("/")) ? "" : "/").ToString(); }
            set
            {
                if (!value.StartsWith("/"))
                {
                    throw (new ApplicationException("Directory should start with /"));
                }
                _currentDirectory = value;
            }
        }

        #endregion

        #region "Supporting fns"

        public bool IsFtpConnection()
        {
            bool result = false;
            try
            {
                switch (_protocal)
                {
                    case Protocol.Ftp:
                        sessionOptions = new SessionOptions
                        {
                            Protocol = Protocol.Ftp,
                            HostName = _hostname,
                            UserName = _username,
                            Password = _password
                        };
                        break;
                    case Protocol.Scp:
                        break;
                    case Protocol.Sftp:
                        sessionOptions = new SessionOptions
                        {
                            Protocol = Protocol.Sftp,
                            HostName =_hostname,
                            UserName = _username,
                            Password = _password,
                            SshHostKeyFingerprint = _keyfingerprint
                        };
                        break;
                    default:
                        break;
                }
                using (session=new Session())
                {
                    // Connect
                    session.Open(sessionOptions);
                    result = true;
                    session.Dispose();
                }               
            }
            catch (SessionException ex)
            {
                Console.WriteLine(ex.Message);
                //throw ex;
            }
            return result;
        }

        public bool FtpConnection()
        {
            bool result = false;
            try
            {
                if (sessionOptions == null)
                {
                    switch (_protocal)
                    {
                        case Protocol.Ftp:
                            sessionOptions = new SessionOptions
                            {
                                Protocol = Protocol.Ftp,
                                HostName = _hostname,
                                UserName = _username,
                                Password = _password
                            };
                            break;
                        case Protocol.Scp:
                            break;
                        case Protocol.Sftp:
                            sessionOptions = new SessionOptions
                            {
                                Protocol = Protocol.Sftp,
                                HostName = _hostname,
                                UserName = _username,
                                Password = _password,
                                SshHostKeyFingerprint = _keyfingerprint
                            };
                            break;
                        default:
                            break;
                    }
                }

                //using (session = new Session())
                //{
                //    // Connect
                //    session.Open(sessionOptions);
                //    result = true;
                //}
                session = new Session();
                session.Open(sessionOptions);
                result = true;
            }
            catch (SessionException ex)
            {
                Console.WriteLine(ex.Message);
                //throw ex;
            }
            return result;
        }

        public bool Dispost()
        {
            bool result = false;
            try
            {
                if (session != null)
                {
                    session.Dispose();
                    result = true;
                }
            }
            catch (Exception)
            {              
                throw;
            }
            return result;
        }

        #endregion

        #region "Directory functions"

        /// <summary>
        /// Return a simple directory listing
        /// </summary>
        /// <param name="directory">Directory to list, e.g. /pub</param>
        /// <returns>A list of filenames and directories as a List(of String)</returns>
        /// <remarks>For a detailed directory listing, use ListDirectoryDetail</remarks>
        public List<string> ListDirectoryFiles(string directory)
        {
            List<string> result = new List<string>();
            //return a simple list of filenames in directory
            RemoteDirectoryInfo directoryInfo = session.ListDirectory(directory);

            foreach (RemoteFileInfo fileInfo in directoryInfo.Files)
            {
                //Console.WriteLine("{0} with size {1}, permissions {2} and last modification at {3}",
                //    fileInfo.Name, fileInfo.Length, fileInfo.FilePermissions, fileInfo.LastWriteTime);
                if (!fileInfo.IsDirectory)
                {
                    //if (fileInfo.FileType)
                    //{
                        
                    //}
                    result.Add(fileInfo.Name);
                }                
            }
            //Set request to do simple list
            return result;
        }

        /// <summary>
        /// Return a detailed directory listing
        /// </summary>
        /// <param name="directory">Directory to list, e.g. /pub/etc</param>
        /// <returns>An FTPDirectory object</returns>
        ///
        public FTPDirectory ListDirectory(string directory)
        {
            RemoteDirectoryInfo directoryInfo = session.ListDirectory(directory);
            FTPDirectory result = new FTPDirectory(directoryInfo,directory);
            return result;
        }

        public List<RemoteFileInfo> ListDirectoryInfos(string directory)
        {
            List<RemoteFileInfo> result = new List<RemoteFileInfo>();

            RemoteDirectoryInfo directoryInfo = session.ListDirectory(directory);
            result = directoryInfo.Files.ToList();
            return result;
        }

        public List<FTPFileInfo> ListDirectoryDetails(string directory)
        {
            List<FTPFileInfo> result = new List<FTPFileInfo>();

            RemoteDirectoryInfo directoryInfo = session.ListDirectory(directory);
            FTPFileInfo data;
            foreach (RemoteFileInfo fileInfo in directoryInfo.Files)
            {
                //Console.WriteLine("{0} with size {1}, permissions {2} and last modification at {3}",
                //    fileInfo.Name, fileInfo.Length, fileInfo.FilePermissions, fileInfo.LastWriteTime);
                data = new FTPFileInfo(fileInfo, directory);
                result.Add(data);
                
            }
            return result;
        }

        /// <summary>
        /// returns a full path using CurrentDirectory for a relative file reference
        /// </summary>
        private string GetFullPath(string file)
        {
            if (file.Contains("/"))
            {
                return AdjustDir(file);
            }
            else
            {
                return this.CurrentDirectory + file;
            }
        }

        /// <summary>
        /// Amend an FTP path so that it always starts with /
        /// </summary>
        /// <param name="path">Path to adjust</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string AdjustDir(string path)
        {
            return ((path.StartsWith("/")) ? "" : "/").ToString() + path;
        }
        
        #endregion

        #region "Upload: File transfer TO ftp server"

        /// <summary>
        /// Copy a local file to the FTP server
        /// </summary>
        /// <param name="localFilename">Full path of the local file</param>
        /// <param name="targetFilename">Target filename, if required</param>
        /// <returns></returns>
        /// <remarks>If the target filename is blank, the source filename is used
        /// (assumes current directory). Otherwise use a filename to specify a name
        /// or a full path and filename if required.</remarks>
        public bool Upload(string localFilename,string targetFilename)
        {
            //1. check source
            if (!File.Exists(localFilename))
            {
                throw (new ApplicationException("File " + localFilename + " not found"));
            }

            //copy to FI
            FileInfo fi = new FileInfo(localFilename);
            return Upload(fi, targetFilename);
        }

        /// <summary>
        /// Upload a local file to the FTP server
        /// </summary>
        /// <param name="fi">Source file</param>
        /// <param name="targetFilename">Target filename (optional)</param>
        /// <returns></returns>
        public bool Upload(FileInfo fi, string targetFilename)
        {
            //copy the file specified to target file: target file can be full path or just filename (uses current dir)
            bool chkError = true;
            //1. check target
            string target = null;
            if ( String.IsNullOrEmpty(targetFilename))
            {
                //Blank target: use source filename & current dir
                target = this.CurrentDirectory + Convert.ToString(fi.Name);
            }
            else if (targetFilename.Contains("/"))
            {
                //If contains / treat as a full path
                target = AdjustDir(targetFilename.Trim());
            }
            else
            {
                //otherwise treat as filename only, use current directory
                target = CurrentDirectory + targetFilename.Trim();
            }
            
            try
            {
                // Upload files
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;

                TransferOperationResult transferResult;
                transferResult = session.PutFiles(fi.FullName, target, false, transferOptions);

                // Throw on any error
                transferResult.Check();
            }
            catch (Exception ex)
            {
                chkError = false;
                throw ex;
            }
            return chkError;
        }

        public bool UploadAllFiles(string localdir, string targetdir)
        {
            //copy the file specified to target file: target file can be full path or just filename (uses current dir)
            bool chkError = true;
            //1. check target
            string target = null;
            if (String.IsNullOrEmpty(targetdir))
            {
                //Blank target: use source filename & current dir
                target = this.CurrentDirectory;
            }
            else if (targetdir.Contains("/"))
            {
                //If contains / treat as a full path
                target = AdjustDir(targetdir.Trim());
            }
            else
            {
                //otherwise treat as filename only, use current directory
                target = CurrentDirectory + targetdir.Trim();
            }

            try
            {
                // Upload files
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;

                TransferOperationResult transferResult;
                transferResult = session.PutFiles(localdir, target, false, transferOptions);

                // Throw on any error
                transferResult.Check();
            }
            catch (Exception ex)
            {
                chkError = false;
                throw ex;
            }
            return chkError;
        }
       
        public static void ExpPutFile()
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = "example.com",
                    UserName = "user",
                    Password = "mypassword",
                    SshHostKeyFingerprint = "ssh-rsa 2048 xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx"
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(@"d:\toupload\*", "/home/user/", false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                        //string finalName = transfer.Destination.Replace(".filepart", ".dat");
                        //session.MoveFile(transfer.Destination, finalName);
                    }
                }

                //return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                //return 1;
            }
        }
       
        #endregion


        #region "Download: File transfer FROM ftp server"
        /// <summary>
        /// Copy a file from FTP server to local
        /// </summary>
        /// <param name="sourceFilename">Target filename, if required</param>
        /// <param name="localFilename">Full path of the local file</param>
        /// <returns></returns>
        /// <remarks>Target can be blank (use same filename), or just a filename
        /// (assumes current directory) or a full path and filename</remarks>
        public bool Download(string sourceFilename, string localFilename, bool PermitOverwrite)
        {
            //2. determine target file
            FileInfo fi = new FileInfo(localFilename);
            return this.Download(sourceFilename, fi, PermitOverwrite);
        }

        //Version taking an FtpFileInfo
        public bool Download(FTPFileInfo file, string localFilename, bool PermitOverwrite)
        {
            return this.Download(file.FullName, localFilename, PermitOverwrite);
        }

        //Another version taking FtpFileInfo and FileInfo
        public bool Download(FTPFileInfo file, FileInfo localFI, bool PermitOverwrite)
        {
            return this.Download(file.FullName, localFI, PermitOverwrite);
        }

        //Version taking string/FileInfo
        public bool Download(string sourceFilename, FileInfo targetFI, bool PermitOverwrite)
        {
            //1. check target
            if (targetFI.Exists && !(PermitOverwrite))
            {
                throw (new ApplicationException("Target file already exists"));
            }

            //2. check source
            
            if (string.IsNullOrEmpty(sourceFilename.Trim()))
            {
                throw (new ApplicationException("File not specified"));
            }
            else if (sourceFilename.Contains("/"))
            {
                //treat as a full path
                sourceFilename = AdjustDir(sourceFilename);
            }
            else
            {
                //treat as filename only, use current directory
                sourceFilename = CurrentDirectory + sourceFilename;
            }

            //3. perform copy
            string targetFilename = targetFI.FullName;

            if (session.FileExists(sourceFilename))
            {
                bool download;
                if (!File.Exists(targetFilename))
                {
                    Console.WriteLine("File {0} exists, local backup {1} does not", sourceFilename, targetFilename);
                    download = true;
                }
                else
                {
                    DateTime remoteWriteTime = session.GetFileInfo(sourceFilename).LastWriteTime;
                    DateTime localWriteTime = File.GetLastWriteTime(targetFilename);

                    if (remoteWriteTime > localWriteTime)
                    {
                        Console.WriteLine(
                            "File {0} as well as local backup {1} exist, " +
                            "but remote file is newer ({2}) than local backup ({3})",
                            sourceFilename, targetFilename, remoteWriteTime, localWriteTime);
                        download = true;
                    }
                    else
                    {
                        Console.WriteLine(
                            "File {0} as well as local backup {1} exist, " +
                            "but remote file is not newer ({2}) than local backup ({3})",
                            sourceFilename, targetFilename, remoteWriteTime, localWriteTime);
                        download = false;
                    }
                }

                if (download)
                {
                    // Download the file and throw on any error
                    session.GetFiles(sourceFilename, targetFilename).Check();
                    Console.WriteLine("Download to backup done.");
                }
            }
            else
            {
                Console.WriteLine("File {0} does not exist yet", targetFilename);
            }

            return true;
        }


        public bool Download(string fname,string sourcePath, string targetPath,bool remove=false)
        {
            bool result = false;

            if (string.IsNullOrEmpty(sourcePath))
            {
                //throw (new ApplicationException("File not specified"));
                sourcePath = CurrentDirectory;
            }
            else if (sourcePath.Contains("/"))
            {
                //treat as a full path
                sourcePath = AdjustDir(sourcePath);
            }
            else
            {
                //treat as filename only, use current directory
                sourcePath = CurrentDirectory + sourcePath;
            }

            if (targetPath.Contains("/"))
            {
                targetPath = AdjustDir(targetPath);
            }
            sourcePath = sourcePath + fname;
            //targetPath = targetPath + fname;


            // Manual "remote to local" synchronization.
            // You can achieve the same using:
            // session.SynchronizeDirectories(
            //     SynchronizationMode.Local, localPath, remotePath, false, false, SynchronizationCriteria.Time, 
            //     new TransferOptions { FileMask = fileName }).Check();

            //if (session.FileExists(sourcePath)) // not check wildcard (*.txt)
            //{

            bool download;
            if (!File.Exists(targetPath))
            {
                Console.WriteLine("File {0} exists, local backup {1} does not", sourcePath, targetPath);
                download = true;
            }
            else
            {
                DateTime remoteWriteTime = session.GetFileInfo(sourcePath).LastWriteTime;
                DateTime localWriteTime = File.GetLastWriteTime(targetPath);

                if (remoteWriteTime > localWriteTime)
                {
                    Console.WriteLine(
                        "File {0} as well as local backup {1} exist, " +
                        "but remote file is newer ({2}) than local backup ({3})",
                        sourcePath, targetPath, remoteWriteTime, localWriteTime);
                    download = true;
                }
                else
                {
                    Console.WriteLine(
                        "File {0} as well as local backup {1} exist, " +
                        "but remote file is not newer ({2}) than local backup ({3})",
                        sourcePath, targetPath, remoteWriteTime, localWriteTime);
                    download = false;
                }
            }

            if (download)
            {
                // Download the file and throw on any error
                session.GetFiles(sourcePath, targetPath,remove).Check();
                result = true;
                Console.WriteLine("Download to backup done.");
            }

            //}
            //else
            //{
            //    Console.WriteLine("File {0} does not exist yet", sourcePath);
            //}

            return result;
        }

        public bool IsFileFileExists(string fname, string sourcePath)
        {
            bool result = false;

            if (string.IsNullOrEmpty(sourcePath))
            {
                //throw (new ApplicationException("File not specified"));
                sourcePath = CurrentDirectory;
            }
            else if (sourcePath.Contains("/"))
            {
                //treat as a full path
                sourcePath = AdjustDir(sourcePath);
            }
            else
            {
                //treat as filename only, use current directory
                sourcePath = CurrentDirectory + sourcePath;
            }

            sourcePath = sourcePath + fname;
            if (session.FileExists(sourcePath)) // not check wildcard (*.txt)
            {
                result = true;
            }
            else
            {
                Console.WriteLine("File {0} does not exist yet", sourcePath);
            }
            return result;
        }

        public static void ExpGetFile()
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = "127.0.0.1",
                    UserName = "FTPAdmin",
                    Password = "password",
                    SshHostKeyFingerprint = "ssh-rsa 2048 93:d5:0d:75:2c:ec:0a:0b:ec:33:ce:6b:43:f8:96:54"
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    string stamp = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                    string fileName = "export_" + stamp + ".txt";
                    string remotePath = "/home/user/sysbatch/" + fileName;
                    string localPath = "d:\\backup\\" + fileName;

                    // Manual "remote to local" synchronization.

                    // You can achieve the same using:
                    // session.SynchronizeDirectories(
                    //     SynchronizationMode.Local, localPath, remotePath, false, false, SynchronizationCriteria.Time, 
                    //     new TransferOptions { FileMask = fileName }).Check();
                    if (session.FileExists(remotePath))
                    {
                        bool download;
                        if (!File.Exists(localPath))
                        {
                            Console.WriteLine("File {0} exists, local backup {1} does not", remotePath, localPath);
                            download = true;
                        }
                        else
                        {
                            DateTime remoteWriteTime = session.GetFileInfo(remotePath).LastWriteTime;
                            DateTime localWriteTime = File.GetLastWriteTime(localPath);

                            if (remoteWriteTime > localWriteTime)
                            {
                                Console.WriteLine(
                                    "File {0} as well as local backup {1} exist, " +
                                    "but remote file is newer ({2}) than local backup ({3})",
                                    remotePath, localPath, remoteWriteTime, localWriteTime);
                                download = true;
                            }
                            else
                            {
                                Console.WriteLine(
                                    "File {0} as well as local backup {1} exist, " +
                                    "but remote file is not newer ({2}) than local backup ({3})",
                                    remotePath, localPath, remoteWriteTime, localWriteTime);
                                download = false;
                            }
                        }

                        if (download)
                        {
                            // Download the file and throw on any error
                            session.GetFiles(remotePath, localPath).Check();

                            Console.WriteLine("Download to backup done.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("File {0} does not exist yet", remotePath);
                    }
                }

                //return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                //return 1;
            }
        }

        #endregion

        #region "Other functions: Delete rename etc."
        
        #endregion
    }
}
