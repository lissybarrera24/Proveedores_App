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