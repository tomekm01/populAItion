using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasChangerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas welcomeScreenCanvas;
    public Canvas gameCanvas;
    public Canvas pauseCanvas;
    void Start()
    {
        gameCanvas.enabled = false;
        pauseCanvas.enabled = false;
        welcomeScreenCanvas.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseCanvas.enabled == false)
        {
            pauseCanvas.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseCanvas.enabled == true)
        {
            pauseCanvas.enabled = false;
        }
    }
    public void resumeGameButton()
    {
        pauseCanvas.enabled = false;
    }
    public void switchToGameCanvas()
    {
        gameCanvas.enabled = true;
        welcomeScreenCanvas.enabled = false;
        pauseCanvas.enabled = false;
    }

    public void switchToWelcomeScreen()
    {
        welcomeScreenCanvas.enabled = true;
        gameCanvas.enabled = false;
        pauseCanvas.enabled = false;
    }

    public void quitApplication()
    {
        Application.Quit();
    }
}
