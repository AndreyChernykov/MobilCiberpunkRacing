using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel03 : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject[] bossHealthIconPref;
    [SerializeField] GameObject canvasHealth;
    [SerializeField] GameObject boom;
    [SerializeField] GameObject tank;
    [SerializeField] GameObject finish;
    [SerializeField] ParticleSystem particleBoom;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioSource audioSourceBoom;
    [SerializeField] private float bossReaction = 1;
    [SerializeField] GameObject shootLite;//свет дула при выстреле

    private AudioSource audioSource;
    private float finishPosition = 200;//позиция финиша
    private Vector3 startPosHealthItem = new Vector3(10, 0, 0);
    private float playerPosX;
    private float playerPosY;
    private float playerPosZ;
    private float bossPosX;
    private float bossPosY;
    private float bossPosZ = 15;//позиция на которую босс дальше от машинки игрока
    private float bossSpeed = 8;//скорость босса
    private int attack = 1;//тип аттаки босса
    private int bossHealth = 10;//количество здоровья босса
    private bool bossActiv = false;
    private float shootTime = 1f;//время между залпами
    GameObject[] displayedHealth;
    
    Player playerScr;
    private Vector3 bulletVolley = new Vector3(0, 0, 10);// вектор полета пули

    


    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        playerScr = player.GetComponent<Player>();
        playerScr.SetTimerOn(false);//отключаем таймер
        displayedHealth = new GameObject[bossHealth];
        boom.SetActive(false);
        DisplayBossHealth();
        finish.SetActive(false);
        particleBoom.Stop();
        
    }

    private void DisplayBossHealth()//отображение здоровья босса
    {
        startPosHealthItem = new Vector3(0, 0, 0);

        foreach (GameObject go in displayedHealth)
        {
            if (go != null)
            {
                Destroy(go);
            }
            
        }

        for (int i = 0; i < bossHealth; i++)
        {
            GameObject objHealth = bossHealthIconPref[2];

            if (bossHealth > 5)
            {
                objHealth = bossHealthIconPref[2];
            }else if(bossHealth <= 5 && bossHealth >= 3)
            {
                objHealth = bossHealthIconPref[1];
            }
            else if (bossHealth < 3)
            {
                objHealth = bossHealthIconPref[0];
            }

            GameObject bossHealthIcon = Instantiate(objHealth);
            bossHealthIcon.transform.SetParent(canvasHealth.transform);
            bossHealthIcon.transform.localPosition = startPosHealthItem;
            startPosHealthItem += new Vector3(1, 0, 0);
            displayedHealth[i] = bossHealthIcon;
        }
    }

    private void FixedUpdate()
    {
        if (bossActiv)
        {
            playerPosX = player.transform.position.x;
            playerPosY = player.transform.position.y;
            playerPosZ = player.transform.position.z;

            bossPosX = gameObject.transform.position.x;
            bossPosY = gameObject.transform.position.y;

            StartCoroutine("TurnBoss");
            MovingBoss();

        }

        if (bossHealth < 1)
        {
            Invoke("BossDestroy", 2f);

        }

    }

    public void SetBossActive(bool active)//активация босса
    {
        bossActiv = active;
    }

    private Vector3 PositionMoving(Vector3 vector)
    {
        return gameObject.transform.position += vector * bossSpeed * Time.deltaTime;
    }

    private float ComparePosition(float pos)
    {
        float result = (float)System.Math.Round(pos, 0);
        return result;
    }

    IEnumerator TurnBoss()//движение босса в сторону игрока
    {

        yield return new WaitForSeconds(bossReaction);
        if(ComparePosition(bossPosX) > ComparePosition(playerPosX))
        {
            PositionMoving(Vector3.left);

        } else if(ComparePosition(bossPosX) < ComparePosition(playerPosX))
        {
            PositionMoving(Vector3.right);
        }

        if (ComparePosition(bossPosY) < ComparePosition(playerPosY))
        {
            PositionMoving(Vector3.up);

        }
        else if (ComparePosition(bossPosY) > ComparePosition(playerPosY))
        {
            PositionMoving(Vector3.down);
        }
        if (ComparePosition(bossPosX) == ComparePosition(playerPosX) && ComparePosition(bossPosY) == ComparePosition(playerPosY))
        {
            if(attack == 1)
            {
                StartCoroutine(Shooting());
                attack = 2;
            } else if(attack == 2)
            {
                StartCoroutine(Shooting());
                attack = 1;
            }
            shootLite.SetActive(false);

            StopCoroutine("TurnBoss");
        }
    }

    private void MovingBoss()//движение босса вперед
    {
        gameObject.transform.position = new Vector3(bossPosX, bossPosY, playerPosZ + bossPosZ);     
    }

    IEnumerator Shooting()//босс стреляет
    {
        if (bossActiv && playerScr.GetIsAlive())
        {
            yield return new WaitForSeconds(shootTime);

            shootLite.SetActive(true);
            GameObject bl = Instantiate(bullet);
            bl.gameObject.transform.position = new Vector3(bossPosX, bossPosY + 0.8f, gameObject.transform.position.z - 3);

            Bullet bulletScript = bl.GetComponent<Bullet>();
            bulletScript.bulletMove = bulletVolley;
            audioSource.clip = shootSound;
            audioSource.Play();


            //yield return new WaitForSeconds(shootTime);
        }


        
        StopCoroutine(Shooting());
        
    }

    private void BossDestroy()//уничтожение босса
    {
        finish.SetActive(true);
        finish.transform.position = new Vector3(0, finish.transform.position.y, gameObject.transform.position.z + finishPosition);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)//столкновение босса с препятствиями
    {
        if (collision.gameObject.tag.Equals("Obstacle"))
        {
            
            audioSourceBoom.Play();
            //gameObject.transform.position = Vector3.zero;

            Destroy(collision.gameObject);
            bossHealth--;
            DisplayBossHealth();
            if (bossHealth <= 0)
            {
                SetBossActive(false);
                BoxCollider bc = gameObject.GetComponent<BoxCollider>();
                bc.isTrigger = true;
                tank.SetActive(false);
                particleBoom.Play();
            }
            else
            {
                boom.SetActive(true);
            }
            Invoke("Boom", 0.3f);

        }

    }

    private void Boom()
    {
        boom.SetActive(false);


    }
}
