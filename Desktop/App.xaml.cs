using LiveChartsCore.SkiaSharpView;
using System.Configuration;
using System.Data;
using System.Windows;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace FinanceControlDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(
                        System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag)));
            LiveChartsCore.LiveCharts.Configure(config => config.AddDarkTheme());
        }


    }

}
