using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace ПР16
{
    public partial class Form1 : Form
    {
        private HttpListener _listener;
        private string _rootDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button startButton = new Button();
            startButton.Text = "Запустити сервер";
            startButton.Location = new Point(10, 10);
            startButton.Click += StartServer;
            Controls.Add(startButton);

            Button stopButton = new Button();
            stopButton.Text = "Зупинити сервер";
            stopButton.Location = new Point(10, 40);
            stopButton.Click += StopServer;
            Controls.Add(stopButton);
        }

        private void StartServer(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                _rootDirectory = folderBrowser.SelectedPath;
            }
            else
            {
                MessageBox.Show("Кореневу папку не вибрано.");
                return;
            }

            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:8080/");
            _listener.Start();

            _listener.BeginGetContext(ListenerCallback, null);

            MessageBox.Show($"Сервер запущено. Коренева папка: {_rootDirectory}");
        }

        private void ListenerCallback(IAsyncResult result)
        {
            HttpListenerContext context = _listener.EndGetContext(result);

            _listener.BeginGetContext(ListenerCallback, null);

            HandleRequest(context);
        }

        private void HandleRequest(HttpListenerContext context)
        {
            string filename = context.Request.RawUrl.Substring(1);
            string filePath = Path.Combine(_rootDirectory, filename);

            if (File.Exists(filePath))
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                context.Response.ContentType = GetContentType(filePath);
                context.Response.ContentLength64 = fileBytes.Length;
                context.Response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
            }
            else
            {
                string message = "404 Not Found";
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentLength64 = messageBytes.Length;
                context.Response.OutputStream.Write(messageBytes, 0, messageBytes.Length);
            }

            context.Response.OutputStream.Close();
        }

        private string GetContentType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".html":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "application/javascript";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream";
            }
        }

        private void StopServer(object sender, EventArgs e)
        {
            if (_listener != null)
            {
                _listener.Stop();
                _listener = null;
                MessageBox.Show("Сервер зупинено.");
            }
        }
    }
}