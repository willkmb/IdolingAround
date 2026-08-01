using UnityEngine;

public class LevelSelectScript : MonoBehaviour
{
    [SerializeField] GameObject spawn;
    [SerializeField] GameObject idol;
    [SerializeField] GameObject tut;
    private void Start()
    {
        if (MenuScript.jungleStart)
        {
            Debug.Log("Jungle Start");
        }
        else if (MenuScript.corridorStart)
        {
            Debug.Log("CorridorStart");
            Invoke("moveIdol", 0.01f);
        }
        else
        {
            Debug.Log("Regular Start");
        }
    }

    void moveIdol()
    {
        idol.transform.position = spawn.transform.position;
        tut.SetActive(false);
    }
}
