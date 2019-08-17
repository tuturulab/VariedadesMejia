using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Variedades.Services;

namespace Variedades.Views
{
    /// <summary>
    /// Interaction logic for PageBackup.xaml
    /// </summary>
    public partial class PageBackup : Page
    {
        PageViewModel _model;
        long fileSize;
        static string[] scopes = { DriveService.Scope.Drive,
                           DriveService.Scope.DriveFile };

        public PageBackup(PageViewModel model)
        {
            InitializeComponent();
            _model = model;
        }

        private void LocalBackup(object sender, RoutedEventArgs e)
        {
            //BackupService backupService = new BackupService(_model);
            //backupService.DoBackupLocal();
            bool result = _model.DoBackupToFile();
            if (result) MessageBox.Show("Respaldo realizado");
            Process.Start(@"C:\Backup");
        }

        private async void CloudBackup(object sender, RoutedEventArgs e)
        {
            //Show progress indicator
            ProgressIndicator.Visibility = Visibility.Visible;
            await UploadBackupToDriveAsync();


            /*var req = backupService.UploadBackupToDriveAsync();
            req.ProgressChanged += UploadProgressEvent;
            req.UploadAsync();*/
        }

        public async Task UploadBackupToDriveAsync()
        {
            UserCredential credential;
            string credentialsFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./credentials.json");

            using (var stream = new FileStream(credentialsFilePath, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/token.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)
                    ).Result;
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MejiaDrive",
            });

            //File info
            var FileMetadata = new Google.Apis.Drive.v3.Data.File();
            FileMetadata.Name = "MejiaBackup";
            FileMetadata.MimeType = "application/octet-stream";

            //Request
            FilesResource.CreateMediaUpload request;
            string fileToUploadPath = @"D:\Backup\DbMejiaBackup.bak";

            //Take fileStream and Execute request
            using (var stream = new FileStream(fileToUploadPath, FileMode.Open))
            {
                fileSize = stream.Length;
                request = service.Files.Create(FileMetadata, stream, "application/octet-stream");
                request.Fields = "id";

                request.ProgressChanged += UploadProgressEvent;
                //await request.UploadAsync();
                //Google.Apis.Upload.IUploadProgress x = await request.UploadAsync();

                await request.UploadAsync();
            }
        }

        private void UploadProgressEvent(Google.Apis.Upload.IUploadProgress uploadProgress)
        {
            switch (uploadProgress.Status)
            {
                case Google.Apis.Upload.UploadStatus.Uploading:
                    Dispatcher.Invoke(() =>
                    {
                        var value = ((uploadProgress.BytesSent * 100) / fileSize);
                        ProgressIndicator.Value = value;
                        Debug.WriteLine(value);
                    });              
                    break;

                case Google.Apis.Upload.UploadStatus.Completed:
                    Dispatcher.Invoke(() =>
                    {
                        ProgressIndicator.Visibility = Visibility.Hidden;
                        ProgressIndicator.Value = 0;
                        MessageBox.Show("Respaldo completado!");
                    });                   
                    break;

                default: break;
            }
        }
    }
}
