using System;

namespace DB
{
    public class Form_Type
    {

        public const String _Name = "Form_Type";
       
 
        private Form_Type() { }
       
 
        public static string Name { get { return "Form_Type"; } }
        public static string ID { get { return "ID"; } }
        public Int32 _ID
        {
            get;
            set;
        }
        public static string OBJ_Name { get { return "Name"; } }
        public String _OBJ_Name
        {
            get;
            set;
        }
    }

}

