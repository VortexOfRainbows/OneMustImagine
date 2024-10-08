using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Utils ///Team members that contributed to this script: Ian Bunnell
{
    /// <summary>
    /// Rotates a vector based on the radians inputted. Useful for created circular effects
    /// </summary>
    /// <param name="spinningpoint"></param>
    /// <param name="radians"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    public static Vector2 RotatedBy(this Vector2 spinningpoint, float radians, Vector2 center = default(Vector2))
    {
        float xMult = (float)MathF.Cos(radians);
        float yMult = (float)MathF.Sin(radians);
        Vector2 vector = spinningpoint - center;
        Vector2 result = center;
        result.x += vector.x * xMult - vector.y * yMult;
        result.y += vector.x * yMult + vector.y * xMult;
        return result;
    }
    /// <summary>
    /// Converts the vector to the rotation in radians which would make the vector (magnitude, 0) become the vector.
    /// Basically runs arctan on the vector. y/x
    /// </summary>
    /// <param name="directionVector"></param>
    /// <returns></returns>
    public static float ToRotation(this Vector2 directionVector)
    {
        return Mathf.Atan2(directionVector.y, directionVector.x);
    }
    /// <summary>
    /// Converts a rotation to a quaternion rotated on the z axis.
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public static Quaternion ToQuaternion(this float rotation)
    {
        Quaternion relativeRotation = Quaternion.AngleAxis(rotation * Mathf.Rad2Deg, new Vector3(0, 0, 1));
        return relativeRotation;
    }
    /// <summary>
    /// Wraps an angle to an equivalent angle between -Pi and Pi
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static float WrapAngle(this float x)
    {
        x = (x + Mathf.PI) % (2 * Mathf.PI);
        if (x < 0)
            x += Mathf.PI * 2;
        return x - Mathf.PI;
    }
    /// <summary>
    /// Gets the position of the mouse in the world. Not sure if this works in 3D since this helper was originally written for 2D.
    /// </summary>
    /// <returns></returns>
    public static Vector2 MouseWorld()
    {
        Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mPos;
    }
    /// <summary>
    /// Converts a string from something like this: "StringHereIs" to "String Here Is"
    /// Adjacent capital letters are considered part of the same string: "SUPERSpeed" to "SUPERSpeed"
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string AddSpaceBetweenCaps(this string str)
    {
        string construct = string.Empty;
        for (int i = 0; i < str.Length - 1; i++)
        {
            char first = str[i];
            char second = str[i + 1];
            construct += first;
            if (Char.IsLower(first) && Char.IsUpper(second))
            {
                construct += " ";
            }
        }
        construct += str[str.Length - 1];
        return construct;
    }
    /// <summary>
    /// Generates a random oval vector within the boundaries of x, y, and z
    /// If edge is true, guarantees that the value will fall on the edge. Otherwise, it will fall elsewhere in the circle.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="edge"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Vector2 randVector2Circular(float width, float height, bool edge = false)
    {
        Vector2 random = UnityEngine.Random.insideUnitCircle;
        if (edge)
        {
            random = random.normalized;
        }
        random.x *= width;
        random.y *= height;
        return random;
    }
    /// <summary>
    /// Generates a random oval vector within the boundaries of x, y, and z
    /// If edge is true, guarantees that the value will fall on the edge. Otherwise, it will fall elsewhere in the circle.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="edge"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Vector3 randVector3Circular(float widthX, float height, float widthZ, bool edge = false)
    {
        Vector3 random = edge ? UnityEngine.Random.onUnitSphere : UnityEngine.Random.insideUnitSphere;
        random.x *= widthX;
        random.y *= height;
        random.z *= widthZ;
        return random;
    }
}
