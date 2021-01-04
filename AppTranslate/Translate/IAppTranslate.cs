using AppTranslate.Translate.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppTranslate.Translate
{
    public interface IAppTranslate
    {
        /// <summary>
        /// return value if key exist or same value 
        /// <para> if <see cref="Translate"/> empty or not loaded yet return three dots "..." </para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string this[string index] { get; }

        /// <summary>
        /// get if language default as "key" side
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// useful when using condition to swap bettwen LTR and RTL 
        /// </summary>
        bool IsSupportRTL { get; }

        /// <summary>
        /// LanguageCode optional and not checking
        /// can use as custom check for your app
        /// </summary>
        string Code { get;}

        /// <summary>
        /// get Thesaurus file path 
        /// </summary>
        string Path { get; }

        /// <summary>
        /// read only list of Thesaurus  contain key: as left side as default and value: as right side as undefault
        /// </summary>
        IReadOnlyDictionary<string, string> Translate { get; set; }

        /// <summary>
        /// notify event state with invoked by 
        /// <para> <see cref="Switch(string, bool)"/> <see cref="SwitchAsync(string, bool)"/>  and <see cref="Inject(string)"/> <see cref="InjectAsync(string)"/>  </para>
        /// </summary>
        event Action OnChange;

        /// <summary>
        /// change Thesaurus file
        /// <para> calling switch to undefault </para>
        /// </summary>
        /// <param name="thesaurusPath"></param>
        /// <param name="code"> this optional pass to switch </param>
        /// <returns></returns>
        ValueTask ChangeThesaurus(string thesaurusPath, string code = null);

        /// <summary>
        /// worked with server sie to injected afterRender
        /// </summary>
        /// <param name="key"></param>
        void Inject(string key = null);

        /// <summary>
        /// worked with server sie to injected afterRenderAsync
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ValueTask InjectAsync(string key = null);

        /// <summary>
        /// support rtl bt flip <see cref="IsSupportRTL"/> to <see langword="false"/>
        /// </summary>
        void OnceSupportLTR();

        /// <summary>
        /// support rtl bt flip <see cref="IsSupportRTL"/> to <see langword="true"/>
        /// </summary>
        void OnceSupportRTL();

        /// <summary>
        /// Async support rtl bt flip <see cref="IsSupportRTL"/> to <see langword="false"/>
        /// </summary>
        ValueTask OnceSupportLTRAsync();

        /// <summary>
        /// Async support rtl bt flip <see cref="IsSupportRTL"/> to <see langword="true"/>
        /// </summary>
        ValueTask OnceSupportRTLAsync();

        /// <summary>
        /// switch side  as flip bettwen left side and right side in Thesaurus file
        /// </summary>
        /// <param name="code"> is optional to help checking in app </param>
        /// <returns></returns>
        LanguageKinds Switch(string code = null);


        /// <summary>
        /// switch to left side 
        /// </summary>
        /// <param name="code"> is optional to help checking in app </param>
        /// <returns></returns>
        LanguageKinds SwitchToDefault(string code = null);


        /// <summary>
        /// switch to roght side 
        /// </summary>
        /// <param name="code"> is optional to help checking in app </param>
        /// <returns></returns>
        LanguageKinds SwitchToUnDefault(string code = null);


        /// <summary>
        /// Async switch side  as flip bettwen legt side and right side in Thesaurus file 
        /// </summary>
        /// <param name="code"> is optional to help checking in app </param>
        /// <returns></returns>
        Task<LanguageKinds> SwitchAsync(string code = null);


        /// <summary>
        /// Async switch to left side 
        /// </summary>
        /// <param name="code"> is optional to help checking in app </param>
        /// <returns></returns>
        Task<LanguageKinds> SwitchToDefaultAsync(string code = null);

        /// <summary>
        /// Async switch to right side 
        /// </summary>
        /// <param name="code"> is optional to help checking in app </param>
        /// <returns></returns>
        Task<LanguageKinds> SwitchToUnDefaultAsync(string code = null);


        /// <summary>
        /// witch with new path file else normal flip <see cref="Switch(string)"/> if downlaod flip to undefault side
        /// </summary>
        /// <param name="thesaurusPath"> will not force redownload if exist  </param>
        /// <param name="code"></param>
        /// <returns> optional async/sync </returns>
        ValueTask<LanguageKinds> Switch(string thesaurusPath, string code = null);
    }
}