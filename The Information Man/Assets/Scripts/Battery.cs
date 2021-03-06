﻿using UnityEngine;
using System.Collections;

public class Battery : MonoBehaviour {

    public Player player;
    private bool activated;
    private bool inc;

    // Use this for initialization
    void Start () {
	
	}

    void FixedUpdate()
    {
        if (activated && Input.GetKey("h"))
        {
            if (inc && player.curHealth < 100) player.increaseHealth(1);
            else { inc = false; player.decreaseHealth(1); }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player")
        {
            inc = true;
            activated = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "player")
        {
            if (Input.GetKey("h"))
            {
                if (inc && player.curHealth < 100) player.increaseHealth(1);
                else { inc = false; player.decreaseHealth(1); }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "player")
        {
            activated = false;
        }
    }
}
