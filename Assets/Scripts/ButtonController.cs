using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Source: https://www.youtube.com/watch?v=cZzf1FQQFA0&list=PLLPYMaP0tgFKZj5VG82316B63eet0Pvsv
    private SpriteRenderer theSR;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress;

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            theSR.sprite = pressedImage;
        }

        if (Input.GetKeyUp(keyToPress))
        {
            theSR.sprite = defaultImage;
        }
    }
}
