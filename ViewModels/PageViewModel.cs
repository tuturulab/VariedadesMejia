using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Variedades.Models;
using Variedades.Utils;
using Variedades.Views;

namespace Variedades
{
    public class PageViewModel : INotifyPropertyChanged
    {
        //DbContext
        private DbmejiaEntities _context;

        static Paging PagedProductTable = new Paging();
        static Paging PagedClientTable = new Paging();
        static Paging PagedVentaTable = new Paging();
        static Paging PagedImportacionTable = new Paging();


        //Lists Methods
        public List<Producto> ProductosList;
        public List<Cliente> ClientesList;
        public List<Venta> VentasList;
        public List<Pedido> ImportacionList;

        
        public List<Pedido> PedidosList;
        public List<Producto> SearchProductList;
        public List<Venta> SearchVentaList;
        public List<Cliente> SearchClientList;

  
    
        //Declaracion del evento para llamar a la paginacion de la pagina productos una vez se llena la observable productos
        //public event EventHandler EventPaginationProduct;

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        //Observable for ImeiList
        private ObservableCollection<ImeiClass> ImeiList;
        public ObservableCollection<ImeiClass> ImeiCollection
        {
            get { return ImeiList; }
            set { ImeiList = value; NotifyPropertyChanged("ImeiCollection"); }
        }

        //Observable for ProductsList
        private ObservableCollection<Producto> Productos;
        public ObservableCollection<Producto> ProductosCollection
        {
            get { return Productos; }
            set { Productos = value; NotifyPropertyChanged("ProductosCollection"); }
        }

        //Observable for ProductListCompleteFull List
        public ObservableCollection<Especificacion_producto> especificacion_Productos;
        public ObservableCollection<Especificacion_producto> ProductosEspecificacionesCollection
        {
            get { return especificacion_Productos; }
            set { especificacion_Productos = value; NotifyPropertyChanged("ProductosEspecificacionesCollection"); }
        }

        private ObservableCollection<Proveedor> Proveedors;

        //Observable for ProveedorFull List
        public ObservableCollection<Proveedor> ProveedorFullCollection
        {
            get { return Proveedors; }
            set { Proveedors = value; NotifyPropertyChanged("ProveedorFullCollection"); }
        }



        //Observable for ImportacionList
        private ObservableCollection<Pedido> Pedidos;
        public ObservableCollection<Pedido> PedidosCollection
        {
            get { return Pedidos; }
            set { Pedidos = value; NotifyPropertyChanged("PedidosCollection"); }
        }


        //Observable for VentassList
        private ObservableCollection<Venta> Ventas;
        public ObservableCollection<Venta> VentasCollection
        {
            get { return Ventas; }
            set { Ventas = value; NotifyPropertyChanged("VentasCollection"); }
        }

        //Observable for ClientList
        private ObservableCollection<Cliente> Clientes;
        public ObservableCollection<Cliente> ClientesCollection
        {
            get { return Clientes; }
            set { Clientes = value; NotifyPropertyChanged("ClientesCollection"); }
        }

        //Observable for ClientFullList
        private ObservableCollection<Cliente> ClientesFull;
        public ObservableCollection<Cliente> ClientesFullCollection
        {
            get { return ClientesFull; }
            set { ClientesFull = value; NotifyPropertyChanged("ClientesFullCollection"); }
        }


        private Producto _SelectedProduct;
        public Producto SelectedProduct
        {
            get { return _SelectedProduct; }
            set { _SelectedProduct = value; NotifyPropertyChanged("SelectedProduct"); }
        }

        private TelefonosAddList _SelectedTelefonoAdd;
        public TelefonosAddList SelectedTelefonoAdd
        {
            get { return _SelectedTelefonoAdd; }
            set { _SelectedTelefonoAdd = value; NotifyPropertyChanged("SelectedTelefonoAdd"); }
        }

        //Selected Client in SelectClientWindow
        private Cliente _SelectedClientWindow;
        public Cliente SelectedClientWindow
        {
            get { return _SelectedClientWindow; }
            set { _SelectedClientWindow = value; NotifyPropertyChanged("SelectedClientWindow"); }
        }

        //Selected Product in SelectProductWindow
        private Especificacion_producto _SelectedProductWindow;
        public Especificacion_producto SelectedProductWindow
        {
            get { return _SelectedProductWindow; }
            set { _SelectedProductWindow = value; NotifyPropertyChanged("SelectedProductWindow"); }
        }

        //Selected Proveedor in SelectProveedorWindow
        private Proveedor _SelectedProveedorWindow;
        public Proveedor SelectedProveedorWindow
        {
            get { return _SelectedProveedorWindow; }
            set { _SelectedProveedorWindow = value; NotifyPropertyChanged("SelectedProveedorWindow"); }
        }


        public PageViewModel()
        {
            _context = new DbmejiaEntities();
            UpdateAll();
        }

        //Find Proveedor

        public Proveedor GetProveedor(int Id)
        {
            return _context.Proveedor.Find(Id);
        }


        /*
         * 
         *  Métodos de la Página Productos
         * 
        */


        //AddProduct Version2
        public void AddProductV2(Producto product)
        {
            _context.Producto.Add(product);
            _context.SaveChanges();
        }
        
        //Agregar existencias a x producto
        public void AddEspecificacionProducto (List<Especificacion_producto> _especificacion_producto)
        {
            foreach (var i in _especificacion_producto)
            {
                _context.Especificacion_producto.Add(i);
            }

            _context.SaveChanges();
        }


        //Agrega en la base de datos, el producto especificado
        public void AddProduct(Producto Product)
        {
            try
            {
                _context.Producto.Add(Product);
                _context.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Error al agregar en la base de datos");
            }

            UpdateProducts(3);
        }

        //Metodo de busqueda en la base de datos 
        public void SearchProduct(string filtro)
        {

            if (filtro != string.Empty)
            {

                SearchProductList = ProductosList.Where(s => (s.Modelo.ToLower().Contains(filtro.ToLower())) || s.Marca.ToLower().Contains(filtro.ToLower())).ToList();



                UpdateProducts(3, SearchProductList);
            }
            else
            {
                SearchProductList = null;
                UpdateProducts(3, ProductosList);
            }

        }

        // Botones de la paginacion de la tabla productos

        public void NextProduct(int NumberOfRecords)
        {
            if (SearchProductList != null)
            {
                PagedProductTable.Next(SearchProductList, NumberOfRecords);
                UpdateProducts(NumberOfRecords, SearchProductList);
            }
            else
            {
                PagedProductTable.Next(ProductosList, NumberOfRecords);
                UpdateProducts(NumberOfRecords);
            }
        }

        public void PreviousProduct(int NumberOfRecords)
        {
            if (SearchProductList != null)
            {
                PagedProductTable.Previous(SearchProductList, NumberOfRecords);
                UpdateProducts(NumberOfRecords, SearchProductList);
                
            }
            else
            {
                PagedProductTable.Previous(ProductosList, NumberOfRecords);
                UpdateProducts(NumberOfRecords);
            }
            
        }

        public void FirstProduct(int NumberOfRecords)
        {
            if (SearchProductList != null)
            {
                PagedProductTable.First(SearchProductList, NumberOfRecords);
                UpdateProducts(NumberOfRecords, SearchProductList);
            }
            else
            {
                PagedProductTable.First(ProductosList, NumberOfRecords);
                UpdateProducts(NumberOfRecords);
            }
        }

        public void LastProduct(int NumberOfRecords)
        {
            if (SearchProductList != null)
            {
                PagedProductTable.Last(SearchProductList, NumberOfRecords);
                UpdateProducts(NumberOfRecords, SearchProductList);
            }
            else
            {
                PagedProductTable.Last(ProductosList, NumberOfRecords);
                UpdateProducts(NumberOfRecords);
            }
        }

        //Actualiza unicamente la tabla productos
        public void UpdateProducts(int NumberOfRecords, List<Producto> SearchProductList = null)
        {
            

            if(SearchProductList !=null)
            {
                PagedProductTable.SomeMethod(SearchProductList, NumberOfRecords);
                ProductosCollection = new ObservableCollection<Producto>(PagedProductTable.Productos);
               
            }else
            {
                //Consulta
                ProductosList = _context.Producto.ToList();
                //Paginacion
                PagedProductTable.SomeMethod(ProductosList, NumberOfRecords);
                ProductosCollection = new ObservableCollection<Producto>(PagedProductTable.Productos);
            }
            
        }

        //Obtener la pagina actual ()
        public int PageProductsNumber()
        {
            return PagedProductTable.PageIndex;
        }

        //Set the Especificaciones Product List 
        public void FillEspecificacionesProducts()
        {
            //Obtener los productos que no se han vendido
            especificacion_Productos = new ObservableCollection<Especificacion_producto>(_context.Especificacion_producto.Where(t => t.Venta == null).ToList());
        }

        //Obtener el maximo numero de paginas ()
        public int PageProductsNumberMax()
        {
            int count = 0;

            //Validamos si buscamos en la lista normal de productos, o en la lista generada al buscar
            if (SearchProductList != null)
            {
                count = SearchProductList.Count;
            }
            else
            {
                count = ProductosList.Count;
            }


            //Obtenemos el total de calculos
            float calculo = (float)count / 3;

            
            //Si es decimal le sumamos 1

            if ( Math.Abs(calculo % 1) <= (Double.Epsilon * 100) )
            {
                return (int)calculo;
            }

            else
            {
                return (int)calculo + 1;  
            }
        }


        //Actualizamos todas las lista de todas las datagrid de cada una de las paginas
        private void UpdateAll()
        {
            ProductosList = _context.Producto.ToList();
            ClientesList = _context.Cliente.ToList();
            VentasList = _context.Venta.ToList();
            ImportacionList = _context.Pedido.ToList();

            //Collecciones usadas en las ventanas donde saldra para seleccionar
            ClientesFullCollection = new ObservableCollection<Cliente>( _context.Cliente.ToList());
            
            //Paginacion
            PagedProductTable.SomeMethod(ProductosList, 3);
            PagedClientTable.SomeMethod(ClientesList, 3);
            PagedVentaTable.SomeMethod(VentasList, 3);
            PagedImportacionTable.SomeMethod(ImportacionList, 3);

            //Vaciando las colecciones anteriores
            ProductosCollection = null;
            ClientesCollection = null;
            VentasCollection = null;
            PedidosCollection = null;

            //Procedemos a actualizar

            ProductosCollection = new ObservableCollection<Producto>( PagedProductTable.Productos);
            
            ClientesCollection = new ObservableCollection<Cliente>( PagedClientTable.Clientes );

            VentasCollection = new ObservableCollection<Venta>(PagedVentaTable.Ventas);

            PedidosCollection = new ObservableCollection<Pedido>(PagedImportacionTable.Pedidos); 
        }
        
        //Modulo de editar producto
        private void EditProduct (int id )
        {
            var producto = _context.Producto.Find(id);
            
        }

        //Actualizar producto
        public void UpdateProduct<T>(T item) where T: Producto
        {
            var entity = _context.Producto.Find(item.IdProducto);
            if(entity == null)
            {
                return;
            }

            _context.Entry(entity).CurrentValues.SetValues(item);
            _context.SaveChanges();

            UpdateProducts(3);
        }

        //Modulo de borrado
        public void DeleteProduct(Producto _Producto)
        {
            //Buscamos el producto seleccionado y lo eliminamos de la base de datos
            _context.Producto.Remove(_Producto);
            
            //Eliminar del observable collection
            Productos.Remove(_Producto);

            //Eliminar de base de datos
            _context.SaveChanges();

            //Actualizamos el datagrid
            UpdateProducts(3);
            
        }

        /*
        * 
        *  Métodos de la Página Clientes
        * 
       */

        //Agrega en la base de datos, el producto especificado
        public void AddClient(Cliente Cliente, List<Telefono> telefonos = null)
        {
            try
            {
                var cliente =_context.Cliente.Add(Cliente);

                foreach (var telefono in telefonos)
                {
                    _context.Telefono.Add(telefono);
                }

                _context.SaveChanges();

            }
            catch
            {
                Console.WriteLine("Error Al ingresar en la base de datos");
            }

          

            UpdateClients(3);
        }

        // Botones de la paginacion de la tabla productos

       public void SearchClient(string FiltroClient)
        {

            if (FiltroClient != string.Empty)
            {
                SearchClientList = ClientesList.Where(c => (c.Nombre.ToLower().Contains(FiltroClient.ToLower())) || (c.Compania.ToLower().Contains(FiltroClient.ToLower())) ).ToList();
                UpdateClients(3, SearchClientList);
            }
            else
            {
                SearchClientList = null;
                UpdateClients(3, ClientesList);
            }

        }

        // Botones de la paginacion de la tabla cliente
        public void NextClient(int NumberOfRecords)
        {
            if (SearchClientList != null)
            {
                PagedClientTable.Next(SearchClientList, NumberOfRecords);
                UpdateClients(NumberOfRecords, SearchClientList);
            }
            else
            {
                PagedClientTable.Next(ClientesList, NumberOfRecords);
                UpdateClients(NumberOfRecords);
            }
        }

        public void PreviousClient(int NumberOfRecords)
        {
            if (SearchClientList != null)
            {
                PagedClientTable.Previous(SearchClientList, NumberOfRecords);
                UpdateClients(NumberOfRecords, SearchClientList);
            }
            else
            {
                PagedClientTable.Previous(ClientesList, NumberOfRecords);
                UpdateClients(NumberOfRecords);
            }

        }

        public void FirstClient(int NumberOfRecords)
        {
            if (SearchClientList != null)
            {
                PagedClientTable.First(SearchClientList, NumberOfRecords);
                UpdateClients(NumberOfRecords, SearchClientList);
            }
            else
            {
                PagedClientTable.First(ClientesList, NumberOfRecords);
                UpdateClients(NumberOfRecords);
            }
        }

        public void LastClient(int NumberOfRecords)
        {
            if (SearchClientList != null)
            {
                PagedClientTable.Last(SearchClientList, NumberOfRecords);
                UpdateClients(NumberOfRecords, SearchClientList);
            }
            else
            {
                PagedClientTable.Last(ClientesList, NumberOfRecords);
                UpdateClients(NumberOfRecords);
            }
        }

        //Actualiza unicamente la tabla productos
        public void UpdateClients(int NumberOfRecords, List<Cliente> SearchClientList = null)
        {


            if (SearchClientList != null)
            {
                PagedClientTable.SomeMethod(SearchClientList, NumberOfRecords);
                ClientesCollection = new ObservableCollection<Cliente>(PagedClientTable.Clientes);

            }
            else
            {
                //Consulta
                ClientesList = _context.Cliente.ToList();
                //Paginacion
                PagedClientTable.SomeMethod(ClientesList, NumberOfRecords);
                ClientesCollection = new ObservableCollection<Cliente>(PagedClientTable.Clientes);
            }

        }

        //Obtener la pagina actual ()
        public int PageClientesNumber()
        {
            return PagedClientTable.PageIndex;
        }

        //Obtener el maximo numero de paginas ()
        public int PageClientesNumberMax()
        {
            int count = ClientesList.Count;
            //Obtenemos el total de calculos
            float calculo = (float)count / 3;
            
            //Si es decimal le sumamos 1
            if (Math.Abs(calculo % 1) <= (Double.Epsilon * 100))
            {
                return (int)calculo;
            }

            else
            {
                return (int)calculo + 1;
            }
        }


        
        //Modulo de editar producto
        private void EditCliente(int id)
        {
            //var producto = _context.Producto.Find(id);

        }

        //Modulo de borrado
        public void DeleteClient(int id)
        {
            //Buscamos el producto seleccionado y lo eliminamos de la base de datos
            var cliente = _context.Cliente.Find(id);
            _context.Cliente.Remove(cliente);

            //Eliminar del observable collection
            Clientes.Remove(cliente);

            //Guardamos los cambios de la base de datos
            _context.SaveChanges();

            //Actualizamos el datagrid
            UpdateClients(3);
        }


        /*
        * 
        *  Métodos de la Página Ventas
        * 
       */

        public void SearchVenta(string FiltroVenta)
        {

            if (FiltroVenta != string.Empty)
            {
                SearchVentaList = VentasList.Where(v => (v.Orden_Pagare.ToLower().Contains(FiltroVenta.ToLower())) || (v.Cliente.Nombre.ToLower().Contains(FiltroVenta.ToLower()))).ToList();
                UpdateVentas(3, SearchVentaList);
            }
            else
            {
                SearchVentaList = null;
                UpdateVentas(3, VentasList);
            }

        }

        //Agrega en la base de datos, el producto especificado
        public void AddVenta(Venta _Venta)
        {
            try
            {
                _context.Venta.Add(_Venta);
                _context.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Error al agregar en la base de datos");
            }
        }

        // Botones de la paginacion de la tabla productos

        public void NextVenta(int NumberOfRecords)
        {
            if (SearchVentaList != null)
            {
                PagedClientTable.Next(SearchVentaList, NumberOfRecords);
                UpdateVentas(NumberOfRecords, SearchVentaList);
            }
            else
            {
                PagedVentaTable.Next(VentasList, NumberOfRecords);
                UpdateVentas(NumberOfRecords);
            }
        }

        public void PreviousVenta(int NumberOfRecords)
        {
            if (SearchVentaList != null)
            {
                PagedVentaTable.Previous(SearchVentaList, NumberOfRecords);
                UpdateVentas(NumberOfRecords, SearchVentaList);
            }
            else
            {
                PagedVentaTable.Previous(VentasList, NumberOfRecords);
                UpdateVentas(NumberOfRecords);
            }
        }

        public void FirstVenta(int NumberOfRecords)
        {
            if (SearchVentaList != null)
            {
                PagedVentaTable.First(SearchVentaList, NumberOfRecords);
                UpdateVentas(NumberOfRecords, SearchVentaList);
            }
            else
            {
                PagedVentaTable.First(VentasList, NumberOfRecords);
                UpdateVentas(NumberOfRecords);
            }
        }

        public void LastVenta(int NumberOfRecords)
        {
            if (SearchVentaList != null)
            {
                PagedVentaTable.Last(SearchVentaList, NumberOfRecords);
                UpdateVentas(NumberOfRecords, SearchVentaList);
            }
            else
            {
                PagedVentaTable.Last(VentasList, NumberOfRecords);
                UpdateVentas(NumberOfRecords);
            }
        }

        //Actualiza unicamente la tabla Ventas
        public void UpdateVentas(int NumberOfRecords, List<Venta> SearchVentaList = null)
        { 
        

            //Si hay algo en la busqueda la mostrara
            if (SearchVentaList != null)
            {
                PagedVentaTable.SomeMethod(SearchVentaList, NumberOfRecords);
                VentasCollection = new ObservableCollection<Venta>(PagedVentaTable.Ventas);

            }
            else
            { 
          
                //Consulta
                VentasList = _context.Venta.ToList();

                //Paginacion
                PagedVentaTable.SomeMethod(VentasList, NumberOfRecords);
                VentasCollection = new ObservableCollection<Venta>(PagedVentaTable.Ventas);
            }
        }


        //Obtener la pagina actual ()
        public int PageVentasNumber()
        {
            return PagedVentaTable.PageIndex;
        }

        //Obtener el maximo numero de paginas ()
        public int PageVentasNumberMax()
        {
            int count = VentasList.Count;
            //Obtenemos el total de calculos
            float calculo = (float)count / 3;

            //Si es decimal le sumamos 1
            if (Math.Abs(calculo % 1) <= (Double.Epsilon * 100))
            {
                return (int)calculo;
            }

            else
            {
                return (int)calculo + 1;
            }
        }



        //Modulo de editar Venta
        private void EditVenta(int id)
        {
            //var producto = _context.Producto.Find(id);

        }

        //Modulo de borrado de venta
        public void DeleteVenta(int id)
        {
            //Buscamos el producto seleccionado y lo eliminamos de la base de datos
            var venta = _context.Venta.Find(id);
            _context.Venta.Remove(venta);

            //Eliminar del observable collection
            Ventas.Remove(venta);

            //Guardamos los cambios de la base de datos
            _context.SaveChanges();

            //Actualizamos el datagrid
            UpdateVentas(3);
        }

        /*
        * 
        *  Métodos de la Página Importación
        * 
       */

        //Agrega en la base de datos, el producto especificado
        public void AddImportacion(Venta _Venta)
        {

        }

        // Botones de la paginacion de la tabla productos

        public void NextImportacion(int NumberOfRecords)
        {
            PagedImportacionTable.Next(ImportacionList, NumberOfRecords);
            UpdateImportacion(NumberOfRecords);
        }

        public void PreviousImportacion(int NumberOfRecords)
        {
            PagedImportacionTable.Previous(ImportacionList, NumberOfRecords);
            UpdateImportacion(NumberOfRecords);
        }

        public void FirstImportacion(int NumberOfRecords)
        {
            PagedImportacionTable.First(ImportacionList, NumberOfRecords);
            UpdateImportacion(NumberOfRecords);
        }

        public void LastImportacion(int NumberOfRecords)
        {
            PagedImportacionTable.Last(ImportacionList, NumberOfRecords);
            UpdateImportacion(NumberOfRecords);
        }

        //Actualiza unicamente la tabla Ventas
        public void UpdateImportacion(int NumberOfRecords)
        {
            //Consulta
            ImportacionList = _context.Pedido.ToList();

            //Paginacion
            PagedImportacionTable.SomeMethod(ImportacionList, NumberOfRecords);
            //ImportacionCol = new ObservableCollection<Pedido>(PagedImportacionTable.Pedidos);
        }

        //Obtener la pagina actual ()
        public int PageImportacionNumber()
        {
            return PagedImportacionTable.PageIndex;
        }

        //Obtener el maximo numero de paginas ()
        public int PageImportacionNumberMax()
        {
            int count = ImportacionList.Count;
            //Obtenemos el total de calculos
            float calculo = (float)count / 3;

            //Si es decimal le sumamos 1
            if (Math.Abs(calculo % 1) <= (Double.Epsilon * 100))
            {
                return (int)calculo;
            }

            else
            {
                return (int)calculo + 1;
            }
        }



        //Modulo de editar Importacion
        private void EditImportacion(int id)
        {
            //var producto = _context.Producto.Find(id);

        }

        //Modulo de borrado de venta
        public void DeleteImportacion(int id)
        {
            //Buscamos el producto seleccionado y lo eliminamos de la base de datos
            var venta = _context.Pedido.Find(id);
            _context.Pedido.Remove(venta);

            //Eliminar del observable collection
            //Pedido.Remove(venta);

            //Guardamos los cambios de la base de datos
            _context.SaveChanges();

            //Actualizamos el datagrid
            UpdateImportacion(3);
        }


        /*
         * 
         * Metodos Proveedor
         *
        */
        
        //Se asegura de rellenar la lista de Proveedores actual
        public void FillProveedorFullList()
        {
            ProveedorFullCollection = new ObservableCollection<Proveedor>(_context.Proveedor.ToList());
        }

        //Método de busqueda
        public void SearchProveedorList(string Filtro)
        {
            if (Filtro != string.Empty)
            {
                ProveedorFullCollection = new ObservableCollection<Proveedor>(_context.Proveedor.Where(s => (s.Empresa.ToLower().Contains(Filtro.ToLower()))));
            }
            else
            {
                ProveedorFullCollection = new ObservableCollection<Proveedor>(_context.Proveedor.ToList());
            }
        }

        //Agregar Proveedor
        public void AddProveedor(Proveedor proveedor)
        {
            var Proveedor = _context.Proveedor.Add(proveedor);
            _context.SaveChanges();

            SelectedProveedorWindow = proveedor;
        }
        
    }

    //Clase usada para rellenar la datagrid de los Imei
    public class ImeiClass
    {
        public string Imei { get; set; }
    }
}
