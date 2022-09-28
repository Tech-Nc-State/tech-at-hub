using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Tech_HubAPI.Services
{
    public class ExecuteService
    {
        private readonly bool _windows;

        public ExecuteService(IConfiguration configuration)
        {
            _windows = configuration["Environment:Platform"] == "Windows";
            WorkingDirectory = configuration["Environment:DefaultWorkingDirectory"].Replace("\\", "/");
            ExecutableDirectory = configuration["Environment:BinPath"].Replace("\\", "/");
        }

        /// <summary>
        /// The working directory for this execute service. Defaults to the tech-at-hub
        /// project root directory.
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Path to folder containing the executables this execute service will run. Defaults
        /// to a system dependent folder.
        /// </summary>
        public string ExecutableDirectory { get; set; }

        /// <summary>
        /// Executes the given process. <see cref="WorkingDirectory"/> controls the
        /// working directory. The <see cref="ExecutableDirectory"/> path will be appended
        /// to the beginning of the program name. If the process is running on Windows, .exe
        /// will be appended to the end of the program name.
        /// </summary>
        /// <param name="programName">Name of the process to execute.</param>
        /// <param name="args">List of command line arguments to pass to the process.</param>
        /// <returns>Standard output from the process.</returns>
        /// <exception cref="Exception">If the processes exit code is not zero.</exception>
        public string ExecuteProcess(string programName, params string[] args)
        {
            if (_windows)
            {
                programName += ".exe";
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
            using Process myProcess = new Process();
            //myProcess.StartInfo.UseShellExecute = false;
            // You can start any process, HelloWorld is a do-nothing example.

            myProcess.StartInfo.FileName = ExecutableDirectory + programName;
            myProcess.StartInfo.Arguments = argsString;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo.RedirectStandardError = true;
            myProcess.StartInfo.CreateNoWindow = false;
            myProcess.StartInfo.WorkingDirectory = WorkingDirectory;

            myProcess.Start();

            // read all the output, store to output.
            output = myProcess.StandardOutput.ReadToEnd();
            myProcess.WaitForExit();

            if (myProcess.ExitCode != 0)
            {
                throw new Exception("Process exited with a status code of "
                    + myProcess.ExitCode + ". "
                    + myProcess.StandardError.ReadToEnd());
            }
            // end yoink

            return output.Trim();
        }
    }
}
