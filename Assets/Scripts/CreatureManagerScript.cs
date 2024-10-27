using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public static class CreatureManager {
    private static List<Creature> creatureList = new List<Creature>();
    public static event System.Action OnCreatureListChanged;

    public static void AddCreature(Creature creature) {
        creatureList.Add(creature);
        OnCreatureListChanged?.Invoke(); 
    }

    public static void RemoveCreature(Creature creature) {
        creatureList.Remove(creature);
        OnCreatureListChanged?.Invoke(); 
    }

    public static List<Creature> GetCreatureList() {
        return creatureList;
    }
}
