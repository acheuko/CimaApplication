using Cima.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.ViewModel
{
    public class UserViewModel
    {
        private LoginModel loginModel;
        public LoginModel LoginModel
        {
            get { return loginModel; }
            set { loginModel = value; }
        }

        private RegisterModel registerModel;
        public RegisterModel RegisterModel
        {
            get { return registerModel; }
            set { registerModel = value; }
        }
    }
}