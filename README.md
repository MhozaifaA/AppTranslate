# AppTranslate
Translate(localization) razor SPA 


<img  src="https://github.com/MhozaifaA/AppTranslate/blob/master/BlazorApp/Resources/sample.gif" >
sample with custom css suppurt ar/en   

### `` check source code to figure more extensions:`` https://github.com/MhozaifaA/AppTranslate/tree/master/AppTranslate/Translate

# How to use

## Blazor Client-Side
**Inject in Program.cs**
```C#
using AppTranslate.Translate.Configure;
//....
  builder.Services.AddAppTranslateClientSide(config=> config.Thesaurus( ("Hello" , "مرحبا") ) );              
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
//....   or 
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
    
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
            Translate.InjectAsync();
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

