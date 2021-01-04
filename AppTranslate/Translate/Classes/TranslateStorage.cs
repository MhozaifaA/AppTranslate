using AppTranslate.Translate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTranslate.Translate.Classes
{
    public class TranslateStorage
    {
        public LanguageKinds Kinds { get; set; }
        public string Code{ get; set; }
        public string Path { get; set; }
    }
}
