/*
@Authors - Craig and Patrick
@Description - Handles game events and UI
*/

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : InputManager
{
    public bool isBattle = false;
    public bool pause = false;

    public static new GameManager instance;
    public GameObject battleUIPrefab;
    public GameObject battleUI;
    public static int MOVES = 4;
    
    public List<Player> players = new List<Player>();
    public List<Enemy> battleEnemies = new List<Enemy>();
    public GameObject playerSpotsParent;
    public List<GameObject> playerSpots = new List<GameObject>();
    public GameObject enemySpotsParent;
    public List<GameObject> enemySpots = new List<GameObject>();
    public Player mainPlayer;
    public GameObject pauseMenuPrefab;
    public GameObject pauseMenu;
    public Hallway currentRoom;
    
    public GameObject statusEffectImagePrefab;
    public Sprite weaknessSprite;
    public Sprite strengthSprite;
    public Sprite shieldSprite;
    public Dictionary<StatusEffectType, Sprite> statusEffectSprites = new Dictionary<StatusEffectType, Sprite>();
    public GameObject doorN, doorS, doorE, doorW;
    public const int ROOMS_BEFORE_BOSS = 9;
    public int roomsCompleted = 0;
    public int roomEnemiesDefeated = 0;
    //TODO - Increment this every time an enemy is created in the room
    public int numEnemiesInRoom = 1;

    void Awake()
    {
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        foreach (Transform t in playerSpotsParent.transform)
        {
            playerSpots.Add(t.gameObject);
        }
        foreach (Transform t in enemySpotsParent.transform)
        {
            enemySpots.Add(t.gameObject);
        }

        foreach (Player p in players)
        {
            if (p.mainCharacter)
            {
                mainPlayer = p;
            }
        }
        battleUI = Instantiate(battleUIPrefab, transform);
        pauseMenu = Instantiate(pauseMenuPrefab);
        statusEffectSprites.Add(StatusEffectType.STRENGTH, strengthSprite);
        statusEffectSprites.Add(StatusEffectType.WEAKNESS, weaknessSprite);
        statusEffectSprites.Add(StatusEffectType.BLOCK, shieldSprite);
    }

    public void StartBattle(Enemy enemy)
    {
        AudioManager manager = FindObjectOfType<AudioManager>();
        if (manager != null)
        {
            manager.Play("BattleMusic");
        }
        
        isBattle = true;
        Player.frozen = true;
        battleUI.SetActive(true);
        battleUI.GetComponent<BattleUIManager>().Reset();
        battleEnemies = new List<Enemy> {enemy};

        for (int i = 0; i < players.Count; i++)
        {
            players[i].gameObject.SetActive(true);
            players[i].transform.position = playerSpots[i].transform.position;
            players[i].StartBattle();
        }

        enemy.transform.position = enemySpots[0].transform.position;
        enemy.StartBattle();
        int extra = enemy.GetPackSize() - 1;
        for (int i = 0; i < extra; i++)
        {
            GameObject obj = Instantiate(enemy.gameObject, enemySpots[i + 1].transform.position, Quaternion.identity);
            battleEnemies.Add(obj.GetComponent<Enemy>());
            obj.GetComponent<Enemy>().StartBattle();
        }
        battleUI.GetComponent<BattleUIManager>().StartBattle();
    }

    public void EndBattle()
    {
        FindObjectOfType<AudioManager>().Stop("BattleMusic");
        battleUI.SetActive(false);
        isBattle = false;
        roomEnemiesDefeated++;
    }

    public void TeleportPlayer(string doorName){
        bool playerHasDefeatedAllEnemies = (roomEnemiesDefeated == numEnemiesInRoom);

        //TODO
        if ((roomsCompleted >= ROOMS_BEFORE_BOSS) && (roomEnemiesDefeated == numEnemiesInRoom)){
            print("Player has teleported to boss room");
        }

        //teleports to OPPOSITE door
        if (doorName == "NorthDoor" && (roomsCompleted < ROOMS_BEFORE_BOSS) && playerHasDefeatedAllEnemies){
            GameManager.instance.mainPlayer.transform.position = doorS.transform.position;
            
            GameObject.Find("Hallway").GetComponent<Hallway>().northDoor = true;
            GameObject.Find("Hallway").GetComponent<Hallway>().southDoor = false;
            GameObject.Find("Hallway").GetComponent<Hallway>().eastDoor = true;
            GameObject.Find("Hallway").GetComponent<Hallway>().westDoor = true;

            roomsCompleted++;
            roomEnemiesDefeated = 0;
            
            GameObject.Find("Randomizer").GetComponent<RandomGenerator>().EnemyRandomizer();
        }
        if (doorName == "SouthDoor" && (roomsCompleted < ROOMS_BEFORE_BOSS) && playerHasDefeatedAllEnemies){
            GameManager.instance.mainPlayer.transform.position = doorN.transform.position;
            
            GameObject.Find("Hallway").GetComponent<Hallway>().northDoor = false;
            GameObject.Find("Hallway").GetComponent<Hallway>().southDoor = true;
            GameObject.Find("Hallway").GetComponent<Hallway>().eastDoor = true;
            GameObject.Find("Hallway").GetComponent<Hallway>().westDoor = true;

            roomsCompleted++;
            roomEnemiesDefeated = 0;

            GameObject.Find("Randomizer").GetComponent<RandomGenerator>().EnemyRandomizer();
        }
        if (doorName == "EastDoor" && (roomsCompleted < ROOMS_BEFORE_BOSS) && playerHasDefeatedAllEnemies){
            GameManager.instance.mainPlayer.transform.position = doorW.transform.position;
            
            GameObject.Find("Hallway").GetComponent<Hallway>().northDoor = true;
            GameObject.Find("Hallway").GetComponent<Hallway>().southDoor = true;
            GameObject.Find("Hallway").GetComponent<Hallway>().eastDoor = true;
            GameObject.Find("Hallway").GetComponent<Hallway>().westDoor = false;

            roomsCompleted++;
            roomEnemiesDefeated = 0;

            GameObject.Find("Randomizer").GetComponent<RandomGenerator>().EnemyRandomizer();
        }
        if (doorName == "WestDoor" && (roomsCompleted < ROOMS_BEFORE_BOSS) && playerHasDefeatedAllEnemies){
            GameManager.instance.mainPlayer.transform.position = doorE.transform.position;
            
            GameObject.Find("Hallway").GetComponent<Hallway>().northDoor = true;
            GameObject.Find("Hallway").GetComponent<Hallway>().southDoor = true;
            GameObject.Find("Hallway").GetComponent<Hallway>().eastDoor = false;
            GameObject.Find("Hallway").GetComponent<Hallway>().westDoor = true;

            roomsCompleted++;
            roomEnemiesDefeated = 0;

            GameObject.Find("Randomizer").GetComponent<RandomGenerator>().EnemyRandomizer();
        }

        print("Teleporting player from: " + doorName + " door");
    }
    
    protected override void Update()
    {
        base.Update();
        if (PausePressed())
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pause = true;
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pause = false;
        pauseMenu.SetActive(false);
    }


    public void SwitchRoom(Hallway h) {
        currentRoom = h;
    }

    public void GetEnemeySpots() {

    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }
}