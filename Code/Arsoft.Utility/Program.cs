using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arsoft.Utility.FTP;

namespace Arsoft.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = "ssh-rsa 1024 a0:83:b7:98:19:43:69:56:6d:61:a6:6c:71:07:5a:15";
            FTPClient ftps = new FTPClient("127.0.0.1","FTPAdmin","password",key);
            ftps.FtpConnection();

            //ftps.Upload(@"D:\1.jpg", null);
            //ftps.UploadAllFiles(@"C:\logs\*", null);
            //ftps.Download("*.txt", null, @"C:\logs\", true);
            //ftps.Download("sshkey.key", null, @"C:\logs\", true);
            ftps.Download("sshkey.key", @"C:\logs\sshkey.key", false);

            //List<string> datas = ftps.ListDirectory(@"/");

            Console.WriteLine("Please any key..");
            Console.ReadKey();
        }
    }
}
