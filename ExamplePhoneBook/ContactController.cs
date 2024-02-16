using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace ExamplePhoneBook
{
    internal class ContactController
    {
        private ContactManager contactManager;
        private Form1 view; // Замените Form1 на название вашей формы

        public ContactController(Form1 view)
        {
            this.view = view;
            contactManager = new ContactManager();
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            view.Load += View_Load;
            view.bAdd.Click += Btn_Click;
            view.DataGrid.CellClick += dataGrid_CellClick;
            // Другие события элементов управления
        }

        private void View_Load(object sender, EventArgs e)
        {
            // При загрузке формы загрузите контакты из модели и отобразите их в DataGridView
            view.DataGrid.DataSource = contactManager.GetContacts();
        }

        public void Btn_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Text.ToLower())
            {
                case "add":
                    // При нажатии кнопки "Добавить контакт" получите данные из TextBox
                    // и добавьте новый контакт в модель
                    add();
                    break;
                case "edit":
                    break;
                case "delete":
                    break;
                case "cancel":
                    clearTextBoxes();
                    break;
                case "find":
                    find(view.vFind.Text);
                    break;
            }

        }

        Contact newContact;

        bool checkValues()
        {
            if (view.vName.Text != "")
            {
                newContact = new Contact
                {
                    Name = view.vName.Text,
                    Email = view.vEmail.Text,
                    PhoneNumber = view.vPhone.Text,
                    Group = view.vGroup.Text,
                };
                return true;
            }
            else { return false; }
        }
        // Другие методы для обработки событий и взаимодействия с моделью
        private void add()
        {
            if (checkValues())
            {
                contactManager.AddContact(newContact);
                //view.DataGrid.DataSource = contactManager.GetContacts();
                // view.DataGrid.Refresh();// Обновите список контактов на форме
                refreshDataGrid(contactManager.GetContacts());
            }

        }

        void clearTextBoxes()
        {
            view.vGroupBox1.Controls.OfType<Button>().ToList().ForEach(x => x.Text = "");
            view.vGroup.SelectedIndex = 0;
        }

        internal void find(string text)
        {
            List<Contact> res = contactManager.GetContacts(text);
            refreshDataGrid(res);
        }
        void refreshDataGrid(List<Contact> source)
        {
            view.DataGrid.DataSource = source;
            var m = view.DataGrid.GetType().GetMethod("OnDataSourceChanged",
                BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(view.DataGrid, new object[] { EventArgs.Empty });
        }
        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(view.DataGrid.CurrentRow.Cells[0].Value.ToString());
            Contact contact = contactManager.GetContact(id);
            fillTextBoxes(contact);
        }
        void fillTextBoxes(Contact contact)
        {
            if (!string.IsNullOrEmpty(contact.Name))
            {
                view.vName.Text = contact.Name;
                view.vEmail.Text = contact.Email;
                view.vPhone.Text = contact.PhoneNumber;
                view.vGroup.Text = contact.Group;
            }
        }
    }

}
