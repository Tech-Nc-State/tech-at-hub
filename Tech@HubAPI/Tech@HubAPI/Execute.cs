using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace Tech_HubAPI
{
    public class Execute
    {
        public static string ExecuteProcess(string programName, params string[] args)
        {
            string output = "";

            // setup arguments to single string
            string argsString = "";
            int maxArgsIndex = args.Length - 1;
            for (int i = 0; i < maxArgsIndex; i++)
            {
                // add spaces
                argsString += args[i] + " ";
            }
            // last one is unspaced
            argsString += args[maxArgsIndex];

            // yoinked from MS Docs
            try
            {
                using (Process myProcess = new Process())
                {
                    //myProcess.StartInfo.UseShellExecute = false;
                    // You can start any process, HelloWorld is a do-nothing example.
                    myProcess.StartInfo.FileName = programName;
                    myProcess.StartInfo.Arguments = argsString;

                    myProcess.Start();

                    // read all the output, store to output.
                    output = myProcess.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                // might as well print the error i guess.
                output = e.Message;
            }
            // end yoink

            return output;
        }



    }
}
