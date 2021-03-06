﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShadyPixel.Singleton
{
    public class Singleton<T> : MonoBehaviour
    {
        /// <summary>
        /// Returns current instance of Singleton.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("Singleton not registered! Make sure the GameObject is active.");
                    return default(T);
                }
                return _instance;
            }
        }

        [SerializeField, TabGroup("Settings"), Tooltip("If the object will not be destroyed when changing scenes.")]
        bool dontDestroyOnLoad;
        [SerializeField, TabGroup("Settings"), Tooltip("Should copys of this object be deleted.")]
        bool allowMoreThanOne;
        static T _instance;

        public Transform GetRootObject(Transform transformToCheck)
        {
            if (transformToCheck.parent == null)
                return transformToCheck;
            else
                return GetRootObject(transformToCheck.parent);
        }

        /// <summary>
        /// Override this method to call code when the Singleton gets registered.
        /// </summary>
        protected virtual void OnRegistration()
        {

        }

        /// <summary>
        /// Register another instance, overriding the current Singleton.
        /// </summary>
        /// <param name="instance"></param>
        public void RegisterSingleton(T instance)
        {
            _instance = instance;
        }

        /// <summary>
        /// MUST CALL THIS WHEN CREATING A NEW SINGLETON!
        /// </summary>
        /// <param name="instance"></param>
        protected void Initialize(T instance)
        {
            if(_instance == null)
            {
                if (dontDestroyOnLoad)
                {
                    //don't destroy on load only works on root objects so let's force this transform to be a root object:
                    //transform.parent = null;
                    DontDestroyOnLoad(GetRootObject(transform).gameObject);
                }
                _instance = instance;
                OnRegistration();
            }
            else if(!allowMoreThanOne)
            {
                Debug.Log("There is already an instance of [" + name + "]. Destroying copy...");
                Destroy(GetRootObject(transform).gameObject);
            }
            else
            {
                _instance = instance;
                OnRegistration();
            }
        }
    }
}
