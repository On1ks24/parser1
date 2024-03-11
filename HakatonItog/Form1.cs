using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logic;
namespace HakatonItog
{
    public partial class Form1 : Form
    {
        Logic1 logic = new Logic1();
        public Form1()
        {
            InitializeComponent();
            if(logic.repository.GetAll().Count()==0)
            {
                logic.teacher();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "hakatonDataSet.Teachers". При необходимости она может быть перемещена или удалена.
            this.teachersTableAdapter.Fill(this.hakatonDataSet.Teachers);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int a = dataGridView1.CurrentRow.Index;
                label1.Text = logic.View(logic.repository.GetById(Convert.ToInt32(dataGridView1.Rows[a].Cells[0].Value)));
            }
            catch
            {
                MessageBox.Show("Вы не выбрали студента.\nВыбери студента и повторите", "Ошибка");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            foreach(var item in logic.repository.GetAll())
            {
                if((item.Name).Contains(textBox1.Text))
                {
                    dataGridView1.Rows.Add(item.Id, item.Name);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(logic.repository.GetAll().Count()>0)
            {
                foreach(var item in logic.repository.GetAll())
                {
                    dataGridView1.Rows.Add(item.Id,item.Name);
                }
            }
        }
    }
}
  
