using System;
using System.Collections.Generic;
using System.Linq;
using Fleck;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApplication2
{
    class Program
    {
        private static PerformanceCounter cpuCounter;
        static void Main(string[] args)
        {
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";


            var allSockets = new List<IWebSocketConnection>();

                var server = new WebSocketServer("ws://0.0.0.0:8181");

                server.Start(socket =>
                {
                    socket.OnOpen = () =>
                    {
                        allSockets.Add(socket);
                    };
                    socket.OnClose = () =>
                    {
                        allSockets.Remove(socket);
                    };
                    
                });

           
                var input = Console.ReadLine();
                while (input != "exit")
                {
                    foreach (var socket in allSockets.ToList())
                    {
                    socket.Send(cpuCounter.NextValue().ToString().Replace(',','.'));
                    Thread.Sleep(50);
                    }
                   
                }

            }

        }
    }

