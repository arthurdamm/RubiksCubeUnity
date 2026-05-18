using NUnit.Framework;
using UnityEngine.TestTools.Utils;
using UnityEngine;

public class QuaternionTests
{
    [Test]
    public void RotateXClockwise90DegreesAroundY_ReturnsNegativeZ()
    {
        Vector3 vector = new Vector3(1, 0, 0);
        Quaternion rotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
        Vector3 result = rotation * vector;
        var comparer = new Vector3EqualityComparer(1e-6f);
        Assert.That(result, Is.EqualTo(new Vector3(0, 0, -1)).Using(comparer));
    }
    
    [Test]
    public void RotateXCounterClockwise90DegreesAroundY_ReturnsZ()
    {
        Vector3 vector = new Vector3(1, 0, 0);
        Quaternion rotation = Quaternion.AngleAxis(-90, new Vector3(0, 1, 0));
        Vector3 result = rotation * vector;
        var comparer = new Vector3EqualityComparer(1e-6f);
        Assert.That(result, Is.EqualTo(new Vector3(0, 0, 1)).Using(comparer));
    }
    
    [Test]
    public void RotateYClockwise90DegreesAroundZ_ReturnsNegativeX()
    {
        Vector3 vector = new Vector3(0, 1, 0);
        Quaternion rotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
        Vector3 result = rotation * vector;
        var comparer = new Vector3EqualityComparer(1e-6f);
        Assert.That(result, Is.EqualTo(new Vector3(-1, 0, 0)).Using(comparer));
    }
    
    [Test]
    public void RotateZCounterClockwise90DegreesAroundNegativeY_ReturnsNegativeX()
    {
        Vector3 vector = new Vector3(0, 0, 1);
        Quaternion rotation = Quaternion.AngleAxis(90, new Vector3(0, -1, 0));
        Vector3 result = rotation * vector;
        var comparer = new Vector3EqualityComparer(1e-6f);
        Assert.That(result, Is.EqualTo(new Vector3(-1, 0, 0)).Using(comparer));
    }
    
    [Test]
    public void RotateZClockwise90DegreesAroundY_ReturnsX()
    {
        Vector3 vector = new Vector3(0, 0, 1);
        Quaternion rotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
        Vector3 result = rotation * vector;
        var comparer = new Vector3EqualityComparer(1e-6f);
        Assert.That(result, Is.EqualTo(new Vector3(1, 0, 0)).Using(comparer));
    }
    
    [Test]
    public void RotateNegativeZClockwise90DegreesAroundY_ReturnsNegativeX()
    {
        Vector3 vector = new Vector3(0, 0, -1);
        Quaternion rotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
        Vector3 result = rotation * vector;
        var comparer = new Vector3EqualityComparer(1e-6f);
        Assert.That(result, Is.EqualTo(new Vector3(-1, 0, 0)).Using(comparer));
    }

    
}
