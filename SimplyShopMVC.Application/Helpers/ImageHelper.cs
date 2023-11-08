using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;

namespace SimplyShopMVC.Application.Helpers
{
    public static class ImageHelper
    {
        public static int SaveImageFromUrl(List<string> imageUrl, string folderName, IWebHostEnvironment webHost)
        {
            int result = 0;
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
                        byte[] imageData = client.DownloadData(imageFromList);
                        string fileName = Guid.NewGuid().ToString() + ".jpg";
                        string filePath = System.IO.Path.Combine(newFolderPath, fileName);
                        System.IO.File.WriteAllBytes(filePath, imageData);
                        result++;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        
        public static List<string> AllImageFromPath(string path) // pobiera nazwy plików
        {
            string folderPath = path;
            List<string> imagePaths = Directory.GetFiles(folderPath)
                .Where(file => IsImageFile(file)).ToList();
            var listFile = imagePaths.Select(f=>Path.GetFileName(f)).ToList();
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

            return result;
        } 
    }
}
