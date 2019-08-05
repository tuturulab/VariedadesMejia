using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variedades.CefConfig
{
    public class CustomProtocolSchemeHandler : ResourceHandler
    {
        // Specifies where you bundled app resides.
        // Basically path to your index.html
        private string frontendFolderPath;

        public CustomProtocolSchemeHandler()
        {
            frontendFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./bundle/");
            //Debug.WriteLine(frontendFolderPath);
        }

        // Process request and craft response.
        public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            var requestedFilePath = frontendFolderPath + fileName;

            var isAccesToFilePermitted = IsRequestedPathInsideFolder(
                new DirectoryInfo(requestedFilePath),
                new DirectoryInfo(frontendFolderPath));

            if (isAccesToFilePermitted && File.Exists(requestedFilePath))
            {
                byte[] bytes = File.ReadAllBytes(requestedFilePath);
                Stream = new MemoryStream(bytes);

                var fileExtension = Path.GetExtension(fileName);
                MimeType = GetMimeType(fileExtension);

                callback.Continue();
                return true;
            }

            callback.Dispose();
            return false;
        }

        // Added for security reasons.
        // In this code it is used to verify that requested file is descendant to your index.html.
        public bool IsRequestedPathInsideFolder(DirectoryInfo path, DirectoryInfo folder)
        {
            if (path.Parent == null)
            {
                return false;
            }

            if (string.Equals(path.Parent.FullName, folder.FullName, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return IsRequestedPathInsideFolder(path.Parent, folder);
        }
    }
}
