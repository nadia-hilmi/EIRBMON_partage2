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
    MapArea mapArea;
    GameState state;

    MenuController menuController;
    
    public void initialisationData (string wallet) {
        //on recupere le wallet
        playerController.setWallet(wallet);
        Debug.Log("Le wallet est: " );
        Debug.Log(wallet);
        //on recupere la position du player
        string[] positionFromServer = new string[2]; 
        Coroutine coroutinePosition = StartCoroutine(GetFromServer.GetPosition(wallet, changePosition));
        //on recupere les eirbees du player
        string[] eirbeesIDFromServer = new string[6]; 
        Coroutine coroutineEirbeesID = StartCoroutine(GetFromServer.GetEirbeesID(wallet, setEirbeesID));
        string[] eirbeesHPFromServer = new string[6]; 
        Coroutine coroutineEirbeesHP = StartCoroutine(GetFromServer.GetEirbeesHP(wallet, setEirbeesHP));
        string[] eirbeesLevelFromServer = new string[6]; 
        Coroutine coroutineEirbeesLevel = StartCoroutine(GetFromServer.GetEirbeesLevel(wallet, setEirbeesLevel));

        string[] eirbeesImageFromServer = new string[6]; 
        Coroutine coroutineEirbeesImage = StartCoroutine(GetFromServer.GetEirbeesImages(wallet, setEirbeesImage));

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


    public void setEirbeesImage(string[] eirbees){
        int[] image= new int[6];
        for(int i=0;i<6;i++)
            image[i]=Int16.Parse(eirbees[i]);
        playerController.setImages(image);
        // Debug.Log("le joueur a bien recu les images");
        // Debug.Log(playerController.images[0]);
        // Debug.Log("Fin verifications images");
    }
    public void setEirbeesHP(string[] eirbees){
        int[] hp= new int[6];
        for(int i=0;i<6;i++)
            hp[i]=Int16.Parse(eirbees[i]);
        playerController.setHP(hp);
        // Debug.Log("le joueur a bien recu les HP");
        // Debug.Log(playerController.hp[0]);
        // Debug.Log("Fin verifications HP");
    }

    public void setEirbeesLevel(string[] eirbees){
        int[] level= new int[6];
        for(int i=0;i<6;i++)
            level[i]=Int16.Parse(eirbees[i]);
        playerController.setLevel(level);
        // Debug.Log("le joueur a bien recu les levels");
        // Debug.Log(playerController.level[0]);
        // Debug.Log("Fin verifications levels");
    }

    public void setEirbeesID(string[] eirbees){
         int[] level= new int[6];
        for(int i=0;i<6;i++)
            level[i]=Int16.Parse(eirbees[i]);
        playerController.setID(level);
        // Debug.Log("le joueur a bien recu les ids");
        // Debug.Log(playerController.id[0]);
        // Debug.Log("Fin verifications ids");
    }

    public void changePosition(string[] position){
        foreach(string el in position)
            Debug.Log(el);
        var posX = Int16.Parse(position[0]);
        var posY = Int16.Parse(position[1]);
        playerController.transform.position = new Vector3(posX,posY,0);
        Debug.Log("le joueur a changé de position");
    }

    void StartBattle(){
       FindObjectOfType<MapArea>().GetComponent<MapArea>().createPlayerParty(playerController);
        // mapArea.createPlayerParty(playerController);
        state=GameState.Battle;
        // battleSystem.gameObject.SetActive(true);
        // worldCamera.gameObject.SetActive(false);
        // var playerParty= playerController.GetComponent<PokemonParty>();
        var playerParty = playerController.getPokemonParty();
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
