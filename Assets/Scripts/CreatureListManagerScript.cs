using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureListManagerScript : MonoBehaviour
{
    public GameObject creatureUIPrefab; // Assign this in the Inspector (your prefab with the button/UI to represent a creature)
    public Transform contentPanel; // Assign this to the Content object of your ScrollView
    public GameObject creatureInspectPanel;


    void OnEnable()
    {
        // Subscribe to the event when this panel is enabled
        CreatureManager.OnCreatureListChanged += DisplayCreatureList;
    }

    void OnDisable()
    {
        // Unsubscribe when this panel is disabled
        CreatureManager.OnCreatureListChanged -= DisplayCreatureList;
    }

    void Start()
    {
        // Initial population of the list
        DisplayCreatureList();
    }

    // Method to display the creature list
    void DisplayCreatureList()
    {
        // Clear the existing list
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Get the list of creatures from the CreatureManager
        List<Creature> creatures = CreatureManager.GetCreatureList();

        // Loop through each creature and add to the UI
        foreach (Creature creature in creatures)
        {
            // Instantiate a new UI element for the creature
            GameObject newCreatureUI = Instantiate(creatureUIPrefab, contentPanel);

            // Update the UI text fields (assuming your prefab has a Text component)
            Text creatureNameText = newCreatureUI.transform.Find("CreatureNameDisplay").GetComponent<Text>();
            creatureNameText.text = creature.name;

            // Optionally, add a button listener to inspect or interact with the creature
            Button creatureButton = newCreatureUI.GetComponent<Button>();
            creatureButton.onClick.AddListener(() => InspectCreature(creature));
        }
    }

    // Method to handle inspecting a creature

    void InspectCreature(Creature creature)
    {
        Debug.Log("Inspecting creature: " + creature.name);

        creatureInspectPanel.SetActive(true);
        CreatureInspectScript inspectScript = FindObjectOfType<CreatureInspectScript>();
        inspectScript.SetCreature(creature);
    }
}
