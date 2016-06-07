using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace Test.IEmosoft.com.BusinessLogic
{
    public static class FTPFileNameCleanup
    {
        public static void CleanupAnyFiles()
        {
            try {
                string basePath = ConfigurationManager.AppSettings["reportsBasePath"];
                WalkTheDirectory(basePath);                

            }catch (Exception outterException){
                LogException(outterException, "CleanupAnyFiles");
            }
        }

        private static void WalkTheDirectory(string path)
        {
            try
            {
                CleanAnyBadFiles(path);
                var subFolders = Directory.GetDirectories(path);

                foreach (var subFolder in subFolders)
                {
                    WalkTheDirectory(subFolder);
                }
            }
            catch (Exception exp) {
                LogException(exp, "WalkTheDirectory");
            }
        }

        private static void CleanAnyBadFiles(string path)
        {
            string replaceText = "FtpTrial-";

            var files = Directory.GetFiles(path, replaceText + "*");
            foreach (var file in files)
            {
                try
                {
                    string rename = file.Replace(replaceText, "");
                    File.Copy(file, rename);
                    File.Delete(file);
                }
                catch (Exception exp){
                    LogException(exp, "CleanAnyBadFiles");
                }
            }
        }

        private static void LogException(Exception exp, string method){
            try
            {
                string msg = exp.Message;
                var inner = exp.InnerException;

                while (inner != null)
                {
                    msg += " - " + inner.Message;
                    inner = inner.InnerException;
                }

                msg = "\n\nError in " + method + ".   " + msg;

                File.AppendAllText("C:\\IEmosoftLog\\Log.txt", msg);
            }
            catch { }
        }
    }
}