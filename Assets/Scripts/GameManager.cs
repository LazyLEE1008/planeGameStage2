using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject enemyobj;
    [SerializeField]
    GameObject bossobj;
    [SerializeField]
    Transform[] spawnpos;

    [SerializeField]
    GameObject player;
    GameObject playerobj;

    [SerializeField]
    Transform playerspawnPos;

    [SerializeField]
    Transform bossspawnPoss;

    public Text scoreText;
    public GameObject gameOver;
    public GameObject gameWin;

    public GameObject bossHpSliderobj;
    public GameObject PlayerHpBarObj;

    public GameObject cam;

    GameObject bossid;
    Slider bossHpSlider;
    Slider playerHpSlider;
    

    bool isspawnboss = true;
    bool isactivboss = false;


    float max_timer = .7f;
    float cur_timer;

    float enemy_speed = 2;
    float bosshp = 10;

    bool isslider = false;

    Player playercs;
    Boss bosscs;
    MoveCamera camcs;
    float hp = 100;
    float curhp = 100;
    public ObjectManager obj_manager;

    private void Awake()
    {
        playerobj = Instantiate(player, playerspawnPos.position, playerspawnPos.rotation);
        playercs = playerobj.GetComponent<Player>();
        playerHpSlider = PlayerHpBarObj.GetComponent<Slider>();

        playercs.score = 0;
        playercs.obj_manager = obj_manager;

        camcs = cam.GetComponent<MoveCamera>();
        camcs.target = playerobj;

        bossHpSliderobj.SetActive(false);


    }
    private void Start()
    {
        Time.timeScale = 1;
    }


    // Update is called once per frame
    void Update()
    {

        cur_timer = cur_timer + Time.deltaTime;

        if (cur_timer >= max_timer)
        {
            SpawnEnemy();
            cur_timer = 0;
        }
        if ((playercs.score >= 100) && isspawnboss){
            SpawnBoss();
            

            isspawnboss = false;
            
        }
            
      
        scoreText.text = string.Format("{0:n0}", playercs.score);
        
    }

    private void LateUpdate()
    {
        

        if (isslider)
        {

            bossHpSlider.value = bosscs.cur_hp / bosscs.max_hp;


        }

        if (isactivboss)
        {
            if (!bossid.activeSelf)
            {
                bossHpSliderobj.SetActive(false);
                gameWin.SetActive(true);
                Invoke("StopGame", 1);
            }
        }

        if (!playerobj.activeSelf)
        {
            gameOver.SetActive(true);

        }
        

        playerHpSlider.value = playercs.player_curhp / playercs.player_maxhp;
    


    }

    void SpawnEnemy()
    {
        int randspawn = Random.Range(0, 6);
        GameObject enemy = obj_manager.SelectObj("Enemy");

        Enemy enemycs = enemy.GetComponent<Enemy>();
        enemycs.playercs = playercs;
        enemycs.objManager = obj_manager;
        enemycs.hp = 3;


        enemy.transform.position = spawnpos[randspawn].position;
        
        Rigidbody2D enemy_rigid = enemy.GetComponent<Rigidbody2D>();



        if (randspawn == 4)
        {
            enemy_rigid.transform.rotation = Quaternion.Euler(new Vector3(0,0, -115));
            enemy_rigid.velocity = new Vector2(1 * enemy_speed, -1);
        }
        else if(randspawn == 5)
        {
            enemy_rigid.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 115));
            enemy_rigid.velocity = new Vector2(-1 * enemy_speed, -1);
        }
        else if(randspawn<4)
        {
            enemy_rigid.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            enemy_rigid.velocity = Vector2.down * enemy_speed;
        }
    }

    void SpawnBoss()
    {
        // playerid-> 인스턴스화 하고

        bossid = Instantiate(bossobj);
        bossid.transform.position = bossspawnPoss.transform.position;
        bossid.transform.Rotate(Vector3.back * 180);

        //boss.cs파일에 player object에 playerid 인스턴스화 된 값을 넣어주기!!! 핵심 어려움
        bosscs = bossid.GetComponent<Boss>();
        bosscs.player = playerobj;
        bosscs.obj_manager = obj_manager;

        bossHpSliderobj.SetActive(true);
        isslider = true;
        isactivboss = true;
        bossHpSlider = bossHpSliderobj.GetComponent<Slider>();
        bosshp = bosscs.cur_hp;
        


    }

    public void ReStart()
    {
        SceneManager.LoadScene("Stage1");
        
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        
    }

    void StopGame()
    {
        Time.timeScale = 0;
    }



}
