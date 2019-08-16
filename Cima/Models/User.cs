using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Models
{
    public class User
    {

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string salt;
        public string Salt
        {
            get { return salt; }
            set { salt = value; }
        }

        private string profils;
        public string Profils
        {
            get { return profils; }
            set { profils = value; }
        }

        private string company;
        public string Company
        {
            get { return company; }
            set { company = value; }
        }
    }
}