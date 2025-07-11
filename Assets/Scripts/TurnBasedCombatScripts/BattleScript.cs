using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField] private EncountersDatabaseScript encounterDatabase;
    [SerializeField] private PartyDatabaseScript partyDatabase;

    public static int ChosenEncounterIndex;

    public BattleState state;

    public Transform[] enemyBattlePos;
    public Transform[] playerBattlePos;

    public List<GameObject> turnlist;
    
    public List<GameObject> alivePlayerCharacters;

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
        
        SetupBattle(encounterDatabase.encounters[ChosenEncounterIndex].enemies, partyDatabase.playerParty);
        //gets the chosen encounters enemy list to be used during battle set up.
    }


    void SetupBattle(GameObject[] enemyEncounter, GameObject[] playerParty)
    {
        //Select party members and what enemies they will fight
        turnlist = new List<GameObject>();


        //Runs through the selected part memebers and enemies add them to a list
        for (int i = 0; i < playerParty.Length; i++)
        {
       
                Transform spawnpos = playerBattlePos[i];
                var chosenplayer = Instantiate(playerParty[i], spawnpos.position, transform.rotation, transform.parent);
                turnlist.Add(chosenplayer);
                alivePlayerCharacters.Add(chosenplayer);

                playerCount++;
            
        }
        for (int i = 0; i < enemyEncounter.Length; i++)
        {
            Transform spawnpos = enemyBattlePos[i];
            var chosenenemy = Instantiate(enemyEncounter[i], spawnpos.position, transform.rotation, transform.parent);
            chosenenemy.name = enemyEncounter[i].name + i;
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
        int target = Random.Range(0, alivePlayerCharacters.Count);
        
      
        //Send attack info to the battle script to be assigned 

        //selects a random ability that the AI has and attacks a random player character from the one curretly still alive.
        TargetSelected(alivePlayerCharacters[target].GetComponent<CharacterStatsScript>());
        
    }

    void PlayerTurnUISet()
    {
        combatUI.SetUI(currentCharacter);
    }

    void TargetToggle()
    {
        if (currentCharacter.abilities[chosenAbility].targetEnemy)
        {
            foreach (var character in turnlist)
            {
                if (character.GetComponent<CharacterStatsScript>().playerCharacter == false)
                {
                    GameObject target = character.gameObject.transform.Find("Target").gameObject;
                    target.SetActive(!target.activeInHierarchy);

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
    }

    public void AbilitySelected(int abilityInt)
    {
        chosenAbility = abilityInt;
        TargetToggle();

        actionSelected = true;
        StartCoroutine(SelectTarget());
    }

    public IEnumerator SelectTarget()
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
                        TargetToggle();
                    }
                }
            }

            yield return new WaitForEndOfFrame();

        }
    }

    public void TargetSelected(CharacterStatsScript stats)
    {
       combatUI.NullUI();
        currentCharacter.currentMana -= currentCharacter.abilities[chosenAbility].manaCost;

        stats.currentHealth -= currentCharacter.abilities[chosenAbility].damage;

        if (stats.currentHealth <= 0)
        {
            turnlist.Remove(stats.gameObject);

            Debug.Log(stats.gameObject + "Died");

            stats.gameObject.SetActive(false);

            switch (stats.playerCharacter)
            {
                case true: playerCount--; alivePlayerCharacters.Remove(stats.gameObject); break;
                case false: enemiesCount--; break;
            }

            
            
        }

        WinLossCheck();
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
