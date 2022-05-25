using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        //SavePosition("0x0079A758B3C95d47efBCda496a3019Bea366b1A1",2,2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator CatchedPokemon(string wallet , int tokenId)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_wallet", wallet);
        form.AddField("nft_id", tokenId);
        Debug.Log(form);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3001/unity/catching",  form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete");
            }
        }
    }
    public static IEnumerator Save(string wallet , float x, float y, PokemonParty pokemonsParty)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_wallet", wallet);
        form.AddField("user_x", x.ToString("0.0000"));
        form.AddField("user_y", y.ToString("0.0000"));

        List<Pokemon> pokemons = pokemonsParty.getList();

        foreach (var pokemon in pokemons)
        {
            var id = pokemon.getId();
            var level = pokemon.getLevel();
            form.AddField("nft_id", id);
            form.AddField("nft_level", level);
        }

        Debug.Log(form);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3001/game/save",  form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete");
            }
        }
    }
}
