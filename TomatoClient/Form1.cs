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

        }
        private async  void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 500; i++)
            {
                button1.Enabled = false;
                var login = new Tomato.Protocol.LoginRequest()
                {
                    UserName = textBox1.Text,
                    PassWrod = textBox2.Text
                };


                var res = await  NetClient.Instance.Request<Tomato.Protocol.LoginResponse>(login) ;

                if (res.Success)
                {
                    label3.Text = ($"登陆次数 : {i}\r\n登陆成功!\r\n{res.Session.ToString()}");
                }
                else
                {
                    label3.Text = ($"登陆失败!!!!");
                }
                button1.Enabled = true;
            }
        
        }
    }
}
