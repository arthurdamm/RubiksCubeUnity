using System;
using NUnit.Framework;
using UnityEngine;

using Random = UnityEngine.Random;

public class BasicRotationTests
{
    [Test]
    public void Test_RotateX90Degrees_point()
    {
        var actual = Rotations.RotateX90Degrees(new Vector3Int(1, 1, 1));
        var expected = new Vector3Int(1, 1, -1);
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void Test_RotateY90Degrees_point()
    {
        var actual = Rotations.RotateY90Degrees(new Vector3Int(1, 1, 1));
        var expected = new Vector3Int(-1, 1, 1);
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void Test_RotateZ90Degrees_point()
    {
        var actual = Rotations.RotateZ90Degrees(new Vector3Int(1, 1, 1));
        var expected = new Vector3Int(1, -1, 1);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test_Inverses_RandomPoint()
    {
        var randomPoint = new Vector3Int(Random.Range(-10, 11), Random.Range(-10, 11), Random.Range(-10, 11));

        var shouldBeEqual = Rotations.RotateX90Degrees(Rotations.RotateX90Degrees(randomPoint), false);
        Assert.That(shouldBeEqual, Is.EqualTo(randomPoint));
        
        shouldBeEqual = Rotations.RotateY90Degrees(Rotations.RotateY90Degrees(randomPoint), false);
        Assert.That(shouldBeEqual, Is.EqualTo(randomPoint));
        
        shouldBeEqual = Rotations.RotateZ90Degrees(Rotations.RotateZ90Degrees(randomPoint), false);
        Assert.That(shouldBeEqual, Is.EqualTo(randomPoint));
    }

    [Test]
    public void Test_FourRotations_Identity()
    {
        var randomPoint = new Vector3Int(Random.Range(-100, 101), Random.Range(-100, 101), Random.Range(-100, 101));

        Func<Vector3Int, bool, Vector3Int>[] rotations = { Rotations.RotateX90Degrees, Rotations.RotateY90Degrees, Rotations.RotateZ90Degrees };

        foreach (var rotation in rotations)
        {
            var result = randomPoint;
            var resultCCW = randomPoint;
            
            for (int i = 1; i <= 4; i++)
            {
                result = rotation(result, true);
                resultCCW = rotation(resultCCW, false);
            }
            Assert.That(result, Is.EqualTo(randomPoint));
            Assert.That(resultCCW, Is.EqualTo(randomPoint));
        }
        
    }
}
