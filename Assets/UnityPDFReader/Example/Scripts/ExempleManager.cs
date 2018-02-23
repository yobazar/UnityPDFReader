using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Threading;
using System.IO;
using System.Linq;

public class ExempleManager : MonoBehaviour {

    public int firstPage = 1;
    public int lastPage = 5;

    public int width = 595;
    public int height = 842;

    public string pathPDF = "UnityPDFReader/Example/SimplePDF.pdf";
    public string pathSave = "UnityPDFReader/PDF";

    public GameObject scrollViewContent;
    public GameObject pdfItemObjectPrefab;

    private bool isLoading = false;
    private bool isThreadEnd = false;

    private void Awake()
    {
        pathPDF = Application.dataPath + "/" + pathPDF;
        pathSave = Application.dataPath + "/" + pathSave;
    }

    public void LoadPDF()
    {
        if (!isLoading)
        {
            isLoading = true;

            Thread thread = new Thread(() =>
            {
                PDFManager.ConvertPDF(pathPDF, pathSave, firstPage, lastPage, width, height);
                isThreadEnd = true;
            });

            thread.Start();

            StartCoroutine(EndLoadPDF());
        }
    }

    IEnumerator EndLoadPDF()
    {
        yield return new WaitUntil(() => isThreadEnd);
        isThreadEnd = false;

        isLoading = false;
        
        Texture2D[] pages = PDFManager.GetAllPDFPages(pathSave);

        for (int i=0; i<pages.Length ; i++)
        {
            GameObject pdfItem = Instantiate(pdfItemObjectPrefab) as GameObject;

            RawImage tmpItem = pdfItem.GetComponent<RawImage>() as RawImage;
            tmpItem.texture = pages[i];

            pdfItem.transform.parent = scrollViewContent.transform;
            pdfItem.transform.localScale = Vector3.one;

            Debug.Log("Add new object: " + i);
        }
    }
}
