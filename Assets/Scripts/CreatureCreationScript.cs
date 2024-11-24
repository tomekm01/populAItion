using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using System.Text.RegularExpressions;

public class CreatureCreationScript : MonoBehaviour
{
    // InputFields for both creatures
    public InputField headInputField1;
    public InputField bodyInputField1;
    public InputField legsInputField1;
    public InputField nameInputField1;

    public InputField headInputField2;
    public InputField bodyInputField2;
    public InputField legsInputField2;
    public InputField nameInputField2;

    // Reference to the DONE button
    public Button doneButton;
    public GameObject creatureCreationPanel;
    public GameObject creatureListPanel;

    void Start()
    {
        // Add the listener to the button
        doneButton.onClick.AddListener(OnDoneButtonClick);

        // Get rid of the carets (they bugged in the build)
        headInputField1.caretWidth = 0;
        headInputField2.caretWidth = 0;
        bodyInputField1.caretWidth = 0;
        bodyInputField2.caretWidth = 0; 
        nameInputField1.caretWidth = 0;
        nameInputField2.caretWidth = 0;
        legsInputField1.caretWidth = 0;
        legsInputField2.caretWidth = 0;
    }

    // Method that gets called when DONE is clicked
    void OnDoneButtonClick()
    {
        // Sanitize the input fields
        headInputField1.text = SanitizeInput(headInputField1.text);
        bodyInputField1.text = SanitizeInput(bodyInputField1.text);
        legsInputField1.text = SanitizeInput(legsInputField1.text);
        nameInputField1.text = SanitizeInput(nameInputField1.text);

        headInputField2.text = SanitizeInput(headInputField2.text);
        bodyInputField2.text = SanitizeInput(bodyInputField2.text);
        legsInputField2.text = SanitizeInput(legsInputField2.text);
        nameInputField2.text = SanitizeInput(nameInputField2.text);

        // Check for empty fields
        if (string.IsNullOrEmpty(headInputField1.text) ||
            string.IsNullOrEmpty(bodyInputField1.text) ||
            string.IsNullOrEmpty(legsInputField1.text) ||
            string.IsNullOrEmpty(nameInputField1.text) ||
            string.IsNullOrEmpty(headInputField2.text) ||
            string.IsNullOrEmpty(bodyInputField2.text) ||
            string.IsNullOrEmpty(legsInputField2.text) ||
            string.IsNullOrEmpty(nameInputField2.text))
        {
            Debug.LogError("All fields must be filled!");
        }
        else
        {
            // Create two creatures using the sanitized input data
            Creature creature1 = new Creature(
                headInputField1.text, 
                bodyInputField1.text, 
                legsInputField1.text, 
                nameInputField1.text
            );

            Creature creature2 = new Creature(
                headInputField2.text, 
                bodyInputField2.text, 
                legsInputField2.text, 
                nameInputField2.text
            );

            // Add the creatures to the CreatureManager
            CreatureManager.AddCreature(creature1);
            CreatureManager.AddCreature(creature2);

            // Optionally, print confirmation
            Debug.Log("Creatures added to the CreatureManager: " + creature1.name + " and " + creature2.name);

            creatureCreationPanel.SetActive(false);
            creatureListPanel.SetActive(true);
        }
    }

    // Method to sanitize input by removing problematic characters
    private string SanitizeInput(string input)
    {
        // Remove all non-alphanumeric characters except underscores and dashes
        string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "");

        return sanitized;
    }
}
