using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueScene : MonoBehaviour
{
    private int tipIndex = 0;
	public GameObject Dialogue1;
	public GameObject Dialogue2;
	public GameObject Dialogue3;
	public GameObject Dialogue4;
	public GameObject Dialogue5;
	public GameObject Dialogue6;
	public GameObject Dialogue7;
	public GameObject Dialogue8;
    public GameObject Dialogue9;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            tipIndex++;
        }

        switch (tipIndex)
        {
            case 0:
                Dialogue1.SetActive(true);
                break;
            case 1:
                Dialogue1.SetActive(false);
                Dialogue2.SetActive(true);
                break;
            case 2:
                Dialogue2.SetActive(false);
                Dialogue3.SetActive(true);
                break;
            case 3:
                Dialogue3.SetActive(false);
                Dialogue4.SetActive(true);
                break;
            case 4:
                Dialogue4.SetActive(false);
                Dialogue5.SetActive(true);
                break;
            case 5:
                Dialogue5.SetActive(false);
                Dialogue6.SetActive(true);
                break;
            case 6:
                Dialogue6.SetActive(false);
                Dialogue7.SetActive(true);
                break;
            case 7:
                Dialogue7.SetActive(false);
                Dialogue8.SetActive(true);
                break;
            case 8:
                Dialogue8.SetActive(false);
                Dialogue9.SetActive(true);
                break;
            case 9:
                Dialogue9.SetActive(false);
                SceneManager.LoadScene("LevelSelect");
                break;
        }
            
    }
}
