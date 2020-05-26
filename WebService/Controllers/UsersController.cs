using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using UserDataAccess;

namespace WebService.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class UsersController : ApiController
    {
        [BasicAuthentication]
        [HttpGet]
        public HttpResponseMessage Get(string Username = "default")
        {
            using (UserDBEntities entities = new UserDBEntities())
            {
                switch (Username.ToLower())
                {
                    case "default":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.UserRegs.ToList());
                    //case "male":
                    //    return Request.CreateResponse(HttpStatusCode.OK, 
                    //        entities.UserRegs.Where(e => e.Username.ToLower() == Username));
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value for username must match.  " + Username + " is invalid");
                }
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            using (UserDBEntities entities = new UserDBEntities())
            {
                var entity = entities.UserRegs.FirstOrDefault(e => e.UserId == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id.ToString() + " not found.");
                }
            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] UserReg user)
        {
            try
            {
                using (UserDBEntities entities = new UserDBEntities())
                {
                    entities.UserRegs.Add(user);
                    entities.SaveChanges();


                    var message = Request.CreateResponse(HttpStatusCode.Created, user);
                    message.Headers.Location = new Uri(Request.RequestUri + user.UserId.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            using (UserDBEntities entities = new UserDBEntities())
            {
                try
                {
                    var entity = entities.UserRegs.FirstOrDefault(e => e.UserId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id.ToString() + " not found");
                    }
                    else
                    {
                        entities.UserRegs.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]int id, [FromUri]UserReg user)
        {
            using (UserDBEntities entities = new UserDBEntities())
            {
                try
                {
                    var entity = entities.UserRegs.FirstOrDefault(e => e.UserId == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id.ToString() + " not found to update.");
                    }
                    else
                    {
                        entity.Username = user.Username;
                        entity.Pwd = user.Pwd;
                        entity.ConfirmPwd = user.ConfirmPwd;
                        entity.Uemail = user.Uemail;
                        entity.Salary = user.Salary;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }


    }
}
