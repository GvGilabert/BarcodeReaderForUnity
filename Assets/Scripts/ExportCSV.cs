using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportCSV : MonoBehaviour
{
    public static string GetDownloadFolder()
    {
        string[] temp = (Application.persistentDataPath.Replace("Android", "")).Split(new string[] { "//" }, System.StringSplitOptions.None);

        return (temp[0] + "/Download");
    }

    public void Export()
    {
        string date = DateTime.Now.ToString().Replace("/","").Replace(" ", "").Replace(":","");

#if UNITY_ANDROID && !UNITY_EDITOR
        string filePath = GetDownloadFolder() + "/" + date + ".csv";
        print("algo");
#else
        string filePath = Application.persistentDataPath + "/" + date + ".csv";
#endif



        GetComponent<PopUpMsg>().NewMsg("path: " + filePath);

        List<TestModel> items = SqLiteDBManager.instance.GetCodes(0);
        using (TextWriter tw = new StreamWriter(filePath))
        {
            foreach (var item in items)
            {
                tw.WriteLine(item.ToString());
            }
            tw.Close();
        }
    }
}
