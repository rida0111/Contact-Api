
using ContactApi_DataAccessLayer;

namespace ContactApi_BusinessLayer
{
    public class Contact
    {
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public DateTime DateofBirth { get; set; }

        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode;


        public Contactdto contdto
        {
            get
            {
                return new Contactdto(this.ContactID, this.FirstName, this.LastName,
             this.Email, this.Phone, this.Address, this.DateofBirth);
            }
        }


        public Contact(Contactdto contact, enMode mode = enMode.AddNew)
        {
            Mode = mode;
            ContactID = contact.ContactID;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Email = contact.Email;
            Phone = contact.Phone;
            Address = contact.Address;
            DateofBirth = contact.DateofBirth;
        }

        public static Contact GetContactbyid(int ID)
        {
            string firstname = "", lastname = "", email = "", phone = "", address = "";

            DateTime dateofbirth = DateTime.Now;

            Contactdto contactinfo = new Contactdto(ID, firstname, lastname, email, phone, address, dateofbirth);


            bool isfound = ContactData.GetContactbyID(contactinfo);

            if (isfound)
                return new Contact(contactinfo, enMode.Update);

            return null;
        }

        public static List<Contactdto> GetAllContacts()
        {
            return ContactData.GetAllContacts();
        }

        public static bool DeleteContact(int ContactID)
        {
            return ContactData.DeleteContact(ContactID);
        }

        private bool _AddNewContact()
        {
            this.ContactID = ContactData.AddNewContact(contdto);

            return this.ContactID != -1;
        }

        private bool _UpdateContInfo()
        {
            return ContactData.Update(contdto);
        }

        public bool Save()
        {
            switch (Mode)
            {

                case enMode.AddNew:
                    {
                        if (_AddNewContact())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        return false;
                    }
                case enMode.Update:
                    return _UpdateContInfo();
                default:
                    return false;

            }

        }

        public static bool IsContactExist(int ID)
        {
            return ContactData.IsContactExist(ID);
        }
    }
}
