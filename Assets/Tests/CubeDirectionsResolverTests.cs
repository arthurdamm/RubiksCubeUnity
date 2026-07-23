using NUnit.Framework;
using UnityEngine.TestTools.Utils;
using UnityEngine;

namespace Tests
{
public class CubeDirectionsResolverTests
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

        var actual = cubeDirections.QuantizeAxialDirection(Vector3.forward);
        var expected = new CubeDirection(CubeAxis.Z, 1);
        Assert.That(actual, Is.EqualTo(expected));
    }
}

public class FindMatchingDirectionTests
{
    [Test]
    public void TestMatchingDirections_FindsPositive()
    {
        Camera camera = new Camera();
        GameObject go = new GameObject();
        go.AddComponent<CubeBuilder>();
        CubeBuilder cubeBuilder = go.GetComponent<CubeBuilder>();
        var directionsResolver = new CubeDirectionsResolver(go.transform);
        var comparer = new Vector3EqualityComparer(10e-6f);
        
        Vector3[] candidates = new[] { Vector3.forward, Vector3.up, Vector3.right };
        var expected = Vector3.up;
        var actual = directionsResolver.FindMatchingDirection(expected, candidates);
        
        Assert.That(actual, Is.EqualTo(expected).Using(comparer));
    }
    
    [Test]
    public void TestMatchingDirections_FindsNegative()
    {
        Camera camera = new Camera();
        GameObject go = new GameObject();
        go.AddComponent<CubeBuilder>();
        CubeBuilder cubeBuilder = go.GetComponent<CubeBuilder>();
        var directionsResolver = new CubeDirectionsResolver(go.transform);
        var comparer = new Vector3EqualityComparer(10e-6f);
        
        Vector3[] candidates = new[] { Vector3.forward, Vector3.up, Vector3.right };
        var expected = -Vector3.up;
        var actual = directionsResolver.FindMatchingDirection(expected, candidates);
        
        Assert.That(actual, Is.EqualTo(expected).Using(comparer));
    }
    
    [Test]
    public void TestMatchingDirections_FindsNegativeOffset()
    {
        Camera camera = new Camera();
        GameObject go = new GameObject();
        go.AddComponent<CubeBuilder>();
        CubeBuilder cubeBuilder = go.GetComponent<CubeBuilder>();
        var directionsResolver = new CubeDirectionsResolver(go.transform);
        var comparer = new Vector3EqualityComparer(10e-6f);
        
        Vector3[] candidates = new[] { Vector3.forward, Vector3.up, Vector3.right };
        var expected = -Vector3.up;
        var actual = directionsResolver.FindMatchingDirection(new Vector3(0.1f, -.9f, 0.1f), candidates);
        
        Assert.That(actual, Is.EqualTo(expected).Using(comparer));
    }
}


}

