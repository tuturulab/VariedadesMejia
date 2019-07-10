using Prism.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Variedades.Business;
using Variedades.Models;
using Variedades.ViewModels.Base;
using Variedades.Security;

namespace Variedades.ViewModels
{
    class LoginViewModel : BindableBase
    {
        private BusinessContext context;

        #region Actions

        /// <summary>
        /// Open the main Window action
        /// </summary>
        public Action OpenMainWindow { get; set; }

        /// <summary>
        /// Close this window action
        /// </summary>
        public Action CloseThis { get; set; }

        #endregion

        public string Username { get; set; }
        public string Password { get; set; }

        //This actually works
        private bool _LoginIsRunning;
        public bool LoginIsRunning
        {
            get { return _LoginIsRunning; }
            set { SetProperty(ref _LoginIsRunning, value); }
        }

        #region Commands

        /// <summary>
        /// Command to to log in
        /// </summary>
        public DelegateCommand<object> LoginCommand { get; set; }


        #endregion

        public LoginViewModel()
        {
            //Instance user repo
            context = new BusinessContext();

            //set commands handlers methods
            LoginCommand = new DelegateCommand<object>(async (parameter) => await Login(parameter));
        }

        public async Task Login(object parameter)
        {
            //The login command is running
            LoginIsRunning = true;

            await Task.Delay(1000);

            //Do login here
            User x = await context.GetUserGivenPasswordAndUsername(
                Username,
                (parameter as IHavePassword).Password.Unsecure());

            if (x == null)
                throw new Exception();
            else
            {
                LoginIsRunning = false;
                OpenMainWindow?.Invoke();
            }

        }
    }
}
