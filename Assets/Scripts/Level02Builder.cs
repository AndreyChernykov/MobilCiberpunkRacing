using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level02Builder : MonoBehaviour
{
    [SerializeField] GameObject[] house;
    [SerializeField] GameObject[] signboard;

    private float houseZ;//позиция дома
    private float houseGap = 10f;//промежуток между домами
    private int amountHouse = 450;//количество домиков которые нужно создать
    private float startPosZ = -105;//стартовая позиция дома

    void Start()
    {
        ArrangementHouses(0, 9, 5.8f);// домики справа
        ArrangementHouses(180, -9, -5.8f);//домики слева
    }


    private void ArrangementHouses(float angl, float posX, float posXsb)//создаем фоновые домики и устанавливаем их на позиции
    {
        
        houseZ = startPosZ;
        for(int i = 0; i < amountHouse; i++)
        {
            int rdmHouse = Random.Range(0, house.Length);
            GameObject h = Instantiate(house[rdmHouse]);
            
            if (h.tag.Equals("multistorey")) {
                int rdmSb = Random.Range(-signboard.Length, signboard.Length);
                if(rdmSb >= 0)
                {
                    GameObject sb = Instantiate(signboard[rdmSb]);
                    sb.transform.position = new Vector3(posXsb, sb.transform.position.y + 1, houseZ - 2.4f);
                }

            }


            h.transform.position = new Vector3(posX, h.transform.position.y, houseZ);
            h.transform.rotation = Quaternion.Euler(0, angl, 0);
            
            houseZ += houseGap;
        }
        houseZ = startPosZ;
    }



}
