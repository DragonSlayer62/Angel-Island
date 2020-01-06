/*
 *	This program is the CONFIDENTIAL and PROPRIETARY property 
 *	of Tomasello Software LLC. Any unauthorized use, reproduction or
 *	transfer of this computer program is strictly prohibited.
 *
 *      Copyright (c) 2004 Tomasello Software LLC.
 *	This is an unpublished work, and is subject to limited distribution and
 *	restricted disclosure only. ALL RIGHTS RESERVED.
 *
 *			RESTRICTED RIGHTS LEGEND
 *	Use, duplication, or disclosure by the Government is subject to
 *	restrictions set forth in subparagraph (c)(1)(ii) of the Rights in
 * 	Technical Data and Computer Software clause at DFARS 252.227-7013.
 *
 *	Angel Island UO Shard	Version 1.0
 *			Release A
 *			March 25, 2004
 */


/* Rebuild/Rebuild.cs
 * CHANGELOG:
 *	3/15/16, Adam
 *		Switch over to SVN
 *  3/10/07, Adam
 *      Replace old hardcoded email list with new distribution list: devnotify@game-master.net
 *	11/13/06, Kit
 *		Initial Creation
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using Server.SMTP;
using System.IO;

namespace rebuild
{
    class rebuild
    {
        // To add reporting emailing, fill in EmailServer and Emails:
        // Example:
        //  private const string Emails = "first@email.here;second@email.here;third@email.here";
        private static string Emails = "luke@tomasello.com";
        //wait two minutes for server to possibly fail compileing and give error code
        private static TimeSpan ServerCompileWait = TimeSpan.FromMinutes(2.0);

        private static void SendEmail(string body, bool testcenter)
        {
            Console.Write("Sending email...");

            try
            {
                MailMessage message = new MailMessage();
                if (testcenter)
                    message.Subject = "Automated RunUO Rebuild/Merge Report Test Center";
                else
                    message.Subject = "Automated RunUO Rebuild/Merge Report Production";

                message.From = new MailAddress(SmtpDirect.FromEmailAddress);
                message.To.Add(SmtpDirect.ClassicList(Emails));
                message.Body = "Automated RunUO Rebuild/Merge Report" + body;

                bool result = new SmtpDirect().SendEmail(message);
                Console.WriteLine("done: {0}", result.ToString());
            }
            catch
            {
                Console.WriteLine("failed");
            }
        }

        static void Main(string[] args)
        {
            string directory;
            string output = "";
            bool testcenter = false;

			string path = AppDomain.CurrentDomain.BaseDirectory;
			path = path.Replace("\\", "/");         // cleanup the path
			Directory.SetCurrentDirectory(path);    // set current directory to the path of this executable

            DateTime LastMessage = DateTime.Now;
            if (args.Length < 2)
            {
                Console.WriteLine("Usage is rebuild processid, test center true/false");
                return;
            }

            // 15 second delay while we attach a debugger
            //Thread.Sleep(1000 * 15);
            Debugger.Break();

            //grab process id of server.exe that was passed to us
            try
            {
                int serverid = Convert.ToInt32(args[0]);
                testcenter = Convert.ToBoolean(args[1]);
                Process p = Process.GetProcessById(serverid);

                //if active get server.exe directory and wait for it to terminate
                if (p != null && p.ProcessName.ToLower().StartsWith("server"))
                {
                    directory = p.StartInfo.WorkingDirectory;

                    while (!p.HasExited)
                    {
                        p.Refresh(); //refresh process info
                        if (DateTime.Now >= LastMessage)
                        {
                            Console.WriteLine("Waiting on {0} to exit", p.ProcessName);
                            LastMessage = DateTime.Now + TimeSpan.FromSeconds(5.0);
                        }

                        Thread.Sleep(1000);
                    }
                    Console.WriteLine("Server.exe exited");
                    Console.WriteLine("Starting svn process");

                    //server exe closed out
                    //start up svn process with checkout command line
					// svn checkout "file:///users/luket/svn/repos/Angel Island/trunk" .
                    Process svn = new Process();
                    svn.StartInfo.FileName = "svn.exe";
                    svn.StartInfo.UseShellExecute = false;
                    svn.StartInfo.RedirectStandardOutput = true;
#if DEBUG
                    svn.StartInfo.Arguments = "checkout \"svn://tamke.net/Angel Island/trunk\" .";
#else
					svn.StartInfo.Arguments = "checkout \"file:///users/luket/svn/repos/Angel Island/trunk\" ."; // for some reason on the server it prefers the path
#endif

                    if (svn.Start())
                    {
                        //wait for svn to finish before doing anything else
                        Console.WriteLine("svn started succesfully");
                        output = svn.StandardOutput.ReadToEnd();
                        svn.WaitForExit();
                        Console.WriteLine("svn exited with code {0}", svn.ExitCode);
                    }
                    else
                    {
                        output = "Svn failed to start.";
                        Console.WriteLine("Svn failed to start!!!");
                    }

                    //startup our server again
                    Process server = new Process();
                    server.StartInfo.WorkingDirectory = directory;
                    server.StartInfo.FileName = "Server.exe";
					server.StartInfo.Arguments = "-uoai";
                    if (server.Start())
                    {

                        bool WaitingOnCompile = true;
                        DateTime compilewait = DateTime.Now + ServerCompileWait;
                        while (!server.HasExited && WaitingOnCompile)
                        {
                            server.Refresh(); //refresh process info
                            if (DateTime.Now >= compilewait) //wait two minutes for server compile time if it hasnt exited
                            {
                                WaitingOnCompile = false;
                            }
                        }
                        if (server.HasExited) //we got some server error
                        {
                            output = output + "Server started but exited with code(script compile failed most likely) : " + server.ExitCode.ToString();
                        }
                        else
                        {
                            output = output + "Server process started okay.";
                            Console.WriteLine("Server.exe started okay");
                        }

                    }
                    else
                    {
                        output = output + "Server process failed to start!!!";
                        Console.WriteLine("Server.exe failed to start");
                        return;
                    }
                }
                else
                {
                    output = output + "Unable to find server process, no work done";
                    Console.WriteLine("Unable to attached to server.exe process, aborting");
                    Console.ReadLine();
                    return;
                }
                SendEmail(output, testcenter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                output = output + e.Message + e.StackTrace;
                SendEmail(output, testcenter);
                Console.WriteLine("Failed to get server.exe process");
            }
        }
    }
}
