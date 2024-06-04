using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu_Behavior : MonoBehaviour
{

    public GameObject menu;
    public GameObject leftRayInteractor;
    public GameObject rightRayInteractor;

    public bool isMenuOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        DisplayMenuUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayMenuUI();
        }
    }

    public void DisplayMenuUI()
    {
        if (isMenuOpened)
        {
            menu.SetActive(false);
            isMenuOpened = false;
            leftRayInteractor.SetActive(false);
            rightRayInteractor.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            menu.SetActive(true);
            isMenuOpened = true;
            Time.timeScale = 0;
            leftRayInteractor.SetActive(true);
            rightRayInteractor.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
