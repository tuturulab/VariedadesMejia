using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Variedades.DataModels;

namespace Variedades.ViewModels
{
    class WindowViewModel : BindableBase
    {
        /// <summary>
        /// Current ApplicationPage
        /// </summary>
        private ApplicationPage _CurrentPage;
        public ApplicationPage CurrentPage
        {
            get { return _CurrentPage; }
            set { SetProperty(ref _CurrentPage, value); }
        }


        #region Commands

        /// <summary>
        /// Command to handle Selection Changes on Main Module's viewList
        /// </summary>
        public DelegateCommand<object> SelectedItemChangedModulesCommand { get; set; }

        #endregion

        public WindowViewModel()
        {
            //Set the default page
            CurrentPage = ApplicationPage.Home;

            //Set commands handlers
            SelectedItemChangedModulesCommand = new DelegateCommand<object>(OnItemMainModuleChanged);
        }

        /// <summary>
        /// Command to handle Main Modules viewlist selection changes
        /// </summary>
        /// <param name="selectedItem">The selected Item</param>
        public void OnItemMainModuleChanged(object selectedItem)
        {

            //Default page  
            ApplicationPage nextPage = ApplicationPage.Home;

            //Get next page
            switch (((ListViewItem)((ListView)selectedItem).SelectedItem).Name)
            {
                case "ItemClients":
                    nextPage = ApplicationPage.Home;
                    break;

                case "ItemProducts":
                    nextPage = ApplicationPage.ProductosPage;
                    break;

                case "ItemSales":
                    nextPage = ApplicationPage.Home;
                    break;

                case "ItemImports":
                    nextPage = ApplicationPage.Home;
                    break;

                case "ItemPedidos":
                    nextPage = ApplicationPage.Home;
                    break;

                case "ItemStats":
                    nextPage = ApplicationPage.Home;
                    break;

                default:
                    break;
            }

            //Set the next Page
            CurrentPage = nextPage;
        }
    }
}
