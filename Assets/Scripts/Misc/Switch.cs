using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IDataPersister
{
    bool m_State;
    SpriteRenderer m_Renderer;

    public DataSettings dataSettings;
    public bool defaultState;
    public UnityEvent onTrue;
    public UnityEvent onFalse;
    

    private void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        PersistentDataManager.RegisterPersister(this);
    }
    private void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
    }
    private void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }
    private void OnDestroy()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    private void Start()
    {
        m_State = defaultState;
        DoSwitchEvent(m_State);
        FlipSprite(m_State);
    }

    public void ToggleSwitch()
    {
        m_State = !m_State;

        DoSwitchEvent(m_State);
        FlipSprite(m_State);
        
    }
    void DoSwitchEvent(bool state)
    {
        if (state)
            onTrue.Invoke();
        else
            onFalse.Invoke();
    }
    void FlipSprite(bool state)
    {
        m_Renderer.flipY = state;
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    public Data SaveData()
    {
        return new Data<bool>(m_State);
    }

    public void LoadData(Data data)
    {
        m_State = ((Data<bool>)data).value;
        FlipSprite(m_State);

        if (dataSettings.persistenceType != DataSettings.PersistenceType.DoNotPersist || dataSettings.persistenceType != DataSettings.PersistenceType.WriteOnly)
            DoSwitchEvent(m_State);
    }
}
