using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warcaby.View
{
    public partial class FormLevelChoice : Form
    {
        public int minMaxLevel;
        public bool shouldFightOption;
        public FormLevelChoice()
        {
            InitializeComponent();
            minMaxLevel = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            minMaxLevel = 1;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            minMaxLevel = 2;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            minMaxLevel = 3;
            Close();
        }

        private void checkBoxShouldFight_CheckedChanged(object sender, EventArgs e)
        {
            shouldFightOption = checkBoxShouldFight.Checked;
        }


    }
}
