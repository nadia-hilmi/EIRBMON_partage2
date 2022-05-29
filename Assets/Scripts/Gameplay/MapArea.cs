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
        // foreach(string el in value)
            // Debug.Log(el);
        // Debug.Log(wildPokemons.Count);

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


    public void createPlayerParty(PlayerController player)
    {
        List<Pokemon> pokemons = new List<Pokemon>();
        var ids = player.getIds();
        Debug.Log("ids length");
        Debug.Log(ids.Length);
        var levels = player.getPokemonsLevel();
        int i = 0;
        var itemId = player.getPokemonImages();
        // Debug.Log("pokemonbase length");
        // Debug.Log(PokemonsBase.Count);
        var pokeBase = PokemonsBase[0];
        foreach(int id in ids)
        {
            Debug.Log("current id");
            Debug.Log(id);
            if(id == 0){i++;break;}
            Debug.Log("current image id");
            Debug.Log(itemId[i]);
            if ( itemId[i] <= 1000){
                pokeBase = PokemonsBase[0];
                pokeBase.SetFrontSprite(spriteArray[itemId[i]-1]);
                pokeBase.SetBackSprite(spriteArray[itemId[i]-1]);

            }
            else if (itemId[i] > 1000 && itemId[i] <= 2000){
                pokeBase = PokemonsBase[1];
                pokeBase.SetFrontSprite(spriteArray[itemId[i]-1]);
                pokeBase.SetBackSprite(spriteArray[itemId[i]-1]);
            }
            else if (itemId[i] > 2000 && itemId[i] <= 3000){
                pokeBase = PokemonsBase[2];
                pokeBase.SetFrontSprite(spriteArray[itemId[i]-1]);
                pokeBase.SetBackSprite(spriteArray[itemId[i]-1]);
            }
            else if (itemId[i] > 3000){
                pokeBase = PokemonsBase[3];
                pokeBase.SetFrontSprite(spriteArray[itemId[i]-1]);
                pokeBase.SetBackSprite(spriteArray[itemId[i]-1]);
            }
            Pokemon newpoke = new Pokemon(pokeBase, levels[i], id);
            newpoke.Init();
            pokemons.Add(newpoke);
            i++;
        }
        Debug.Log("Pokemon party length final");
        Debug.Log(pokemons.Count);
        PokemonParty pokemonParty = new PokemonParty(pokemons);
        player.setPokemonParty(pokemonParty);
        Debug.Log("finished creating party");

    }
}
