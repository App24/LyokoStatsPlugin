// MIT License - Copyright (c) 2016 Can Güney Aksakalli
// https://aksakalli.github.io/2014/02/24/simple-http-server-with-csparp.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using LyokoAPI.Plugin;

namespace LyokoStatsPlugin
{
    class SimpleHTTPServer
    {
        private Thread _serverThread;
        private HttpListener _listener;
        private int _port;
        public PluginConfig PluginConfig;

        public int Port
        {
            get { return _port; }
            private set { }
        }

        /// <summary>
        /// Construct server with given port.
        /// </summary>
        /// <param name="port">Port of the server.</param>
        public SimpleHTTPServer(int port)
        {
            this.Initialize(port);
        }

        /// <summary>
        /// Construct server with suitable port.
        /// </summary>
        public SimpleHTTPServer()
        {
            //get an empty port
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            this.Initialize(port);
        }

        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            _serverThread.Abort();
            _listener.Stop();
        }

        private void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + _port.ToString() + "/");
            _listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();
                    Process(context);
                }
                catch{}
            }
        }

        private void Process(HttpListenerContext context)
        {

            string html = @"
                    <html>
                        <head>
                            <title>Lyoko Statistics</title>
                        </head>
                        <body>";

            foreach (var item in PluginConfig.Values)
            {
                if (!item.Key.StartsWith("stat_")) continue;
                string key = item.Key.Substring(5);
                html += $"<p><span style=\"font-weight:bold;\">{key}</span>: {item.Value}</p>";
            }

            html += @"
                        </body>
                    </html>";

            try
            {

                // convert string to stream
                byte[] byteArray = Encoding.UTF8.GetBytes(html);
                //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                MemoryStream input = new MemoryStream(byteArray);

                //Adding permanent http response headers
                context.Response.ContentType = "text /html";
                context.Response.ContentLength64 = input.Length;
                context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                context.Response.AddHeader("Last-Modified", new DateTime().ToString("r"));

                byte[] buffer = new byte[1024 * 16];
                int nbytes;
                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, nbytes);
                input.Close();

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Response.OutputStream.Close();
        }

        private void Initialize(int port)
        {
            this._port = port;
            _serverThread = new Thread(this.Listen);
            _serverThread.IsBackground = true;
            _serverThread.Start();
        }
    }
}
