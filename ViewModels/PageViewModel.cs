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

        public List<Producto> ProductosList;
        public List<Cliente> ClientesList;
        public List<Venta> VentasList;
        public List<Pedido> ImportacionList;


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
        private ObservableCollection<Producto> ProductosFull;
        public ObservableCollection<Producto> ProductosFullCollection
        {
            get { return ProductosFull; }
            set { ProductosFull = value; NotifyPropertyChanged("ProductosCollection"); }
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

        //Selected Client in SelectClientWindow
        private Cliente _SelectedClientWindow;
        public Cliente SelectedClientWindow
        {
            get { return _SelectedClientWindow; }
            set { _SelectedClientWindow = value; NotifyPropertyChanged("SelectedClientWindow"); }
        }


        public PageViewModel()
        {
            _context = new DbmejiaEntities();
            UpdateAll();
        }


        /*
         * 
         *  Métodos de la Página Productos
         * 
        */

        //Agrega en la base de datos, el producto especificado
        public void AddProduct(Producto Product, List<Especificacion_producto> Especificaciones, Proveedor _Proveedor = null)
        {
            try
            {
                _context.Producto.Add(Product);
                foreach (var especificacion in Especificaciones)
                {
                    _context.Especificacion_producto.Add(especificacion);
                }

                //Al insertarse directamente en producto, no hace falta ya agregar el valor del seguimiento y verificar cuanto llego de lo que pidio
                if (_Proveedor != null)
                {
                    var ProveedorProducto = new Proveedor_producto();
                    ProveedorProducto.Producto = Product;
                    
                    //Creamos la entidad intermedia
                    var ProveedoresProductos = new List<Proveedor_producto>();
                    ProveedoresProductos.Add(ProveedorProducto);

                    //Agregamos ambas y las relacionamos
                    _Proveedor.Proveedores_producto = ProveedoresProductos;

                    _context.Proveedor.Add(_Proveedor);
                    _context.Proveedor_producto.Add(ProveedorProducto);

                }
                _context.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Error al agregar en la base de datos");
            }

            UpdateProducts(3);
        }

        // Botones de la paginacion de la tabla productos

        public void NextProduct(int NumberOfRecords)
        {
            PagedProductTable.Next(ProductosList, NumberOfRecords);
            UpdateProducts(NumberOfRecords);
        }

        public void PreviousProduct(int NumberOfRecords)
        {
            PagedProductTable.Previous(ProductosList, NumberOfRecords);
            UpdateProducts(NumberOfRecords);
        }

        public void FirstProduct(int NumberOfRecords)
        {
            PagedProductTable.First(ProductosList, NumberOfRecords);
            UpdateProducts(NumberOfRecords);
        }

        public void LastProduct(int NumberOfRecords)
        {
            PagedProductTable.Last(ProductosList, NumberOfRecords);
            UpdateProducts(NumberOfRecords);
        }

        //Actualiza unicamente la tabla productos
        public void UpdateProducts(int NumberOfRecords)
        {
            //Consulta
            ProductosList = _context.Producto.ToList();
            //Paginacion
            PagedProductTable.SomeMethod(ProductosList, NumberOfRecords);
            ProductosCollection = new ObservableCollection<Producto>(PagedProductTable.Productos);
        }

        //Texto para mostrar el numero de paginas , y la pagina actual de la paginacion en la tabla productos
        /*Por si se llega a necesitar
        public string PageProductNumberDisplay(int NumberOfRecords)
        {
            int PagedNumber = NumberOfRecords * (PagedTable.PageIndex + 1);
            if (PagedNumber > ProductosList.Count)
            {
                PagedNumber = ProductosList.Count;
            }
            return "Showing " + PagedNumber + " of " + ProductosList.Count;
        }
        */

        //Obtener la pagina actual ()
        public int PageProductsNumber()
        {
            return PagedProductTable.PageIndex;
        }

        //Obtener el maximo numero de paginas ()
        public int PageProductsNumberMax()
        {
            int count = ProductosList.Count;


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
            ProductosFullCollection = new ObservableCollection<Producto>(_context.Producto.ToList() );
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
        }

        //Modulo de borrado
        public void DeleteProduct(int id)
        {
            //Buscamos el producto seleccionado y lo eliminamos de la base de datos
            var producto = _context.Producto.Find(id);
            _context.Producto.Remove(producto);
            
            //Eliminar del observable collection
            Productos.Remove(producto);

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
                _context.Cliente.Add(Cliente);

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

        public void NextClient(int NumberOfRecords)
        {
            PagedClientTable.Next(ClientesList, NumberOfRecords);
            UpdateClients(NumberOfRecords);
        }

        public void PreviousClient(int NumberOfRecords)
        {
            PagedClientTable.Previous(ClientesList, NumberOfRecords);
            UpdateClients(NumberOfRecords);
        }

        public void FirstClient(int NumberOfRecords)
        {
            PagedClientTable.First(ClientesList, NumberOfRecords);
            UpdateClients(NumberOfRecords);
        }

        public void LastClient(int NumberOfRecords)
        {
            PagedClientTable.Last(ClientesList, NumberOfRecords);
            UpdateClients(NumberOfRecords);
        }

        //Actualiza unicamente la tabla productos
        public void UpdateClients(int NumberOfRecords)
        {
            //Consulta
            ClientesList = _context.Cliente.ToList();

            //Paginacion
            PagedClientTable.SomeMethod(ClientesList, NumberOfRecords);
            ClientesCollection = new ObservableCollection<Cliente>(PagedClientTable.Clientes);
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

        //Agrega en la base de datos, el producto especificado
        public void AddVenta(Venta _Venta)
        {

        }

        // Botones de la paginacion de la tabla productos

        public void NextVenta(int NumberOfRecords)
        {
            PagedVentaTable.Next(VentasList, NumberOfRecords);
            UpdateVentas(NumberOfRecords);
        }

        public void PreviousVenta(int NumberOfRecords)
        {
            PagedVentaTable.Previous(VentasList, NumberOfRecords);
            UpdateVentas(NumberOfRecords);
        }

        public void FirstVenta(int NumberOfRecords)
        {
            PagedVentaTable.First(VentasList, NumberOfRecords);
            UpdateVentas(NumberOfRecords);
        }

        public void LastVenta(int NumberOfRecords)
        {
            PagedVentaTable.Last(ClientesList, NumberOfRecords);
            UpdateVentas(NumberOfRecords);
        }

        //Actualiza unicamente la tabla Ventas
        public void UpdateVentas(int NumberOfRecords)
        {
            //Consulta
            VentasList = _context.Venta.ToList();

            //Paginacion
            PagedClientTable.SomeMethod(VentasList, NumberOfRecords);
            VentasCollection = new ObservableCollection<Venta>(PagedVentaTable.Ventas);
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
        
    }

    //Clase usada para rellenar la datagrid de los Imei
    public class ImeiClass
    {
        public string Imei { get; set; }
    }
}
