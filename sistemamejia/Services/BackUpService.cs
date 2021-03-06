﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Variedades.Services
{
    public class BackupService
    {
        static string[] scopes = { DriveService.Scope.Drive,
                           DriveService.Scope.DriveFile };

        public long fileSize;
        public Google.Apis.Upload.IUploadProgress uploadTask;
        private PageViewModel _Model;
        public IProgress<double> progress;

        public BackupService(PageViewModel Vm, IProgress<double> pss)
        {
            _Model = Vm;
            progress = pss;
            //_connectionString = "Data Source=DESKTOP-SEBNLEB\\SQLEXPRESS;Initial Catalog=DbMejia;User ID=administrator;Password=Lagunarr98;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //_backupFolderFullPath = "C:\\Users\\Lagunez\\Desktop\\Backup";
        }

        public void DoBackupLocal()
        {
            //string filePath = BuildBackupPathWithFilename(databaseName);
            bool backupDone = _Model.DoBackupToFile();

            if (backupDone)
            {
                MessageBox.Show("Respaldo hecho");
            }
        }

        public async void UploadBackupToDriveAsync()
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

                request.ProgressChanged += ProgressHandler;
                //await request.UploadAsync();
                //Google.Apis.Upload.IUploadProgress x = await request.UploadAsync();

                await request.UploadAsync();
            }
        }

        private void ProgressHandler(Google.Apis.Upload.IUploadProgress uploadProgress)
        {
            if(uploadProgress.Status == Google.Apis.Upload.UploadStatus.Uploading)
            {
                progress.Report((double)(uploadProgress.BytesSent / fileSize) * 100);
            } else if(uploadProgress.Status == Google.Apis.Upload.UploadStatus.Completed)
            {
                progress.Report(200);
            }
        }
    }
}
