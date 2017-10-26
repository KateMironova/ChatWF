using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatWF
{
    public partial class Panel : UserControl
    {
        ClientConnect client;
        public Panel()
        {
            InitializeComponent();
            client = new ClientConnect(this);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            client.Go();
        }
    }
}
