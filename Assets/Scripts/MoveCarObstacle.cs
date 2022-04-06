using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCarObstacle : MonoBehaviour
{

    [SerializeField] private float speed = 5;//скорость движения машинки
    bool onMotor = false;//включён ли звук двигателя
    public bool isMove = false;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            MotorSound();
            gameObject.transform.position += speed * Vector3.back * Time.deltaTime;
        }
        

    }

    private void MotorSound()
    {        
        if (!onMotor)
        {
            audioSource.Play();
            onMotor = true;
        }
    }




}
