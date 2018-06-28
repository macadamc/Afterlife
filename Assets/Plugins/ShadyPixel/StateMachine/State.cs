using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace ShadyPixel.StateMachine
{
    public class State : MonoBehaviour
    {
        [System.Serializable]
        public class Events
        {
            public UnityEvent onEnterEvent;
            public UnityEvent onExitEvent;
        }

        [DrawWithUnity]
        public Events events;

        StateMachine _stateMachine;

        public StateMachine StateMachine
        {
            get
            {
                if (_stateMachine == null)
                {
                    _stateMachine = transform.parent.GetComponent<StateMachine>();
                    if (_stateMachine == null)
                    {
                        Debug.LogError("States must be the child of a StateMachine to operate.");
                        return null;
                    }
                }

                return _stateMachine;
            }
        }

        public void ChangeState(GameObject state)
        {
            StateMachine.ChangeState(state);
        }

        public GameObject Next()
        {
            return StateMachine.Next();
        }

        public GameObject Previous()
        {
            return StateMachine.Previous();
        }

        public void Exit()
        {
            StateMachine.Exit();
        }

        private void OnEnable()
        {
            if (events.onEnterEvent != null)
                events.onEnterEvent.Invoke();
        }

        private void OnDisable()
        {
            if (events.onExitEvent != null)
                events.onExitEvent.Invoke();
        }
    }
}
