using NUnit.Framework;
using UnityEngine;

namespace Tests
{
public class CubeDirectionsResolverTests
{
    [Test]
    public void TestRightDirection()
    {
        CubeDirectionsResolver cubeDirections = new();

        var actual = cubeDirections.QuantizeAxialDirection(Vector3.right);
        var expected = new CubeDirection(CubeAxis.X, 1);
        Assert.That(actual, Is.EqualTo(expected));
        
    }
    
    [Test]
    public void TestLeftDirection()
    {
        CubeDirectionsResolver cubeDirections = new();

        var actual = cubeDirections.QuantizeAxialDirection(-Vector3.right);
        var expected = new CubeDirection(CubeAxis.X, -1);
        Assert.That(actual, Is.EqualTo(expected));
        
    }
    
    [Test]
    public void TestUpDirection()
    {
        CubeDirectionsResolver cubeDirections = new();

        var actual = cubeDirections.QuantizeAxialDirection(Vector3.up);
        var expected = new CubeDirection(CubeAxis.Y, 1);
        Assert.That(actual, Is.EqualTo(expected));
        
    }
    
    [Test]
    public void TestDownDirection()
    {
        CubeDirectionsResolver cubeDirections = new();

        var actual = cubeDirections.QuantizeAxialDirection(-Vector3.up);
        var expected = new CubeDirection(CubeAxis.Y, -1);
        Assert.That(actual, Is.EqualTo(expected));
        
    }
    
    [Test]
    public void TestForwardDirection()
    {
        CubeDirectionsResolver cubeDirections = new();

        var actual = cubeDirections.QuantizeAxialDirection(Vector3.forward);
        var expected = new CubeDirection(CubeAxis.Z, 1);
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void TestBackwardDirection()
    {
        CubeDirectionsResolver cubeDirections = new();

        var actual = cubeDirections.QuantizeAxialDirection(Vector3.forward);
        var expected = new CubeDirection(CubeAxis.Z, 1);
        Assert.That(actual, Is.EqualTo(expected));
    }
    

}
}