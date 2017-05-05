using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebAuthenticatePostCard.Model;
using WebAuthenticatePostCard.SQLiteContext;

namespace WebAuthenticatePostCard.Services
{
    //Authenticate Service access to the sqlite database and and run CRUD query
    public class AuthenticateService
    {
        public static bool Login(string email, string password, out UserModel user)
        {
                DataTable dt = WebPostCardDb.GetUser(email, password);
                if (dt.Rows.Count > 0)
                {
                    var tempUser = new UserModel();
                    // pardon my double cast, just a quick fix because Int in sqlite is long
                    tempUser.ID = (int)(long)(dt.Rows[0]["id"]);
                    tempUser.Name = (string)dt.Rows[0]["Name"]; ;
                    user = tempUser;
                    return true;
                }
                user = null;
                return false;
        }
        public static bool Register(string email, string name, string password)
        {
            try
            {
                if (!WebPostCardDb.InsertUser(email, name, password))
                {
                    return false;
                };
                return true;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}