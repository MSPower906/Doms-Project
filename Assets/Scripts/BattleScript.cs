using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Classification
{
    Pride, Humility,

    Greed, Generosity,

    Lust, Chastity,

    Envy, Charity,

    Gluttony, Temperance,

    Sloth, Diligence,

    Wrath, Patience,

}
public enum ActionEffects { None, Stun, Buff, Debuff } //used for attack and item effects as needed
public class Resistances
{
    public Classification unitResType;
    public int unitResAmount;
}

public enum BattleState { Start, Waiting, Enemy, Win, Loss }

public class BattleScript : MonoBehaviour
{
    public BattleState state;

    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;

    public Transform[] enemyBattlePos;
    public Transform[] playerBattlePos;

    public List<GameObject> turnlist;
    
    public List<GameObject> playerCharacters;

    [SerializeField] private CombatUI combatUI;
    [SerializeField] private int currentTurnOrderIndex;
    [SerializeField] private CharacterStatsScript currentCharacter;
    [SerializeField] private bool actionSelected;
    [SerializeField] private Camera cam;

    private int enemiesCount;
    private int playerCount;
    private int chosenAbility;

    void Start()
    {
        state = BattleState.Start;
        currentTurnOrderIndex = 0;
        SetupBattle();
    }

    void SetupBattle()
    {
        //Select party members and what enemies they will fight
        turnlist = new List<GameObject>();

        //Runs through the selected part memebers and enemies add them to a list
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            Transform spawnpos = playerBattlePos[i];
            var chosenplayer = Instantiate(playerPrefabs[i], spawnpos.position, transform.rotation, transform.parent);
            turnlist.Add(chosenplayer);
            playerCharacters.Add(chosenplayer);
            playerCount++;
        }
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            Transform spawnpos = enemyBattlePos[i];
            var chosenenemy = Instantiate(enemyPrefabs[i], spawnpos.position, transform.rotation, transform.parent);
            turnlist.Add(chosenenemy);
            enemiesCount++;
        }

        //Organises the list depending on each characters speed stat
        turnlist = turnlist.OrderByDescending(gameObject => gameObject.GetComponent<CharacterStatsScript>().unitSpd).ToList();

        StartOfTurn();
    }

    void StartOfTurn()
    {

        //Add any start of turn stuff we want e.g DOT/effects, could always broadcast a message to a flowchart?

        currentCharacter = turnlist[currentTurnOrderIndex].GetComponent<CharacterStatsScript>();

        switch (currentCharacter.playerCharacter)
        {
            case true: PlayerTurnUISet(); break;
            case false: AITurn(); break;
        }
    }


    void AITurn()
    {

        chosenAbility = Random.Range(0, currentCharacter.abilities.Length);
        int target = Random.Range(0, playerCharacters.Count);
        //Send attack info to the battle script to be assigned 

        Debug.Log("I am attacking:" + playerCharacters[target].name + "With" + currentCharacter.abilities[chosenAbility].abilName);
        TargetSelected(playerCharacters[target].GetComponent<CharacterStatsScript>());
        
    }

    void PlayerTurnUISet()
    {
        combatUI.SetUI(currentCharacter);
    }

    public void AbilitySelected(int abilityInt)
    {
        chosenAbility = abilityInt;

        if (currentCharacter.abilities[chosenAbility].targetEnemy)
        {
            foreach (var character in turnlist)
            {
                if (character.GetComponent<CharacterStatsScript>().playerCharacter == false)
                {

                    character.gameObject.transform.Find("Target").gameObject.SetActive(true);

                }
            }
        }
        else
        {
            foreach (var character in turnlist)
            {
                if (character.GetComponent<CharacterStatsScript>().playerCharacter == true)
                {
                    //character.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }

        actionSelected = true;
        StartCoroutine(WaitingForActions());
    }

    public IEnumerator WaitingForActions()
    {
        while (actionSelected)
        {

            if (Input.GetKey(KeyCode.Mouse0))
            {

                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "Target")
                    {
                        TargetSelected(hit.transform.gameObject.transform.GetComponentInParent<CharacterStatsScript>());

                        actionSelected = false;
                    }
                }
            }

            yield return new WaitForEndOfFrame();

        }
    }

    public void TargetSelected(CharacterStatsScript stats)
    {
       
        currentCharacter.currentMana -= currentCharacter.abilities[chosenAbility].manaCost;

        stats.currentHealth -= currentCharacter.abilities[chosenAbility].damage;

        if (stats.currentHealth <= 0)
        {
            turnlist.Remove(stats.gameObject);

            Debug.Log(stats.gameObject + "Died");

            switch (stats.playerCharacter)
            {
                case true: playerCount--; playerCharacters.Remove(stats.gameObject); break;
                case false: enemiesCount--; break;
            }

            stats.gameObject.SetActive(false);

            WinLossCheck();
        }

        //Effected the chosen targets + anis

        //Update effect UIs

        //Check if fight is over


      
    }

    void WinLossCheck()
    {

        if (enemiesCount <= 0)
        {
            //Player wins
            Debug.Log("You Win!");
            return;
        }
        else if (playerCount <= 0)
        {
            //Player Loss
            Debug.Log("You lose!");
            return;
        }


        EndTurn();
    }

    public void EndTurn()
    {
        currentTurnOrderIndex++;
        if (currentTurnOrderIndex >= turnlist.Count)
        {
            currentTurnOrderIndex = 0;
        }

        StartOfTurn();
    }
}
