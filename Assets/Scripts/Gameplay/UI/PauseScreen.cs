using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject Pause_Menu;   

    bool GameIsPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Pause_Menu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Pause_Menu.SetActive(false);
        Time.timeScale = 1f;
    }
}
