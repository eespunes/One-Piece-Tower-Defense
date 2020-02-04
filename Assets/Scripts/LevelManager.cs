using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject[] minions, bosses;
    public GameObject enemiesFolder;
    public GameObject bossAlert, roundStart, clearRound, levelCompleted;
    private GameObject sliderGO;

    public int rounds;
    public int numberOfMinions, addMinions;
    private int actualRound, actualMinions;

    public float timePerRound, timePerMinion,timePerBoss;

    public string scene;

    private Vector3 start;

    private GameController gc;

    public Text roundTXT, maxRoundTXT;

    public Slider slider;

    void Start()
    {
        sliderGO = slider.gameObject;
        slider.maxValue = timePerRound;
        slider.value = 0;

        actualRound = 1;
        actualMinions = 0;

        MyGrid grid = GameObject.Find("MyGrid").GetComponent<MyGrid>();
        start = grid.TheNodes[(int)grid.start.x, (int)grid.start.y].Coord;

        gc = GetComponent<GameController>();

        maxRoundTXT.text = "of " + rounds;
        roundTXT.text = actualRound.ToString();

        Invoke("LessTimeRound", 1);
    }

    private void Update()
    {
        if (actualMinions > numberOfMinions)
            if (enemiesFolder.transform.childCount == 0)
                NextRound();
    }

    //Round countdown
    public void LessTimeRound()
    {
        slider.value++;
        if (slider.value < timePerRound)
            Invoke("LessTimeRound", 1);
        else
        {
            sliderGO.SetActive(false);
            roundStart.SetActive(true);
            Invoke("StopRoundAnim", 2);
        }
    }
    public void StopRoundAnim()
    {
        roundStart.SetActive(false);
        StartRound();
    }

    private void StartRound()
    {
        if (actualMinions == 0)
        {
            int alea = Random.Range(0, minions.Length - 1);
            GameObject go = Instantiate(minions[alea]);
            go.transform.position = start;
            go.transform.parent = enemiesFolder.transform;
            actualMinions++;
            StartRound();
        }
        else if (actualMinions < numberOfMinions)
            Invoke("NextEnemy", timePerMinion);
        else if (actualMinions >= numberOfMinions)
        {
            bossAlert.SetActive(true);
            Invoke("NextBoss", timePerBoss);
        }
    }
    public void NextRound()
    {
        actualRound++;
        if (actualRound > rounds)
        {
            if(gc.life>0)
            {
                levelCompleted.SetActive(true);
                Invoke("NextLevel", 2);
            }
        }
        else
        {
            clearRound.SetActive(true);
            sliderGO.SetActive(true);
            slider.value = 0;
            roundTXT.text = actualRound.ToString();
            actualMinions = 0;
            numberOfMinions += addMinions;
            Invoke("ClearRoundCompleted", 2);
            Invoke("LessTimeRound", 1);
        }
    }

    public void NextEnemy()
    {
        int alea = Random.Range(0, minions.Length - 1);
        GameObject go = Instantiate(minions[alea]);
        go.transform.position = start;
        go.transform.parent = enemiesFolder.transform;
        actualMinions++;
        StartRound();
    }
    public void NextBoss()
    {
        bossAlert.SetActive(false);
        GameObject go = Instantiate(bosses[actualRound - 1]);
        go.transform.position = start;
        go.transform.parent = enemiesFolder.transform;
        actualMinions++;
    }

    public void ClearRoundCompleted()
    {
        clearRound.SetActive(false);
    }
    public void NextLevel()
    {
        levelCompleted.SetActive(false);
        SceneManager.LoadScene(scene);
    }
}
