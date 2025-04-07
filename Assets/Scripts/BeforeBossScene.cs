using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeBossScene : MonoBehaviour
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
    public GameObject Dialogue10;
	public GameObject Dialogue11;
	public GameObject Dialogue12;
	public GameObject Dialogue13;
	public GameObject Dialogue14;
	public GameObject Dialogue15;
	public GameObject Dialogue16;
	public GameObject Dialogue17;
    public GameObject Dialogue18;
    public GameObject Dialogue19;
	public GameObject Dialogue20;
    public GameObject Dialogue21;
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
                Dialogue9.SetActive(true);
                Dialogue10.SetActive(true);
                break;
            case 10:
                Dialogue10.SetActive(false);
                Dialogue11.SetActive(true);
                break;
            case 11:
                Dialogue11.SetActive(false);
                Dialogue12.SetActive(true);
                break;
            case 12:
                Dialogue12.SetActive(false);
                Dialogue13.SetActive(true);
                break;
            case 13:
                Dialogue13.SetActive(false);
                Dialogue14.SetActive(true);
                break;
            case 14:
                Dialogue14.SetActive(false);
                Dialogue15.SetActive(true);
                break;
            case 15:
                Dialogue15.SetActive(false);
                Dialogue16.SetActive(true);
                break;
            case 16:
                Dialogue16.SetActive(false);
                Dialogue17.SetActive(true);
                break;
            case 17:
                Dialogue17.SetActive(false);
                Dialogue18.SetActive(true);
                break;
            case 18:
                Dialogue18.SetActive(false);
                Dialogue19.SetActive(true);
                break;
            case 19:
                Dialogue19.SetActive(false);
                Dialogue20.SetActive(true);
                break;
            case 20:
                Dialogue20.SetActive(false);
                Dialogue21.SetActive(true);
                break;
            case 21:
                Dialogue21.SetActive(false);
                SceneManager.LoadScene("Level5");
                break;
        }
    }
}
