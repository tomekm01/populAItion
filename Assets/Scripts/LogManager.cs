using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    // Reference to UI Text for displaying logs
    public Text logText;  
    public GameObject logPanel;  // Panel to display the log
    public Button toggleLogButton;
    public Button closeLogsButton;

    private List<string> logEntries = new List<string>();

    private void Start()
    {
        // Toggle log panel on button click
        toggleLogButton.onClick.AddListener(ToggleLogPanel);
        closeLogsButton.onClick.AddListener(CloseLogPanel);
        logPanel.SetActive(false);  // Initially hidden
    }

    // Add a new log entry
    public void AddLogEntry(string message)
    {
        // Store the message and update the UI display
        logEntries.Add(message);
        UpdateLogDisplay();
    }

    // Update log display text
    private void UpdateLogDisplay()
    {
        logText.text = string.Join("\n", logEntries);
    }

    // Toggle the visibility of the log panel
    private void ToggleLogPanel()
    {
        logPanel.SetActive(true);
    }

    private void CloseLogPanel()
    {
        logPanel.SetActive(false);
    }
}
