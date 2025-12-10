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