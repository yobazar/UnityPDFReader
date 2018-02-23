using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using ConvertPDF;

public static class PDFManager 
{
    public static void ConvertPDF(string pathPDF, string pathSave, int firstPage, int lastPage, int width, int height)
    {
        Debug.Log("PDF File : " + pathPDF);        
        Debug.Log("Save Folder : " + pathSave);

        if (Directory.Exists(pathSave))
            Directory.Delete(pathSave, true);

        Directory.CreateDirectory(pathSave);

        PDFConvert converter = new PDFConvert();
        converter.Convert(pathPDF, pathSave + "\\%01d.jpg", firstPage, lastPage, "jpeg", width, height);
    }

    public static Texture2D[] GetAllPDFPages(string pathSave)
    {
        DirectoryInfo dir = new DirectoryInfo(pathSave);
        List<FileInfo> info = dir.GetFiles("*.jpg").Where(file => Regex.IsMatch(file.Name, "^[0-9]+")).ToList();

        if (info.Count < 1)
            return null;

        Texture2D[] pdfPages = new Texture2D[info.Count];
        for (int i=0; i<pdfPages.Length; i++)
        {
            pdfPages[i] = LoadImageFromFile(info[i].Directory + "\\" + info[i].Name);
        }

        return pdfPages;
    }

    private static Texture2D LoadImageFromFile(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}

