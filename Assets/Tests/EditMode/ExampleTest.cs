using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExampleTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void ExampleTestSimplePasses()
    {
        // Use the Assert class to test conditions
        int integer = 1;
        Assert.AreEqual(1, integer);
    }
}
