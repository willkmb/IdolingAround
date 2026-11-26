using UnityEngine;
using UnityEngine.SceneManagement;


public class TestLevelButton : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene(5);
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
