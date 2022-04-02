using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public sealed class Curves
{
    #region Function Finding
    /*Usage:
     * 1 - Have a public FunctionType field for selecting the easing function to use.
     * 2 - Find the method using GetFunctionXXX(FunctionType) and cache it (slow!).
     * 3 - Call the method cached when needed as usual (start, end, t).
     */

    public enum FunctionType
    {
        Hermite, Sinerp, Coserp, Berp,
        QuadEaseIn, QuadEaseOut, QuadEaseInOut,
        CubicEaseIn, CubicEaseOut, CubicEaseInOut,
        QuartEaseIn, QuartEaseOut, QuartEaseInOut,
        QuintEaseIn, QuintEaseOut, QuintEaseInOut,
        SinEaseIn, SinEaseOut, SinEaseInOut,
        ExpEaseIn, ExpEaseOut, ExpEaseInOut,
        CircEaseIn, CircEaseOut, CircEaseInOut,
        BounceEaseIn, BounceEaseOut, BounceEaseInOut,
        BackEaseIn, BackEaseOut, BackEaseInOut,
        ElasticEaseIn, ElasticEaseOut, ElasticEaseInOut,
    };

    /// <summary>
    /// Action for an easing function.
    /// </summary>
    /// <typeparam name="T">Type of data to process (float, Vector2 or Vector3)</typeparam>
    /// <param name="start">return value at t=0.</param>
    /// <param name="end">return value at t=1.</param>
    /// <param name="t">progress value between 0 and 1. Going out of this range may returns unexpected results.</param>
    public delegate T Function<T>(T start, T end, float t);

    public static Function<float> GetFunctionFloat(FunctionType function)
    {
        return GetFunctionFloat(FindMethod(function.ToString(), typeof(float)));
    }

    public static Function<Vector2> GetFunctionVector2(FunctionType function)
    {
        return GetFunctionVector2(FindMethod(function.ToString(), typeof(Vector2)));
    }

    public static Function<Vector3> GetFunctionVector3(FunctionType function)
    {
        return GetFunctionVector3(FindMethod(function.ToString(), typeof(Vector3)));
    }

    static Function<float> GetFunctionFloat(MethodInfo method)
    {
        return (Function<float>)Delegate.CreateDelegate(typeof(Function<float>), method);
    }

    static Function<Vector2> GetFunctionVector2(MethodInfo method)
    {
        return (Function<Vector2>)Delegate.CreateDelegate(typeof(Function<Vector2>), method);
    }

    static Function<Vector3> GetFunctionVector3(MethodInfo method)
    {
        return (Function<Vector3>)Delegate.CreateDelegate(typeof(Function<Vector3>), method);
    }

    /// <summary>
    /// Find the method with given name and type of the first parameter using reflection and returns it.
    /// </summary>
    /// <param name="type">Class of the method</param>
    /// <param name="methodName">Name of the method</param>
    static MethodInfo FindMethod(String methodName, Type firstParametersType)
    {
        //Find all the public static methods
        MethodInfo[] methods = typeof(Curves).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);

        //Find the method with the same name and same parameters type
        return methods.First(m => m.Name == methodName && m.GetParameters()[0].ParameterType == firstParametersType);
    }
    #endregion

    #region Maths
    private const float NATURAL_LOG_OF_2 = 0.693147181f;

    //_____________________Mathfx_____________
    //http://wiki.unity3d.com/index.php?title=Mathfx

    //Ease in out
    public static float Hermite(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, t * t * (3.0f - 2.0f * t));
    }

    public static Vector2 Hermite(Vector2 start, Vector2 end, float t)
    {
        return new Vector2(Hermite(start.x, end.x, t), Hermite(start.y, end.y, t));
    }

    public static Vector3 Hermite(Vector3 start, Vector3 end, float t)
    {
        return new Vector3(Hermite(start.x, end.x, t), Hermite(start.y, end.y, t), Hermite(start.z, end.z, t));
    }

    //Ease out
    public static float Sinerp(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(t * Mathf.PI * 0.5f));
    }

    public static Vector2 Sinerp(Vector2 start, Vector2 end, float t)
    {
        return new Vector2(Mathf.Lerp(start.x, end.x, Mathf.Sin(t * Mathf.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(t * Mathf.PI * 0.5f)));
    }

    public static Vector3 Sinerp(Vector3 start, Vector3 end, float t)
    {
        return new Vector3(Mathf.Lerp(start.x, end.x, Mathf.Sin(t * Mathf.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(t * Mathf.PI * 0.5f)), Mathf.Lerp(start.z, end.z, Mathf.Sin(t * Mathf.PI * 0.5f)));
    }
    //Ease in
    public static float Coserp(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(t * Mathf.PI * 0.5f));
    }

    public static Vector2 Coserp(Vector2 start, Vector2 end, float t)
    {
        return new Vector2(Coserp(start.x, end.x, t), Coserp(start.y, end.y, t));
    }

    public static Vector3 Coserp(Vector3 start, Vector3 end, float t)
    {
        return new Vector3(Coserp(start.x, end.x, t), Coserp(start.y, end.y, t), Coserp(start.z, end.z, t));
    }

    //Boing
    public static float Berp(float start, float end, float t)
    {
        t = Mathf.Clamp01(t);
        t = (Mathf.Sin(t * Mathf.PI * (0.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + (1.2f * (1f - t)));
        return start + (end - start) * t;
    }

    public static Vector2 Berp(Vector2 start, Vector2 end, float t)
    {
        return new Vector2(Berp(start.x, end.x, t), Berp(start.y, end.y, t));
    }

    public static Vector3 Berp(Vector3 start, Vector3 end, float t)
    {
        return new Vector3(Berp(start.x, end.x, t), Berp(start.y, end.y, t), Berp(start.z, end.z, t));
    }

    //Bounce
    public static float Bounce(float x)
    {
        return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
    }

    public static Vector2 Bounce(Vector2 vec)
    {
        return new Vector2(Bounce(vec.x), Bounce(vec.y));
    }

    public static Vector3 Bounce(Vector3 vec)
    {
        return new Vector3(Bounce(vec.x), Bounce(vec.y), Bounce(vec.z));
    }


    //__________________Others____________________
    //http://gizma.com/easing/

    //Quadratic
    public static float QuadEaseIn(float start,float end, float t)
    {
        return (end - start) * t * t + start;
    }

    public static Vector2 QuadEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuadEaseIn(vec.x, end.x, t), QuadEaseIn(vec.y, end.y, t));
    }

    public static Vector3 QuadEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(QuadEaseIn(vec.x, end.x, t), QuadEaseIn(vec.y, end.y, t), QuadEaseIn(vec.z, end.z, t));
    }


    public static float QuadEaseOut(float start, float end, float t)
    {
        return -(end - start) * t * (t - 2) + start;
    }

    public static Vector2 QuadEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuadEaseOut(vec.x, end.x, t), QuadEaseOut(vec.y, end.y, t));
    }

    public static Vector3 QuadEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(QuadEaseOut(vec.x, end.x, t), QuadEaseOut(vec.y, end.y, t), QuadEaseOut(vec.z, end.z, t));
    }

    public static float QuadEaseInOut(float start, float end, float t)
    {
        t *= 2;
        if (t < 1)
            return (end - start) / 2 * t * t + start;
        t--;
        return -(end - start) / 2 * (t * (t - 2) - 1) + start;
    }

    public static Vector2 QuadEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuadEaseInOut(vec.x, end.x, t), QuadEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 QuadEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(QuadEaseInOut(vec.x, end.x, t), QuadEaseInOut(vec.y, end.y, t), QuadEaseInOut(vec.z, end.z, t));
    }

    //Cubic
    public static float CubicEaseIn(float start, float end, float t)
    {
        return (end - start) * t * t * t + start;
    }

    public static Vector2 CubicEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(CubicEaseIn(vec.x, end.x, t), CubicEaseIn(vec.y, end.y, t));
    }

    public static Vector3 CubicEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(CubicEaseIn(vec.x, end.x, t), CubicEaseIn(vec.y, end.y, t), CubicEaseIn(vec.z, end.z, t));
    }

    public static float CubicEaseOut(float start, float end, float t)
    {
        t--;
        return (end - start) * (t * t * t + 1) + start;
    }

    public static Vector2 CubicEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(CubicEaseOut(vec.x, end.x, t), CubicEaseOut(vec.y, end.y, t));
    }

    public static Vector3 CubicEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(CubicEaseOut(vec.x, end.x, t), CubicEaseOut(vec.y, end.y, t), CubicEaseOut(vec.z, end.z, t));
    }

    public static float CubicEaseInOut(float start, float end, float t)
    {
        t *= 2;
        if (t < 1)
            return (end - start) / 2 * t * t * t + start;
        t -= 2;
        return (end - start) / 2 * (t * t * t + 2) + start;
    }

    public static Vector2 CubicEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(CubicEaseInOut(vec.x, end.x, t), CubicEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 CubicEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(CubicEaseInOut(vec.x, end.x, t), CubicEaseInOut(vec.y, end.y, t), CubicEaseInOut(vec.z, end.z, t));
    }

    //Quartic
    public static float QuartEaseIn(float start, float end, float t)
    {
        return (end - start) * t * t * t * t + start;
    }

    public static Vector2 QuartEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuartEaseIn(vec.x, end.x, t), QuartEaseIn(vec.y, end.y, t));
    }

    public static Vector3 QuartEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(QuartEaseIn(vec.x, end.x, t), QuartEaseIn(vec.y, end.y, t), QuartEaseIn(vec.z, end.z, t));
    }

    public static float QuartEaseOut(float start, float end, float t)
    {
        t--;
        return -(end - start) * (t * t * t * t - 1) + start;
    }

    public static Vector2 QuartEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuartEaseOut(vec.x, end.x, t), QuartEaseOut(vec.y, end.y, t));
    }

    public static Vector3 QuartEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(QuartEaseOut(vec.x, end.x, t), QuartEaseOut(vec.y, end.y, t), QuartEaseOut(vec.z, end.z, t));
    }

    public static float QuartEaseInOut(float start, float end, float t)
    {
        t *= 2;
        if (t < 1)
            return (end - start) / 2 * t * t * t * t + start;
        t -= 2;
        return -(end - start) / 2 * (t * t * t * t - 2) + start;
    }

    public static Vector2 QuartEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuartEaseInOut(vec.x, end.x, t), QuartEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 QuartEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(QuartEaseInOut(vec.x, end.x, t), QuartEaseInOut(vec.y, end.y, t), QuartEaseInOut(vec.z, end.z, t));
    }

    //Quintic
    public static float QuintEaseIn(float start, float end, float t)
    {
        return (end - start) * t * t * t * t * t + start;
    }

    public static Vector2 QuintEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuintEaseIn(vec.x, end.x, t), QuintEaseIn(vec.y, end.y, t));
    }

    public static Vector3 QuintEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(QuintEaseIn(vec.x, end.x, t), QuintEaseIn(vec.y, end.y, t), QuintEaseIn(vec.z, end.z, t));
    }

    public static float QuintEaseOut(float start, float end, float t)
    {
        t--;
        return (end - start) * (t * t * t * t * t + 1) + start;
    }

    public static Vector2 QuintEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuintEaseOut(vec.x, end.x, t), QuintEaseOut(vec.y, end.y, t));
    }

    public static Vector3 QuintEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(QuintEaseOut(vec.x, end.x, t), QuintEaseOut(vec.y, end.y, t), QuintEaseOut(vec.z, end.z, t));
    }

    public static float QuintEaseInOut(float start, float end, float t)
    {
        t *= 2;
        if (t < 1)
            return (end - start) / 2 * t * t * t * t * t + start;
        t -= 2;
        return (end - start) / 2 * (t * t * t * t * t + 2) + start;
    }

    public static Vector2 QuintEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuintEaseInOut(vec.x, end.x, t), QuintEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 QuintEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(QuintEaseInOut(vec.x, end.x, t), QuintEaseInOut(vec.y, end.y, t), QuintEaseInOut(vec.z, end.z, t));
    }

    //Sinusoidal
    public static float SinEaseIn(float start, float end, float t)
    {
        return -(end - start) * Mathf.Cos(t * (Mathf.PI / 2)) + end + start;
    }

    public static Vector2 SinEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(QuadEaseIn(vec.x, end.x, t), QuadEaseIn(vec.y, end.y, t));
    }

    public static Vector3 SinEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(SinEaseIn(vec.x, end.x, t), SinEaseIn(vec.y, end.y, t), SinEaseIn(vec.z, end.z, t));
    }

    public static float SinEaseOut(float start, float end, float t)
    {
        return (end - start) * Mathf.Sin(t * (Mathf.PI / 2)) + start;
    }

    public static Vector2 SinEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(SinEaseOut(vec.x, end.x, t), SinEaseOut(vec.y, end.y, t));
    }

    public static Vector3 SinEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(SinEaseOut(vec.x, end.x, t), SinEaseOut(vec.y, end.y, t), SinEaseOut(vec.z, end.z, t));
    }

    public static float SinEaseInOut(float start, float end, float t)
    {
        return -(end - start) / 2 * (Mathf.Cos(Mathf.PI * t) - 1) + start;
    }

    public static Vector2 SinEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(SinEaseInOut(vec.x, end.x, t), SinEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 SinEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(SinEaseInOut(vec.x, end.x, t), SinEaseInOut(vec.y, end.y, t), SinEaseInOut(vec.z, end.z, t));
    }

    //Exponential 
    public static float ExpEaseIn(float start, float end, float t)
    {
        return (end - start) * Mathf.Pow(2, 10 * (t - 1)) + start;
    }

    public static Vector2 ExpEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(ExpEaseIn(vec.x, end.x, t), ExpEaseIn(vec.y, end.y, t));
    }

    public static Vector3 ExpEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(ExpEaseIn(vec.x, end.x, t), ExpEaseIn(vec.y, end.y, t), ExpEaseIn(vec.z, end.z, t));
    }

    public static float ExpEaseOut(float start, float end, float t)
    {
        return (end - start) * (-Mathf.Pow(2, -10 * t) + 1) + start;
    }

    public static Vector2 ExpEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(ExpEaseOut(vec.x, end.x, t), ExpEaseOut(vec.y, end.y, t));
    }

    public static Vector3 ExpEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(ExpEaseOut(vec.x, end.x, t), ExpEaseOut(vec.y, end.y, t), ExpEaseOut(vec.z, end.z, t));
    }

    public static float ExpEaseInOut(float start, float end, float t)
    {
        t *= 2;
        if (t < 1)
            return (end - start) / 2 * Mathf.Pow(2, 10 * (t - 1)) + start;
        t--;
        return (end - start) / 2 * (-Mathf.Pow(2, -10 * t) + 2) + start;
    }

    public static Vector2 ExpEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(ExpEaseInOut(vec.x, end.x, t), ExpEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 ExpEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(ExpEaseInOut(vec.x, end.x, t), ExpEaseInOut(vec.y, end.y, t), ExpEaseInOut(vec.z, end.z, t));
    }

    //Circular 
    public static float CircEaseIn(float start, float end, float t)
    {
        return -(end - start) * (Mathf.Sqrt(1 - t * t) - 1) + start;
    }

    public static Vector2 CircEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(CircEaseIn(vec.x, end.x, t), CircEaseIn(vec.y, end.y, t));
    }

    public static Vector3 CircEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(CircEaseIn(vec.x, end.x, t), CircEaseIn(vec.y, end.y, t), CircEaseIn(vec.z, end.z, t));
    }

    public static float CircEaseOut(float start, float end, float t)
    {
        t--;
        return (end - start) * Mathf.Sqrt(1 - t * t) + start;
    }

    public static Vector2 CircEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(CircEaseOut(vec.x, end.x, t), CircEaseOut(vec.y, end.y, t));
    }

    public static Vector3 CircEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(CircEaseOut(vec.x, end.x, t), CircEaseOut(vec.y, end.y, t), CircEaseOut(vec.z, end.z, t));
    }

    public static float CircEaseInOut(float start, float end, float t)
    {
        t *= 2;
        if (t < 1)
            return -(end - start) / 2 * (Mathf.Sqrt(1 - t * t) - 1) + start;
        t -= 2;
        return (end - start) / 2 * (Mathf.Sqrt(1 - t * t) + 1) + start;
    }

    public static Vector2 CircEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(CircEaseInOut(vec.x, end.x, t), CircEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 CircEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(CircEaseInOut(vec.x, end.x, t), CircEaseInOut(vec.y, end.y, t), CircEaseInOut(vec.z, end.z, t));
    }

    public static float BounceEaseIn(float start, float end, float t)
    {
        end -= start;
        float d = 1f;
        return end - BounceEaseOut(0, end, d - t) + start;
    }

    public static Vector2 BounceEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(BounceEaseIn(vec.x, end.x, t), BounceEaseIn(vec.y, end.y, t));
    }

    public static Vector3 BounceEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(BounceEaseIn(vec.x, end.x, t), BounceEaseIn(vec.y, end.y, t), BounceEaseIn(vec.z, end.z, t));
    }

    public static float BounceEaseOut(float start, float end, float t)
    {
        t /= 1f;
        end -= start;
        if (t < (1 / 2.75f))
        {
            return end * (7.5625f * t * t) + start;
        }
        else if (t < (2 / 2.75f))
        {
            t -= (1.5f / 2.75f);
            return end * (7.5625f * (t) * t + .75f) + start;
        }
        else if (t < (2.5 / 2.75))
        {
            t -= (2.25f / 2.75f);
            return end * (7.5625f * (t) * t + .9375f) + start;
        }
        else
        {
            t -= (2.625f / 2.75f);
            return end * (7.5625f * (t) * t + .984375f) + start;
        }
    }

    public static Vector2 BounceEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(BounceEaseOut(vec.x, end.x, t), BounceEaseOut(vec.y, end.y, t));
    }

    public static Vector3 BounceEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(BounceEaseOut(vec.x, end.x, t), BounceEaseOut(vec.y, end.y, t), BounceEaseOut(vec.z, end.z, t));
    }

    public static float BounceEaseInOut(float start, float end, float t)
    {
        end -= start;
        float d = 1f;
        if (t < d * 0.5f) return BounceEaseIn(0, end, t * 2) * 0.5f + start;
        else return BounceEaseOut(0, end, t * 2 - d) * 0.5f + end * 0.5f + start;
    }

    public static Vector2 BounceEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(BounceEaseInOut(vec.x, end.x, t), BounceEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 BounceEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(BounceEaseInOut(vec.x, end.x, t), BounceEaseInOut(vec.y, end.y, t), BounceEaseInOut(vec.z, end.z, t));
    }

    public static float BackEaseIn(float start, float end, float t)
    {
        end -= start;
        t /= 1;
        float s = 1.70158f;
        return end * (t) * t * ((s + 1) * t - s) + start;
    }

    public static Vector2 BackEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(BackEaseIn(vec.x, end.x, t), BackEaseIn(vec.y, end.y, t));
    }

    public static Vector3 BackEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(BackEaseIn(vec.x, end.x, t), BackEaseIn(vec.y, end.y, t), BackEaseIn(vec.z, end.z, t));
    }

    public static float BackEaseOut(float start, float end, float t)
    {
        float s = 1.70158f;
        end -= start;
        t = (t) - 1;
        return end * ((t) * t * ((s + 1) * t + s) + 1) + start;
    }

    public static Vector2 BackEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(BackEaseOut(vec.x, end.x, t), BackEaseOut(vec.y, end.y, t));
    }

    public static Vector3 BackEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(BackEaseOut(vec.x, end.x, t), BackEaseOut(vec.y, end.y, t), BackEaseOut(vec.z, end.z, t));
    }

    public static float BackEaseInOut(float start, float end, float t)
    {
        float s = 1.70158f;
        end -= start;
        t /= .5f;
        if ((t) < 1)
        {
            s *= (1.525f);
            return end * 0.5f * (t * t * (((s) + 1) * t - s)) + start;
        }
        t -= 2;
        s *= (1.525f);
        return end * 0.5f * ((t) * t * (((s) + 1) * t + s) + 2) + start;
    }

    public static Vector2 BackEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(BackEaseInOut(vec.x, end.x, t), BackEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 BackEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(BackEaseInOut(vec.x, end.x, t), BackEaseInOut(vec.y, end.y, t), BackEaseInOut(vec.z, end.z, t));
    }

    public static float ElasticEaseIn(float start, float end, float t)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s;
        float a = 0;

        if (t == 0) return start;

        if ((t /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        return -(a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + start;
    }

    public static Vector2 ElasticEaseIn(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(ElasticEaseIn(vec.x, end.x, t), ElasticEaseIn(vec.y, end.y, t));
    }

    public static Vector3 ElasticEaseIn(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(ElasticEaseIn(vec.x, end.x, t), ElasticEaseIn(vec.y, end.y, t), ElasticEaseIn(vec.z, end.z, t));
    }

    public static float ElasticEaseOut(float start, float end, float t)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s;
        float a = 0;

        if (t == 0) return start;

        if ((t /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p * 0.25f;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        return (a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + end + start);
    }

    public static Vector2 ElasticEaseOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(ElasticEaseOut(vec.x, end.x, t), ElasticEaseOut(vec.y, end.y, t));
    }

    public static Vector3 ElasticEaseOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(ElasticEaseOut(vec.x, end.x, t), ElasticEaseOut(vec.y, end.y, t), ElasticEaseOut(vec.z, end.z, t));
    }

    public static float ElasticEaseInOut(float start, float end, float t)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s;
        float a = 0;

        if (t == 0) return start;

        if ((t /= d * 0.5f) == 2) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        if (t < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + start;
        return a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
    }

    public static Vector2 ElasticEaseInOut(Vector2 vec, Vector2 end, float t)
    {
        return new Vector2(ElasticEaseInOut(vec.x, end.x, t), ElasticEaseInOut(vec.y, end.y, t));
    }

    public static Vector3 ElasticEaseInOut(Vector3 vec, Vector3 end, float t)
    {
        return new Vector3(ElasticEaseInOut(vec.x, end.x, t), ElasticEaseInOut(vec.y, end.y, t), ElasticEaseInOut(vec.z, end.z, t));
    }

    //Bezier
    //y=u0(1−x)3+3u1(1−x)2x+3u2(1−x)x2+u3x3

    /// <summary>
    /// Evaluate the t in y at t [0,1] given the start and end handles
    /// </summary>
    /// <param name="start">Y start t</param>
    /// <param name="startHandle">Position of the first handle</param>
    /// <param name="end">Y end t</param>
    /// <param name="endHandle">Position of the end handle</param>
    /// <param name="t">Porgression</param>
    /// <returns>Evaluation of the t in y at t on the bezier curve</returns>
    public static float Bezier(float start, Vector2 startHandle, float end, Vector2 endHandle, float t)
    {
        float u = 1 - t;

        float b = (u * u * u) * startHandle.x + 3 * (u * u) * t * startHandle.y + 3 * u * (t * t) * endHandle.x + (t * t * t) * endHandle.y;

        return (end - start) * b + start;
    }

    /// <summary>
    /// Evaluate the t in y at t [0,1] given the start and end weights (Y t of the bezier handles aligned vertically with their points)
    /// </summary>
    /// <param name="start">Y start t</param>
    /// <param name="startWeight">Y Position of the first handle</param>
    /// <param name="end">Y end t</param>
    /// <param name="endWeight">Y Position of the end handle</param>
    /// <param name="t">Progression</param>
    /// <returns>Evaluation of the t in y at on the bezier curve</returns>
    public static float Bezier(float start, float startWeight, float end, float endWeight, float t)
    {
        float u = 1 - t;

        float b = 3 * (u * u) * t * startWeight + (t * t * t) * endWeight;

        return (end - start) * b + start;

    }
    #endregion
}

#if UNITY_EDITOR
/// <summary>
/// Monobehaviour that draws the curves on screen, for debugging only.
/// </summary>
public class CurvesLibrary : MonoBehaviour
{
    [SerializeField] Curves.FunctionType functionToView;
    const int SAMPLES = 100;
    const float HEIGHT = 10;
    const float LENGTH = 10;

    // Update is called once per frame
    void OnDrawGizmos()
    {
        //Draw the box where the curve will be displayed
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position + Vector3.zero, transform.position + Vector3.up * HEIGHT);
        Gizmos.DrawLine(transform.position + new Vector3(0, HEIGHT), transform.position + new Vector3(LENGTH, HEIGHT));
        Gizmos.DrawLine(transform.position + Vector3.zero, transform.position + Vector3.right * LENGTH);
        Gizmos.DrawLine(transform.position + new Vector3(LENGTH, 0), transform.position + new Vector3(LENGTH, HEIGHT));

        //Draw the function
        Gizmos.color = Color.red;
        Curves.Function<float> method = Curves.GetFunctionFloat(functionToView);
        DrawPointArray(GetPointsFromFunction(method, SAMPLES));
    }

    /// <summary>
    /// Calculate the position of a {precision} number of points using the method and store them into a vector3 array
    /// </summary>
    /// <param name="method">Method to use</param>
    /// <param name="precision">Number of samples</param>
    static float[] GetPointsFromFunction(Curves.Function<float> method, int precision)
    {
        float[] points = new float[precision];
        for (int i = 0; i < precision; i++)
        {
            //calculate the y coordinate of the point of x = t
            points[i] = method(0f, 1f, i / (precision - 1f));
        }
        return points;
    }

    /// <summary>
    /// Draw a curve from an array of points in gizmos.
    /// </summary>
    /// <param name="points">Points to draw</param>
    void DrawPointArray(float[] points)
    {
        Vector3 lastPoint = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            //Extract the x coordinate and calcul the y coordinate from the index i
            Vector3 currentPoint = new Vector3(i / (SAMPLES - 1f) * LENGTH, points[i] * HEIGHT);
            Gizmos.DrawLine(transform.position + lastPoint, transform.position + currentPoint);
            lastPoint = currentPoint;
        }
    }
}
#endif