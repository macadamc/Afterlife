public abstract class CompareObject
{
    public string key;
    public virtual bool Check(GlobalStorageObject storage)
    {
        throw new System.NotImplementedException();
    }
}