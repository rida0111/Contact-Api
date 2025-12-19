using System.Data;
using Microsoft.Data.SqlClient;

namespace ContactApi_DataAccessLayer
{
    public class Contactdto
    {
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public DateTime DateofBirth { get; set; }


        public Contactdto(int contactID, string firstName, string lastName, string email, string phone, string address, DateTime dateofBirth)
        {
            ContactID = contactID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
            DateofBirth = dateofBirth;

        }

    }

    public class ContactData
    {


        public static int AddNewContact(Contactdto contact)
        {

            int newContactID = -1;

            try
            {
                string connectionString = Connection.connectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_AddNewContact", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                        command.Parameters.AddWithValue("@LastName", contact.LastName);

                        command.Parameters.AddWithValue("@Email", contact.Email);
                        command.Parameters.AddWithValue("@Phone", contact.Phone);

                        command.Parameters.AddWithValue("@Address", contact.Address);
                        command.Parameters.AddWithValue("@DateofBirth", contact.DateofBirth);

                        SqlParameter outputIdParam = new SqlParameter("@NewContactID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        newContactID = (int)command.Parameters["@NewContactID"].Value;

                    }
                    ;

                }
                ;

            }
            catch
            {
                newContactID = -1;
            }

            return newContactID;

        }

        public static bool Update(Contactdto contact)
        {
            bool IsEffected = false;


            try
            {
                string connectionString = Connection.connectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))

                using (SqlCommand command = new SqlCommand("SP_UpdateContact", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ContactID", contact.ContactID);
                    command.Parameters.AddWithValue("@FirstName", contact.FirstName);

                    command.Parameters.AddWithValue("@LastName", contact.LastName);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@Phone", contact.Phone);

                    command.Parameters.AddWithValue("@Address", contact.Address);
                    command.Parameters.AddWithValue("@DateofBirth", contact.DateofBirth);

                    connection.Open();

                    command.ExecuteNonQuery();

                    IsEffected = (int)command.Parameters["@IsEffected"].Value == 1;

                }


            }
            catch
            {
                IsEffected = false;
            }

            return IsEffected;

        }

        public static bool GetContactbyID(Contactdto contact)
        {

            try
            {
                string connectionString = Connection.connectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("SP_GetContactbyID", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ContactID", contact.ContactID);

                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                contact.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                contact.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                                contact.Address = reader.GetString(reader.GetOrdinal("Address"));
                                contact.Email = reader.GetString(reader.GetOrdinal("Email"));
                                contact.Phone = reader.GetString(reader.GetOrdinal("Phone"));
                                contact.DateofBirth = reader.GetDateTime(reader.GetOrdinal("DateofBirth"));

                                return true;
                            }
                            else
                                return false;
                        }


                    }
                    ;

                }
                ;

            }
            catch
            {
                return false;
            }


        }

        public static bool DeleteContact(int ContactID)
        {
            bool IsDeleted = false;

            try
            {
                string connectionString = Connection.connectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))

                using (SqlCommand cmd = new SqlCommand("SP_DeleteContact", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ContactID", (object)ContactID ?? DBNull.Value);

                    connection.Open();

                    var result = cmd.ExecuteScalar();

                    if (result != null)
                        IsDeleted = (int)result == 1;
                    else
                        IsDeleted = false;
                }


            }
            catch
            {
                IsDeleted = false;
            }

            return IsDeleted;
        }

        public static List<Contactdto> GetAllContacts()
        {
            List<Contactdto> ContactList = new List<Contactdto>();

            try
            {
                string connectionString = Connection.connectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("SP_GetAllContacts", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                ContactList.Add(new Contactdto(
                                reader.GetInt32(reader.GetOrdinal("ContactID")),
                                reader.GetString(reader.GetOrdinal("FirstName")),
                                reader.GetString(reader.GetOrdinal("LastName")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Phone")),
                                reader.GetString(reader.GetOrdinal("Address")),
                                reader.GetDateTime(reader.GetOrdinal("DateOfBirth"))
                                ));
                            }
                        }


                    }
                    ;
                }
                ;

            }
            catch
            {
                ContactList = null;
            }

            return ContactList;

        }

        public static bool IsContactExist(int ContactID)
        {
            bool IsExist = false;

            try
            {
                string connectionString = Connection.connectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))

                using (SqlCommand cmd = new SqlCommand("SP_IsContactExist", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ContactID", (object)ContactID ?? DBNull.Value);

                    connection.Open();
                    SqlParameter returnParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    cmd.Parameters.Add(returnParameter);

                    cmd.ExecuteNonQuery();

                    IsExist = (int)returnParameter.Value == 1;
                }


            }
            catch
            {
                IsExist = false;
            }

            return IsExist;
        }


    
    }
}
