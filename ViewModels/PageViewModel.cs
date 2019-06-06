using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
        static Paging PagedPedidoTable = new Paging();


        //Lists Methods
        public List<Producto> ProductosList;
        public List<Cliente> ClientesList;
        public List<Venta> VentasList;
        public List<DetalleProveedor> ImportacionList;
        public List<Pedido> PedidosList;

        //Lists for searchs
        public List<Producto> SearchProductList;
        public List<Venta> SearchVentaList;
        public List<Cliente> SearchClientList;
        public List<Pedido> SearchPedidoList;

        
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

        //Observable for ImeiList
        private ObservableCollection<Especificacion_producto> ProductosDeUnaImportacion;
        public ObservableCollection<Especificacion_producto> ListaProductosDeUnaImportacion
        {
            get { return ProductosDeUnaImportacion; }
            set { ProductosDeUnaImportacion = value; NotifyPropertyChanged("ListaProductosDeUnaImportacion"); }
        }


        //Observable for Pedidos Importados in ImportacionToProductWindow
        private ObservableCollection<Producto_importado> ProductosImportados;
        public ObservableCollection<Producto_importado> ListaProductosImportados
        {
            get { return ProductosImportados; }
            set { ProductosImportados = value; NotifyPropertyChanged("ListaProductosImportados"); }
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


        //Observable for ProveedorFull List
        private ObservableCollection<Proveedor> Proveedors;
        public ObservableCollection<Proveedor> ProveedorFullCollection
        {
            get { return Proveedors; }
            set { Proveedors = value; NotifyPropertyChanged("ProveedorFullCollection"); }
        }

        private ObservableCollection<Especificacion_pedido> Especificacion_Pedidos;
        public ObservableCollection<Especificacion_pedido> Especificaciones_De_un_Pedido
        {
            get { return Especificacion_Pedidos;  }
            set { Especificacion_Pedidos = value; NotifyPropertyChanged("EspecificacionesDePedidoCollection"); }
        }

        //Observable for PedidosList
        private ObservableCollection<Pedido> Pedidos;
        public ObservableCollection<Pedido> PedidosCollection
        {
            get { return Pedidos; }
            set { Pedidos = value; NotifyPropertyChanged("PedidosCollection"); }
        }

        //Observable for Product Father Collection

        private ObservableCollection<Producto> ProductosFather;
        public ObservableCollection<Producto> ProductosFatherCollection
        {
            get { return ProductosFather;  }
            set { ProductosFather = value; NotifyPropertyChanged("ProductosFatherCollection"); }
        }

        private Producto _SelectedFatherProduct;
        public Producto SelectedFatherProduct
        {
            get { return _SelectedFatherProduct; }
            set { _SelectedFatherProduct = value; NotifyPropertyChanged("SelectedFatherProduct"); }
        }

        //Ventana Detalle Venta
        private ObservableCollection<Pago> Pagos;
        public ObservableCollection<Pago> PagosCollection
        {
            get { return Pagos; }
            set { Pagos = value; NotifyPropertyChanged("PagosCollection"); }
        }

        private ObservableCollection<Especificacion_producto> ProductosComprados;
        public ObservableCollection<Especificacion_producto> ProductosCompradosCollection
        {
            get { return ProductosComprados;  }
            set { ProductosComprados = value; NotifyPropertyChanged("ProductosCompradosCollection"); }
        }

       
        //Observable for ImportacionList
        private ObservableCollection<DetalleProveedor> Importaciones;
        public ObservableCollection<DetalleProveedor> ImportacionesCollection
        {
            get { return Importaciones; }
            set { Importaciones = value; NotifyPropertyChanged("ImportacionesCollection"); }
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

        //Observable for ClientFullList
        private ObservableCollection<DetalleProveedor> Importacion;
        public ObservableCollection<DetalleProveedor> ImportacionCollection
        {
            get { return Importacion; }
            set { Importacion = value; NotifyPropertyChanged("ImportacionCollection"); }
        }

        //Observable for Productos de un pedido
        private ObservableCollection<Especificacion_pedido> ProductosPedido;
        public ObservableCollection<Especificacion_pedido> ProductosPedidoCollection
        {
            get { return ProductosPedido; }
            set { ProductosPedido = value; NotifyPropertyChanged("ProductosPedidoCollection"); }
        }

        //Observable for ClientFullList
        private ObservableCollection<Pedido> Pedido;
        public ObservableCollection<Pedido> PedidoCollection
        {
            get { return Pedido; }
            set { Pedido = value; NotifyPropertyChanged("ImportacionCollection"); }
        }


        private Producto _SelectedProduct;
        public Producto SelectedProduct
        {
            get { return _SelectedProduct; }
            set { _SelectedProduct = value; NotifyPropertyChanged("SelectedProduct"); }
        }

        private DetalleProveedor _SelectedImportacion;
        public DetalleProveedor SelectedImportacion
        {
            get { return _SelectedImportacion; }
            set { _SelectedImportacion = value; NotifyPropertyChanged("SelectedImportacion"); }
        }

        private Pedido _SelectedPedido;
        public Pedido SelectedPedido
        {
            get { return _SelectedPedido; }
            set { _SelectedPedido = value; NotifyPropertyChanged("SelectedPedido"); }
        }


        private Venta _SelectedVenta;
        public Venta SelectedVenta
        {
            get { return _SelectedVenta; }
            set { _SelectedVenta = value; NotifyPropertyChanged("SelectedVenta"); }
        }

        private Especificacion_producto _SelectedEspecificacionProductoInImport;
        public Especificacion_producto SelectedEspecificacionProductoInImport
        {
            get { return _SelectedEspecificacionProductoInImport; }
            set { _SelectedEspecificacionProductoInImport = value; NotifyPropertyChanged("SelectedEspecificacionProductoInImport"); }
        }

        private Especificacion_pedido _SelectedEspecificacion_Pedido;
        public Especificacion_pedido SelectedEspecificacionPedido
        {
            get { return _SelectedEspecificacion_Pedido; }
            set { _SelectedEspecificacion_Pedido = value; NotifyPropertyChanged("SelectedEspecificacionPedido"); }
        }

        private Pedido _SelectedPedidoWindow;
        public Pedido SelectedPedidoWindow
        {
            get { return _SelectedPedidoWindow; }
            set { _SelectedPedidoWindow = value; NotifyPropertyChanged("SelectedPedidoWindow"); }
        }

        private TelefonosAddList _SelectedTelefonoAdd;
        public TelefonosAddList SelectedTelefonoAdd
        {
            get { return _SelectedTelefonoAdd; }
            set { _SelectedTelefonoAdd = value; NotifyPropertyChanged("SelectedTelefonoAdd"); }
        }

        private Telefono _SelectedTelefono;
        public Telefono SelectedEditTelefono
        {
            get { return _SelectedTelefono; }
            set { _SelectedTelefono = value; NotifyPropertyChanged("SelectedEditTelefono"); }
        }

        //Selected Client in SelectClientWindow
        private Cliente _SelectedClientWindow;
        public Cliente SelectedClientWindow
        {
            get { return _SelectedClientWindow; }
            set { _SelectedClientWindow = value; NotifyPropertyChanged("SelectedClientWindow"); }
        }

        //Selected Client in SelectClient
        private Cliente _SelectedClient;
        public Cliente SelectedClient
        {
            get { return _SelectedClient; }
            set { _SelectedClient = value; NotifyPropertyChanged("SelectedClient"); }
        }

        //Selected Product in SelectProductWindow
        private Especificacion_producto _SelectedProductWindow;
        public Especificacion_producto SelectedProductWindow
        {
            get { return _SelectedProductWindow; }
            set { _SelectedProductWindow = value; NotifyPropertyChanged("SelectedProductWindow"); }
        }

        //Selected Product in SelectProductImportado
        private Producto_importado _SelectedProductImportado;
        public Producto_importado SelectedProductImportado
        {
            get { return _SelectedProductImportado; }
            set { _SelectedProductImportado = value; NotifyPropertyChanged("SelectedProductImportado"); }
        }

        //Selected Product in the moment of Create Product
        private EspecificacionClass _SelectedEspecificacionProductoInProductoWindow;
        public EspecificacionClass SelectedEspecificacionProductoInProductoWindow
        {
            get { return _SelectedEspecificacionProductoInProductoWindow; }
            set { _SelectedEspecificacionProductoInProductoWindow = value; NotifyPropertyChanged("SelectedEspecificacionProductoInProductoWindow"); }
        }

        //Selected Proveedor in SelectProveedorWindow
        private Proveedor _SelectedProveedorWindow;
        public Proveedor SelectedProveedorWindow
        {
            get { return _SelectedProveedorWindow; }
            set { _SelectedProveedorWindow = value; NotifyPropertyChanged("SelectedProveedorWindow"); }
        }

        private Especificacion_producto _SelectedEspecificacionProducto;
        public Especificacion_producto SelectedEspecificacionProducto
        {
            get { return _SelectedEspecificacionProducto; }
            set { _SelectedEspecificacionProducto = value; NotifyPropertyChanged("SelectedEspecificacionProducto"); }
        }


        public PageViewModel()
        {
            _context = new DbmejiaEntities();

            if (isAvaliable())
            {
                Debug.WriteLine("Database is avaliable");
                UpdateAll();
            }
            
        }

        public User Login (String Nombre, String Password)
        {
            var user = _context.Users.FirstOrDefault(t => t.Nombre.Equals(Nombre));

            if (user == null)
            {
                return null;
            }

            if (user.Password.Equals(Password) == false )
            {
                return null;
            }

            return user;

        }

        public bool CheckIfAccountsExist ()
        {
            int num = _context.Users.Count();

            if (num == 0)
            {
                return false;
            }

            return true;
        }

        public User CreateAccount (User user)
        {
            var UserNew = _context.Users.Add(user);
            _context.SaveChanges();

            return UserNew;
        }

        public int ExistPagare(String OrdenPagare )
        {
            var cantidad = _context.Venta.Where(t => t.Orden_Pagare.Equals(OrdenPagare)).Count();
            return cantidad;
        }

        public void FillProductosDeUnPedido(Pedido _pedido)
        {
            List<Especificacion_pedido> Productos = _pedido.Especificaciones_pedido.ToList();
            ProductosPedidoCollection = new ObservableCollection<Especificacion_pedido>(Productos);
        }

        public void FillProductosDeUnaImportacion(DetalleProveedor _importacion)
        {
            List<Producto_importado> Productos = _importacion.Producto_Importados.ToList();
            ListaProductosImportados = new ObservableCollection<Producto_importado>(Productos);


        }

        public bool isAvaliable()
        {
            try
            {
                _context.Database.Connection.Open();
                _context.Database.Connection.Close();

                return true;
            }
            catch(SqlException)
            {
                return false;
            }
        }

        public void FillProductoFatherFullList ()
        {
            var Lista = _context.Producto.ToList();
            ProductosFatherCollection = new ObservableCollection<Producto>(Lista);
            
        }

        public void AddPago ( Pago pago_)
        {
            _context.Pago.Add(pago_);
            _context.SaveChanges();
        }


        public void ChangeEstadoImportacion( DetalleProveedor _Importacion )
        {
            DetalleProveedor Importacion = _context.DetalleProveedor.Find(_Importacion.IdDetalleProveedor);

            Importacion.Fecha_Llegada = DateTime.Now;
            Importacion.Estado = "Llegado";

            _context.SaveChanges();

            UpdateImportacion(10);

        }
    
        public void VerificarVentaEstado (Venta venta_)
        {
            double saldo = 0;
            foreach (var i in venta_.Pagos)
            {
                saldo = saldo + i.Monto;
            }

            if (saldo == venta_.MontoVenta)
            {
                var ventaNueva = _context.Venta.Find(venta_.IdVenta);
                ventaNueva.VentaCompletada = "Si";

                _context.SaveChanges();

                UpdateVentas(10);
            }
        }

        public void FillProductosDeUnaVenta (int idVenta)
        {
            var Lista = _context.Venta.Find(idVenta).Especificaciones_producto.ToList();

            ProductosCompradosCollection = new ObservableCollection<Especificacion_producto>(Lista);

            var ListaPagos = _context.Venta.Find(idVenta).Pagos.ToList();

            PagosCollection = new ObservableCollection<Pago>(ListaPagos);

        }

        //Rellenamos los productos importados de una importacion para la ventana ImportacionToProductWindow
        public void SetProductosImportados (DetalleProveedor Importacion)
        {
            List<Producto_importado> ListaProductos = Importacion.Producto_Importados.ToList();
            ListaProductosImportados = new ObservableCollection<Producto_importado>(ListaProductos);
        }

        public void SetEspecificacionPedido (Pedido pedido)
        {
            var Especifificaciones = pedido.Especificaciones_pedido.ToList();
            Especificaciones_De_un_Pedido = new ObservableCollection<Especificacion_pedido>(Especifificaciones);
        }

        //Rellena los datos insertados
        public void SearchProductosDeUnaImportacion (int Importacion)
        {
            var _Productos = _context.Especificacion_producto.Where(t => t.Proveedor_Producto.Idproveedor_producto == Importacion).ToList();

            ListaProductosDeUnaImportacion = new ObservableCollection<Especificacion_producto>(_Productos);
            
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

        public void DeleteEspecificacionProducto ()
        {          
            _context.Especificacion_producto.Remove(SelectedEspecificacionProductoInImport);
            ListaProductosDeUnaImportacion.Remove(SelectedEspecificacionProductoInImport);
            _context.SaveChanges();
            
            UpdateProducts(10);
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

        public void AddSingleEspecificacionProducto (Especificacion_producto especificacion)
        {
            _context.Especificacion_producto.Add(especificacion);
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

            UpdateProducts(10);
        }

        //Metodo de busqueda en la base de datos 
        public void SearchProduct(string filtro)
        {

            if (filtro != string.Empty)
            {

                SearchProductList = ProductosList.Where(s => (s.Modelo.ToLower().Contains(filtro.ToLower())) || s.Marca.ToLower().Contains(filtro.ToLower())).ToList();



                UpdateProducts(10, SearchProductList);
            }
            else
            {
                SearchProductList = null;
                UpdateProducts(10, ProductosList);
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
            especificacion_Productos = new ObservableCollection<Especificacion_producto>(_context.Especificacion_producto.Where(t => t.Vendido.Equals("No") ).ToList());
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
            float calculo = (float)count / 10;

            
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

        //Colleciones para rellenar ventanas menores
        public void FillClientesFullCollection ()
        {
            //Collecciones usadas en las ventanas donde saldra para seleccionar
            ClientesFullCollection = new ObservableCollection<Cliente>(_context.Cliente.ToList());
        }

        public void FillPedidos()
        {
            PedidosCollection = new ObservableCollection<Pedido>(_context.Pedido.Where(t => t.Estado_Pedido != "Completado"));
        }

        public void SetProductosVendidos(Especificacion_producto producto_)
        {
            var Producto = _context.Especificacion_producto.Find(producto_.IdEspecificaciones_Producto);

            Producto.Vendido = "Si";
            _context.SaveChanges();
            
        }
        
        //Actualizamos todas las lista de todas las datagrid de cada una de las paginas
        private void UpdateAll()
        {
            /* entity procedure call */
            //ProductosList = _context.Producto.ToList();
            ProductosList = _context.Database.SqlQuery<Producto>("dbo.GetAllProductos").ToList();

            ClientesList = _context.Database.SqlQuery<Cliente>("dbo.GetAllClientes").ToList();

            //VentasList = _context.Venta.ToList();
            VentasList = _context.Database.SqlQuery<Venta>("dbo.GetAllVentas").ToList();

            //ImportacionList = _context.DetalleProveedor.ToList();
            ImportacionList = _context.Database.SqlQuery<DetalleProveedor>("dbo.GetAllImportaciones").ToList();

            //PedidosList = _context.Pedido.ToList();
            PedidosList = _context.Database.SqlQuery<Pedido>("dbo.GetAllPedidos").ToList();

            //Collecciones usadas en las ventanas donde saldra para seleccionar
            //ClientesFullCollection = new ObservableCollection<Cliente>( _context.Cliente.ToList());

            //Paginacion
            PagedProductTable.SomeMethod(ProductosList, 10);
            PagedClientTable.SomeMethod(ClientesList, 10);
            PagedVentaTable.SomeMethod(VentasList, 10);
            PagedImportacionTable.SomeMethod(ImportacionList, 10);
            PagedPedidoTable.SomeMethod(PedidosList, 10);

            //Vaciando las colecciones anteriores
            ProductosCollection = null;
            ClientesCollection = null;
            VentasCollection = null;
            PedidosCollection = null;
            ImportacionCollection = null;

            //Procedemos a actualizar

            ProductosCollection = new ObservableCollection<Producto>( PagedProductTable.Productos);
            
            ClientesCollection = new ObservableCollection<Cliente>( PagedClientTable.Clientes );

            VentasCollection = new ObservableCollection<Venta>(PagedVentaTable.Ventas);

            PedidosCollection = new ObservableCollection<Pedido>(PagedPedidoTable.Pedidos);

            ImportacionCollection = new ObservableCollection<DetalleProveedor>(PagedImportacionTable.Importaciones);
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

            UpdateProducts(10);
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
            UpdateProducts(10);
            
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
                //var cliente = _context.Cliente.Add(Cliente);

                SqlParameter[] _params = {
                    new SqlParameter("@Nombre", Cliente.Nombre),
                    new SqlParameter("@Email", Cliente.Email),
                    new SqlParameter("@Domicilio", Cliente.Domicilio),
                    new SqlParameter("@Tipo_Pago", Cliente.Tipo_Pago),
                    new SqlParameter("@Compania", Cliente.Compania),
                    new SqlParameter("@Cedula", Cliente.Cedula),
                    new SqlParameter("@Fecha_Pago_1", Cliente.Fecha_Pago_1),
                    new SqlParameter("@Fecha_Pago_2", Cliente.Fecha_Pago_2),
                    new SqlParameter()
                    {
                        ParameterName = "@IdCliente",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    }
                };

                _context.Database.ExecuteSqlCommand("dbo.InsertClient @Nombre, @Email, @Domicilio, @Tipo_Pago, @Compania, @Cedula, @Fecha_Pago_1, @Fecha_Pago_2, @IdCliente OUT", _params);

                int ClienteId = (int)_params.Last().Value;

                foreach (var telefono in telefonos)
                {
                    //_context.Telefono.Add(telefono);
                    SqlParameter[] _telparams = {
                        new SqlParameter("@Id_Cliente", ClienteId),
                        new SqlParameter("@Numero", telefono.Numero),
                        new SqlParameter("@Tipo_Numero", telefono.Tipo_Numero),
                        new SqlParameter("@Empresa", telefono.Empresa)
                    };

                    _context.Database.ExecuteSqlCommand("dbo.InsertarTelefono @Id_Cliente, @Numero, @Tipo_Numero, @Empresa", _telparams);
                }

                _context.SaveChanges();

            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Error Al ingresar en la base de datos");
            }

            UpdateClients(10);
        }

        //Update cliente
        //public void UpdateCliente<T>(T item) where T : Cliente
        public void UpdateCliente(Cliente item)
        {
            /*var entity = _context.Cliente.Find(item.IdCliente);
            if (entity == null)
            {
                return;
            }

            _context.Entry(entity).CurrentValues.SetValues(item);
            _context.SaveChanges();*/
            try
            {
                SqlParameter[] cliente_params = {
                    new SqlParameter("@IdCliente", item.IdCliente),
                    new SqlParameter("@Nombre", item.Nombre),
                    new SqlParameter("@Email", item.Email),
                    new SqlParameter("@Domicilio", item.Domicilio),
                    new SqlParameter("@Tipo_Pago", item.Tipo_Pago),
                    new SqlParameter("@Compania", item.Compania),
                    new SqlParameter("@Cedula", item.Cedula),
                    new SqlParameter("@Fecha_Pago_1", item.Fecha_Pago_1),
                    new SqlParameter("@Fecha_Pago_2", item.Fecha_Pago_2)
                };

                //Do Cliente Update
                _context.Database.ExecuteSqlCommand("dbo.UpdateCliente @IdCliente, @Nombre, @Email, @Domicilio, @Tipo_Pago, @Compania, @Cedula, @Fecha_Pago_1, @Fecha_Pago_2", cliente_params);

                foreach(Telefono tel in item.Telefonos)
                {
                    SqlParameter[] telefono_params = {
                        new SqlParameter("@IdTelefono", tel.IdTelefono),
                        new SqlParameter("@Numero", tel.Numero),
                        new SqlParameter("@Tipo_Numero", tel.Tipo_Numero),
                        new SqlParameter("@Empresa", tel.Empresa)
                    };

                    _context.Database.ExecuteSqlCommand("dbo.UpdateTelefono @IdTelefono, @Numero, @Tipo_Numero, @Empresa", telefono_params);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine("Error Al ingresar en la base de datos");
            }

            UpdateClients(10);
        }

        // Botones de la paginacion de la tabla productos

        public void SearchClient(string FiltroClient)
        {
            if (FiltroClient != string.Empty)
            {
                SearchClientList = ClientesList.Where(c => (c.Nombre.ToLower().Contains(FiltroClient.ToLower())) || (c.Compania.ToLower().Contains(FiltroClient.ToLower())) || (c.Cedula.ToLower().Contains(FiltroClient.ToLower() ) ) ).ToList()  ;
                UpdateClients(10, SearchClientList);
            }
            else
            {
                SearchClientList = null;
                UpdateClients(10, ClientesList);
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
            float calculo = (float)count / 10;
            
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
        public void DeleteClient(Cliente cliente_)
        {

            //_context.Cliente.Remove(cliente_);
            _context.Database.ExecuteSqlCommand("dbo.DeleteCliente @IdCliente", new SqlParameter("@IdCliente", cliente_.IdCliente));

            //Eliminar del observable collection
            Clientes.Remove(cliente_);

            //Guardamos los cambios de la base de datos
            _context.SaveChanges();

            //Actualizamos el datagrid
            UpdateClients(10);
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
                SearchVentaList = VentasList.Where(v => (v.Cliente.Nombre.ToLower().Contains(FiltroVenta.ToLower()))).ToList();
                UpdateVentas(10, SearchVentaList);
            }
            else
            {
                SearchVentaList = null;
                UpdateVentas(10, VentasList);
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

            UpdateVentas(10);
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
            float calculo = (float)count / 10;

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
        public void DeleteVenta(Venta _venta)
        {
            _venta.Especificaciones_producto.Clear();
            _context.Venta.Remove(_venta);

            //Eliminar del observable collection
            Ventas.Remove(_venta);

            //Guardamos los cambios de la base de datos
            _context.SaveChanges();

            //Actualizamos el datagrid
            UpdateVentas(10);
        }

        /*
        * 
        *  Métodos de la Página Importación
        * 
       */

        public void ChangeEstatusPedido(Pedido pedido)
        {
            pedido.Estado_Pedido = "Completado";
            _context.SaveChanges();
        }

        //Agrega en la base de datos, el producto especificado
        public void AddImportacion(DetalleProveedor _Detalle, List<Producto_importado> producto_Importados)
        {
            _Detalle.Producto_Importados = producto_Importados;
            _context.DetalleProveedor.Add(_Detalle);
            
            _context.SaveChanges();

            UpdateImportacion(10);
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
            ImportacionList = _context.DetalleProveedor.ToList();

            //Paginacion
            PagedImportacionTable.SomeMethod(ImportacionList, NumberOfRecords);
            ImportacionCollection = new ObservableCollection<DetalleProveedor>(PagedImportacionTable.Importaciones);
            
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
            float calculo = (float)count / 10;

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
        public void DeleteImportacion(DetalleProveedor _import)
        {
            if (_import != null)
            {
                _context.DetalleProveedor.Remove(_import);
                _context.SaveChanges();
            }
            
            //Actualizamos el datagrid
            UpdateImportacion(10);
        }

        /*
       * 
       *  Métodos de la Página Pedidos
       * 
      */

        //Agrega en la base de datos, el Pedido especificado
        public void AddPedido(Pedido _Pedido)
        {
            try
            {
                _context.Pedido.Add(_Pedido);
                _context.SaveChanges();
                UpdatePedido(10);
            }
            catch
            {
                Console.WriteLine("Error al guardar en la base de datos");
            }
           
        }

        // Botones de la paginacion de la tabla productos
        public void NextPedido(int NumberOfRecords)
        {
            PagedPedidoTable.Next(PedidosList, NumberOfRecords);
            UpdatePedido(NumberOfRecords);
        }

        public void PreviousPedido(int NumberOfRecords)
        {
            PagedPedidoTable.Previous(PedidosList, NumberOfRecords);
            UpdatePedido(NumberOfRecords);
        }

        public void FirstPedido(int NumberOfRecords)
        {
            PagedPedidoTable.First(PedidosList, NumberOfRecords);
            UpdatePedido(NumberOfRecords);
        }

        public void LastPedido(int NumberOfRecords)
        {
            PagedPedidoTable.Last(PedidosList, NumberOfRecords);
            UpdatePedido(NumberOfRecords);
        }

        //Actualiza unicamente la tabla Ventas
        public void UpdatePedido(int NumberOfRecords, List<Pedido> SearchPedidoList = null)
        {
            //Si hay algo en la busqueda la mostrara
            if (SearchPedidoList != null)
            {
                PagedPedidoTable.SomeMethod(SearchPedidoList, NumberOfRecords);
                PedidosCollection = new ObservableCollection<Pedido>(PagedPedidoTable.Pedidos);

            }
            else
            {
                //Consulta
                PedidosList = _context.Pedido.ToList();

                //Paginacion
                PagedPedidoTable.SomeMethod(PedidosList, NumberOfRecords);
                PedidosCollection = new ObservableCollection<Pedido>(PagedPedidoTable.Pedidos);
            }
        }

        //Obtener la pagina actual ()
        public int PagePedidosNumber()
        {
            return PagedPedidoTable.PageIndex;
        }

        //Obtener el maximo numero de paginas ()
        public int PagePedidosNumberMax()
        {
            int count = PedidosList.Count;
            //Obtenemos el total de calculos
            float calculo = (float)count / 10;

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



        //Modulo de editar Pedido
        private void EditPedido(int id)
        {
            //var producto = _context.Producto.Find(id);

        }

        //Modulo de borrado de Pedido
        public void DeletePedido(Pedido pedido)
        {
            //Buscamos el pedido seleccionado y lo eliminamos de la base de datos

            _context.Pedido.Remove(pedido);

            //Eliminar del observable collection
            //Pedido.Remove(venta);

            //Guardamos los cambios de la base de datos
            _context.SaveChanges();

            //Actualizamos el datagrid
            UpdatePedido(10);
        }

        //Metodo de busqueda en la base de datos 
        public void SearchPedido(string filtro)
        {

            if (filtro != string.Empty)
            {

                SearchPedidoList = PedidosList.Where(s => s.cliente.Nombre.ToLower().Contains(filtro.ToLower() )).ToList();

                UpdatePedido(10,SearchPedidoList);
            }
            else
            {
                SearchPedidoList = null;
                UpdatePedido(10);
            }

        }

        /*
         * 
         * Metodos Proveedor
         *
        */

        public void AddProveedorProducto(Proveedor_producto proveedor)
        {
            _context.Proveedor_producto.Add(proveedor);
            _context.SaveChanges();
        }

        public void AddEspecificacionPedido (List<Especificacion_pedido> especificacions)
        {
            foreach (var i in especificacions)
            {
                _context.Especificacion_pedido.Add(i);
            }
            _context.SaveChanges();
        }

        public void AddDetalleProveedor (DetalleProveedor DetalleProveedor)
        {
            _context.DetalleProveedor.Add(DetalleProveedor);
            _context.SaveChanges();
        }

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

        public void SearchClienteFullList (string Filtro)
        {
            if (Filtro != string.Empty)
            {
                ClientesFullCollection = new ObservableCollection<Cliente>(_context.Cliente.Where(s => (s.Nombre.ToLower().Contains(Filtro.ToLower()))));
            }
            else
            {
                ClientesFullCollection = new ObservableCollection<Cliente>(_context.Cliente.ToList());
            }
        }


        //Busqueda en SelectProductWindow

        public void SearchProductoselect(string filtro)
        {
            if(filtro != string.Empty)
            {
                ProductosEspecificacionesCollection = new ObservableCollection<Especificacion_producto>(_context.Especificacion_producto.Where(s => ((s.Producto.Marca.ToLower().Contains(filtro.ToLower())) || (s.Producto.Modelo.ToLower().Contains(filtro.ToLower())) || (s.Descripcion.ToLower().Contains(filtro.ToLower()))) && (s.Venta == null)));
            }
            else
            {
                ProductosEspecificacionesCollection = new ObservableCollection<Especificacion_producto>(_context.Especificacion_producto.Where(t => (t.Venta ==null)).ToList());
            }
            
        }


        public void SearchImportacionList(string Filtro)
        {
            if(Filtro != string.Empty)
            {
                ImportacionCollection = new ObservableCollection<DetalleProveedor>(_context.DetalleProveedor.Where(s => (s.Numero_Seguimiento.ToLower().Contains(Filtro.ToLower()))));
            }
            else
            {
                ImportacionCollection = new ObservableCollection<DetalleProveedor>(_context.DetalleProveedor.ToList());
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
