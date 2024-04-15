using System;
using System.Windows.Forms;
using System.Collections;

namespace pr14
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            listView1.Columns.Add("Ім'я", 100);
            listView1.Columns.Add("Фамілія", 100);
            listView1.Columns.Add("Номер телефону", 100);
            listView1.Columns.Add("Електронна пошта", 150);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            string surname = surnameTextBox.Text;
            string phoneNumber = phoneNumberTextBox.Text;
            string email = emailTextBox.Text;

            ListViewItem item = new ListViewItem(name);
            item.SubItems.Add(surname);
            item.SubItems.Add(phoneNumber);
            item.SubItems.Add(email);

            listView1.Items.Add(item);

            nameTextBox.Text = "";
            surnameTextBox.Text = "";
            phoneNumberTextBox.Text = "";
            emailTextBox.Text = "";

            SortListView();
        }

        private void SortListView()
        {
            listView1.ListViewItemSorter = new ListViewComparer();
            listView1.Sort();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string searchName = searchTextBox.Text;
            bool found = false;

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[0].Text == searchName)
                {
                    listView1.SelectedItems.Clear();
                    item.Selected = true;
                    listView1.Focus();
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                MessageBox.Show("Немає такого імені в списку.");
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            string deleteName = deleteTextBox.Text;
            bool found = false;

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[0].Text == deleteName)
                {
                    listView1.Items.Remove(item);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                MessageBox.Show("Немає такого імені в списку.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    public class ListViewComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return String.Compare(((ListViewItem)x).SubItems[0].Text, ((ListViewItem)y).SubItems[0].Text);
        }
    }
}
