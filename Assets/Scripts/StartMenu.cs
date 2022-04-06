using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject imageWindowTrack;
    [SerializeField] GameObject contentScrollView;
    [SerializeField] GameObject scrollView;
    [SerializeField] GameObject[] buttonsLevel;
    [SerializeField] ParticleSystem rain;
    [SerializeField] TextMeshProUGUI textItems;
    private string[] levelsName = {"track01", "track02", "track03" };//имена уровней (нужно добавлять новые уровни)
    private int[] itemScore = { 0, 100, 350 };//количество итемов необходимое для открытия следующего уровня(нужно добавлять с новыми уровнями)
    private ScrollRect scrRect;
    private bool ivtVisable = true;
    private int shiftPosition = 400;//для кнопок уровня

    void Start()
    {

        scrRect = scrollView.GetComponent<ScrollRect>();        
        contentScrollView.transform.localPosition = new Vector2(-buttonsLevel[0].transform.position.x + shiftPosition, contentScrollView.transform.localPosition.y);

        DisplayCollectedItems();

    }

    private void DisplayCollectedItems()//отображение количества собраных итемов
    {
        //PlayerPrefs.DeleteAll();//cброс прогресса

        int itemSum = 0;
        int numTrack = 0;
        Button btn;

        for (int i = 1; i < buttonsLevel.Length; i++)
        {
            btn = buttonsLevel[i].GetComponent<Button>();
            btn.interactable = false;
        }

        for(int i = 0; i < levelsName.Length; i++)
        {
            if (PlayerPrefs.HasKey(levelsName[i]))
            {
                itemSum += PlayerPrefs.GetInt(levelsName[i]);
            }
        }

        for(int i = 0; i < itemScore.Length; i++)
        {
            if (itemScore[i] <= itemSum)
            {
                numTrack = i;
            }
            else break;
        }

        for (int i = 0; i <= numTrack; i++)
        {
            btn = buttonsLevel[i].GetComponent<Button>();
            btn.interactable = true;
        }

        textItems.text = itemSum.ToString();
    }

    private void FixedUpdate()
    {
        ScrollingLevelMenu();
        
    }

    private void ScrollingLevelMenu()//прокрутка меню уровней
    {
        if (contentScrollView.transform.localPosition.x > -buttonsLevel[0].transform.localPosition.x + shiftPosition)
        {
            scrRect.inertia = false;
            contentScrollView.transform.localPosition = new Vector2(-buttonsLevel[0].transform.localPosition.x + shiftPosition, contentScrollView.transform.localPosition.y);

        }else if(contentScrollView.transform.localPosition.x < -buttonsLevel[buttonsLevel.Length-1].transform.localPosition.x + shiftPosition)
        {
            scrRect.inertia = false;
            contentScrollView.transform.localPosition = new Vector2(-buttonsLevel[buttonsLevel.Length-1].transform.localPosition.x + shiftPosition, contentScrollView.transform.localPosition.y);
        }
        else
        {
            scrRect.inertia = true;
        }

    }

    public void StartScene(string name)
    {
        switch (name)
        {
            case "Play":
                OpenWindowTrack();
                break;
            case "Track01":
            case "Track02": 
            case "Track03":
                
                SceneManager.LoadScene(name);//загрузка уровня
                break;
            case "Options":
                
                break;
            case "Exit":
                Application.Quit();//выход из игры
                break;
        }
    }

    private void OpenWindowTrack()//открытие окна с отображением доступных уровней
    {
        if (ivtVisable)
        {
            imageWindowTrack.SetActive(true);
            ivtVisable = false;
        }
        else
        {
            imageWindowTrack.SetActive(false);
            ivtVisable = true;
        }
    }

}
