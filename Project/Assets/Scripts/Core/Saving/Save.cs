using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Save
{
    public SaveLocationTypes LocationType;

    public string SavePath
    {
        get { return _savePath; }
        set
        {
            if (Directory.Exists(value))
                _savePath = value;
        }
    }
    private string _savePath;

    public Save(SaveData saveData)
    {
        switch (LocationType)
        {
            case SaveLocationTypes.Appdata:
                SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.productName, "Saves");
                break;
            case SaveLocationTypes.Documents:
                SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.productName, "Saves");
                break;
            case SaveLocationTypes.GameLocation:
                SavePath = Path.Combine(Application.dataPath, "Saves");
                break;
        }
    }
}
