using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTranslate.Translate.Option
{
    public class AppTranslateOptions
    {
        public Dictionary<string, string> Translate { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public void Thesaurus(params (string lang1 , string lang2)[] lang)
        {
            foreach (var item in lang)
                Translate.Add(item.lang1, item.lang2);
        }
    }
}
