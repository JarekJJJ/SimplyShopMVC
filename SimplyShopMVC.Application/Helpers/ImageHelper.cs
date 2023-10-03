﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Helpers
{
    public static class ImageHelper
    {
        
        public static List<string> AllImageFromPath(string path)
        {
            string folderPath = path;
            List<string> imagePaths = Directory.GetFiles(folderPath)
                .Where(file => IsImageFile(file)).ToList();
            var listFile = imagePaths.Select(f=>Path.GetFileName(f)).ToList();
            return listFile;
        }
        static bool IsImageFile(string filePath)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }; // Rozszerzenia plików graficznych

            string extension = Path.GetExtension(filePath);

            return validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }
}