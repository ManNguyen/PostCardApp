using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace WebAuthenticatePostCard.Controllers
{
    [TokenAuthenticateFilter]
    public class PostCardController : ApiController
    {
        public HttpResponseMessage Get()
        {
            try
            {
                Random rnd = new Random();
                int num = rnd.Next(1, 5);
                string dataFile = Path.Combine(HttpContext.Current.Server.MapPath("~") , "PostCard//" + "Postcard"+num+ ".jpg");
                string header = "application/jpeg";

                FileStream fileStream = new FileStream(dataFile, FileMode.Open);
                byte[] fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, fileBytes.Length);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileBytes)
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(header);
                fileStream.Close();
                return result;
            }

            catch (Exception ex)
            {
             throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
