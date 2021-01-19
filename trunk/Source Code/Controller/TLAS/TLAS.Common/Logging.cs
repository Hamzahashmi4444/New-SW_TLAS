using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TLAS.Common
{
    public enum LoggingType
    {
        Error,
        Message,
        MissingRecord
    }

    public class Logging
    {
        
        
        public static void TraceLog(string message, LoggingType Type)
        {  
                string tracingDirectory = @"C:\CIPLogs" +"\\"+ DateTime.Now.ToString("yyyy-MM") + "\\";
                string fileName;

                StreamWriter sr = null;

                DateTime now = DateTime.Now;
                try
                {
                    if (Directory.Exists(tracingDirectory) == false)
                    {
                        Directory.CreateDirectory(tracingDirectory);
                    }
                    if (tracingDirectory == null || tracingDirectory.Length == 0)
                        fileName = String.Concat(Type, " - ", now.ToString("dd MMM yyyy HH"), ".txt");
                    else
                        fileName = String.Concat(tracingDirectory, Type, " - ", now.ToString("dd MMM yyyy HH"), ".txt");
                    // write message
                    sr = File.AppendText(fileName);
                    sr.WriteLine(DateTime.Now.ToString() + " : " + message);
                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    if (sr != null)
                        sr.Close();
                }
            

            
        }

    }
}

