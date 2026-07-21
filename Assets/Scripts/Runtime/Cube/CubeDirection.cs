using System;
using UnityEngine;

public readonly struct CubeDirection
{
    public readonly CubeAxis Axis { get; init; }
    public readonly int Sign { get; init; }

    public CubeDirection(CubeAxis axis, int sign)
    {
        Axis = axis;
        Sign = sign;
    }

    public Vector3Int Vector => Axis switch
    {
        CubeAxis.X => new Vector3Int(Sign, 0, 0),
        CubeAxis.Y => new Vector3Int(0, Sign, 0),
        CubeAxis.Z => new Vector3Int(0, 0, Sign),
        _ => throw new ArgumentOutOfRangeException()
    };
    
    public override string ToString() => $"CubeDirection(Axis={Axis}, Sign={Sign})";
}
