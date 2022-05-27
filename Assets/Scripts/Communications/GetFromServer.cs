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

    public class PlayerData{
        public int posX;
        public int posY;
    }

    


    public static IEnumerator GetWildPokemon(System.Action<string[]> callbackOnFinish)

    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3001/game/catchables"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success){
                Debug.Log(www.error);
            }
            else
            {
                var res = www.downloadHandler.text;
                Info info = JsonUtility.FromJson<Info>(res);
                //Debug.Log("passed"+info);

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


    public static IEnumerator GetPosition(string wallet, System.Action<string[]> callbackOnFinish)

    {
        // string wallet="0x0079A758B3C95d47efBCda496a3019Bea366b1A1";
        WWWForm form = new WWWForm();
        form.AddField("user_wallet", wallet);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3001/game/position",form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success){
                Debug.Log(www.error);
            }
            else
            {
                var res = www.downloadHandler.text;
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(res);

                string[] data = new string[2];
                data[0] = playerData.posX.ToString();
                data[1] = playerData.posY.ToString();
                
                callbackOnFinish(data);
                yield return null;
            }
        }
    }
}
