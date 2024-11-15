using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    public float mutationRate = 0.8f;
    public int offspringCount;
    public Button evolveButton;

    private LogManager logManager;

    private void Start()
    {
        evolveButton.onClick.AddListener(() => StartCoroutine(EvolvePopulation()));

        //Find the LogManager instance
        logManager = FindObjectOfType<LogManager>();
        if (logManager == null)
        {
            Debug.LogError("LogManager not found in the scene!");
        }
    }

    private IEnumerator EvolvePopulation()
    {
        Debug.Log("Starting evolution");
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

            yield return StartCoroutine(GenerateOffspring(parent1, parent2, (child) =>
            {
                newGeneration.Add(child);
            }));
        }
    }

    private IEnumerator GenerateOffspring(Creature parent1, Creature parent2, Action<Creature> callback)
    {
        Debug.Log("Generating offspring");
        string head, body, legs;
        string mutatedPart = "no mutation";

        do
        {
            head = UnityEngine.Random.value < 0.5f ? parent1.head : parent2.head;
            body = UnityEngine.Random.value < 0.5f ? parent1.body : parent2.body;
            legs = UnityEngine.Random.value < 0.5f ? parent1.legs : parent2.legs;
        } while ((head == parent1.head && body == parent1.body && legs == parent1.legs) ||
                 (head == parent2.head && body == parent2.body && legs == parent2.legs));

        if (UnityEngine.Random.value < mutationRate)
        {
            int partToMutate = UnityEngine.Random.Range(0, 3);
            string mutation = null;

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

        yield return StartCoroutine(GenerateCreatureName(head, body, legs, (finalChildName) =>
        {
            Creature offspring = new Creature(head, body, legs, finalChildName);
            CreatureManager.AddCreature(offspring);

            if (logManager != null)
            {
                logManager.AddLogEntry($"Creature {offspring.name} evolved from {parent1.name} and {parent2.name} with {mutatedPart}.");
            }
            else
            {
                Debug.LogWarning("LogManager is not initialized.");
            }
            callback(offspring);
        }));
    }

    private IEnumerator GenerateCreatureName(string head, string body, string legs, Action<string> callback)
    {
        Debug.Log("Generating name");
        string prompt = $"reply-with-maximum-2-words-generate-a-random-name-for-a-creature-with-a-{head}-head-{body}-body-and-{legs}-legs-{DateTime.UtcNow.Ticks}";
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

    private IEnumerator FetchRandomMutation(Action<string> callback)
    {
        Debug.Log("Fetching a mutation");
        string[] prompts = new string[]
        {
        $"maximum-3-words-reply-with-a-random-thing-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-random-simple-thing-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-random-animal-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-reply-with-a-random-thing-from-nature-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-give-me-a-scary-thing-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-give-me-a-random-thing-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-give-me-a-simple-household-thing-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-give-me-a-sci-fi-thing-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-give-me-a-fantasy-thing-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-give-me-a-medieval-thing-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-give-me-a-sports-thing-{DateTime.UtcNow.Ticks}",
        $"maximum-3-words-give-me-a-fashion-thing-{DateTime.UtcNow.Ticks}"
        };

        string selectedPrompt = prompts[UnityEngine.Random.Range(0, prompts.Length)];
        string url = "https://text.pollinations.ai/prompt/" + selectedPrompt;

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
            callback("Mystery");
        }
    }
}

