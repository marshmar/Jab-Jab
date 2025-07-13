using UnityEngine;
using UnityEngine.InputSystem;

public static class Extensions
{
    public static T GetComponentSafe<T>(this Component component)
    {
        var ret = component.GetComponent<T>();
        Debug.Assert(ret != null, $"{typeof(T).Name} is not assigned");

        return ret;
    }

    public static T GetComponentSafe<T>(this GameObject gameObject)
    {
        var ret = gameObject.GetComponent<T>();
        Debug.Assert(ret != null, $"{typeof(T).Name} is not assigned");

        return ret;
    }

    public static bool IsNull<T>(this T t)
    {
        Debug.Assert(t != null, $"{typeof(T).Name} is missing");
        return t == null;
    }
}
