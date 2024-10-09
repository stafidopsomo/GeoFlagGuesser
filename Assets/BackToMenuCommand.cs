using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenuCommand : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
