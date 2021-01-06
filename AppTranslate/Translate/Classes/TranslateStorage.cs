using AppTranslate.Translate.Enums;
using System;

namespace AppTranslate.Translate.Classes
{
    [Serializable]
    public record TranslateStorage(LanguageKinds Kinds=LanguageKinds.Default, string Path="" , string Code=null, bool SupportRTL=false) 
    {
        public LanguageKinds Kinds { get; set; } = Kinds;
        public string Path { get; set; } = Path;
        public string Code { get; set; } = Code;
        public bool SupportRTL { get; set; } = SupportRTL;
    }
}
