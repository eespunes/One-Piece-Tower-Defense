using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public GameObject[] directions, staticArms, dynamicArms, largeArms;
    public GameObject attackFolder, ratioFolder, speedFolder, maxAttack, maxRatio, maxSpeed;
    public GameObject actionRatio;
    public GameObject upgrade;
    private GameObject target;
    private GameObject cost;
    private GameObject actualPlace;
    private List<GameObject> enemies;

    public int upgradePrice;
    private int direction;
    private int levelAttack, levelSpeed, levelRatio;

    public float damage;
    public float ratio;
    private float speed;
    private float angle;
    private float section;
    private float percentage;

    public bool isNami;
    private bool inPlace;
    private bool canAttack;
    private bool hasBeenKilled;

    private MyGrid grid;
    private GameController gc;

    public Animator[] animator;
    public AnimationClip attack;

    private SphereCollider sc;

    public Text levelAttackTXT, attackUpgradeActual, attackUpgradeNext, levelSpeedTXT, speedUpgradeActual, speedUpgradeNext, levelRatioTXT, ratioUpgradeActual, ratioUpgradeNext;

    private AudioSource audio;

    private LineRenderer lineRenderer;

    private void Start()
    {
        cost = GameObject.Find("Selector");
        enemies = new List<GameObject>();

        direction = 0;
        levelAttack = 1;
        levelRatio = 1;

        percentage = 1.25f;
        section = 360 / directions.Length;

        canAttack = true;
        inPlace = true;
        hasBeenKilled = false;


        grid = GameObject.Find("MyGrid").GetComponent<MyGrid>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();

        audio = GetComponent<AudioSource>();

        sc = GetComponent<SphereCollider>();

        if (isNami)
        {
            cost = cost.transform.Find("Nami").transform.Find("NamiCost").gameObject;

            levelSpeed = 4;

            lineRenderer = GetComponent<LineRenderer>();
        }
        else
        {
            if (gameObject.name.Contains("Luffy"))
                cost = cost.transform.Find("Luffy").transform.Find("LuffyCost").gameObject;
            else cost = cost.transform.Find("Zoro").transform.Find("ZoroCost").gameObject;

            levelSpeed = 1;

            isNami = false;
        }
    }

    //Map Position
    public void Place(Button b)
    {
        if (!inPlace)
        {
            if (actualPlace == null)
            {
                b.ReturnMoney();
                Destroy(transform.parent.gameObject);
            }
            else
            {
                transform.position = actualPlace.transform.position;
                actualPlace.GetComponent<Place>().SetOccupied(this);
                inPlace = true;
                sc.radius = ratio;
                actionRatio.transform.localScale = new Vector3(ratio * 2, ratio * 2, ratio * 2);
            }
        }
    }
    public void Place(Vector3 v)
    {
        if (!inPlace)
            transform.position = v;
    }

    //Attack
    private void Update()
    {
        if (target != null)
        {
            float angle = (float)((Mathf.Atan2(transform.position.x - target.transform.position.x, transform.position.z - target.transform.position.z) / Math.PI) * 180f);
            if (angle < 0) angle += 360f;
            ChangeDirection(angle);
            Attack(angle - (direction * section));
        }
        else
        {
            audio.Stop();
            if (isNami)
                lineRenderer.enabled = false;
            if (hasBeenKilled)
            {
                AllDirectionsFalse();
                directions[direction].SetActive(true);
                hasBeenKilled = false;
            }
        }
    }
    private void Attack(float armDir)
    {
        dynamicArms[direction].SetActive(true);
        if (isNami)
        {
            lineRenderer.enabled = true;
            canAttack = true;
            Hurt();
            Laser();
        }
        else if (largeArms[direction].activeInHierarchy)
        {
            largeArms[direction].transform.rotation = Quaternion.Euler(90, 0, -armDir);
            canAttack = true;
        }
        else if (canAttack)
        {
            Hurt();
            canAttack = false;
        }
        staticArms[direction].SetActive(false);
    }
    private void Hurt()
    {
        if (!audio.isPlaying)
            audio.Play();
        target.GetComponent<Enemy>().Hurt(damage, this);
    }
    private void Laser()
    {
        lineRenderer.SetPosition(0, largeArms[direction].transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
    }

    //Upgrades
    private void OnMouseDown()
    {
        actionRatio.SetActive(true);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(cost.name.Substring(0, cost.name.Length - 4) + "'s Canvas"))
            go.SetActive(false);
        Upgrade();

    }
    private void Upgrade()
    {
        upgrade.SetActive(true);
        cost.SetActive(false);
        levelAttackTXT.text = levelAttack.ToString();
        levelRatioTXT.text = levelRatio.ToString();
        levelSpeedTXT.text = levelSpeed.ToString();
        attackUpgradeActual.text = damage.ToString();
        attackUpgradeNext.text = (damage * percentage).ToString();
        if (!isNami)
        {
            speedUpgradeActual.text = animator[0].speed.ToString();
            speedUpgradeNext.text = (animator[0].speed * percentage).ToString();
        }
        ratioUpgradeActual.text = ratio.ToString();
        ratioUpgradeNext.text = (ratio * percentage).ToString();
    }
    public void UpgradeDamage()
    {
        if (gc.coins >= upgradePrice)
        {
            damage *= percentage;
            levelAttack++;
            gc.LoseCoins(upgradePrice);
            if (levelAttack >= 4)
            {
                maxAttack.gameObject.SetActive(true);
                attackFolder.gameObject.SetActive(false);
            }
            else
            {
                attackUpgradeActual.text = damage.ToString();
                attackUpgradeNext.text = (damage * percentage).ToString();
            }

            levelAttackTXT.text = levelAttack.ToString();
        }
    }
    public void UpgradeVelocity()
    {
        if (gc.coins >= upgradePrice)
        {
            foreach (Animator a in animator)
                a.speed *= percentage;
            gc.LoseCoins(upgradePrice);
            levelSpeed++;
            if (levelSpeed >= 4)
            {
                maxSpeed.gameObject.SetActive(true);
                speedFolder.gameObject.SetActive(false);
            }
            else
            {
                speedUpgradeActual.text = animator[0].speed.ToString();
                speedUpgradeNext.text = (animator[0].speed * percentage).ToString();
            }
            levelSpeedTXT.text = levelSpeed.ToString();
        }

    }
    public void UpgradeRatio()
    {
        if (gc.coins >= upgradePrice)
        {
            ratio *= percentage;
            sc.radius = ratio;
            actionRatio.transform.localScale = new Vector3(ratio * 2, ratio * 2, ratio * 2);
            gc.LoseCoins(upgradePrice);
            levelRatio++;
            if (levelRatio >= 4)
            {
                maxRatio.gameObject.SetActive(true);
                ratioFolder.gameObject.SetActive(false);
            }
            else
            {
                ratioUpgradeActual.text = ratio.ToString();
                ratioUpgradeNext.text = (ratio * percentage).ToString();
            }
            levelRatioTXT.text = levelRatio.ToString();
        }
    }
    private void OnMouseUp()
    {
        Invoke("DisableUpgrades", 5);
    }
    public void DisableUpgrades()
    {
        actionRatio.SetActive(false);
        upgrade.SetActive(false);
        cost.SetActive(true);
    }

    //Change sprites of characters
    private void ChangeDirection(float angle)
    {
        int times = 1;
        float limit = section / 2;
        int actualDir = direction;

        if (angle > (section * times) - limit && angle < (section * times) + limit)
            direction = times;
        else
        {
            times++;
            if (angle > (section * times) - limit && angle < (section * times) + limit)
                direction = times;
            else
            {
                times++;
                if (angle > (section * times) - limit && angle < (section * times) + limit)
                    direction = times;
                else
                {
                    times++;
                    if (angle > (section * times) - limit && angle < (section * times) + limit)
                        direction = times;
                    else
                    {
                        times++;
                        if (angle > (section * times) - limit && angle < (section * times) + limit)
                            direction = times;
                        else
                        {
                            times++;
                            if (angle > (section * times) - limit && angle < (section * times) + limit)
                                direction = times;
                            else
                            {
                                times++;
                                if (angle > (section * times) - limit && angle < (section * times) + limit)
                                    direction = times;
                                else
                                    direction = 0;
                            }
                        }
                    }
                }
            }
        }
        if (direction != actualDir)
        {
            AllDirectionsFalse();
            directions[direction].SetActive(true);
        }
    }
    private void AllDirectionsFalse()
    {
        foreach (GameObject go in directions)
            go.SetActive(false);
        foreach (GameObject go in staticArms)
            go.SetActive(true);
        foreach (GameObject go in dynamicArms)
            go.SetActive(false);
    }

    //Choose target
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Add(other.gameObject);
            if (enemies.Count == 1)
                target = enemies[0];
        }
        else if (other.tag == "Node")
            actualPlace = other.gameObject;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Remove(other.gameObject);
            if (enemies.Count > 0)
                target = enemies[0];
            else
            {
                canAttack = false;
                target = null;
                AllDirectionsFalse();
                directions[direction].SetActive(true);
            }
        }

    }

    //Gets and setters
    public bool HasBeenKilled
    {
        get
        {
            return hasBeenKilled;
        }

        set
        {
            hasBeenKilled = value;
        }
    }

    public bool InPlace
    {
        get
        {
            return inPlace;
        }

        set
        {
            inPlace = value;
        }
    }

    public List<GameObject> Enemies
    {
        get
        {
            return enemies;
        }

        set
        {
            enemies = value;
        }
    }
}
