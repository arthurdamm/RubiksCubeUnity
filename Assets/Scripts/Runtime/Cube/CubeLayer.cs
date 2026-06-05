using System.ComponentModel;
using UnityEngine;

// IsExternalInit definition is necessary to allow "init" prop accessors in Unity C#
// https://docs.unity3d.com/6000.3/Documentation/Manual/csharp-compiler.html
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit {}
}

public enum CubeNotation
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

public readonly struct CubeLayer
{
    public CubeLayer(CubeAxis axis, int index)
    {
        Axis = axis;
        Index = index;
    }
    
    public CubeAxis Axis { get; init; }
    
    // The index of this layer's axis in a cube[x,y,z] grid
    public int Index { get; init; }
}

public readonly struct CubeLayerRotation
{
    public CubeLayerRotation(CubeLayer layer, float degrees)
    {
        Layer = layer;
        Degrees = degrees;
    }
    
    public CubeLayer Layer { get; init; }
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