using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace Tech_HubAPI.Services
{
    public class Execute
    {
        private const bool Windows = true;

        private const string PathPrefix = Windows
            ? "C:\\Program Files\\Git\\usr\\bin\\"
            : "";

        private const string PathPostfix = Windows
            ? ".exe"
            : "";

        public string ExecuteProcess(string programName, params string[] args)
        {
            if (!programName.Contains("\\"))
            {
                programName = PathPrefix + programName + PathPostfix;
            }

            string output = "";

            // setup arguments to single string
            string argsString = "";
            int maxArgsIndex = args.Length - 1;
            if (args.Length != 0)
            {
                for (int i = 0; i < maxArgsIndex; i++)
                {
                    // add spaces
                    argsString += args[i] + " ";
                }
                // last one is unspaced
                argsString += args[maxArgsIndex];
            }

            // yoinked from MS Docs
            try
            {
                using Process myProcess = new Process();
                //myProcess.StartInfo.UseShellExecute = false;
                // You can start any process, HelloWorld is a do-nothing example.

                myProcess.StartInfo.FileName = programName;
                myProcess.StartInfo.Arguments = argsString;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.CreateNoWindow = false;

                myProcess.Start();

                // read all the output, store to output.
                output = myProcess.StandardOutput.ReadToEnd();
            }
            catch (Exception e)
            {
                // might as well print the error i guess.
                // actually this turned out to be very useful for unit tests so yeah.
                output = e.Message;
            }
            // end yoink

            return output.Trim();
        }

    }
}

