﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            other.gameObject.GetComponent<Player>().ItemCounter();
            Destroy(gameObject);
        }
    }
}
