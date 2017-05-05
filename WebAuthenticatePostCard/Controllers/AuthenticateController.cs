using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebAuthenticatePostCard.Model;
using WebAuthenticatePostCard.Services;

namespace WebAuthenticatePostCard.Controllers
{
    public class AuthenticateController : ApiController
    {
        const int MAX_INVALID_LOGIN_COUNT = 9;

        [HttpGet]
        public HttpResponseMessage Login()
        {
            string email = "";
            try
            {
                //user email and password are stored in Authrization header seperated by "|" and based 64 encoded
                var encodedData = Request.Headers.Authorization.Parameter;
                byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
                string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
                var para = returnValue.Split('|');
                email = para[0];
                string password = para[1];

                var user = new UserModel();
                ////if user is valid then create token 
                if (AuthenticateService.Login(email, password, out user))
                {
                    string mintToken = TokenService.CreateToken(user);
                    var result = new HttpResponseMessage(HttpStatusCode.OK) { };
                    result.Headers.Add("AuthenticateToken", mintToken);
                    result.Content = new StringContent(user.Name);
                    return result;
                }
                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.NotAcceptable;
                //response.Content = new StringContent(message);
                return response;

            }

            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public HttpResponseMessage Register(string email, string name, string password)
        {
            try
            {
                if (AuthenticateService.Register(email, name, password))
                {
                    var result = new HttpResponseMessage(HttpStatusCode.OK) { };
                    result.Content = new StringContent("Register Success");
                    return result;
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                    response.Content = new StringContent("Cannot register with this email");
                    return response;
                };
            
               
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

       
    }
}
