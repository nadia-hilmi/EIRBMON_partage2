using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, PartyScreen, BattleOver}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    

    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;//to determine when a battle is over
                                           //bool to know if the player won or lost the battle
    BattleState state;
    int currentAction;
    int currentMove;
    int currentMember;

    PokemonParty playerParty;
    Pokemon wildPokemon;

    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(wildPokemon);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.name} appeared.");

        ActionSelection();
    }

    void BattleOver(bool won) // to know if the batlle is over
    {
        state=BattleState.BattleOver;
        OnBattleOver(won);
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }

    void OpenPartyScreen(){
        state= BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    void MoveSelection()
    {

        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;
        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return RunMove(playerUnit,enemyUnit,move);
        //If the battle stat was not changer by RunMOve then continue the battle
        if(state==BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());
        }
           
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;
        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return RunMove(enemyUnit,playerUnit,move);
        //If the battle stat was not changer by RunMOve then continue the battle
        if(state==BattleState.PerformMove)
        {
            ActionSelection();
        }
            
    }

    IEnumerator RunMove(BattleUnit sourceUnit,BattleUnit targetUnit, Move move)
    {
        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.name} used {move.Base.name}");

        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        targetUnit.PlayerHitAnimation();

        var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
        yield return targetUnit.Hud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.name} fainted. ");
            targetUnit.PlayerFaintAnimation();

            yield return new WaitForSeconds(2f);
            CheckForBattleOver(targetUnit);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if(faintedUnit.IsPlayerUnit)
        {
            var nextPokemon= playerParty.GetHealthyPokemon();
            if (nextPokemon!=null){
                OpenPartyScreen();
            }
            else{
                BattleOver(false); // false= the player lost
            }

        }
        else    
            BattleOver(true);
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("A critical hit!");
        }
        if (damageDetails.TypeEffectiveness > 1)
        {
            yield return dialogBox.TypeDialog("It's a super effective attack !");
        }
        else if (damageDetails.TypeEffectiveness < 1)
        {
            yield return dialogBox.TypeDialog("It's not a very effective attack...");
        }
    }

    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartyScreenSelection();
        }
    }

    void HandleActionSelection()
    {
        //choose to fight, run, bag or choose pokemon
        if(Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction+=2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction-=2;

        currentAction=Mathf.Clamp(currentAction,0,3); //check if 0<=currentAction<=3
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentAction == 0)
            {
                //Fight selected
                MoveSelection();
            }
            else if (currentAction == 1)
            {
                //Bag selected
            }
            else if (currentAction == 2)
            {
                //Pokemon selected
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                //Run selected
            }
        }
    }

    void HandleMoveSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMove;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove+=2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove-=2;

        currentMove=Mathf.Clamp(currentMove,0,playerUnit.Pokemon.Moves.Count-1); //check if 0<=currentMove<=nb moves pokemon 
        
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);
        if (Input.GetKeyDown(KeyCode.Return))
        {
            dialogBox.EnableMoveSelector(false); //on enlève texte pour choisir moves
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false); //on enlève texte pour choisir moves
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }

    }

    void HandlePartyScreenSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMember;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMember+=2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMember-=2;

        currentMember=Mathf.Clamp(currentMember,0,playerParty.Pokemons.Count-1); //check if 0<=currentMove<=nb moves pokemon 
        
        partyScreen.UpdateMemberSelection(currentMember);

        //selection of a Pokemon

        if(Input.GetKeyDown(KeyCode.Return))
        {
            var selectedMember=playerParty.Pokemons[currentMember];
            if(selectedMember.HP<=0)
            {
                partyScreen.SetMessageText("You can't send out a fainted pokemon");
                return; //si pokemon HP<=0
            }
            if(selectedMember==playerUnit.Pokemon)
            {   
                partyScreen.SetMessageText("You can't swotch with the same pokemon");
                return; //si pokemon HP<=0

            }
            partyScreen.gameObject.SetActive(false);
            state=BattleState.Busy;
            StartCoroutine(SwitchPokemon(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            ActionSelection();
        }

    }

    IEnumerator SwitchPokemon(Pokemon newPokemon)
        {
            if(playerUnit.Pokemon.HP>0)
            {
                yield return dialogBox.TypeDialog($"Come back {playerUnit.Pokemon.Base.name}");
                playerUnit.PlayerFaintAnimation();
                yield return new WaitForSeconds(2f);
            }
            

            playerUnit.Setup(newPokemon);
            dialogBox.SetMoveNames(newPokemon.Moves);
            yield return dialogBox.TypeDialog($"Go {newPokemon.Base.name}!");

            StartCoroutine(EnemyMove());
                
        }
}
