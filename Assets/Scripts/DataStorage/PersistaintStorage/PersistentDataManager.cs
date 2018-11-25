using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class PersistentDataManager : MonoBehaviour
{
    public static PersistentDataManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<PersistentDataManager>();
            if (instance != null)
                return instance;

            Create();
            return instance;
        }
    }

    protected static PersistentDataManager instance;
    protected static bool quitting;

    public static PersistentDataManager Create()
    {
        GameObject dataManagerGameObject = new GameObject("PersistentDataManager");
        DontDestroyOnLoad(dataManagerGameObject);
        instance = dataManagerGameObject.AddComponent<PersistentDataManager>();
        return instance;
    }

    protected HashSet<IDataPersister> m_DataPersisters = new HashSet<IDataPersister>();
    [ShowInInspector]
    protected Dictionary<string, Data> m_Store = new Dictionary<string, Data>();

    event System.Action schedule = null;

    void Update()
    {
        DoSheduled();
    }

    public void DoSheduled()
    {
        if (schedule != null)
        {
            schedule();
            schedule = null;
        }
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            SceneManager.sceneLoaded += delegate { StartCoroutine(SheduleAtEndOfFFrame()); };
        }
            

        
    }

    IEnumerator SheduleAtEndOfFFrame()
    {
        yield return new WaitForEndOfFrame();
        LoadAllData();
    }

    void OnDestroy()
    {
        if (instance == this)
            quitting = true;
    }

    public static T GetData<T>(string key) where T : Data
    {
        if(instance.m_Store.ContainsKey(key))
            return (T)instance.m_Store[key];

        return null;
    }

    /// <summary>
    /// Registers a <see cref="IDataPersister"/> to the <see cref="PersistentDataManager"/> singleton.
    /// </summary>
    /// <param name="persister">object to persist.</param>
    public static void RegisterPersister(IDataPersister persister)
    {
        var ds = persister.GetDataSettings();
        if (!string.IsNullOrEmpty(ds.dataTag))
        {
            Instance.Register(persister);
        }
    }

    /// <summary>
    /// Unregisters a <see cref="IDataPersister"/> from the <see cref="PersistentDataManager"/> singleton.
    /// </summary>
    /// <param name="persister">object to unregister.</param>
    public static void UnregisterPersister(IDataPersister persister)
    {
        if (!quitting)
        {
            Instance.Unregister(persister);
        }
    }

    /// <summary>
    /// Loads all <see cref="IDataPersister"/>s in the scene into the <see cref="PersistentDataManager"/> singleton.
    /// </summary>
    [Button("Write Objects to Storage")]
    public static void SaveAllData()
    {
        Instance.SaveAllDataInternal();
    }

    /// <summary>
    /// Writes all <see cref="IDataPersister"/>s saved  the <see cref="PersistentDataManager"/> singleton to the scene.
    /// </summary>
    [Button("Load Objects from Storage")]
    public static void LoadAllData()
    {
        Instance.LoadAllDataInternal();
    }

    /// <summary>
    /// Unregister all <see cref="IDataPersister"/> currently registered with the <see cref="PersistentDataManager"/> singleton.
    /// </summary>
    public static void ClearPersisters()
    {
        Instance.m_DataPersisters.Clear();
    }

    /// <summary>
    /// saves a single <see cref="IDataPersister"/> to the <see cref="PersistentDataManager"/> singleton.
    /// </summary>
    /// <param name="dp"></param>
    public static void SetDirty(IDataPersister dp)
    {
        Instance.Save(dp);
    }

    /// <summary>
    /// loads data from the current <see cref="IDataPersister"/>s in the scene into the <see cref="PersistentDataManager"/> singleton, then saves that data to a file.
    /// </summary>
    ///<param name="saveName">the name of the save file. Defaults to "SaveData"</param>
    [Button]
    public static void SaveExternal(string saveName = "SaveData")
    {
        string fileName = $"{saveName}.bin";

        instance.SaveAllDataInternal();
        Debug.Log("Writing persisient data to File.");
        FileStream stream = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.OpenOrCreate);
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, instance.m_Store);
        stream.Close();
    }

    /// <summary>
    /// Load a save file by Name, then load data into the current <see cref="IDataPersister"/>s in the scene.
    /// </summary>
    ///<param name="saveName">the name of the save file. Defaults to "SaveData"</param>
    [Button]
    public static void LoadExternal(string saveName = "SaveData")
    {
        string fileName = $"{saveName}.bin";

        FileStream stream = File.OpenRead(Application.persistentDataPath + "/" + fileName);
        var formatter = new BinaryFormatter();
        instance.m_Store = (Dictionary<string, Data>)formatter.Deserialize(stream);
        stream.Close();
        instance.DoSheduled();
        instance.LoadAllDataInternal();
        instance.DoSheduled();

    }


    protected void Register(IDataPersister persister)
    {
        schedule += () =>
        {
            m_DataPersisters.Add(persister);
        };
    }

    protected void Unregister(IDataPersister persister)
    {
        schedule += () =>
        {
            m_DataPersisters.Remove(persister);
        };
        
    }

    //Writes a single IDataPersister into this object.
    protected void Save(IDataPersister dp)
    {
        var dataSettings = dp.GetDataSettings();
        if (dataSettings.persistenceType == DataSettings.PersistenceType.ReadOnly || dataSettings.persistenceType == DataSettings.PersistenceType.DoNotPersist)
            return;
        if (!string.IsNullOrEmpty(dataSettings.dataTag))
        {
            m_Store[dataSettings.dataTag] = dp.SaveData();
        }
    }

    //Saves all IDataPersisters in the current scene into this object.
    protected void SaveAllDataInternal()
    {
        Debug.Log("Save scene Objects into storage");
        foreach (var dp in m_DataPersisters)
        {
            Save(dp);
        }
    }

    //Loads all the data saved in this object INTO the IDataPersisters in the Scene.
    protected void LoadAllDataInternal()
    {
        schedule += () =>
        {
            Debug.Log("Loading Objects into Scene");
            foreach (var dp in m_DataPersisters)
            {
                var dataSettings = dp.GetDataSettings();
                if (dataSettings.persistenceType == DataSettings.PersistenceType.WriteOnly || dataSettings.persistenceType == DataSettings.PersistenceType.DoNotPersist)
                    continue;
                if (!string.IsNullOrEmpty(dataSettings.dataTag))
                {
                    if (m_Store.ContainsKey(dataSettings.dataTag))
                    {
                        dp.LoadData(m_Store[dataSettings.dataTag]);
                    }
                }
            }
        };
    }



    #if UNITY_EDITOR
    [Button("Show In Explorer")]
    void Show()
    {
        string SavePath = $"{Application.persistentDataPath}/";
        UnityEditor.EditorUtility.RevealInFinder(SavePath);
    }
    #endif
}
