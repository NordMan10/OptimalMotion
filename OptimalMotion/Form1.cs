using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OptimalMoving.Domain;

namespace OptimalMoving
{
    public class TableString
    {
        public int Id { get; set; }
        public int Time { get; set; }

        public TableString(int id, int time)
        {
            Id = id;
            Time = time;
        }

    }

    public partial class Form1 : Form
    {
        private DataGridView dataGridView1 = new DataGridView();
        public Form1()
        {
            InitializeComponent();

            dataGridView1.Dock = DockStyle.Fill;

            Controls.Add(dataGridView1);

            var data = new BindingList<TableString>(); 
            data.Add(new TableString(1, 10));
            data.Add(new TableString(2, 15));
            data.Add(new TableString(3, 20));

            dataGridView1.DataSource = data;

            data.Add(new TableString(4, 20));
            data.Insert(0, new TableString(0, 0));

            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space) 
                Close();
        }
    }
}
