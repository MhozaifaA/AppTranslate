using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppTranslate.Translate
{
    public interface IAppTranslate
    {



        public string ThesaurusPath { get; set; }
        string this[string index] { get; }

        IReadOnlyDictionary<string, string> Translate { get; }
        ValueTask UpdateThesaurus(string thesaurusPath);


        void Inject();
        void Inject(string @default);
        ValueTask InjectAsync();

        void Refresh();
        string Switch();
        string Switch(string language);

        Task RefreshAsync();
        Task<string> SwitchAsync();
        Task<string> SwitchAsync(string language);


        bool IsDefault();


        event Action OnChange;
    }
}