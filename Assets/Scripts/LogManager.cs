using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LogManager : MonoBehaviour
{
    public Transform logContent;      // Content parent in Scroll View
    public GameObject logPanel;  // Panel to display the log
    public Button toggleLogButton;
    public Button closeLogButton;
    public Text logText;


    private List<string> logEntries = new List<string>();
    private void Start()
    {
        if (logContent == null)
        {
            Debug.LogError("LogManager is missing UI references.");
            return;
        }
        toggleLogButton.onClick.AddListener(ToggleLogPanel);
        closeLogButton.onClick.AddListener(CloseLogPanel);
        logPanel.SetActive(false);
    }

    public void AddLogEntry(string message)
    {
        Debug.Log("Adding log entry");
        // Store the message and update the UI display
        logEntries.Add(message);
        UpdateLogDisplay();
    }
    private void UpdateLogDisplay()
    {
        logText.text = string.Join("\n", logEntries);
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
