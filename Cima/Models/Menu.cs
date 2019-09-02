using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cima.Models
{
    public class Menu
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        private string controller;
        public string Controller
        {
            get { return controller; }
            set
            {
                controller = value;
            }
        }

        private string action;
        public string Action
        {
            get { return action; }
            set
            {
                action = value;
            }
        }

        private string icon;
        public string Icon
        {
            get { return icon; }
            set
            {
                icon = value;
            }
        }

        private string paramUrl;
        public string ParamUrl
        {
            get { return paramUrl; }
            set
            {
                paramUrl = value;
            }
        }

        private List<Menu> menuItems;
        public List<Menu> MenuItems
        {
            get { return menuItems; }
            set
            {
                menuItems = value;
            }
        }
        
    }
}