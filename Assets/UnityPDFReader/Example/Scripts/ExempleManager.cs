using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.IO;
using System.Linq;

[RequireComponent(typeof(PDFManager))]
public class ExempleManager : MonoBehaviour {

    private PDFManager pdfManager;

    public GameObject scrollViewContent;
    public GameObject pdfItemObjectPrefab;

    void Awake()
    {
        pdfManager = GetComponent<PDFManager>();
    }

    public void LoadPDF()
    {
        string folder = Application.dataPath + "/" + pdfManager.folderSaveImages;

        DirectoryInfo dir = new DirectoryInfo(folder);
        List<FileInfo> info = dir.GetFiles("*.jpg").Where(file => Regex.IsMatch(file.Name, "^[0-9]+")).ToList();

        foreach (FileInfo f in info)
        {
            RectTransform rt = scrollViewContent.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + pdfManager.height);

            GameObject pdfItem = Instantiate(pdfItemObjectPrefab) as GameObject;
            Texture2D tmpTexture = LoadImageFromFile(f.Directory + "\\" + f.Name);

            RawImage tmpItem = pdfItem.GetComponent<RawImage>() as RawImage;
            tmpItem.texture = tmpTexture;

            pdfItem.transform.parent = scrollViewContent.transform;
            pdfItem.transform.localScale = Vector3.one;

            Debug.Log("Add new object: " + f.Name);
            Debug.Log("full path: " + f.Directory + "\\" + f.Name);
        }
    }

    public static Texture2D LoadImageFromFile(string filePath)
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
