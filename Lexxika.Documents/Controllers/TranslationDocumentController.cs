using Microsoft.AspNetCore.Mvc;
using Lexxika.Documents.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Text;

namespace Lexxika.Documents.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class TranslationDocumentController : ControllerBase
    {
        #region Constructor
        public TranslationDocumentController()
        {
        }
        #endregion

        #region Private Helper Routines
        /// <summary>
        /// Extract the validation errors.
        /// </summary>
        /// <returns></returns>
        private string ValidateErrors()
        {
            var errors = new StringBuilder();

            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    if (error.Exception != null)
                    {
                        errors.Append(error.Exception.Message);
                    }
                    else if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        errors.Append(error.ErrorMessage);
                    }
                }
            }

            return errors.ToString();
        }
        #endregion

        // GET: api/documents
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<TranslationDocument>> GetAll()
        {
            try
            {
                #region Get User Claims
                var userClaims = new UserClaims(User);
                if (!userClaims.IsValid)
                {
                    return Unauthorized();
                }
                #endregion

                if (InMemoryStorage.Storage == null)
                {
                    return NotFound();
                }

                if (userClaims.IsAdmin)
                {
                    // return all documents if an admin
                    return InMemoryStorage.Storage.ToList();
                }
                else
                {
                    // only return user documents if not an admin
                    var userDocuments = new List<TranslationDocument>();
                    foreach (var document in InMemoryStorage.Storage)
                    {
                        if (document.User == userClaims.UserName)
                        {
                            userDocuments.Add(document);
                        }
                    }

                    return userDocuments;
                }
            }
            catch
            {
                // log error
                return NotFound();
            }
        }

        // GET: api/documents
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<TranslationDocument> Get(string id)
        {
            try
            {
                #region Get User Claims
                var userClaims = new UserClaims(User);
                if (!userClaims.IsValid)
                {
                    return Unauthorized();
                }
                #endregion

                if (InMemoryStorage.Instance == null)
                {
                    return NotFound();
                }

                foreach (var document in InMemoryStorage.Storage)
                {
                    if (document.Id == id && ((document.User == userClaims.UserName) || userClaims.IsAdmin))
                    {
                        return document;
                    }
                }
            }
            catch
            {
                // log error
            }

            return NotFound();
        }

        // DELETE: api/documents
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<TranslationDocument> DeleteDocument(string id)
        {
            try
            {
                #region Get User Claims
                var userClaims = new UserClaims(User);
                if (!userClaims.IsValid)
                {
                    return Unauthorized();
                }
                #endregion

                if (InMemoryStorage.Instance == null)
                {
                    return NotFound();
                }

                foreach (var document in InMemoryStorage.Storage)
                {
                    if (document.Id == id && ((document.User == userClaims.UserName) || userClaims.IsAdmin))
                    {
                        if (InMemoryStorage.Storage.Remove(document))
                        {
                            return Ok();
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch
            {
                // log error
            }

            return NotFound();
        }

        // PUT: api/documents
        [HttpPut()]
        [Authorize]
        public ActionResult<TranslationDocument> PutDocument([FromBody] TranslationDocument item)
        {
            try
            {
                #region Get User Claims
                var userClaims = new UserClaims(User);
                if (!userClaims.IsValid)
                {
                    return Unauthorized();
                }
                #endregion

                #region Validate Incoming Model
                if (item == null)
                {
                    return BadRequest("No document sent");
                }

                if (!ModelState.IsValid)
                {
                    var errors = ValidateErrors();
                    if (errors.Length > 0)
                    {
                        return BadRequest(errors);
                    }
                    else 
                    { 
                        return BadRequest(); 
                    }
                }
                #endregion

                if (InMemoryStorage.Instance == null)
                {
                    return NotFound();
                }

                // search for document to update
                foreach (var document in InMemoryStorage.Storage)
                {
                    if (document.Id == item.Id && ((document.User == userClaims.UserName) || userClaims.IsAdmin))
                    {
                        document.Title = item.Title;
                        document.TranslationText = item.TranslationText;
                        document.User = item.User;
                        return document;
                    }
                }
            }
            catch
            {
                // log error
            }

            return NotFound();
        }

        // POST: api/documents
        [HttpPost]
        [Authorize]
        public ActionResult<TranslationDocument> Post([FromBody] TranslationDocument item)
        {
            try
            {
                #region Get User Claims
                var userClaims = new UserClaims(User);
                if (!userClaims.IsValid)
                {
                    return Unauthorized();
                }
                #endregion

                // update document with valid GUID
                item.Id = Guid.NewGuid().ToString();

                // add to storage
                InMemoryStorage.Storage.Add(item);
                return item;
            }
            catch
            {
                // log error
            }

            return BadRequest();
        }
    }
}

