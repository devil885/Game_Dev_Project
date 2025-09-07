using UnityEngine;
using UnityEngine.SceneManagement;

public class SeceneController : MonoBehaviour
{
    public void SceneChange(string name) 
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1;
    } 
}
