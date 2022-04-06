using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private float timeDestroy = 5;//через сколько пуля уничтожится
    private string[] tagsArr = { "Player", "Obstacle"};//список объектов с которыми сталкивается снаряд

    public Vector3 bulletMove;//угол и скорость полета пули 

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Invoke("BulletDestroy", timeDestroy);
    }

    private void Move()// полет пули
    {

        //gameObject.transform.position += Vector3.back * speed * Time.deltaTime;

        //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 1) * -speed * Time.deltaTime;
        gameObject.GetComponent<Rigidbody>().AddForce(-bulletMove);
    }

    private void BulletDestroy()//уничтожение пули
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(string tag in tagsArr)
        {
            if (other.gameObject.tag.Equals(tag))
            {
                BulletDestroy();
            }
        }

        
    }
}
