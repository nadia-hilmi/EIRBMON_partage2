using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{FreeRoam,Battle, Menu}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    GameState state;

    MenuController menuController;
    public void playerInitializePosition () {
        playerController.transform.position = new Vector3(0, 0, 0);
    }
    public void playerSetPositionX (int pos) {
        playerController.transform.position += new Vector3(pos, 0, 0);
    }

    public void playerSetPositionY (int pos) {
        playerController.transform.position += new Vector3(0, pos, 0);
    }

    public void UpdateLevel(string levelText){
        var playerParty= playerController.GetComponent<PokemonParty>();
        int i=0;
        string[] eachLevelText = levelText.Split(",");
        
        foreach(var pokemon in playerParty.Pokemons){
            int level=int.Parse(eachLevelText[i]);
            pokemon.setLevel(level);
            i+=1;
        }
    }

    public void UpdateHP(string hpText){
        var playerParty= playerController.GetComponent<PokemonParty>();
        int i=0;
        string[] eachHpText = hpText.Split(",");
        
        foreach(var pokemon in playerParty.Pokemons){
            int hp=int.Parse(eachHpText[i]);
            // pokemon.SetHP(hp);
            i+=1;
        }
    }
    

    private void Start(){
        playerController.OnEncountered+=StartBattle;
        battleSystem.OnBattleOver+=EndBattle;

        menuController = GetComponent<MenuController>();

        menuController.onBack += () => 
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += onMenuSelected; 
    }

    void StartBattle(){
        state=GameState.Battle;
        // battleSystem.gameObject.SetActive(true);
        // worldCamera.gameObject.SetActive(false);
        var playerParty= playerController.GetComponent<PokemonParty>();
        var wildPokemon=FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon(playerParty);
        // battleSystem.StartBattle(playerParty,wildPokemon);
    }

    void EndBattle(bool won){
        state=GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }
    private void Update(){
        if(state==GameState.FreeRoam){
            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.M)){
                menuController.OpenMenu();
                state = GameState.Menu;
                 
            }
        }
        else if (state==GameState.Battle){
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Menu){
            menuController.HandleUpdate();
        }
    }

     void onMenuSelected(int selector) 
     {
         if (selector == 0)
         {
             // pokemon
         }
         else if (selector == 1)
         {
             // bag
         }
         else if ( selector == 2)
         {
             // save
             var playerParty= playerController.GetComponent<PokemonParty>();
             float[] position = playerController.getPosition();
            //  string wallet = playerController.getWallet();

             StartCoroutine(SendToServer.Save("0x0klj13jklj24", position[0], position[1], playerParty));
         }

         state = GameState.FreeRoam;
     }

}
