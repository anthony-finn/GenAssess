using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;


namespace GenAssess
{
    class JSON
    {
        public static string Serialize(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }

        public static object Deserialize(string json, Type type)
        {
            return new JavaScriptSerializer().Deserialize(json, type);
        }
    }
}
