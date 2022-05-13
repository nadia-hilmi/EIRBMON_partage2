using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{FreeRoam,Battle}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    GameState state;

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
            pokemon.SetHP(hp);
            i+=1;
        }
    }
    

    private void Start(){
        playerController.OnEncountered+=StartBattle;
        battleSystem.OnBattleOver+=EndBattle;
    }

    void StartBattle(){
        state=GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        var playerParty= playerController.GetComponent<PokemonParty>();
        var wildPokemon=FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();
        battleSystem.StartBattle(playerParty,wildPokemon);
    }

    void EndBattle(bool won){
        state=GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }
    private void Update(){
        if(state==GameState.FreeRoam){
            playerController.HandleUpdate();
        }
        else if (state==GameState.Battle){
            battleSystem.HandleUpdate();
        }
    }

}
