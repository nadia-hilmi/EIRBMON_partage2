using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        SavePosition("0x0079A758B3C95d47efBCda496a3019Bea366b1A1",2,2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator CatchedPokemon(string addr , int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("owner", addr);
        form.AddField("id", id);
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

    public static IEnumerator SavePosition(string wallet , int posX,int posY)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_wallet", wallet);
        form.AddField("user_x", posX);
        form.AddField("user_y",posY);
        Debug.Log(form);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3001/game/savePosition",  form))
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
