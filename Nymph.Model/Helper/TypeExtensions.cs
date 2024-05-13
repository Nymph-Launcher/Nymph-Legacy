namespace Nymph.Model.Helper;

public static class TypeExtensions
{
    public static bool IsInstanceOfGenericType(this object obj, Type genericType)
    {
        var type = obj.GetType();
        while (type != null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
            type = type.BaseType;
        }
        return false;
    }
}