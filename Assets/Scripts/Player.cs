using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] FixedJoystick joystick;
    [SerializeField] TextMeshProUGUI textItems;
    [SerializeField] TextMeshProUGUI textTime;
    [SerializeField] Image panelInfo;
    [SerializeField] TextMeshProUGUI textInfo;
    [SerializeField] Button btnPause;
    [SerializeField] Button btnPlay;
    [SerializeField] GameObject[] nums;
    [SerializeField] GameObject startLine;
    [SerializeField] GameObject[] healthHeart;
    [SerializeField] GameObject uiIconItem;
    [SerializeField] GameObject spark;
    [SerializeField] GameObject boom;
    [SerializeField] private int timeToTrack = 100;//время на прохождение уровня в секундах
    [SerializeField] string trackName;//название уровня
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioClip itemSound;
    [SerializeField] AudioClip itemHealthSound;
    [SerializeField] AudioClip finishSound;
    [SerializeField] AudioClip[] counydownSound;

    AudioSource audioSource;
    private float velocity = 0f;// скорость машины
    private float maxSpeed = 30.0f;// максимальная скорость машины
    private float turnSpeed = 10.0f;// поворот
    private bool toMove = false;// нажата ли кнопка газ
    private int items = 0;//количество собраных итемов
    private int kickForce = 8;//отскок при столкновении с препятствием
    private int health = 3;//количество здоровья
    private bool isAlive = true;//жив ли герой
    private bool timerOn = true;//для включения/отключения таймера
    
    const string timeStr = "Time: ";
    const string gameOverStr = "GAME OVER";
    const string pauseStr = "PAUSE";
    const string finishStr = "FINISH!";
    public const string saveItemKey = "Items";
    public const string saveTrackKey = "SaveTrack";
    const int healthMax = 5;//максимальное значение здоровья


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        HealthVisibl();

        Time.timeScale = 1;

        panelInfo.gameObject.SetActive(false);

        StartCoroutine("StartCountdown");

        spark.SetActive(false);
        boom.SetActive(false);
    }

    
    void FixedUpdate()
    {
        if (health <= 0)
        {
            isAlive = false;
        }

        if(isAlive)//если жив
        {
            Turn();

            if (Input.GetKeyDown(KeyCode.Space)) Move();//для тестирования на компе
            if (Input.GetKeyUp(KeyCode.Space)) Stop();//для тестирования на компе

        }
        else//если проиграл
        {
            GameOver();
        }

    }

    public void Pause()
    {
        Time.timeScale = 0;
        btnPause.gameObject.SetActive(false);
        panelInfo.gameObject.SetActive(true);
        textInfo.text = pauseStr;
    }

    public void Play()
    {
        Time.timeScale = 1;
        btnPause.gameObject.SetActive(true);
        panelInfo.gameObject.SetActive(false);
    }

    public void RePlay()
    {
        SceneManager.LoadScene(trackName);
    }

    public void ExitTrack()//выход в главное меню
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }

    private void Turn()//движение машины вбок
    {
        //transform.position += new Vector3(joystick.Direction.x * turnSpeed, 
        //  joystick.Direction.y * turnSpeed, 0) * Time.deltaTime;

        Vector3 moveX = transform.right * (joystick.Direction.x * turnSpeed);
        Vector3 moveY = transform.up * (joystick.Direction.y * turnSpeed);

        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(moveX.x, moveY.y, velocity);

        //if (velocity < 0) velocity = 0;
        

    }

    public void GameOver()//при проигрыше
    {
        
        StartCoroutine("Braking");
        StopCoroutine("Timer");
        panelInfo.gameObject.SetActive(true);
        textInfo.text = gameOverStr;

        btnPause.gameObject.SetActive(false);
        btnPlay.gameObject.SetActive(false);

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void Move()//движение машины вперед
    {
        if (isAlive)
        {
            StartCoroutine("Acceleration");
            toMove = true;

        }

    }

    public void Stop()// остановка машины
    {
        toMove = false;

        StopCoroutine("Acceleration");
        StartCoroutine("Braking");

    }

    IEnumerator StartCountdown()//стартовый отсчёт
    {
        audioSource.clip = counydownSound[1];
        foreach(GameObject go in nums)
        {
            go.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            audioSource.Play();
            yield return new WaitForSeconds(0.5f);
            go.SetActive(false);
        }
        audioSource.clip = counydownSound[0];
        audioSource.Play();
        Animator animator = GameObject.Find("Start").GetComponent<Animator>();
        animator.SetBool("isStart", true);

        yield return new WaitForSeconds(1.5f);
        StartCoroutine("Timer");
        Destroy(startLine);

        StopCoroutine("StartCountdown");
    }

    IEnumerator Timer()//таймер времени на прохождение уровня
    {
        if (timerOn)
        {
            Animator animator = textTime.GetComponent<Animator>();
            while (timeToTrack > 0)
            {
                if (timeToTrack < 10)
                {
                    audioSource.clip = counydownSound[1];
                    audioSource.Play();
                    animator.SetBool("endOfTime", true);
                    textTime.faceColor = new Color(255, 0, 0);
                }
                timeToTrack--;
                textTime.text = timeStr + timeToTrack.ToString() + " sec";
                yield return new WaitForSeconds(1);
            }

            if (timeToTrack <= 0)
            {
                isAlive = false;
                StopCoroutine("Timer");
            }
        }
        
    }

    public void SetTimerOn(bool timerOn)//для управлением таймером из уровней
    {
        this.timerOn = timerOn;
    }

    IEnumerator Acceleration()// ускорение машинки
    {
        StopCoroutine("Braking");

        while (isAlive)
        {
            //Debug.Log("velosity " + velocity);

            if (velocity < maxSpeed && toMove)
            {
                velocity += 1.0f;                
                gameObject.GetComponent<Rigidbody>().velocity = transform.forward * velocity;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Braking()//торможение
    {
        
        while(velocity > 0)
        {
            //Debug.Log("velosity " + velocity);
            gameObject.GetComponent<Rigidbody>().velocity = transform.forward * velocity;
            velocity -= 1.0f;
            yield return new WaitForSeconds(0.1f);
        }

        StopCoroutine("Braking");
    }

    public void ItemCounter()//добавляем итемы при сборе
    {
        audioSource.clip = itemSound;
        audioSource.Play();
        items++;
        textItems.text = items.ToString();
        StartCoroutine("AnimItemTake");

    }

    IEnumerator AnimItemTake()
    {
        Animator anim = uiIconItem.GetComponent<Animator>();
        anim.SetBool("takeItem", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("takeItem", false);
        StopCoroutine("AnimItemTake");
    }

    public void HealthAdd()//добавляем здоровье при сборе
    {
        audioSource.clip = itemHealthSound;
        audioSource.Play();
        if(health < healthMax)
        {
            health++;
            HealthVisibl();
        }

    }

    public void HealthSub()//отнимаем здоровье при столкновении
    {
        if(health > 0)
        {
            health--;
            HealthVisibl();
        }
    }

    private void HealthVisibl()//отображение значка здоровья
    {
        foreach (GameObject hh in healthHeart)
        {
            hh.gameObject.SetActive(false);
        }
        if(health > 0)
        {
            healthHeart[health - 1].gameObject.SetActive(true);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag.Equals("Obstacle") || tag.Equals("Bullet"))//обработка столкновенй с препятствиями
        {
            audioSource.clip = boomSound;
            audioSource.Play();

            boom.SetActive(true);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, other.gameObject.transform.position.z - kickForce);
            velocity = 0;

            HealthSub();
            Invoke("ToBoom", 0.3f);
        }

        if (other.gameObject.GetComponentInParent<MoveCarObstacle>() != null)//если машинка движущаяся
        {
            other.gameObject.GetComponentInParent<MoveCarObstacle>().isMove = false;
        }

        if (tag.Equals("Finish"))//при достижении финиша
        {
            StartCoroutine("Finish");
            
        }
    }

    private void ToBoom()
    {
        boom.SetActive(false);
    }

    private void OnCollisionStay(Collision collision)//столкновение со стенкой
    {

        if (collision.gameObject.tag.Equals("wall") && velocity > 0)
        {
            spark.SetActive(true);
            if (collision.gameObject.transform.position.x < gameObject.transform.position.x)
            {
                
                spark.gameObject.transform.localPosition = new Vector3(-0.9f, 0, 0);
                Debug.Log("collision left");
            }
            else if (collision.gameObject.transform.position.x > gameObject.transform.position.x)
            {
                
                spark.gameObject.transform.localPosition = new Vector3(0.9f, 0, 0);
                Debug.Log("collision right");

            }

        }
        else
        {
            spark.SetActive(false);
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("wall"))
        {
            spark.SetActive(false);
        }
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    IEnumerator Finish()//когда доехал до финиша
    {
        audioSource.clip = finishSound;
        audioSource.Play();
        StopCoroutine("Timer");
        btnPause.gameObject.SetActive(false);
        btnPlay.gameObject.SetActive(false);
        panelInfo.gameObject.SetActive(true);

        SaveResult();

        textInfo.text = finishStr;
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
        StopCoroutine("Finish");
    }

    private void SaveResult()//сохронение при прохождении
    {
        
        PlayerPrefs.SetInt(trackName, items);
        //PlayerPrefs.SetString(saveTrackKey, trackName);
        PlayerPrefs.Save();
        
    }
}
