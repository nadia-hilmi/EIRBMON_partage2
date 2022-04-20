using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Color highlightedColor;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon=pokemon;
        nameText.text=pokemon.Base.name;
        levelText.text="Lvl " + pokemon.Level;
        hpBar.SetHP((float) pokemon.HP/pokemon.MaxHp); //hp normalized
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color=highlightedColor;
        else    
            nameText.color=Color.black;
    }
}
