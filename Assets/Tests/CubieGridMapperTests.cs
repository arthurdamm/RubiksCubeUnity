using NUnit.Framework;
using UnityEngine;

public class CubieGridMapperTests
{
    [Test]
    public void TestInverse()
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
        var cubeLayout = new CubeLayout(3, 0f, bounds); 

        CubieGridMapper mapper = new CubieGridMapper(cubeLayout);

        var original = new Vector3Int(0, 0, 0);
        Assert.That(mapper.LocalPositionToGridIndex(mapper.GridIndexToLocalPosition(original)), Is.EqualTo(original));
        
        original = new Vector3Int(1, 1, 1);
        Assert.That(mapper.LocalPositionToGridIndex(mapper.GridIndexToLocalPosition(original)), Is.EqualTo(original));
        
        original = new Vector3Int(-1, -1, -1);
        Assert.That(mapper.LocalPositionToGridIndex(mapper.GridIndexToLocalPosition(original)), Is.EqualTo(original));
        
        original = new Vector3Int(1, 0, -1);
        Assert.That(mapper.LocalPositionToGridIndex(mapper.GridIndexToLocalPosition(original)), Is.EqualTo(original));
        
    }
    
    [Test]
    public void TestInverse_WithSizeAndPadding()
    {
        Bounds bounds = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(1.01f, 1.01f, 1.01f));
        var cubeLayout = new CubeLayout(3, 0.1f, bounds); 

        CubieGridMapper mapper = new CubieGridMapper(cubeLayout);

        var original = new Vector3Int(0, 0, 0);
        Assert.That(mapper.LocalPositionToGridIndex(mapper.GridIndexToLocalPosition(original)), Is.EqualTo(original));
        
        original = new Vector3Int(1, 1, 1);
        Assert.That(mapper.LocalPositionToGridIndex(mapper.GridIndexToLocalPosition(original)), Is.EqualTo(original));
        
        original = new Vector3Int(-1, -1, -1);
        Assert.That(mapper.LocalPositionToGridIndex(mapper.GridIndexToLocalPosition(original)), Is.EqualTo(original));
        
        original = new Vector3Int(1, 0, -1);
        Assert.That(mapper.LocalPositionToGridIndex(mapper.GridIndexToLocalPosition(original)), Is.EqualTo(original));
        
    }
}
