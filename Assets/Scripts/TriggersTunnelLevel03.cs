using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersTunnelLevel03 : MonoBehaviour
{
    [SerializeField] GameObject[] tunnelPart;
    [SerializeField] GameObject road;
    [SerializeField] GameObject itemHealthPref;
    [SerializeField] GameObject boss;
    private float timeEmergenceBoss = 10;//через сколько секунд активируется босс
    private GameObject itemHealth;
    //private Vector3 itemHealthPos = new Vector3(0, 0.5f, 150);//позиция на которую спавним итем здоровья
    [SerializeField] GameObject[] columnsObstacle;
    public GameObject[] tempObstacle = new GameObject[3];
    private float[] posX = { -4, 4 };//позиции препятствий
    private float[] posY = { 4f, 0.5f, 7.5f };
    private float[] posZ = { 10, 100, 200 };

    private float tunnelLength;//длинна части тунеля

    private void Start()
    {
        tunnelLength = road.transform.localScale.z;
        Invoke("BossEmergence", timeEmergenceBoss);
    }

    private void BossEmergence()//активация босса
    {
        BossLevel03 script = boss.GetComponent<BossLevel03>();
        script.SetBossActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            
            switch (gameObject.name)
            {
                case "Trigger01":
                    DestroyObstacle(tunnelPart[0]);
                    tunnelPart[1].transform.position = new Vector3(tunnelPart[0].transform.position.x, tunnelPart[0].transform.position.y, tunnelPart[0].transform.position.z + tunnelLength);
                    
                    HealthDestroy();
                    BuildObstacle(tunnelPart[1]);
                    break;
                case "Trigger02":
                    DestroyObstacle(tunnelPart[1]);
                    tunnelPart[0].transform.position = new Vector3(tunnelPart[1].transform.position.x, tunnelPart[1].transform.position.y, tunnelPart[1].transform.position.z + tunnelLength);
                    HealthSpawn();
                    BuildObstacle(tunnelPart[0]);
                    break;

            }
        }

    }

    private void BuildObstacle(GameObject obj)//создаем колонны препятствия и размещаем их
    {
        for(int i = 0; i < tempObstacle.Length; i++)
        {
            int rdmColumn = Random.Range(0, columnsObstacle.Length);
            GameObject column = Instantiate(columnsObstacle[rdmColumn]);
            column.transform.SetParent(obj.transform);
            float x = 0, y = 0;
            switch (rdmColumn)
            {
                case 0:
                    x = posX[Random.Range(0, posX.Length)];
                    y = 5;
                    break;
                case 1:
                    x = 0;
                    y = posY[Random.Range(0, posY.Length)];
                    break;
            }
            column.transform.position = new Vector3(x, y, posZ[i] + obj.transform.position.z);
            
            tempObstacle[i] = column;
        }
    }

    private void DestroyObstacle(GameObject part)//уничтожаем пройденые препятствия
    {
        Transform []tempObj = part.GetComponentsInChildren<Transform>();
        foreach(Transform obj in tempObj)
        {
            if (obj.gameObject.name.Equals("Trigger01") || obj.gameObject.name.Equals("Trigger02"))
            {
                TriggersTunnelLevel03 script = obj.GetComponent<TriggersTunnelLevel03>();
                foreach (GameObject obstacle in script.tempObstacle)
                {

                    Destroy(obstacle);
                }
            }
        }
    }

    private void HealthSpawn()//создаем и размещаем итем здоровья
    {
        itemHealth = Instantiate(itemHealthPref);
        itemHealth.transform.SetParent(tunnelPart[1].transform);
        itemHealth.transform.position = new Vector3(0, 7, 150 + tunnelPart[1].transform.position.z);
    }

    private void HealthDestroy()//уничтожаем не собраный итем здоровья доделать!!!
    {
        Transform[] tempObj = tunnelPart[1].GetComponentsInChildren<Transform>();
        foreach(Transform obj in tempObj)
        {
            if (obj.gameObject.tag.Equals("ItemHealth"))
            {
                ItemHealth script = obj.GetComponentInChildren<ItemHealth>();
                script.ItemDestroy();
                
            }
        }
        
    }
}
