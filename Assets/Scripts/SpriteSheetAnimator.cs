using UnityEngine;
using System.Collections.Generic;

public class SpriteSheetAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 0.12f;

    public float lifetime = 2.0f; // Lifetime of the animation in seconds

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Set initial sprite
        spriteRenderer.sprite = frames[0];
        Destroy(gameObject, lifetime); // auto-destroy when done
    }

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
        }
    }
}
