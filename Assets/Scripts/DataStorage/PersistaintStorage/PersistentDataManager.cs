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
        if (schedule != null)
        {
            schedule();
            schedule = null;
        }
    }

    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);

        SceneManager.sceneLoaded += OnSceneLoad;
    }
    protected void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Writing To persistent Objects");
        LoadAllData();
    }

    void OnDestroy()
    {
        if (instance == this)
            quitting = true;
    }

    public static void RegisterPersister(IDataPersister persister)
    {
        var ds = persister.GetDataSettings();
        if (!string.IsNullOrEmpty(ds.dataTag))
        {
            Instance.Register(persister);
        }
    }

    public static void UnregisterPersister(IDataPersister persister)
    {
        if (!quitting)
        {
            Instance.Unregister(persister);
        }
    }

    [Button("Write Objects to Storage")]
    public static void SaveAllData()
    {
        Instance.SaveAllDataInternal();
    }

    [Button("Load Objects from Storage")]
    public static void LoadAllData()
    {
        Instance.LoadAllDataInternal();
    }

    public static void ClearPersisters()
    {
        Instance.m_DataPersisters.Clear();
    }

    public static void SetDirty(IDataPersister dp)
    {
        Instance.Save(dp);
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
        schedule += () => m_DataPersisters.Remove(persister);
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

    //Saves all IDataPeresisters in the current scene into this object.
    protected void SaveAllDataInternal()
    {
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

    bool IsGameRunning()
    {
        return Application.isPlaying;
    }

    [Button]
    public static void SaveExternal(string saveName)
    {
        string fileName = $"{saveName}.bin";

        instance.SaveAllDataInternal();
        FileStream stream = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.OpenOrCreate);
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, instance.m_Store);
        stream.Close();

        // Restore from file
        

    }
    [Button]
    public static void LoadExternal(string saveName)
    {
        string fileName = $"{saveName}.bin";

        FileStream stream = File.OpenRead(Application.persistentDataPath + "/" + fileName);
        var formatter = new BinaryFormatter();
        instance.m_Store = (Dictionary<string, Data>)formatter.Deserialize(stream);
        stream.Close();
        instance.LoadAllDataInternal();
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
