using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NullCheckExtension
{
    public static void InvokeIfNotNull(this Action action)
    {
        action?.Invoke();
    }

    public static void InvokeIfNotNull<T>(this Action<T> action, T values)
    {
        action?.Invoke(values);
    }
}
