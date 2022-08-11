using System;
using System.Data;
using System.Configuration;
using System.Web;

    /// <summary>
    /// Summary description for DescriptionAttribute
    /// </summary>
    public class DescriptionAttribute : Attribute
    {
        private String description;

        public DescriptionAttribute(String description)
        {
            this.description = description;
        }

        public String Description
        {
            get { return description; }
        }
    }

