using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// 組件的一般相關資訊是透過下列
// 屬性設定來控制。變更這些屬性值，可修改
// 與組件相關聯的資訊。
[assembly: AssemblyTitle("WTFTips")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("WTFTips")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 將 ComVisible 設為 false 會導致此組件中的類型不會顯示在
// COM 元件中，如果您需要從 COM 存取此組件中的某個類型，
// 請將該類型的ComVisible 屬性設定為true。
[assembly: ComVisible(false)]

//若要開始建置本地化應用程式，請在 .csproj 檔案中設定
//<UICulture>CultureYouAreCodingWith</UICulture
//(位於 <PropertyGroup>)。例如，若您的來源檔案使用的是美式英文，
//請將 <UICulture> 設定為 en-US。然後取消註解
//下方的 NeutralResourceLanguage 屬性。更新下方行中的 "en-US"，
//讓它與專案檔案中的 UICulture 設定相符。

//[組件: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly:ThemeInfo(
    ResourceDictionaryLocation.None, //佈景主題專屬資源字典的所在位置
                             //(在頁面或應用程式資源字典中
                             // 找不到資源時適用)
    ResourceDictionaryLocation.SourceAssembly //泛型資源字典的位置
                                      //(在頁面或應用程式資源字典中
                                      // 或任何佈景主題專屬資源字典中找不到資源時適用)
)]


// 組件的版本資訊包含下列四個值:
//
//      主要版本
//      次要版本 
//      組建編號
//      修訂
//
// 您可以指定所有的值或預設組建編號和修訂編號，
// 指定的方法是使用 '*'，如下:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
