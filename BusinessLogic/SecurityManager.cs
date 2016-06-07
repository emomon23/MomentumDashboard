using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Test.IEmosoft.com.ORMModel;
using Test.IEmosoft.com.ORMModel.DTOs;

namespace Test.IEmosoft.com.BusinessLogic
{
    public class SecurityManager
    {
        TestDBContext dbContext = new TestDBContext();
        public void RegisterClientUser(UserDTO userDTO)
        {
            var client = dbContext.Clients.FirstOrDefault(c => c.ClientGuid == userDTO.ClientId);

            User user = new User()
            {
                ClientId = client.ClientId,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Password = AES.Encrypt(userDTO.Password),
                UserName = userDTO.UserName,
                MustChangePwd = true
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public void RegisterAdminUser(AuthenticationDTO authDTO)
        {
            var adminUser = dbContext.Admins.FirstOrDefault(u => u.UserName == authDTO.UserName);

            if (adminUser == null)
            {
                adminUser = new Administrator()
                {
                    UserName = authDTO.UserName,
                    
                };
                dbContext.Admins.Add(adminUser);
            }

            adminUser.Password = AES.Encrypt(authDTO.Password);
            dbContext.SaveChanges();
        }

        public bool UserExists(Guid userId)
        {
            bool result = dbContext.Users.FirstOrDefault(u => u.UserGuid == userId) != null;
            if (!result)
            {
                result = dbContext.Admins.FirstOrDefault(a => a.AdministratorId == userId) != null; 
            }

            return result;
        }

        public void ChangePassword(Guid userId, string newPassword)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserGuid == userId);

            if (user != null)
            {
                user.Password = AES.Encrypt(newPassword);
                user.MustChangePwd = false;
                dbContext.SaveChanges();
            }
        }

        public AuthenticationResult Authenticate(AuthenticationDTO user)
        {
            string password = AES.Encrypt(user.Password);
            AuthenticationResult result = new AuthenticationResult() { AuthentiationStatus = AuthenticationResult.AuthenticationResponseEnumeration.InvalidCredentials };


            var clientUser = (from u in dbContext.Users
                             join c in dbContext.Clients
                                on u.ClientId equals c.ClientId
                             where u.UserName == user.UserName
                             select new { u.UserName, u.UserGuid, u.Password, c.ClientGuid, u.MustChangePwd }).FirstOrDefault();

            if (clientUser != null)
            {
                if (clientUser.Password == password)
                {
                    result.AuthentiationStatus = AuthenticationResult.AuthenticationResponseEnumeration.IsClientUser;
                    result.UserId = clientUser.UserGuid;
                    result.ClientId = clientUser.ClientGuid;
                    result.MustChangePassword = clientUser.MustChangePwd;
                }
            }
            else
            {
                var adminUser = dbContext.Admins.FirstOrDefault(a => a.UserName == user.UserName);
                if (adminUser != null)
                {
                    if (adminUser.Password == password)
                    {
                        result.AuthentiationStatus = AuthenticationResult.AuthenticationResponseEnumeration.IsAdminUser;
                        result.UserId = adminUser.AdministratorId;
                    }
                }
                else
                {
                    if (dbContext.Admins.Count() == 0)
                    {
                        this.RegisterAdminUser(new AuthenticationDTO() { UserName = "memo", Password = "P@ssword23!" });
                        return this.Authenticate(user);
                    }
                }
            }

            return result;
        }
    }
}