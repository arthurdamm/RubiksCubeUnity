using NUnit.Framework;
using UnityEngine.TestTools.Utils;
using UnityEngine;

public class SixCubeDirectionsBasic
{
     [Test]
    public void TestRightDirection()
    {
        var go = new GameObject();
        CubeDirectionsResolver cubeDirections = new(go.transform);

        var actual = cubeDirections.QuantizeAxialDirection(Vector3.right);
        var expected = new CubeDirection(CubeAxis.X, 1);
        Assert.That(actual, Is.EqualTo(expected));
        
    }
    
    [Test]
    public void TestLeftDirection()
    {
        var go = new GameObject();
        CubeDirectionsResolver cubeDirections = new(go.transform);

        var actual = cubeDirections.QuantizeAxialDirection(-Vector3.right);
        var expected = new CubeDirection(CubeAxis.X, -1);
        Assert.That(actual, Is.EqualTo(expected));
        
    }
    
    [Test]
    public void TestUpDirection()
    {
        var go = new GameObject();
        CubeDirectionsResolver cubeDirections = new(go.transform);

        var actual = cubeDirections.QuantizeAxialDirection(Vector3.up);
        var expected = new CubeDirection(CubeAxis.Y, 1);
        Assert.That(actual, Is.EqualTo(expected));
        
    }
    
    [Test]
    public void TestDownDirection()
    {
        var go = new GameObject();
        CubeDirectionsResolver cubeDirections = new(go.transform);

        var actual = cubeDirections.QuantizeAxialDirection(-Vector3.up);
        var expected = new CubeDirection(CubeAxis.Y, -1);
        Assert.That(actual, Is.EqualTo(expected));
        
    }
    
    [Test]
    public void TestForwardDirection()
    {
        var go = new GameObject();
        CubeDirectionsResolver cubeDirections = new(go.transform);

        var actual = cubeDirections.QuantizeAxialDirection(Vector3.forward);
        var expected = new CubeDirection(CubeAxis.Z, 1);
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void TestBackwardDirection()
    {
        var go = new GameObject();
        CubeDirectionsResolver cubeDirections = new(go.transform);

        var actual = cubeDirections.QuantizeAxialDirection(-Vector3.forward);
        var expected = new CubeDirection(CubeAxis.Z, -1);
        Assert.That(actual, Is.EqualTo(expected));
    }
}
