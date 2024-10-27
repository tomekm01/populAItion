using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

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
    }

    // Method that gets called when DONE is clicked
    void OnDoneButtonClick()
    {
        // Create two creatures using the input data
        if (string.IsNullOrEmpty(headInputField1.text) ||
            string.IsNullOrEmpty(bodyInputField1.text) ||
            string.IsNullOrEmpty(legsInputField1.text) ||
            string.IsNullOrEmpty(nameInputField1.text) ||
            string.IsNullOrEmpty(headInputField2.text) ||
            string.IsNullOrEmpty(bodyInputField2.text) ||
            string.IsNullOrEmpty(legsInputField2.text) ||
            string.IsNullOrEmpty(nameInputField2.text))
        {

        }
        else
        {
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
}