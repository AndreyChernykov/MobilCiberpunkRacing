using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCarObstacle : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;

    private void OnTriggerExit(Collider other)
    {

        Debug.Log(other.tag);

        if (other.gameObject.name.Equals("Player"))//триггер включающий движение машин препятствий
        {
            foreach(GameObject c in cars)
            {
                if(c != null)
                {
                    MoveCarObstacle moveCarObstacle = c.GetComponent<MoveCarObstacle>();
                    moveCarObstacle.isMove = true;
                }

            }
        }

        if (other.gameObject.tag.Equals("car"))
        {
            Destroy(other.gameObject);
        }
    }
}
