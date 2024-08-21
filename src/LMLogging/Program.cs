/*
 * Copyright, 2022, LogicMonitor, Inc.
 * This Source Code Form is subject to the terms of the 
 * Mozilla Public License, v. 2.0. If a copy of the MPL 
 * was not distributed with this file, You can obtain 
 * one at https://mozilla.org/MPL/2.0/.
 */

using System;
using LogicMonitor.DataSDK.Model;
using LogicMonitor.DataSDK.Api;
using LogicMonitor.DataSDK;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using RestSharp;
using Microsoft.Extensions.Logging;

namespace LMLogging
{
    class Program
    {
        static void Main(string[] args)
        {
            var temp = Process.GetCurrentProcess();
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().PrivilegedProcessorTime;
            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;

            ILogger logs = new LMLogger();

            while (true)
            {
                string msg = $"Program function  has CPU Usage " + (cpuUsedMs / (Environment.ProcessorCount * totalMsPassed)).ToString() + " Milliseconds across (" + Environment.ProcessorCount+ ")";

                logs.Log(LogLevel.Warning, msg);
                Thread.Sleep(15000);
            }
        }
    }
    class MyResponse : IResponseInterface
    {
        public void ErrorCallback(RestResponse response)
        {
            Console.WriteLine("Custom message for ErrorCallback");
        }

        public void SuccessCallback(RestResponse response)
        {
            Console.WriteLine("Custom message for SuccessCallback");
        }
    }
}
