using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NodeCanvas.Framework;

public static class GlobalBlackboardExtensions
{
    private static bool TryGetValue<TValue>(string a_key, out TValue a_value)
    {
        a_value = default(TValue);
        var blackboard = GlobalBlackboard.allGlobals.FirstOrDefault();
        if (blackboard == null) { return false; }
        if (blackboard.variables == null || blackboard.variables.ContainsKey(a_key) == false) { return false; }
        var variable = blackboard.variables[a_key] as Variable<TValue>;
        if (variable == null) { return false; }
        a_value = variable.value;
        return true;
    }

    private static bool TrySetValue<TValue>(string a_key, TValue a_value)
    {
        var blackboard = GlobalBlackboard.allGlobals.FirstOrDefault();
        if (blackboard == null) { return false; }
        if (blackboard.variables == null || blackboard.variables.ContainsKey(a_key) == false) { return false; }
        var variable = blackboard.variables[a_key] as Variable<TValue>;
        if (variable == null) { return false; }
        variable.value = a_value;
        return true;
    }

    public static TValue GetValue<TValue>(string a_key)
    {
        TValue l_value;
        if (TryGetValue(a_key, out l_value) == false)
        {
            throw new System.InvalidOperationException("Error reading from global blackboard." + " Key: " + a_key);
        }

        return l_value;
    }

    public static void SetValue<TValue>(string a_key, TValue a_value)
    {
        if (TrySetValue(a_key, a_value) == false)
        {
            throw new System.InvalidOperationException("Error writing into global blackboard." + " Key: " + a_key);
        }
    }
}
