using UnityEngine;

namespace MorePegs.Shared;

public static class QuaternionExtensions
{
    public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        var angle = Vector3.Angle(fromDirection, toDirection);
        var axis = Vector3.Cross(fromDirection, toDirection);

        return AngleAxis(angle, axis);
    }

    public static Quaternion AngleAxis(float angle, Vector3 axis)
    {
        var angleHalvedInRadians = angle * .5f * Mathf.Deg2Rad;

        axis.Normalize();
        axis *= Mathf.Sin(angleHalvedInRadians);

        return new Quaternion(axis.x, axis.y, axis.z, Mathf.Cos(angleHalvedInRadians));
    }
}