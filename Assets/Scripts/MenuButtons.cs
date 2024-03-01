using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
