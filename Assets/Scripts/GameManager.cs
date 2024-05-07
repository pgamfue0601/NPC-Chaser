using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int difficulty;
    public int booksToBeFound;
    public GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Easy()
    {
        difficulty = 0;
        booksToBeFound = 2;
    }
    
    public void Medium()
    {
        difficulty = 1;
        booksToBeFound = 4;
    }

    public void Hard()
    {
        difficulty = 2;
        booksToBeFound = 6;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
