// IsExternalInit definition is necessary to allow "init" prop accessors
// https://docs.unity3d.com/6000.3/Documentation/Manual/csharp-compiler.html
using System.ComponentModel;
using UnityEngine;

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit{}
}


public enum CubeLayer
{
    L,
    X,
    R,

    F,
    Y,
    B,
    
    D,
    Z,
    U,
}

public enum CubeAxis
{
    X,
    Y,
    Z
}

public readonly struct CubeLayerGeneral
{
    public CubeLayerGeneral(CubeAxis axis, int index)
    {
        Axis = axis;
        Index = index;
    }
    
    public CubeAxis Axis { get; init; }
    public int Index { get; init; }
}

public readonly struct CubeLayerRotation
{
    public CubeLayerRotation(CubeLayerGeneral layer, float degrees)
    {
        Layer = layer;
        Degrees = degrees;
    }
    
    public CubeLayerGeneral Layer { get; init; }
    public float Degrees { get; init; }
}

public readonly struct CubeLayout
{
    public CubeLayout(int cubeSize, float cubiePadding, Bounds cubieBounds)
    {
        CubeSize = cubeSize;
        CubiePadding = cubiePadding;
        CubieBounds = cubieBounds;
    }
    
    public int CubeSize { get; init; }
    public float CubiePadding { get; init; }
    public Bounds CubieBounds { get; init; }
}