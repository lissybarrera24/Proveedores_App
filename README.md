# Proveedores_App
ModelosProveedor.cs
using SQLite;

namespace ProveedoresApp.Models
{
    public class Proveedor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
    }
}

ViewModelsProveedores.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProveedoresApp.Models;
using ProveedoresApp.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace ProveedoresApp.ViewModels
{
    public partial class ProveedoresViewModel : ObservableObject
    {
        private readonly ProveedorService _service;

        [ObservableProperty]
        private string nombre;

        [ObservableProperty]
        private string telefono;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string direccion;

        [ObservableProperty]
        private string ciudad;

        [ObservableProperty]
        private Proveedor proveedorSeleccionado;

        [ObservableProperty]
        private ObservableCollection<Proveedor> proveedores;

        public ProveedoresViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "proveedores.db3");
            _service = new ProveedorService(dbPath);
            Proveedores = new ObservableCollection<Proveedor>();
            CargarProveedores();
        }

        [RelayCommand]
        private async void CargarProveedores()
        {
            var lista = await _service.ObtenerProveedoresAsync();
            Proveedores.Clear();
            foreach (var p in lista)
            {
                Proveedores.Add(p);
            }
        }

        [RelayCommand]
        private async void AgregarProveedor()
        {
            var p = new Proveedor
            {
                Nombre = Nombre,
                Telefono = Telefono,
                Email = Email,
                Direccion = Direccion,
                Ciudad = Ciudad
            };

            await _service.AgregarProveedorAsync(p);
            CargarProveedores();
            LimpiarCampos();
        }

        [RelayCommand]
        private async void ActualizarProveedor()
        {
            if (ProveedorSeleccionado != null)
            {
                ProveedorSeleccionado.Nombre = Nombre;
                ProveedorSeleccionado.Telefono = Telefono;
                ProveedorSeleccionado.Email = Email;
                ProveedorSeleccionado.Direccion = Direccion;
                ProveedorSeleccionado.Ciudad = Ciudad;

                await _service.ActualizarProveedorAsync(ProveedorSeleccionado);
                CargarProveedores();
                LimpiarCampos();
            }
        }

        [RelayCommand]
        private async void EliminarProveedor()
        {
            if (ProveedorSeleccionado != null)
            {
                await _service.EliminarProveedorAsync(ProveedorSeleccionado);
                CargarProveedores();
                LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            Nombre = string.Empty;
            Telefono = string.Empty;
            Email = string.Empty;
            Direccion = string.Empty;
            Ciudad = string.Empty;
            ProveedorSeleccionado = null;
        }
    }
}

ViewsProveedoresPage.xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:viewmodels="clr-namespace:ProveedoresApp.ViewModels"
             x:Class="ProveedoresApp.Views.ProveedoresPage"
             Title="Gestión de Proveedores">

    <ContentPage.BindingContext>
        <viewmodels:ProveedoresViewModel />
    </ContentPage.BindingContext>

    <ScrollView>
        <VerticalStackLayout Padding="20" Spacing="10">

            <Label Text="Gestión de Proveedores"
                   FontSize="24"
                   FontAttributes="Bold"
                   HorizontalOptions="Center" />

            <Entry Placeholder="Nombre" Text="{Binding Nombre}" />
            <Entry Placeholder="Teléfono" Text="{Binding Telefono}" Keyboard="Telephone" />
            <Entry Placeholder="Email" Text="{Binding Email}" Keyboard="Email" />
            <Entry Placeholder="Dirección" Text="{Binding Direccion}" />
            <Entry Placeholder="Ciudad" Text="{Binding Ciudad}" />

            <HorizontalStackLayout Spacing="10">
                <Button Text="Agregar" Command="{Binding AgregarProveedorCommand}" BackgroundColor="#28a745" TextColor="White" />
                <Button Text="Actualizar" Command="{Binding ActualizarProveedorCommand}" BackgroundColor="#ffc107" TextColor="Black" />
                <Button Text="Eliminar" Command="{Binding EliminarProveedorCommand}" BackgroundColor="#dc3545" TextColor="White" />
            </HorizontalStackLayout>

            <Label Text="Lista de Proveedores" FontAttributes="Bold" FontSize="18" />

            <CollectionView ItemsSource="{Binding Proveedores}" SelectionMode="Single" SelectedItem="{Binding ProveedorSeleccionado}">
                <CollectionView.ItemTemplate>
                    <DataTemplate>
                        <Frame BorderColor="Gray" CornerRadius="5" Padding="10" Margin="5">
                            <StackLayout>
                                <Label Text="{Binding Nombre}" FontAttributes="Bold" />
                                <Label Text="{Binding Telefono}" />
                                <Label Text="{Binding Email}" />
                                <Label Text="{Binding Direccion}" />
                                <Label Text="{Binding Ciudad}" />
                            </StackLayout>
                        </Frame>
                    </DataTemplate>
                </CollectionView.ItemTemplate>
            </CollectionView>

        </VerticalStackLayout>
    </ScrollView>
</ContentPage>

ViewsProveedoresPage.xmal.cs
namespace ProveedoresApp.Views
{
    public partial class ProveedoresPage : ContentPage
    {
        public ProveedoresPage()
        {
            InitializeComponent();
        }
    }
}

Service.cs
using SQLite;
using ProveedoresApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProveedoresApp.Services
{
    public class ProveedorService
    {
        private readonly SQLiteAsyncConnection _db;

        public ProveedorService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Proveedor>().Wait();
        }

        public Task<List<Proveedor>> ObtenerProveedoresAsync()
        {
            return _db.Table<Proveedor>().ToListAsync();
        }

        public Task<int> AgregarProveedorAsync(Proveedor proveedor)
        {
            return _db.InsertAsync(proveedor);
        }

        public Task<int> ActualizarProveedorAsync(Proveedor proveedor)
        {
            return _db.UpdateAsync(proveedor);
        }

        public Task<int> EliminarProveedorAsync(Proveedor proveedor)
        {
            return _db.DeleteAsync(proveedor);
        }
    }
}

App.xaml
namespace ProveedoresApp.Views
{
    public partial class ProveedoresPage : ContentPage
    {
        public ProveedoresPage()
        {
            InitializeComponent();
        }
    }
}


App.xaml
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
