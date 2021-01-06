# AppTranslate
Translate(localization) razor SPA 


<img  src="https://github.com/MhozaifaA/AppTranslate/blob/master/BlazorApp/Resources/sample.gif" >
sample with custom css suppurt ar/en   

### `` check source code to figure more extensions:`` https://github.com/MhozaifaA/AppTranslate/tree/master/AppTranslate/Translate

# Get started

## Blazor Client-Side
**Inject in Program.cs**
```C#
using AppTranslate.Translate.Configure;
//....
  builder.Services.AddAppTranslateClientSide(config=> config.Thesaurus( ("Hello" , "مرحبا") ) );              
//....  or 
   await services.AddAppTranslateClientSide("thesaurus.json");
//.... 
```

**in Component**
```C#
@inject AppTranslate.Translate.IAppTranslate Translate
@implements IDisposable

<h1 class="arFlip-1 arAlign">@Translate["Hello"]</h1>
<button class="btn btn-primary" @onclick="ChangeLanguge">@Translate["..."]</button>

@code{
 protected override void OnInitialized()
    {
        Translate.OnChange += StateHasChanged;
    }
    
    private void ChangeLanguge()
    {
        Translate.Switch();
    }
   
    public void Dispose()
    {
        Translate.OnChange -= StateHasChanged;
    }
}
   
```


## Blazor Server-Side

**Inject in Startup.cs**
```C#
using AppTranslate.Translate.Configure;
//....
    services.AddAppTranslateServerSide(config =>  config.Thesaurus(("Hello", "مرحبا")));
    //....  or 
    services.AddAppTranslateServerSide("thesaurus.json");
//.... 
```

**in Component**
```C#
@inject AppTranslate.Translate.IAppTranslate Translate
@implements IDisposable

<h1 class="arFlip-1 arAlign">@Translate["Hello"]</h1>
<button class="btn btn-primary" @onclick="ChangeLanguge">@Translate["..."]</button>

@code{
    protected override void OnInitialized()
    {
        Translate.OnChange += StateHasChanged;
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await Translate.Inject();
    }
    
    private void ChangeLanguge()
    {
        Translate.Switch();
    }
    
    public void Dispose()
    {
        Translate.OnChange -= StateHasChanged;
    }
    
   
}
   
```

# Functionality

```C# 
ChangeThesaurus(thesaurusPath,code?) 

``enabled async``
Switch(code?)
SwitchToDefault(code?)
SwitchToUnDefault(code?)
Switch(thesaurusPath,code?)

``using only in server side``
Inject(key?)

``enabled async``
OnceSupportLTR()
OnceSupportRTL()

``properties``
IsDefault
Path
Code
IsSupportRTL

``state``
Action OnChange
```
# What features in future!
- Enable multi languages in one file
- Stable async in WASM
- Support own influencing HTML head included inside AppTranslate
- Custome localization components
- Default Language code in URL app
- Storage localy files with reload

# Samples
``client side`` https://github.com/MhozaifaA/AppTranslate/tree/master/BlazorApp
<br>
``server side`` https://github.com/MhozaifaA/AppTranslate/tree/master/BlazorAppServer
