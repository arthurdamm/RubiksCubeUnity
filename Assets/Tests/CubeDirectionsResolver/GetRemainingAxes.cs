using NUnit.Framework;
using UnityEngine;


[TestFixture]
public class GetRemainingAxes
{
    private GameObject _cube;
    private CubeBuilder _cubeBuilder;
    private CubeDirectionsResolver _directionsResolver;
    
    [SetUp]
    public void SetUp()
    {
        _cube = new GameObject("TestCube");
        _cube.AddComponent<CubeBuilder>();
        _cubeBuilder = _cube.GetComponent<CubeBuilder>();
        _directionsResolver = new CubeDirectionsResolver(_cube.transform);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_cube);
    }
    
    [Test]
    public void GivenXAndY_ReturnsZ()
    {
        var actual = _directionsResolver.GetRemainingAxis(CubeAxis.X, CubeAxis.Y);
        var expected = CubeAxis.Z;
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void GivenXAndZ_ReturnsY()
    {
        var actual = _directionsResolver.GetRemainingAxis(CubeAxis.X, CubeAxis.Z);
        var expected = CubeAxis.Y;
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void GivenZAndY_ReturnsX()
    {
        var actual = _directionsResolver.GetRemainingAxis(CubeAxis.Z, CubeAxis.Y);
        var expected = CubeAxis.X;
        Assert.That(actual, Is.EqualTo(expected));
    }
    
}
