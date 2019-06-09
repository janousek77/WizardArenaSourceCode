// Pause Menu Script
// Camden Brewster

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;

    public GameObject pauseMenu;
	
	// Update is called once per frame
	void Update ()
    {
		Cursor.visible = Paused;                  //Removes cursor visability

		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
	}

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
    }
}
