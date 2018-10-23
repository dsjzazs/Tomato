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

namespace TomatoClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WindowHeight = 10;
            Console.WindowWidth = 60;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var login = new Tomato.Protocol.LoginRequest()
            {
                UserName = textBox1.Text,
                PassWord = textBox2.Text
            };
            var res = await NetClient.Instance.Request<Tomato.Protocol.LoginResponse>(login);
            if (res.Success)
            {
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
            var reg = new Tomato.Protocol.RegisterRequest()
            {
                UserName = textBox1.Text,
                Password = textBox2.Text
            };
            var res = await NetClient.Instance.Request<Tomato.Protocol.RegisterResponse>(reg);
            if (res.Success)
                label3.Text = ($"注册成功!\r\n{res.Session.ToString()}");
            else
                label3.Text = (res.Message);
            button2.Enabled = true;
        }
    }
}
