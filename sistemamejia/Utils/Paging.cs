using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variedades.Utils
{
    class Paging
    {
        public int PageIndex { get; set; }

        public IList<Models.Producto> Productos { get; set; }
        public IList<Models.Cliente> Clientes { get; set; }
        public IList<Models.Venta> Ventas { get; set; }
        public IList<Models.Pedido> Pedidos { get; set; }
        public IList<Models.DetalleProveedor> Importaciones { get; set; }

        public void SomeMethod<T>(IList<T> List, int RecordsPerPage)
        {
            //Convirtiendo las listas genericas a predeterminadas
            if (typeof(T) == typeof(Models.Producto))
            {
                var lista2 = List.Cast<Models.Producto>().ToList();
                SetProductPaging(lista2, RecordsPerPage);
            }
            else if (typeof(T) == typeof(Models.Venta))
            {
                var lista2 = List.Cast<Models.Venta>().ToList();
                SetVentaPaging(lista2, RecordsPerPage);
            }
            else if (typeof(T) == typeof(Models.Pedido))
            {
                var lista2 = List.Cast<Models.Pedido>().ToList();
                SetPedidoPaging(lista2, RecordsPerPage);
            }
            else if (typeof(T) == typeof(Models.DetalleProveedor))
            {
                var lista2 = List.Cast<Models.DetalleProveedor>().ToList();
                SetImportacionPaging(lista2, RecordsPerPage);
            }
            else
            {
                var lista2 = List.Cast<Models.Cliente>().ToList();
                SetClientPaging(lista2, RecordsPerPage);
            }
        }

        //Metodos de paginacion

        public void SetClientPaging(IList<Models.Cliente> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;

            IList<Models.Cliente> PagedList = new List<Models.Cliente>();

            Clientes = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();

        }

        public void SetVentaPaging(IList<Models.Venta> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;

            IList<Models.Venta> PagedList = new List<Models.Venta>();

            Ventas = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();

        }

        public void SetImportacionPaging(IList<Models.DetalleProveedor> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;

            IList<Models.DetalleProveedor> PagedList = new List<Models.DetalleProveedor>();

            Importaciones = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();

        }

        public void SetPedidoPaging(IList<Models.Pedido> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;

            IList<Models.Pedido> PagedList = new List<Models.Pedido>();

            Pedidos = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();

        }

        public void SetProductPaging (IList<Models.Producto> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;
            

            //IList<Models.Producto> PagedList = new List<Models.Producto>();

            Productos = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList(); 
            
        }
        
       
        //Accion de botones para avanzar y navegar entre las paginas
        public void Next<T>(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
      
            SomeMethod(ListToPage, RecordsPerPage);
            
        }

        public void Previous<T>(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
            SomeMethod(ListToPage, RecordsPerPage);
        }

        

        public void First<T>(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            SomeMethod(ListToPage, RecordsPerPage);
        }

        public void Last<T>(IList<T> ListToPage, int RecordsPerPage)
        {
            int count = ListToPage.Count;

            //Obtenemos el total de calculos
            float calculo = (float)count / RecordsPerPage;

            
            //Si es decimal le sumamos 1
            if (Math.Abs(calculo % 1) <= (Double.Epsilon * 100))
            {
                PageIndex = (int)calculo  ;
                //Es entero
            }

            else
            {
                //No Es entero
                PageIndex = (int)calculo;
            }

            //Bug solve
            if (ListToPage.Count % RecordsPerPage == 0)
            {
                PageIndex--;
            }

            SomeMethod(ListToPage, RecordsPerPage);
        }
        
    }
}
