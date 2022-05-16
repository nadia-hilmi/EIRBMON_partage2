using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Catching : MonoBehaviour
{
    // Start is called before the first frame update
    // void Start()
    // {
    //
    // }

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
}
