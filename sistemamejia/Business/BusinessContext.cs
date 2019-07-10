using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Variedades.Models;

namespace Variedades.Business
{
    class BusinessContext : IDisposable
    {
        private readonly DbmejiaEntities context;
        private bool disposed;

        public BusinessContext()
        {
            context = new DbmejiaEntities();
        }

        #region User Members

        /// <summary>
        /// Check if user exists
        /// </summary>
        /// <param name="Username">The username to check</param>
        /// <param name="password">The password to check</param>
        /// <returns></returns>
        public async Task<User> GetUserGivenPasswordAndUsername(string Username, string Password)
        {
            var user = await context.Users.FirstOrDefaultAsync(t => t.Nombre.Equals(Username) && t.Password.Equals(Password));

            /*
            if (user == null)
            {
                return null;
            }

            if (user.Password.Equals(Password) == false)
            {
                return null;
            }*/

            return user ?? null;
        }

        #endregion

        #region Product Members

        public List<Producto> GetAllProducts()
        {
            return context.Producto.ToList();
        }

        public Producto AddNewProduct(Producto producto)
        {
            var prod = context.Producto.Add(producto);
            context.SaveChanges();

            return prod;
        }

        #endregion

        #region Proveedor members

        public List<Proveedor> GetAllProveedors()
        {
            return context.Proveedor.ToList();
        }

        public Proveedor AddNewProveedor(Proveedor proveedor)
        {
            var prov = context.Proveedor.Add(proveedor);
            context.SaveChanges();

            return prov;
        }

        #endregion


        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed || !disposing)
                return;

            if (context != null)
                context.Dispose();

            disposed = true;
        }

        #endregion
    }
}
