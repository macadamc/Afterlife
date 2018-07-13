using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yarn.Unity;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ShadyPixel.Variables;
using System.Text;

/*
/// An extremely simple implementation of DialogueUnityVariableStorage, which
/// just stores everything in a Dictionary.
public class VariableStorage2 : VariableStorageBehaviour
{
    /// Where we actually keeping our variables
    Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();
    /// A default value to apply when the object wakes up, or
    /// when ResetToDefaults is called
    [System.Serializable]
    public class DefaultVariable
    {
        /// Name of the variable
        public string name;
        /// Value of the variable
        public string value;
        /// Type of the variable
        public Yarn.Value.Type type;
    }

    /// Our list of default variables, for debugging.
    public DefaultVariable[] defaultVariables;

    [Header("Optional debugging tools")]
    /// A UI.Text that can show the current list of all variables. Optional.
    public UnityEngine.UI.Text debugTextView;

    /// Reset to our default values when the game starts
    void Awake()
    {
        ResetToDefaults();
    }

    /// Erase all variables and reset to default values
    public override void ResetToDefaults()
    {
        Clear();

        // For each default variable that's been defined, parse the string
        // that the user typed in in Unity and store the variable
        foreach (var variable in defaultVariables)
        {
            object value;

            switch (variable.type)
            {
                case Yarn.Value.Type.Number:
                    float f = 0.0f;
                    float.TryParse(variable.value, out f);
                    value = f;
                    break;

                case Yarn.Value.Type.String:
                    value = variable.value;
                    break;

                case Yarn.Value.Type.Bool:
                    bool b = false;
                    bool.TryParse(variable.value, out b);
                    value = b;
                    break;

                case Yarn.Value.Type.Variable:
                    // We don't support assigning default variables from other variables
                    // yet
                    Debug.LogErrorFormat("Can't set variable {0} to {1}: You can't " +
                        "set a default variable to be another variable, because it " +
                        "may not have been initialised yet.", variable.name, variable.value);
                    continue;

                case Yarn.Value.Type.Null:
                    value = null;
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException();

            }

            var v = new Yarn.Value(value);

            SetValue("$" + variable.name, v);
        }
    }

    /// Set a variable's value
    public override void SetValue(string variableName, Yarn.Value value)
    {
        // Copy this value into our list
        variables[variableName] = new Yarn.Value(value);
    }

    /// Get a variable's value
    public override Yarn.Value GetValue(string variableName)
    {
        // If we don't have a variable with this name, return the null value
        if (variables.ContainsKey(variableName) == false)
            return Yarn.Value.NULL;

        return variables[variableName];
    }

    /// Erase all variables
    public override void Clear()
    {
        variables.Clear();
    }



    /// If we have a debug view, show the list of all variables in it
    void Update()
    {
        if (debugTextView != null)
        {
            var stringBuilder = new System.Text.StringBuilder();
            foreach (KeyValuePair<string, Yarn.Value> item in variables)
            {
                stringBuilder.AppendLine(string.Format("{0} = {1}",
                                                         item.Key,
                                                         item.Value));
            }
            debugTextView.text = stringBuilder.ToString();
        }
    }
}
*/
public class VariableStorage : VariableStorageBehaviour
{
    /// Where we actually keeping our variables
    public Dictionary<string, Yarn.Value> variables;
    /// A default value to apply when the object wakes up, or
    /// when ResetToDefaults is called
    [System.Serializable]
    public class DefaultVariable
    {
        /// Name of the variable
        public string name;
        /// Value of the variable
        public string value;
        /// Type of the variable
        public Yarn.Value.Type valueType;
    }

    /// Our list of default variables, for debugging.
    public DefaultVariable[] defaultVariables;

    /// Reset to our default values when the game starts
    void Awake()
    {
        if (variables == null)
            variables = new Dictionary<string, Yarn.Value>();

        ResetToDefaults();
    }

    /// Erase all variables and reset to default values
    public override void ResetToDefaults()
    {
        Clear();

        // For each default variable that's been defined, parse the string
        // that the user typed in in Unity and store the variable
        foreach (var variable in defaultVariables)
        {
            var v = new Yarn.Value(ParseObj(variable));

            SetValue(variable.name, v);
        }
    }

    /// Set a variable's value
    public override void SetValue(string variableName, Yarn.Value value)
    {
        variables[variableName.TrimStart('$')] = new Yarn.Value(value);
    }
    public void SetValue(string[] args)
    {
        SetValue(args[0].TrimStart('$'), new Yarn.Value(ParseObj(args[1])));
    }

    /// Get a variable's value
    public override Yarn.Value GetValue(string variableName)
    {
        variableName = variableName.TrimStart('$');
        // If we don't have a variable with this name, return the null value
        if (variables.ContainsKey(variableName) == false)
            return Yarn.Value.NULL;

        return variables[variableName];
    }

    /// Erase all variables
    public override void Clear()
    {
        variables.Clear();
    }

    public bool HasKey(string key)
    {
        return variables.ContainsKey(key.TrimStart('$'));
    }

    [Button]
    public void Save()
    {
        List<DefaultVariable> data = new List<DefaultVariable>();
        BinaryFormatter formatter = new BinaryFormatter();

        foreach (var keyVal in variables)
        {
            Yarn.Value val = keyVal.Value;
            data.Add(
                new DefaultVariable()
                {
                    name = keyVal.Key,
                    value = val.AsString,
                    valueType = val.type,
                });
        }

        using (FileStream fs = File.Open($"{Application.persistentDataPath}/Variables.data", FileMode.OpenOrCreate))
        {
            formatter.Serialize(fs, data);
        }
    }

    [Button]
    public void Load()
    {
        List<DefaultVariable> data = new List<DefaultVariable>();
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fs = File.Open($"{Application.persistentDataPath}/Variables.data", FileMode.OpenOrCreate))
        {
            data = (List<DefaultVariable>)formatter.Deserialize(fs);
        }

        // For each default variable that's been defined, parse the string
        // that the user typed in in Unity and store the variable
        foreach (var variable in data)
        {
            object value;

            switch (variable.valueType)
            {
                case Yarn.Value.Type.Number:
                    float f = 0.0f;
                    float.TryParse(variable.value, out f);
                    value = f;
                    break;

                case Yarn.Value.Type.String:
                    value = variable.value;
                    break;

                case Yarn.Value.Type.Bool:
                    bool b = false;
                    bool.TryParse(variable.value, out b);
                    value = b;
                    break;

                case Yarn.Value.Type.Variable:
                    // We don't support assigning default variables from other variables
                    // yet
                    Debug.LogErrorFormat("Can't set variable {0} to {1}: You can't " +
                        "set a default variable to be another variable, because it " +
                        "may not have been initialised yet.", variable.name, variable.value);
                    continue;

                case Yarn.Value.Type.Null:
                    value = null;
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException();

            }

            var v = new Yarn.Value(value);

            SetValue(variable.name, v);
            Debug.Log($"{variable.name}: {variable.value}");
        }
    }

    private object ParseObj(string val)
    {
        object value = null;
        //number
        if (char.IsDigit(val[0]) || val[0] == '.')
        {
            float f = 0.0f;
            float.TryParse(val, out f);
            value = f;
        }
        //Bool
        else if (val.ToLower() == "true" || val.ToLower() == "false")
        {
            bool b = false;
            bool.TryParse(val, out b);
            value = b;
        }
        //string
        else if (val.Length > 0)
        {
            value = val;
        }
        else
        {
            value = null;
        }
        return value;
    }
    private object ParseObj(DefaultVariable variable)
    {
        object value = null;
        //number
        if (char.IsDigit(variable.value[0]) || variable.value[0] == '.')
        {
            float f = 0.0f;
            float.TryParse(variable.value, out f);
            value = f;
        }
        //Bool
        else if (variable.value.ToLower() == "true" || variable.value.ToLower() == "false")
        {
            bool b = false;
            bool.TryParse(variable.value, out b);
            value = b;
        }
        //string
        else if (variable.value.Length > 0)
        {
            value = variable.value;
        }
        else
        {
            value = null;
        }

        /*
        switch (variable.valueType)
        {
            case Yarn.Value.Type.Number:
                float f = 0.0f;
                float.TryParse(variable.value, out f);
                value = f;
                break;

            case Yarn.Value.Type.String:
                value = variable.value;
                break;

            case Yarn.Value.Type.Bool:
                bool b = false;
                bool.TryParse(variable.value, out b);
                value = b;
                break;

            case Yarn.Value.Type.Variable:
                // We don't support assigning default variables from other variables
                // yet
                Debug.LogErrorFormat("Can't set variable {0} to {1}: You can't " +
                    "set a default variable to be another variable, because it " +
                    "may not have been initialised yet.", variable.name, variable.value);
                break;

            case Yarn.Value.Type.Null:
                value = null;
                break;

            default:
                throw new System.ArgumentOutOfRangeException();
                
        }
        */
        return value;
    }
}