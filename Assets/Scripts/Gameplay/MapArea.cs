using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;


public class MapArea : MonoBehaviour
{
    [SerializeField] List<Pokemon> wildPokemons;
    [SerializeField] List<PokemonBase> PokemonsBase;
    // private Pokemon wildPokemon;
    public Sprite[] spriteArray;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    PokemonParty party;


    public Pokemon GetRandomWildPokemon(PokemonParty playerParty){
        party = playerParty;

        // get id, iq, hp, level and img from server
        string[] pokemonFromServer = new string[5]; 
        Coroutine coroutine = StartCoroutine(GetFromServer.GetWildPokemon(OnComplete));

        return null;
    }

    public void OnComplete(string[] value) {
        foreach(string el in value)
            Debug.Log(el);
        Debug.Log(wildPokemons.Count);

        // var wildPokemon = wildPokemons[0];
        var pokeBase = PokemonsBase[0];
        var tokenId = Int16.Parse(value[0]);
        var level = Int16.Parse(value[3]);
        var itemId = Int16.Parse(value[4]);
        string image = value[4];       


        if ( itemId <= 1000){
            // wildPokemon = wildPokemons[0];
            pokeBase = PokemonsBase[0];
            pokeBase.SetFrontSprite(spriteArray[itemId-1]);

        }

        else if (itemId > 1000 && itemId <= 2000){
            // wildPokemon = wildPokemons[1];
            pokeBase = PokemonsBase[1];
            pokeBase.SetFrontSprite(spriteArray[itemId-1]);
            // wildPokemon.Init();
            // battleSystem.gameObject.SetActive(true);
            // worldCamera.gameObject.SetActive(false);
            // battleSystem.StartBattle(party,wildPokemon);
            // return;

        }
        else if (itemId > 2000 && itemId <= 3000){
            // wildPokemon = wildPokemons[2];
            pokeBase = PokemonsBase[2];
            pokeBase.SetFrontSprite(spriteArray[itemId-1]);

        }
        else if (itemId > 3000){
            // wildPokemon = wildPokemons[3];
            pokeBase = PokemonsBase[3];
            pokeBase.SetFrontSprite(spriteArray[itemId-1]);
           
        }

       
        
        Pokemon newPoke = new Pokemon(pokeBase, level, tokenId);
        
        newPoke.Init();
        // wildPokemon.Init();

        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        // battleSystem.StartBattle(party,wildPokemon);
        battleSystem.StartBattle(party, newPoke);
    }
}
