using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Stage21Thoughts
{
    public Player player;
    private DialogueAPI api;

    void Start()
    {
        api = GameObject.Find("DialoguePanel").GetComponent<DialogueAPI>();
        player.SetMove(false);
        api.textPanel.text = api.player.fullname + ": I've passed my tests. That wasn't easy at all.\n "
            + "Let's see what Innopolis University has prepared for me next...\n"
            + "->Press 'Enter' to continue...";
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            api.EraseLine();
            player.SetMove(true);
        }
    }

}

