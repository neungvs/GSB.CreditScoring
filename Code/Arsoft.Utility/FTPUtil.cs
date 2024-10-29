using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using WinSCP;

namespace Arsoft.Utility
{
    public class FTPUtil
    {
        enum FTPProtocal { FTP,SFTP };

        #region "Attributes"
            string hostName;
            string user;
            string pwd;
            string keyFingerPrint;
        #endregion

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
    
    }


}
