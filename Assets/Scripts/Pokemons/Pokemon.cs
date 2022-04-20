using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Pokemon 
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;   
    public PokemonBase Base {
        get{
            return _base;
        }
    }
    public int Level {
        get{
            return level;
        }
    }

    public int HP {get; set;} //current HP of the pokemon

    public List<Move> Moves {get; set;}

    public void Init()
    {
        HP=MaxHp;

        //generates moves to pokemon according to his level
        Moves= new List<Move>();
        foreach (var move in Base.GetLearnableMoves())
        {
            if(move.GetLevel()<=Level)
            {
                Moves.Add(new Move(move.GetBase()));
            }
            if(Moves.Count>=4)
            {
                break;
            }
        }
    }

    public int Attack{
        get{return Mathf.FloorToInt((Base.GetAttack()*Level)/100f)+5;}
    }

    public int Defense{
        get{return Mathf.FloorToInt((Base.GetDefense()*Level)/100f)+5;}
    }

    public int SpAttack{
        get{return Mathf.FloorToInt((Base.GetSpAttack()*Level)/100f)+5;}
    }

    public int SpDefense{
        get{return Mathf.FloorToInt((Base.GetSpDefense()*Level)/100f)+5;}
    }
    public int Speed{
        get{return Mathf.FloorToInt((Base.GetSpeed()*Level)/100f)+5;}
    }

    public int MaxHp{
        get{return Mathf.FloorToInt((Base.GetMaxHp()*Level)/100f)+10;}
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    //return true if batlle is finished else the battle continue=false
    {
        float critical = 1f;
        if(Random.value*100f<=6.25f)
        {
            critical=2f;
        }
        float type= TypeChart.GetEffectiveness(move.Base.GetType(),this.Base.GetType1())*TypeChart.GetEffectiveness(move.Base.GetType(),this.Base.GetType2());//puissance d'attaque selon le type de pokemon
        
        var damageDetails= new DamageDetails()
        {
            TypeEffectiveness=type,
            Critical=critical,
            Fainted=false,
        };

        float attack=(move.Base.isSpecial)? attacker.SpAttack : attacker.Attack; //savoir si c'est une attaque spécial true=> SpAttack
        float defense=(move.Base.isSpecial)? SpDefense : Defense;

        float modifiers= Random.Range(0.85f,1f)*type*critical;
        float a = (2*attacker.Level+10)/250f;
        float d = a*move.Base.GetPower()*((float)attack/defense)+2;
        int damage=Mathf.FloorToInt(d*modifiers);

        HP-=damage;
        if(HP<=0)
        {
            HP=0;
            damageDetails.Fainted=true;
        }

        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r= Random.Range(0,Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted{get; set;}
    public float TypeEffectiveness {get;set;}
    public float Critical {get; set;}
    
}