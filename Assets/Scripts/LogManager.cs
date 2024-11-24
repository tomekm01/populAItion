using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class LogManager : MonoBehaviour
{
    public GameObject logPanel;  
    public Button toggleLogButton;
    public Button closeLogButton;
    public ScrollRect logView;
    public Text logText;


    private List<string> logEntries = new List<string>();
    private void Start()
    {
        toggleLogButton.onClick.AddListener(ToggleLogPanel);
        closeLogButton.onClick.AddListener(CloseLogPanel);
        logPanel.SetActive(false);
    }

    public void AddLogEntry(string message)
    {
        Debug.Log("Adding log entry");
        logEntries.Add(message);
        UpdateLogDisplay();
        Debug.Log("Log entries updated");
    }
    private void UpdateLogDisplay()
    {
        logText.text = string.Join("\n", logEntries);
        Canvas.ForceUpdateCanvases();
        logView.verticalNormalizedPosition = 0f;
    }
    private void ToggleLogPanel()
    {
        logPanel.SetActive(true);
    }

    private void CloseLogPanel()
    {
        logPanel.SetActive(false);
    }
}
