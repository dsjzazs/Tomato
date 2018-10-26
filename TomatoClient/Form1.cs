using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetMQ;
using Tomato.Net;
using Tomato.Protocol.Request;
using Tomato.Protocol.Response;

namespace Tomato.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
  
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var login = new ReqUserLogin()
            {
                UserName = textBox1.Text,
                PassWord = textBox2.Text
            };
            var res = await NetClient.Instance.Request<ResUserLogin>(login);
            if (res.Success)
            {
                NetClient.Instance.Session = res.Session;
                Console.WriteLine($"登陆成功!\r\n{res.Session.ToString()}");
                label3.Text = ($"登陆成功!\r\n{res.Session.ToString()}");
            }
            else
                label3.Text = (res.Message);
            button1.Enabled = true;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            var reg = new ReqUserRegister()
            {
                UserName = textBox1.Text,
                Password = textBox2.Text
            };
            var res = await NetClient.Instance.Request<ResUserRegister>(reg);
            if (res.Success)
                label3.Text = ($"注册成功!\r\n{res.Session.ToString()}");
            else
                label3.Text = (res.Message);
            button2.Enabled = true;
        }
    }
}
