using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using ShadyPixel.Events;

namespace ShadyPixel.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [InfoBox("StateMachine has no States. Please add children objects to base StateMachine!", InfoMessageType.Error, "hasNoStates")]
        [InfoBox("State must be a child of the StateMachine!", InfoMessageType.Error, "defaultIsNotChild")]
        [Tooltip("Default State of StateMachine.")]

        #region Public Variables

        public GameObject defaultState;

        [TabGroup("Settings")]
        [Tooltip("If the StateMachine will loop around to the beginning or end when calling Next or Previous at the edges.")]
        public bool loopStates = true;
        [TabGroup("Settings")]
        [Tooltip("If the StateMachine will print out Debug.log() statements.")]
        public bool logToConsole = true;
        [TabGroup("Settings")]
        [Tooltip("If the StateMachine is allowed to enter a State it is currently in.")]
        public bool allowStateReentry = false;
        [TabGroup("Settings")]
        [Tooltip("If the StateMachine should return to default values if disabled at runtime.")]
        public bool returnToDefaultOnDisable = true;

        [TabGroup("Runtime")]
        [Tooltip("Current running State of StateMachine.")]
        public GameObject currentState;
        [DrawWithUnity, TabGroup("Runtime")]
        public StateMachineEvents events;

        #endregion

        #region Public Classes

        [System.Serializable]
        public class StateMachineEvents
        {
            public UnityEvent OnStart;
            public GameObjectEvent OnStateEnter;
            public GameObjectEvent OnStateExit;
        }
        [System.Serializable]
        public class StateObject
        {
            [ReadOnly, HorizontalGroup, HideLabel, HideReferenceObjectPicker]
            public GameObject state;
            StateMachine stateMachine;

            public StateObject(GameObject go, StateMachine sm)
            {
                state = go;
                stateMachine = sm;

            }

            [Button, HorizontalGroup]
            void SetState()
            {
                stateMachine.ChangeState(state);
            }
        }

        #endregion

        #region Private Variables

        [ListDrawerSettings(IsReadOnly = true), SerializeField]
        List<StateObject> states;

        bool _atFirst;
        bool _atLast;

        #endregion

        #region Public Functions

        [Button("Exit Current State"),HideInEditorMode]
        public void Exit()
        {
            if (currentState == null) return;

            Log("(-) " + name + " EXITED state: " + currentState.name);

            if (events.OnStateExit != null) events.OnStateExit.Invoke(currentState);
            currentState.SetActive(false);
            currentState = null;
        }

        public GameObject ChangeState(GameObject state)
        {
            if(currentState != null)
            {
                if(!allowStateReentry && currentState == state)
                {
                    Log("State change ignored. State machine \"" + name + "\" already in \"" + state.name + "\" state.");
                    return null;
                }

                if (state.transform.parent != transform)
                {
                    Log("State \"" + state.name + "\" is not a child of \"" + name + "\" StateMachine state change canceled.");
                    return null;
                }
            }

            Exit();
            Enter(state);
            return currentState;
        }

        [ButtonGroup, HideInEditorMode]
        public GameObject Previous()
        {
            if (currentState == null)
                return ChangeState(transform.GetChild(0).gameObject);

            int currentIndex = currentState.transform.GetSiblingIndex();
            if (currentIndex == 0)
            {
                if (loopStates)
                    return ChangeState(transform.GetChild(transform.childCount - 1).gameObject);
                else
                    return currentState;
            }
            else
            {
                return ChangeState(transform.GetChild(currentIndex - 1).gameObject);
            }
        }

        [ButtonGroup, HideInEditorMode]
        public GameObject Next()
        {
            if (currentState == null)
                return ChangeState(transform.GetChild(0).gameObject);

            int currentIndex = currentState.transform.GetSiblingIndex();
            if (currentIndex == transform.childCount - 1)
            {
                if (loopStates)
                    return ChangeState(transform.GetChild(0).gameObject);
                else
                    return currentState;
            }
            else
            {
                return ChangeState(transform.GetChild(currentIndex + 1).gameObject);
            }
        }

        protected virtual void Enter(GameObject state)
        {
            currentState = state;

            Log("(+) " + name + " ENTERED state: " + state.name);

            if (events.OnStateEnter != null) events.OnStateEnter.Invoke(currentState);

            currentState.SetActive(true);
        }

        #endregion

        #region Private Functions

        private void Start()
        {
            InitializeList();

            HideStates();

            if (events.OnStart != null) events.OnStart.Invoke();

            if (defaultState != null) ChangeState(defaultState);
        }

        private void InitializeList()
        {
            states = new List<StateObject>();

            for (int i = 0; i < transform.childCount; i++)
            {
                StateObject so = new StateObject(transform.GetChild(i).gameObject, this);
                states.Add(so);
            }
        }

        [Button("Hide States"), HideInPlayMode]
        private void HideStates()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void AddState(StateObject stateObject)
        {
            if (!states.Contains(stateObject))
                states.Add(stateObject);
        }

        private void RemoveState(StateObject stateObject)
        {
            if (states.Contains(stateObject))
                states.Remove(stateObject);
        }

        protected void Log(string message)
        {
            if (!logToConsole) return;

            Debug.Log(message);
        }


        private bool hasNoStates()
        {
            if (defaultState == null && transform.childCount > 0)
                defaultState = transform.GetChild(0).gameObject;

            return transform.childCount == 0;
        }

        private bool defaultIsNotChild()
        {
            if (defaultState != null)
                return defaultState.transform.parent != transform;
            else
                return false;
        }

        #endregion
    }
}
