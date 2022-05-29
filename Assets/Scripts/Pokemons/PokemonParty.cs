using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    private List<Pokemon> pokemons;

    public PokemonParty(List<Pokemon> _pokemons){
        pokemons = _pokemons;
    }
    public List<Pokemon> Pokemons{
        get{
            return pokemons;
        }
    }

    public List<Pokemon> getList(){return pokemons;}

    private void Start(){
        foreach(var pokemon in pokemons){
            pokemon.Init();
        }

    }


    public Pokemon GetHealthyPokemon(){
        return pokemons.Where(x=>x.HP>0).FirstOrDefault();//return le premier pokemon avec HP>0
    }

    public void AddPokemon(Pokemon newPokemon) 
    {
        if (pokemons.Count < 6)
        {
            pokemons.Add(newPokemon);
        }
        else
        {
            // TODO: ADD to the pc
        }
    }

    public void number()
    {
        Debug.Log(pokemons.Count);
    }
}
