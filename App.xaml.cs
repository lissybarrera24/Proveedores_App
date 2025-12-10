using ProveedoresApp.Views;

namespace ProveedoresApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new ProveedoresPage();
    }
}