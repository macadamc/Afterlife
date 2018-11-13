using Yarn;

[System.Serializable]
public class YarnStorageWrapper : Yarn.VariableStorage
{
    public GlobalStorageObject storage;

    public YarnStorageWrapper(GlobalStorageObject storage)
    {
        this.storage = storage;
    }
    // Yarn Storage.
    public void Clear() => storage.Clear();

    public float GetNumber(string variableName)
    {
        return storage.GetFloat(variableName);
    }

    public void SetNumber(string variableName, float number) => storage.SetValue(variableName, number);

    public Value GetValue(string variableName)
    {
        return new Value(storage.GetFirstOrNull(variableName));
    }

    public void SetValue(string variableName, Value value)
    {
        switch (value.type)
        {
            case Value.Type.String:
                storage.SetValue(variableName, value.AsString);
                break;
            case Value.Type.Number:
                storage.SetValue(variableName, value.AsNumber);
                break;
            case Value.Type.Bool:
                storage.SetValue(variableName, value.AsBool);
                break;

        }
    }
}