using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour
{
    public BoolType isGameOver;
    // Start is called before the first frame update

    public void RestartGame()
    {
        isGameOver.value = false;
        SceneManager.LoadScene(0);
    }
    
}
