
using UnityEngine;

public static class MathUtil
{
    public static Quaternion LerpQuaternion(Quaternion a_a, Quaternion a_b, float a_time)
    {
        var w = Mathf.Lerp(a_a.w, a_b.w, a_time);
        var x = Mathf.Lerp(a_a.x, a_b.x, a_time);
        var y = Mathf.Lerp(a_a.y, a_b.y, a_time);
        var z = Mathf.Lerp(a_a.z, a_b.z, a_time);

        return new Quaternion(x, y, z, w);
    }
}
