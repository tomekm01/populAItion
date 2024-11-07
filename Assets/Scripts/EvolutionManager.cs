using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class EvolutionManager : MonoBehaviour
{
    public float mutationRate = 0.5f;  // High mutation rate to encourage variety
    public int offspringCount;         // Number of offspring to create during evolution

    public Button evolveButton;
    private LogManager logManager;

    private void Start()
    {
        evolveButton.onClick.AddListener(() => StartCoroutine(EvolvePopulation()));
        logManager = FindObjectOfType<LogManager>();
    }

    private IEnumerator EvolvePopulation()
    {
        offspringCount = Mathf.FloorToInt(CreatureManager.GetCreatureList().Count / 2);
        List<Creature> newGeneration = new List<Creature>();

        for (int i = 0; i < offspringCount; i++)
        {
            Creature parent1 = CreatureManager.GetCreatureList()[UnityEngine.Random.Range(0, CreatureManager.GetCreatureList().Count)];
            Creature parent2;
            do
            {
                parent2 = CreatureManager.GetCreatureList()[UnityEngine.Random.Range(0, CreatureManager.GetCreatureList().Count)];
            } while (parent2 == parent1);

            // Generate AI-based name for the new creature
            yield return GenerateCreatureName((childName) =>
            {
                // Combine parts and apply mutations
                StartCoroutine(GenerateOffspring(parent1, parent2, childName, (child) =>
                {
                    newGeneration.Add(child);
                }));
            });
        }
    }

    private IEnumerator GenerateCreatureName(Action<string> callback)
    {
        string prompt = $"generate-a-random-name-for-a-creature-reply-with-1-or-2-words-{DateTime.UtcNow.Ticks}";
        string url = "https://text.pollinations.ai/prompt/" + prompt;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string creatureName = request.downloadHandler.text.Trim();
            callback(creatureName);
        }
        else
        {
            Debug.LogError("Failed to fetch creature name from AI.");
            callback("Unnamed");
        }
    }

private IEnumerator GenerateOffspring(Creature parent1, Creature parent2, string childName, Action<Creature> callback)
{
    string head = UnityEngine.Random.value < 0.5f ? parent1.head : parent2.head;
    string body = UnityEngine.Random.value < 0.5f ? parent1.body : parent2.body;
    string legs = UnityEngine.Random.value < 0.5f ? parent1.legs : parent2.legs;
    string mutatedPart = "no mutation";

    // Apply a maximum of one mutation per creature
    if (UnityEngine.Random.value < mutationRate)
    {
        int partToMutate = UnityEngine.Random.Range(0, 3);
        string mutation = null;

        // Fetch mutation
        if (partToMutate == 0)
        {
            mutatedPart = "mutation on head";
            yield return StartCoroutine(FetchRandomMutation((mutationResult) => mutation = mutationResult));
            head = mutation;
        }
        else if (partToMutate == 1)
        {
            mutatedPart = "mutation on body";
            yield return StartCoroutine(FetchRandomMutation((mutationResult) => mutation = mutationResult));
            body = mutation;
        }
        else
        {
            mutatedPart = "mutation on legs";
            yield return StartCoroutine(FetchRandomMutation((mutationResult) => mutation = mutationResult));
            legs = mutation;
        }
    }

    Creature offspring = new Creature(head, body, legs, childName);
    CreatureManager.AddCreature(offspring);
    logManager.AddLogEntry($"Creature {offspring.name} evolved from {parent1.name} and {parent2.name} with {mutatedPart}.");
    callback(offspring);
}

// Coroutine version of FetchRandomMutation
private IEnumerator FetchRandomMutation(Action<string> callback)
{
    string prompt = $"reply-with-a-random-thing-maximum-3-words-{DateTime.UtcNow.Ticks}";
    string url = "https://text.pollinations.ai/prompt/" + prompt;

    UnityWebRequest request = UnityWebRequest.Get(url);
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        string result = request.downloadHandler.text.Trim();
        callback(result);
    }
    else
    {
        Debug.LogError("Failed to fetch mutation from Pollinations AI.");
        callback("Mystery"); // Default mutation name if request fails
    }
}


}
