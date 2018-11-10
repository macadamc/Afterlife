using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace ShadyPixel.StateMachine
{
    public class State : MonoBehaviour
    {
        [TabGroup("Events")]
        public UnityEvent onEnterEvent;
        [TabGroup("Events")]
        public UnityEvent onExitEvent;

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

        protected virtual void OnEnable()
        {
            onEnterEvent?.Invoke();
        }

        protected virtual void OnDisable()
        {
            onExitEvent?.Invoke();
        }
    }
}
