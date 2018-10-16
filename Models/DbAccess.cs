using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class DbAccess
    {
        VegaContext db = new VegaContext();

        public IEnumerable<TblModels> GetModels()
        {
            return db.TblModels.ToList();
        }

        public bool AddUser(TblUsers user)
        {
            bool UserWasAdded = false;
            try
            {
                if (UserExists(user.Email)) { UserWasAdded = false; }
                else
                {
                    user.PasswordHash = PasswordHashing.Hash(user.PasswordHash);
                    db.TblUsers.Add(user);
                    db.SaveChanges();
                    UserWasAdded = true;
                }
            }
            catch
            {
                throw;
            }
            return UserWasAdded;
        }

        public bool LoginDataCheck(LoginDetails login)
        {
            bool CorrectData = false;
            var userExists = UserExists(login.Email);

            if (userExists)
            {
                var user = db.TblUsers.Where(userData => userData.Email == login.Email).FirstOrDefault();
                if (string.Compare(PasswordHashing.Hash(login.PasswordHash), user.PasswordHash) == 0) { CorrectData = true; }
            }
            else { CorrectData = false; }

            return CorrectData;
        }

        public bool UserExists(string Email)
        {
            bool Exists = false;
            var user = db.TblUsers.Where(userDt => userDt.Email == Email).FirstOrDefault();
            if (user != null) { Exists = true; }
            return Exists;
        }

        public TblUsers GetUserById(string CurrentUserEmail)
        {
            var user = db.TblUsers.Where(u => u.Email == CurrentUserEmail).FirstOrDefault();
            if (user != null) { return user; } else { return null; }
        }

        public TblUsers SaveChangesToProfile(TblUsers user)
        {
            var UserData = db.TblUsers.SingleOrDefault(email => email.Email == user.Email);
            UserData.FirstName = user.FirstName;
            UserData.LastName = user.LastName;
            UserData.Email = user.Email;
            UserData.PasswordHash = PasswordHashing.Hash(user.PasswordHash);
            db.SaveChanges();
            return user;
        }
    }
}