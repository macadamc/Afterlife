using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShadyPixel.DisplayObjects
{
    public class DisplayObject : MonoBehaviour
    {
        bool _registered;

        public void Register()
        {
            if (!_registered)
            {
                _registered = true;
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Wrapper for GameObject's SetActive method.
        /// </summary>
        public void SetActive(bool value)
        {
            _registered = true;
            gameObject.SetActive(value);
        }

        /// <summary>
        /// Solo this DisplayObject within other DisplayObjects at the same level in the hierarchy.
        /// </summary>
        [ButtonGroup]
        public void Solo()
        {
            if (transform.parent != null)
            {
                foreach (Transform item in transform.parent)
                {
                    if (item == transform) continue;
                    DisplayObject displayObject = item.GetComponent<DisplayObject>();
                    if (displayObject != null) displayObject.SetActive(false);
                }
                gameObject.SetActive(true);
            }
            else
            {
                foreach (var item in Resources.FindObjectsOfTypeAll<DisplayObject>())
                {
                    if (item.transform.parent == null)
                    {
                        if (item == this)
                        {
                            item.SetActive(true);
                        }
                        else
                        {
                            item.SetActive(false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Hides all DisplayObjects at the same level in the hierarchy as this DisplayObject.
        /// </summary>
        [ButtonGroup]
        public void HideAll()
        {
            if (transform.parent != null)
            {
                foreach (Transform item in transform.parent)
                {
                    if (item.GetComponent<DisplayObject>() != null) item.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (var item in Resources.FindObjectsOfTypeAll<DisplayObject>())
                {
                    if (item.transform.parent == null) item.gameObject.SetActive(false);
                }
            }
        }

        private void Awake()
        {
            Register();
        }

    }
}
