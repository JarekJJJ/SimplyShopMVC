﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using SimplyShopMVC.Domain.Model;
using System.Net.Http;
using System.Security.Authentication;

namespace SimplyShopMVC.Application.Helpers
{
    public static class ImageHelper
    {
        public static int SaveImageFromUrl(List<string> imageUrl, string folderName, IWebHostEnvironment webHost)
        {
            int result = 0;
            if (imageUrl != null)
            {



                try
                {
                    string newFolderPath = Path.Combine(webHost.WebRootPath, "media\\itemimg", folderName);
                    try
                    {
                        Directory.CreateDirectory(newFolderPath);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    using (WebClient client = new WebClient())
                    {
                        foreach (string imageFromList in imageUrl)
                        {
                            try
                            {
                                if(!String.IsNullOrEmpty(imageFromList) && imageFromList.Length > 10)
                                {                                  
                                        byte[] imageData = client.DownloadData(imageFromList);
                                        string fileName = Guid.NewGuid().ToString() + ".jpg";
                                        string filePath = System.IO.Path.Combine(newFolderPath, fileName);
                                        if (imageData.Length > 2048)
                                        {
                                            System.IO.File.WriteAllBytes(filePath, imageData);
                                            result++;
                                        }                                                                                            
                                }
                              
                               
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                      
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return result;
        }
        public static async Task<int> SaveImageFromUrlAsync(List<string> imageUrl, string folderName, IWebHostEnvironment webHost)
        {
            int result = 0;
            if (imageUrl != null)
            {



                try
                {
                    string newFolderPath = Path.Combine(webHost.WebRootPath, "media\\itemimg", folderName);
                    try
                    {
                        Directory.CreateDirectory(newFolderPath);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    using (WebClient client = new WebClient())
                    {
                        foreach (string imageFromList in imageUrl)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(imageFromList) && imageFromList.Length > 10)
                                {
                                    byte[] imageData = client.DownloadData(imageFromList);
                                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                                    string filePath = System.IO.Path.Combine(newFolderPath, fileName);
                                    if (imageData.Length > 2048)
                                    {
                                       await System.IO.File.WriteAllBytesAsync(filePath, imageData);
                                        result++;
                                    }
                                }


                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return result;
        }

        public static int SaveOrinkImageFromUrl(List<string> imageUrl, string folderName, IWebHostEnvironment webHost, IHttpClientFactory httpClientFactory)
        {


            int result = 0;
            if (imageUrl != null)
            {
                try
                {
                    string newFolderPath = Path.Combine(webHost.WebRootPath, "media\\itemimg", folderName);
                    Directory.CreateDirectory(newFolderPath);

                
                    var clientHandler = new HttpClientHandler();
                    clientHandler.SslProtocols = SslProtocols.Tls13; // Ustawienie protokołu TLS na TLS 1.3

                    var client = new HttpClient(clientHandler);

                    foreach (string imageFromList in imageUrl)
                    {
                        try
                        {
                          //string newUrl =  imageFromList.Replace("https", "http");
                            if (imageFromList != null)
                            {
                                HttpResponseMessage response = client.GetAsync(imageFromList).Result;
                                response.EnsureSuccessStatusCode();

                                byte[] imageData = response.Content.ReadAsByteArrayAsync().Result;
                                string fileName = Guid.NewGuid().ToString() + ".jpg";
                                string filePath = Path.Combine(newFolderPath, fileName);                             
                                    File.WriteAllBytes(filePath, imageData);
                                    result++;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }

        public static List<string> AllImageFromPath(string path) // pobiera nazwy plików
        {
            string folderPath = path;
            List<string> listFile = new List<string>();
            if (Directory.Exists(folderPath))
            {
                List<string> imagePaths = Directory.GetFiles(folderPath)
          .Where(file => IsImageFile(file)).ToList();
                listFile = imagePaths.Select(f => Path.GetFileName(f)).ToList();
            }       
            return listFile;
        }
        public static List<string> AllImageUrlFromPath(string path) // pobiera pliki wraz ze ścieżką
        {
            string folderPath = path;
            List<string> imagePaths = Directory.GetFiles(folderPath)
                .Where(file => IsImageFile(file)).ToList();
            //var listFile = imagePaths.Select(f => Path.GetFileName(f)).ToList();
            return imagePaths;
        }
        public static string GetMainImageUrlFromPath(string folderName, IWebHostEnvironment webHost) // pobiera pliki wraz ze ścieżką
        {
            string folderPath = $"{webHost.WebRootPath}\\media\\itemimg\\{folderName}\\";
                string imagePaths = Directory.GetFiles(folderPath)
                .FirstOrDefault(file => IsImageFile(file));
            var listFile = Path.GetFileName(imagePaths);
            imagePaths = $"\\media\\itemimg\\{folderName}\\{listFile}";
            if (string.IsNullOrEmpty(listFile))
            {
                folderPath = $"\\media\\nophoto.jpg";
                imagePaths = folderPath;
            }
            return imagePaths;
        }
        static bool IsImageFile(string filePath)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }; // Rozszerzenia plików graficznych

            string extension = Path.GetExtension(filePath);

            return validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
        public static bool DeleteImage(string fileName, string path)
        {
            bool result = false;
            string filePath = Path.Combine(path, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                result = true;
            }
            else
            {
                try
                {
                   filePath = Path.Combine(path);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        result = true;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }
    }
}
