using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature
{
    public string name;
    public string head;
    public string body;
    public string legs;

    public Creature(string head, string body, string legs, string name)
    {
        this.head = head;
        this.body = body;
        this.legs = legs;
        this.name = name;
    }

    public string getPrompt()
    {
        string promptHead = this.head.Replace(" ", "-");
        string promptBody = this.body.Replace(" ", "-");
        string promptLegs = this.legs.Replace(" ", "-");

        return
            $"a-creature-with-{promptHead}-for-head-and-{promptBody}-for-torso-and-{promptLegs}-for-legs?width=800&height=800";
    }
}