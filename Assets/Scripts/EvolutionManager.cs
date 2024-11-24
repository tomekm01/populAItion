using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    public Button evolveButton;
    public Text alertText;
    public GameObject errorPanel;
    public Button closeErrorButton;

    private LogManager logManager;

    private string[] prompts = new string[] { //20 prompts for diverse results
        $"maximum-3-words-reply-with-a-random-thing-or-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-funny-thing-or-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-scary-thing-or-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-mysterious-thing-or-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-an-exciting-thing-or-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-sad-thing-or-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-surprising-thing-or-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-thing-or-object-from-nature-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-random-word-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-happy-thing-or-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-hidden-natural-treasure-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-mythical-creature-feature-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-sci-fi-fantasy-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-traditional-craft-innovation-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-strange-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-bewitching-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-an-enigmatic-item-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-random-creature-plus-a-magical-ability-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-cultural-fusion-object-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-gothic-steampunk-element-{DateTime.UtcNow.Ticks}"
    };
    private void Start()
    {
        evolveButton.onClick.AddListener(EvolvePopulation);
        logManager = FindObjectOfType<LogManager>();
        closeErrorButton.onClick.AddListener(CloseErrorPanel);
    }

    // Method to check for internet connectivity
    bool IsConnectedToInternet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    private async void EvolvePopulation()
    {
        int creatureCount = CreatureManager.GetCreatureList().Count;
        if (creatureCount < 8 && creatureCount > 1 && IsConnectedToInternet())
        {
            Debug.Log("Starting evolution");
            evolveButton.interactable = false; // Disable button during evolution

            List<Creature> currentCreatures = CreatureManager.GetCreatureList();
            int offspringCount = Mathf.FloorToInt(currentCreatures.Count / 2);

            for (int i = 0; i < offspringCount; i++)
            {
                Debug.Log($"Creating offspring {i + 1} of {offspringCount}");

                Creature parent1 = currentCreatures[UnityEngine.Random.Range(0, currentCreatures.Count)];
                Creature parent2;

                do
                {
                    parent2 = currentCreatures[UnityEngine.Random.Range(0, currentCreatures.Count)];
                } while (parent2 == parent1);

                Debug.Log("chosen parent");
                // Generate offspring
                Creature offspring = await GenerateOffspringAsync(parent1, parent2);

                logManager.AddLogEntry($"Creature {offspring.name} evolved from {parent1.name} and {parent2.name}.");
            }

            Debug.Log("Evolution complete");
            evolveButton.interactable = true;
        }
        else if (!IsConnectedToInternet())
        {
            ShowAlert("No internet connectivity. Can't proceed with evolution.");
        }
        else if (creatureCount < 2)
        {
            ShowAlert("Not enough creatures to evolve. Start again!");
        }
        else
        {
            ShowAlert("Too many creatures. Make sure there's 7 or fewer creatures in the population.");
        }
    }

    private async Task<Creature> GenerateOffspringAsync(Creature parent1, Creature parent2)
    {
        List<Creature> currentCreatures = CreatureManager.GetCreatureList();
        float mutationCoefficient = 0.8f;
        string head, body, legs;
        Creature offspring;

        int maxGenerationAttempts = 10; // Limit to avoid infinite loops
        int generationAttempts = 0;

        do
        {
            generationAttempts++;

            // Inherit parts from parents
            head = UnityEngine.Random.value < 0.5f ? parent1.head : parent2.head;
            body = UnityEngine.Random.value < 0.5f ? parent1.body : parent2.body;
            legs = UnityEngine.Random.value < 0.5f ? parent1.legs : parent2.legs;

            Debug.Log("parts inherited");

            // Mutation check
            if (UnityEngine.Random.value <= mutationCoefficient)
            {
                string selectedPrompt = prompts[UnityEngine.Random.Range(0, prompts.Length)];
                string mutation = await FetchAIResponseAsync(selectedPrompt);

                int partToMutate = UnityEngine.Random.Range(0, 3);

                if (partToMutate == 0) head = mutation;
                else if (partToMutate == 1) body = mutation;
                else legs = mutation;
            }

            // Generate a unique name
            string name;
            do
            {
                name = await FetchAIResponseAsync($"reply-with-max-3-words-randomly-generate-a-simple-name-based-on-{head}-head-{body}-body-and-{legs}-legs");
            } while (currentCreatures.Exists(c => c.name == name));

            offspring = new Creature(head, body, legs, name);

        } while (currentCreatures.Exists(c => c.head == head && c.body == body && c.legs == legs) && generationAttempts < maxGenerationAttempts);

        if (generationAttempts >= maxGenerationAttempts)
        {
            Debug.LogError("Exceeded maximum generation attempts! Something went wrong with offspring creation.");
        }

        // Add the unique creature to the list
        CreatureManager.AddCreature(offspring);

        return offspring;
    }

    private async Task<string> FetchAIResponseAsync(string prompt)
    {
        string url = "https://text.pollinations.ai/prompt/" + prompt + "-" + DateTime.UtcNow.Ticks;
        UnityWebRequest request = UnityWebRequest.Get(url);
        var operation = request.SendWebRequest();

        float startTime = Time.time;
        while (!operation.isDone)
        {
            if (Time.time - startTime > 10f)
            {
                Debug.LogError("AI request timed out");
                return "Unknown";
            }
            await Task.Yield(); // This allows Unity to continue executing other tasks.
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            return request.downloadHandler.text.Trim();
        }
        else
        {
            ShowAlert("Failed to fetch data. Please try again.");
            return "Unknown";
        }
    }

    void ShowAlert(string message)
    {
        errorPanel.SetActive(true);
        alertText.text = message;
    }

    public void CloseErrorPanel()
    {
        errorPanel.SetActive(false);
    }
}
