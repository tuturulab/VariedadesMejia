using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variedades.CefConfig
{
    public class CustomProtocolSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "customFileProtocol";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new CustomProtocolSchemeHandler();
        }
    }
}
