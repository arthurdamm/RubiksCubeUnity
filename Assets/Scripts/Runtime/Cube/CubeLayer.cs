using UnityEngine;

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

    public override string ToString() => $"CubeLayer(Axis={Axis}, Index={Index})";
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

    public override string ToString() => $"CubeLayerRotation(Layer={Layer}, Degrees={Degrees})";
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
    
    public override string ToString() => $"CubeLayout(CubeSize={CubeSize}, CubiePadding={CubiePadding} CubieBounds={CubieBounds})";
}