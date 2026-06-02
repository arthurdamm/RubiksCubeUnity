// IsExternalInit definition is necessary to allow "init" prop accessors
// https://docs.unity3d.com/6000.3/Documentation/Manual/csharp-compiler.html
using System.ComponentModel;
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
    public CubeLayerGeneral(CubeAxis axis, int offset)
    {
        Axis = axis;
        Offset = offset;
    }
    
    public CubeAxis Axis { get; init; }
    public int Offset { get; init; }
}

public readonly struct CubeLayerRotation
{
    public CubeLayerGeneral Layer { get; init; }
    public float Degrees { get; init; }
}