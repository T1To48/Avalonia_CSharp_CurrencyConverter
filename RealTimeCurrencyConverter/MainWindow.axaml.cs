using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
namespace RealTimeCurrencyConverter;

using RealTimeRate.Services;
using System.Threading.Tasks;

using System;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        OpenOnSecondDisplayMonitor();
    }
    //function that always opens the App-Window on Popup on the Secondary Display
    //because primary display is used for Coding
    private void OpenOnSecondDisplayMonitor()
    {
        var screens = Screens.All;
        if (screens.Count > 1)
        {
            var secondDisplay = screens[1];
            Position = new PixelPoint(secondDisplay.WorkingArea.X, secondDisplay.WorkingArea.Y);
        }
    }

    private void Input_OnChange_FilterNumbers(object? sender, TextChangedEventArgs e)
    {
        var txtBox = sender as TextBox;

        if (txtBox?.Text?.Length > 0 && txtBox.Text is not null)
        {
            string input = txtBox.Text;
            //Proper way to handle unexpected errors
            //try to parse value to double if failed means non number char was typed 
            //then we delete it with range operator 
            if (!double.TryParse(input, out _))
            {
                txtBox.Text = input[..^1];//remove the ":" from the unit string value using RANGE OPERATOR 
            }
        }

    }

    private void ToggleExchangeRate(object? sender, RoutedEventArgs e)
    {
        var chkBx = sender as CheckBox;
        UserRate.Text = "0";
        UserRate.IsEnabled = chkBx?.IsChecked != true;
    }

    private async Task<double> GetRate()
    {
        RealtimeRate service = new();
        // Console.WriteLine(RateChkBx.IsChecked);
        if (RateChkBx.IsChecked == true)
        {
            return await service.GetRealTimeRate();
        }

        return double.Parse(UserRate?.Text);
    }
    private async void OnClick_convertToILS(object? sender, RoutedEventArgs e)
    {
        double exchangeRate = await GetRate();

        if (USDValue is not null && USDValue.Text?.Length > 0)
        {
            double USDVal = double.Parse(USDValue.Text);
            ILSValue.Text = Math.Round(USDVal * exchangeRate, 2) + "₪";
        }
    }

}