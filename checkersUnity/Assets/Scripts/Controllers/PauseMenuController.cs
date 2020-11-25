using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    public GameObject pauseMenu;
    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Apertou scape");

            paused = !paused;
            Menu();

        }
    }

    void Menu()
    {
        if (paused)
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
            //Screen.showCursor = false;
            //Screen.lockCursor = true;
            //Camera.audio.Play();
            //paused = false;
        }
        else
        {
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
            /*
            Time.timeScale = 0.0f;
            Canvas.gameObject.SetActive(true);
            Screen.showCursor = true;
            Screen.lockCursor = false;
            Camera.audio.Pause();
            paused = true;
            */
        }
    }
    public void BtnResumeClicked()
    {
        paused = false;
        Menu();
    }

    public void BtnResetClicked()
    {
        //Reinicia scene
        //paused = false;
        //Menu();
        //Application.LoadLevel(Application.loadedLevel);
        //Scene scene = SceneManager.GetActiveScene(); 
        //SceneManager.LoadScene(scene.name);
    }

    public void BtnExitClicked()
    {
        //Voltar scene
        //paused = false;
    }
}
