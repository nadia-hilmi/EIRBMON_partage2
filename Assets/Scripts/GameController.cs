using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public enum GameState{FreeRoam,Battle, Menu}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    GameState state;

    MenuController menuController;
    
    public void setWalletUser (string wallet) {
        playerController.setWallet(wallet);
        Debug.Log("Le wallet est: " );
        Debug.Log(wallet);
        string[] positionFromServer = new string[2]; 
        Coroutine coroutine = StartCoroutine(GetFromServer.GetPosition(wallet, changePosition));
    }

     
    /*
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
    }*/

    

    public void Start(){
        
        playerController.OnEncountered+=StartBattle;
        battleSystem.OnBattleOver+=EndBattle;

        menuController = GetComponent<MenuController>();

        menuController.onBack += () => 
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += onMenuSelected; 
        //GetFromServer.GetPosition(playerController.getWallet());

        
        
    }

   /* public void receivePosition(){
        var wallet=playerController.getWallet();
        Debug.Log("Le wallet est: " );
        Debug.Log(wallet);
        string[] positionFromServer = new string[2]; 
        Coroutine coroutine = StartCoroutine(GetFromServer.GetPosition(changePosition));

    }*/

    public void changePosition(string[] position){
        foreach(string el in position)
            Debug.Log(el);
        var posX = Int16.Parse(position[0]);
        var posY = Int16.Parse(position[1]);
        playerController.transform.position = new Vector3(posX,posY,0);
        Debug.Log("le joueur a changé de position");
    }

    void StartBattle(){
        state=GameState.Battle;
        // battleSystem.gameObject.SetActive(true);
        // worldCamera.gameObject.SetActive(false);
        var playerParty= playerController.GetComponent<PokemonParty>();
        // var playerParty = playerController.getParty();
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
         else if ( selector == 3)
         {
            var playerParty=playerController.GetComponent<PokemonParty>();
            foreach( var pokemon in playerParty.Pokemons){
                pokemon.SetHP(1);
            }
            Debug.Log("All pokemons healed");
         }

         state = GameState.FreeRoam;
     }

}
