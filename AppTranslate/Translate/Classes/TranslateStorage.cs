using AppTranslate.Translate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTranslate.Translate.Classes
{
    public record TranslateStorage(LanguageKinds Kinds=LanguageKinds.Default, string Path="" , string Code=null, bool SupportRTL=false)
    {
        public LanguageKinds Kinds { get; set; }
        public string Path { get; set; }
        public string Code { get; set; }
        public bool SupportRTL { get; set; }
    }
}
