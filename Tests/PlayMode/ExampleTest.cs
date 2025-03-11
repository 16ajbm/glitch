using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExampleTest
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator Heal()
    {
        var gameObject = new GameObject();

        var character = gameObject.AddComponent<Character>();

        character.maxHealth = 10;
        character.currentHealth = 5;

        var turnManager = gameObject.AddComponent<TurnManager>();

        turnManager.characters = new System.Collections.Generic.List<Character> { character };

        // Simulating a heal-over-time effect
        character.Heal(5);

        // Wait for a frame to simulate Play Mode behavior
        yield return null;

        Assert.AreEqual(10, character.currentHealth);
    }
}
