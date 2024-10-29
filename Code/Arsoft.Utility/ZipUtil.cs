using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;

namespace Arsoft.Utility
{
    public class ZipUtil
    {

        public static void Compress(string fileName,string zipName)
        {
            using (ZipFile zip = new ZipFile( ))
            {
                zip.AddFile(fileName,"");
                zip.Save(zipName);
            }
        }

        public static void Compress(ArrayList fileNames, string zipName)
        {
            using (ZipFile zip = new ZipFile( ))
            {
                foreach (var file in fileNames)
                {
                    zip.AddFile(file.ToString(),"");
                }                
                zip.Save(zipName);
            }
        }

        public static void Decompress(string zipName, string targetDirectory)
        {
            using (ZipFile zip = ZipFile.Read(zipName))
            {
                foreach (ZipEntry e in zip)
                {
                    e.Extract(targetDirectory);
                }
            }
        }

    }
}
