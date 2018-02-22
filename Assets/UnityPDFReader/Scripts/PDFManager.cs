using UnityEngine;
using System.IO;
using ConvertPDF;

public class PDFManager : MonoBehaviour
{
    public string pathPDF = "UnityPDFReader/Example/SimplePDF.pdf";
    public string folderSaveImages = "UnityPDFReader/PDF";

    public int firstPage = 1;
    public int lastPage = 2;

    public int width = 600;
    public int height = 800;

    public void SavePDF()
    {

        PDFConvert converter = new PDFConvert();

        string folder = Application.dataPath + "/" + folderSaveImages;
        Debug.Log("Images Folder : " + folder);

        if (Directory.Exists(folder))
            Directory.Delete(folder, true);

        Directory.CreateDirectory(folder);

        string pdf = Application.dataPath + "/" + pathPDF;
        Debug.Log("PDF : " + pdf);

        converter.Convert(pdf, folder + "\\%01d.jpg", firstPage, lastPage, "jpeg", width, height);
    }
}

