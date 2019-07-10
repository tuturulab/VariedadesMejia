using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Variedades.Business;
using Variedades.Models;

namespace Variedades.ViewModels
{
    class LoginViewModel
    {
        private readonly BusinessContext context;

        private ObservableCollection<Proveedor> _ProveedorCollection;
        public ObservableCollection<Proveedor> ProveedorCollection
        {
            get { return _ProveedorCollection; }
            set { _ProveedorCollection = value; }
        }
        public LoginViewModel()
        {
            context = new BusinessContext();

            ProveedorCollection = new ObservableCollection<Proveedor>(context.GetAllProveedors());
        }
    }
}
