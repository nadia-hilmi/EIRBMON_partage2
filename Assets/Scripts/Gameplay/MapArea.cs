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
    private string[] res;
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

        var wildPokemon = wildPokemons[0];
        var id = Int16.Parse(value[0]);
        var level = Int16.Parse(value[3]);
        string image = value[4];       


        if (id > 0 && id < 1000)
            wildPokemon = wildPokemons[0];
        else if (id >= 1000 && id < 2000)
            wildPokemon = wildPokemons[1];
        else if (id >= 2000 && id < 3000)
            wildPokemon = wildPokemons[2];
        else if (id >= 3000)
            wildPokemon = wildPokemons[3];


        Pokemon newPoke = new Pokemon(wildPokemon, level, id);

        newPoke.Init();
        // wildPokemon.Init();

        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        // battleSystem.StartBattle(party,wildPokemon);
        battleSystem.StartBattle(party, newPoke);
    }
}
