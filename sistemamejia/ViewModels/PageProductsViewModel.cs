using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using Variedades.Business;

namespace Variedades.ViewModels
{
    class PageProductsViewModel : BindableBase
    {
        private readonly BusinessContext context;
        public PageProductsViewModel()
        {
            context = new BusinessContext();
        }
    }
}
