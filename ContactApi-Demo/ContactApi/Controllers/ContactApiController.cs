using ContactApi_BusinessLayer;
using ContactApi_DataAccessLayer;
using Microsoft.AspNetCore.Mvc;

namespace ContactApi.Controllers
{
    [Route("api/Contact")]
    [ApiController]
    public class ContactApiController : ControllerBase
    {
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Contactdto>> GetAllContacts()
        {
            List<Contactdto> contactList = Contact.GetAllContacts();

            return Ok(contactList);
        }


        [HttpGet("{ID}", Name = "GetByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Contactdto> GetContactByID(int ID)
        {
            if (ID <= -1)
                return BadRequest($"Bad request {ID}");

            Contact contact = Contact.GetContactbyid(ID);

            if (contact == null)
                return NotFound($"No contact with id: {ID}!");

            return Ok(contact.contdto);
        }


        [HttpPost("AddNewContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Contactdto> AddNewContact(Contactdto NewContact)
        {
            if (NewContact == null)
                return BadRequest("Bad request !");

            Contact cont = new Contact(new Contactdto(NewContact.ContactID, NewContact.FirstName, NewContact.LastName,
                NewContact.Email, NewContact.Phone, NewContact.Address, NewContact.DateofBirth));

            cont.Save();

            return CreatedAtRoute("GetByID", new { ID = cont.ContactID }, cont.contdto);
        }


        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Contactdto> UpdateContact(int ID, Contactdto Updatecontact)
        {
            if (Updatecontact == null || ID <= -1)
                return BadRequest("Bad request !");

            Contact contactinfo = Contact.GetContactbyid(ID);

            if (contactinfo == null)
                return NotFound($"No contact with id: {ID}!");

            contactinfo.FirstName = Updatecontact.FirstName;
            contactinfo.LastName = Updatecontact.LastName;
            contactinfo.Email = Updatecontact.Email;
            contactinfo.Phone = Updatecontact.Phone;
            contactinfo.Address = Updatecontact.Address;
            contactinfo.DateofBirth = Updatecontact.DateofBirth;

            contactinfo.Save();

            return CreatedAtRoute("GetByID", new { ID = contactinfo.ContactID }, contactinfo.contdto);
        }

        [HttpDelete("DeleteContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult DeleteContact(int ContactID)
        {
            if (ContactID <= -1)
                return BadRequest($"Bad request {ContactID}");

            if (!Contact.IsContactExist(ContactID))
                return NotFound($"No contact with id: {ContactID}!");

            if (!Contact.DeleteContact(ContactID))
                return Conflict("Cannot delete this record because it is referenced by other data.");

            return Ok($"Contact with id ={ContactID} was deleted Successfully !");
        }


    }
}
