using ShadyPixel.Variables;

public class FloatCompare : CompareObject
{
    public FloatReference Other;

    public enum ComparisonType { Equals, GreaterThan, GreaterOrEqual, LessThan, LessOrEqual, NotEqual }
    public ComparisonType comparisonType;

    public override bool Check(GlobalStorageObject storageObject)
    {
        if (storageObject.floats.ContainsKey(key))
        {
            switch (comparisonType)
            {
                case ComparisonType.Equals:
                    return storageObject.GetFloat(key) == Other.Value;
                case ComparisonType.NotEqual:
                    return storageObject.GetFloat(key) != Other.Value;
                case ComparisonType.GreaterThan:
                    return storageObject.GetFloat(key) > Other.Value;
                case ComparisonType.LessThan:
                    return storageObject.GetFloat(key) < Other.Value;
                case ComparisonType.GreaterOrEqual:
                    return storageObject.GetFloat(key) >= Other.Value;
                case ComparisonType.LessOrEqual:
                    return storageObject.GetFloat(key) <= Other.Value;
            }
        }

        return false;
    }
}