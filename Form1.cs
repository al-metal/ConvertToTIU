using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using web;

namespace ConvertToTIU
{
    public partial class Form1 : Form
    {
        WebRequest webRequest = new WebRequest();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string otv = null;
            otv = webRequest.PostRequest("https://my.tiu.ru/cabinet/sign-in");
        }
    }
}
