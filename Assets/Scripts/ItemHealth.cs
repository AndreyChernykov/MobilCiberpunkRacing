using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            other.gameObject.GetComponent<Player>().HealthAdd();
            ItemDestroy();
        }
    }

    public void ItemDestroy()
    {
        Destroy(gameObject);
    }
}
