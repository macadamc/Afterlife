using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;

namespace ShadyPixel.GameEvents
{
    public class GameEventListener : MonoBehaviour
    {
        [InfoBox("There is no referenced Game Event!", InfoMessageType.Warning, "IsNull")]
        public GameEvent gameEvent;

        [HideIf("IsNull"), DrawWithUnity]
        public UnityEvent response;


        [Button("Force Response", ButtonSizes.Large)]
        public void OnEventRaised()
        {
            response.Invoke();
        }

        private bool IsNull()
        {
            return gameEvent == null;
        }

        private void OnEnable()
        {
            gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            gameEvent.RemoveListener(this);
        }

    }

}
