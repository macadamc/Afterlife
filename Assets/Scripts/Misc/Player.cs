using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;
using UnityEngine.SceneManagement;
using SeawispHunter.MinibufferConsole;
using SeawispHunter.MinibufferConsole.Extensions;

public class Player : Singleton<Player> {

    public PersistentVariableStorage pvs;
    Health health;
    private void Awake()
    {
        health = GetComponent<Health>();
        pvs = GetComponent<PersistentVariableStorage>();
        Initialize(this);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        health.onHealthChanged += PlayerHealthChanged;
    }

    private void Start()
    {
        Minibuffer.Register(this);
        Minibuffer.With((Minibuffer miniBuffer) =>
        {
            miniBuffer.completers["ItemCompleter"] = new ItemCompleter().ToEntity();

            miniBuffer.RegisterVariable(new Variable("healthCurrent"), () => { return health.currentHealth.Value; }, (int val) => { health.currentHealth.Value = val; });
            miniBuffer.RegisterVariable(new Variable("healthMax"), () => { return health.maxHealth.Value; }, (int val) => { health.maxHealth.Value = val; });

            foreach (var kvPair in pvs.storage.strings)
            {
                miniBuffer.RegisterVariable(
                    new Variable(kvPair.Key),
                    () => { return pvs.storage.GetString(kvPair.Key); },
                    (string val) => { pvs.storage.SetValue(kvPair.Key, val); }
                    );
            }
            foreach (var kvPair in pvs.storage.floats)
            {
                miniBuffer.RegisterVariable(
                    new Variable(kvPair.Key),
                    () => { return pvs.storage.GetFloat(kvPair.Key); },
                    (float val) => { pvs.storage.SetValue(kvPair.Key, val); }
                    );
            }
            foreach (var kvPair in pvs.storage.bools)
            {
                miniBuffer.RegisterVariable(
                    new Variable(kvPair.Key),
                    () => { return pvs.storage.GetBool(kvPair.Key); },
                    (bool val) => { pvs.storage.SetValue(kvPair.Key, val); }
                    );
            }
        });


        pvs.storage.OnAdd += OnAdd;
        pvs.storage.OnRemove += OnRemove;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        health.onHealthChanged -= PlayerHealthChanged;

        Minibuffer.With((Minibuffer mb) =>
        {
            mb.UnregisterVariable("healthCurrent");
            mb.UnregisterVariable("healthMax");

            foreach (var kvPair in pvs.storage.strings)
            {
                mb.UnregisterVariable(kvPair.Key);
            }
            foreach (var kvPair in pvs.storage.floats)
            {
                mb.UnregisterVariable(kvPair.Key);
            }
            foreach (var kvPair in pvs.storage.bools)
            {
                mb.UnregisterVariable(kvPair.Key);
            }
        });
        Minibuffer.Unregister(this);

        pvs.storage.OnAdd -= OnAdd;
        pvs.storage.OnRemove -= OnRemove;
    }

    void PlayerHealthChanged(int change)
    {
        if(change < 0)
        {
            CameraShake.Instance.SmallShake();
            PauseManager.Instance.StartCoroutine(PauseManager.Instance.FreezeFrame(0.2f,0.3f));
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("Scene Loaded, trying to teleport");
        Teleport[] teleporters = FindObjectsOfType<Teleport>();
        foreach(Teleport t in teleporters)
        {
            if (pvs.storage.strings.ContainsKey("doorTag"))
            {
                if(pvs.storage.strings["doorTag"] == t.doorTag)
                {
                    Debug.Log("Found doortag");
                    transform.position = t.enterTransform.position;
                    //CameraFollow.Instance.SetPosition(t.TeleportTransform.position, t.cameraZone);
                    return;
                }
                else
                {
                    Debug.Log("Could not find corrent door tag in scene.");
                }
            }
            else
            {
                Debug.Log("No Door Tag in Persistant Variable Storage");
                //CameraFollow.Instance.SetPosition(transform.position);
            }
        }
    }

    [Command("TeleportPlayer", description ="Teleports the player to a 'door' in the currrent scene.")]
    public void TeleportPlayer(string DoorTag)
    {
        foreach (Teleport t in FindObjectsOfType<Teleport>())
        {
            if (DoorTag == t.doorTag)
            {
                transform.position = t.enterTransform.position;
                return;
            }

        }
        Minibuffer.instance.MessageAlert($"No door with doorTag '{DoorTag}' in the scene.");
    }

    [Command("Equip", description ="Equips an item To the player. (Loaded from Resources)")]
    public void EquipItem([Prompt(completer = "ItemCompleter", requireCoerce = true)]Item item)
    {
        var inventory = GetComponent<Inventory>();

        if(inventory.itemSet.Items.Contains(item) == false)
            inventory.AddItem(item);

        inventory.ItemController.EquipItem(inventory.itemSet.Items[inventory.itemSet.Items.Count - 1]);
    }

    [Command(description ="Swaps the players Color.")]
    public void PlayerColor(Color c)
    {
        this.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = c;
    }

    [Command]
    public void SetString(string key, string val)
    {
        pvs.storage.SetValue(key, val);
    }
    [Command]
    public void SetNumber(string key, float val)
    {
        pvs.storage.SetValue(key, val);
    }
    [Command]
    public void SetBool(string key, bool val)
    {
        pvs.storage.SetValue(key, val);
    }

    protected void OnAdd(string key, GlobalStorageObject.VarType varType)
    {
        switch(varType)
        {
            case GlobalStorageObject.VarType.BOOL :
                Minibuffer.instance.RegisterVariable(new Variable(key), () => { return pvs.storage.GetBool(key); }, (bool val) => { pvs.storage.SetValue(key, val); });
                break;
            case GlobalStorageObject.VarType.FLOAT:
                Minibuffer.instance.RegisterVariable(new Variable(key), () => { return pvs.storage.GetFloat(key); }, (float val) => { pvs.storage.SetValue(key, val); });
                break;
            case GlobalStorageObject.VarType.STRING:
                Minibuffer.instance.RegisterVariable(new Variable(key), () => { return pvs.storage.GetString(key); }, (string val) => { pvs.storage.SetValue(key, val); });
                break;
        }

        
    }
    protected void OnRemove(string key, GlobalStorageObject.VarType varType)
    {
        Minibuffer.instance.UnregisterVariable(key);
    }
}
