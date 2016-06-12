using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCode
{
    public partial class FormMain : Form 
    {
        Dictionary<string, Type> dict;
        public FormMain(Dictionary<string, Type> dict)
        {
            InitializeComponent();
            this.dict = dict;
        }
        public class MyClass
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            List<MyClass> list = new List<MyClass>();
            foreach (var item in dict)
            {
                list.Add(new MyClass() { Name = item.Key, Description = item.Value.ToString() });
            }
            this.dataGridView1.DataSource = list;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                var type = (MyClass)item.DataBoundItem;
                Program.Generate(dict[type.Name]);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var query = dict.AsEnumerable();

            var text1 = this.textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(text1))
            {
                query = query.Where(m => { return m.Key.StartsWith(text1, StringComparison.CurrentCultureIgnoreCase); });
            }
            var text2 = this.textBox2.Text.Trim();
            if (!string.IsNullOrEmpty(text2))
            {
                query = query.Where(m => { return m.Key.EndsWith(text2, StringComparison.CurrentCultureIgnoreCase); });
            }
            var text3 = this.textBox3.Text.Trim();
            if (!string.IsNullOrEmpty(text3))
            {
                query = query.Where(m => { return m.Key.IndexOf(text3, StringComparison.CurrentCultureIgnoreCase) > 0; });
            }
            var text4 = this.textBox4.Text.Trim();
            if (!string.IsNullOrEmpty(text4))
            {
                query = query.Where(m => { return m.Key.IndexOf(text4, StringComparison.CurrentCultureIgnoreCase) > 0; });
            }
            List<MyClass> list = new List<MyClass>();
            foreach (var item in query.ToList())
            {
                list.Add(new MyClass() { Name = item.Key, Description = item.Value.ToString() });
            }
            this.dataGridView1.DataSource = list;
        }
    }
}