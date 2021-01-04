using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTranslate.Translate.Enums
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value) where T: Enum
            => (T)Enum.Parse(typeof(T), value,true);


        //public static T? ToEnum<T>(this string value) where T : Enum
        //{
        //    if (string.IsNullOrEmpty(value)) //Enum.TryParse
        //        return default(T);
        //    return (T)Enum.Parse(typeof(T), value, true);
        //}


        public static LanguageKinds Switch(this LanguageKinds kind )
        {
            return  kind == LanguageKinds.Default ? LanguageKinds.UnDefault : LanguageKinds.Default;
        }
    }
}
