using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Pokemon",menuName ="Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string Name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;
    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    //Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    [SerializeField] List<LearnableMove> learnableMoves;

    public int GetSpeed()
    {
        return speed;
    }
    public int GetSpDefense()
    {
        return spDefense;
    }
    public int GetSpAttack()
    {
        return spAttack;
    }
    public int GetDefense()
    {
        return defense;
    }
    public int GetAttack()
    {
        return attack;
    }
    public int GetMaxHp()
    {
        return maxHp;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetFrontSprite()
    {
        return frontSprite;
    }

    public Sprite GetBackSprite()
    {
        return backSprite;
    }

    public PokemonType GetType1()
    {
        return type1;
    }
    public PokemonType GetType2()
    {
        return type2;
    }

    public List<LearnableMove> GetLearnableMoves()
    {
        return learnableMoves;
    }

  
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase GetBase()
    {
        return moveBase;
    }
    public int GetLevel()
    {
        return level;
    }

}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon
}

public enum Stat{ //boost stat
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,
}

public class TypeChart
{
    static float[][]chart=
    {   //                 NOR  FIR  WAT  ELE  GRA ICE FIG  POI
        /*NOR*/new float[]{ 1f, 1f , 1f , 1f , 1f , 1f , 1f ,1f },
        /*FIR*/new float[]{ 1f,0.5f ,0.5f , 1f , 2f , 2f , 1f , 1f },
        /*WAT*/new float[]{ 1f, 2f , 0.5f , 2f , 0.5f , 1f , 1f , 1f},
        /*ELE*/new float[]{ 1f, 1f , 2f , 0.5f , 0.5f , 2f , 1f ,1f },
        /*GRA*/new float[]{ 1f, 0.5f , 2f , 2f , 0.5f , 1f , 1f , 0.5f},
        /*POI*/new float[]{ 1f, 1f , 1f , 1f , 2f , 1f , 1f , 1f}
    };

    public static float GetEffectiveness(PokemonType attackType,PokemonType defenseType){
        if(attackType==PokemonType.None || defenseType==PokemonType.None){
            return 1;
        }
        int row= (int) attackType-1;
        int col= (int) defenseType-1;

        return chart[row][col];
    }
}