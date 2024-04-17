using System;
using System.IO;
using System.Windows.Forms;

namespace ПР15
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            listView1.Columns.Add("Назва книги", 200);
            listView1.Columns.Add("Автор", 200);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string bookName = bookNameTextBox.Text;
            string bookAuthor = bookAuthorTextBox.Text;

            ListViewItem item = new ListViewItem(new string[] { bookName, bookAuthor });
            listView1.Items.Add(item);

            using (StreamWriter writer = new StreamWriter("books.txt", true))
            {
                writer.WriteLine($"{bookName},{bookAuthor}");
            }

            bookNameTextBox.Text = "";
            bookAuthorTextBox.Text = "";
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            using (StreamReader reader = new StreamReader("books.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    ListViewItem item = new ListViewItem(new string[] { parts[0], parts[1] });
                    listView1.Items.Add(item);
                }
            }
        }
    }
}
