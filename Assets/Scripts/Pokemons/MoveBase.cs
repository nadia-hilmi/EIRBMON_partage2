using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move",menuName ="Pokemon/Create new move")]
public class MoveBase : ScriptableObject
{
   [SerializeField] string Name;

   [TextArea]
   [SerializeField] string description;
   [SerializeField] PokemonType type;
   [SerializeField] int power;
   [SerializeField] int accuracy;
   [SerializeField] int pp; // number of times a move can be performed

   public string GetName()
    {
        return Name;
    }

    public string GetDescription()
    {
        return description;
    }

    public int GetPp()
    {
        return pp;
    }

    public int GetPower()
    {
        return power;
    }

    public int GetAccuracy()
    {
        return accuracy;
    }

    public PokemonType GetType()
    {
        return type;
    }

    public bool isSpecial{
        get{
            if(type==PokemonType.Fire || type==PokemonType.Water|| type==PokemonType.Grass|| type==PokemonType.Ice || type==PokemonType.Electric|| type==PokemonType.Dragon){
                return true;
            }
            else{
                return false;
            }
        }
    }

}
