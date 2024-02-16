using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamplePhoneBook
{
    public class ContactManager
    {
        private List<Contact> contacts;
        public ContactManager()
        {
            contacts = new List<Contact>();
        }

        public void AddContact(Contact contact)
        {
            contacts.Sort((x, y) => x.Id.CompareTo(y.Id));
            contact.Id = contacts.Count() == 0 ? 1 : contacts.Last().Id + 1;
            contacts.Add(contact);
        }
        public bool EditContact(int id, Contact contact)
        {
            Contact old = contacts.Find(x => x.Id == id);
            try
            {
                contacts.Remove(contact);
                contacts.Add(contact);
                return true;
            }
            catch { return false; }
        }

        public bool RemoveContact(Contact contact)
        {
            try
            {
                contacts.Remove(contact);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Contact GetContact(int id)
        {
            return contacts.Find(x => x.Id == id);
        }

        public List<Contact> GetContacts()
        {
            return contacts;
        }
        public List<Contact> GetContacts(string value)
        {
            value = value.ToLower();
            return contacts.FindAll(x => x.Name.ToLower().Contains(value)
            || x.PhoneNumber.Contains(value)
            || x.Email.Contains(value)
            || x.Group.ToLower().Contains(value));
        }
    }

}
