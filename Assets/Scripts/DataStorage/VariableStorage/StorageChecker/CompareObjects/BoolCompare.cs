public class BoolCompare : CompareObject
{
    public bool isTrue = true;
    public override bool Check(GlobalStorageObject storage)
    {
        return storage.GetBool(key) == isTrue;
    }
}
