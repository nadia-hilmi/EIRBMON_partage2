using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetFromServer : MonoBehaviour
{

    [System.Serializable]
    public class Info 
    {
        public int _id;
        public int nft_id;
        public int nft_potential;
        public string nft_image;
        public string nft_hp;
        public string nft_level;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator GetWildPokemon(System.Action<string[]> callbackOnFinish)

    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3001/game/catchables"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                var res = www.downloadHandler.text;
                Info info = JsonUtility.FromJson<Info>(res);
                // Debug.Log("passed"+res);

                string[] infos = new string[5];
                infos[0] = info.nft_id.ToString();
                infos[1] = info.nft_potential.ToString();
                infos[2] = info.nft_hp;
                infos[3] = info.nft_level;
                infos[4] = info.nft_image;
                // Debug.Log(infos);
                callbackOnFinish(infos);
                yield return null;
            }
        }
    }
}
