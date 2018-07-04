using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public delegate void OnSaveGame();
    public OnSaveGame onSaveGame;

    public delegate void OnLoadGame();
    public OnSaveGame onLoadGame;

    public VariableStorage savedVariables;
    public VariableStorage tempVariables;

    private void OnEnable()
    {
        Initialize(this);
    }


    public void Save()
    {
        onSaveGame?.Invoke();
        savedVariables.Save();
    }

    public void Load()
    {
        savedVariables.Load();
        onSaveGame?.Invoke();
    }
}
