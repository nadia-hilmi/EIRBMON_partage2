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

    public class EirbeesDataID{

        public int id0;
        public int id1;
        public int id2;
        public int id3;
        public int id4;
        public int id5;

    }

    public class EirbeesDataHP{

        public int hp0;
        public int hp1;
        public int hp2;
        public int hp3;
        public int hp4;
        public int hp5;

    }

    public class EirbeesDataLevel{

        public int lvl0;
        public int lvl1;
        public int lvl2;
        public int lvl3;
        public int lvl4;
        public int lvl5;

    }
    

    public class EirbeesDataImage{

        public int img0;
        public int img1;
        public int img2;
        public int img3;
        public int img4;
        public int img5;

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

    public static IEnumerator GetEirbeesID(string wallet, System.Action<string[]> callbackOnFinish)

    {
        WWWForm form = new WWWForm();
        form.AddField("user_wallet", wallet);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3001/game/dataEirbeesID",form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success){
                Debug.Log(www.error);
            }
            else
            {
                var res = www.downloadHandler.text;
                EirbeesDataID eirbeesData = JsonUtility.FromJson<EirbeesDataID>(res);

                string[] data= new string [6];
                data[0] = eirbeesData.id0.ToString();
                data[1] = eirbeesData.id1.ToString();
                data[2] = eirbeesData.id2.ToString();
                data[3] = eirbeesData.id3.ToString();
                data[4] = eirbeesData.id4.ToString();
                data[5] = eirbeesData.id5.ToString();
                Debug.Log("data 0: id");
                Debug.Log(data[0]);
                Debug.Log("data 5: id");
                Debug.Log(data[5]);

                callbackOnFinish(data);
                yield return null;
            }
        }
    }

    public static IEnumerator GetEirbeesHP(string wallet, System.Action<string[]> callbackOnFinish)

    {
        WWWForm form = new WWWForm();
        form.AddField("user_wallet", wallet);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3001/game/dataEirbeesHP",form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success){
                Debug.Log(www.error);
            }
            else
            {
                var res = www.downloadHandler.text;
                EirbeesDataHP eirbeesData = JsonUtility.FromJson<EirbeesDataHP>(res);

                string[] data= new string [6];
                data[0] = eirbeesData.hp0.ToString();
                data[1] = eirbeesData.hp1.ToString();
                data[2] = eirbeesData.hp2.ToString();
                data[3] = eirbeesData.hp3.ToString();
                data[4] = eirbeesData.hp4.ToString();
                data[5] = eirbeesData.hp5.ToString();
                Debug.Log("data 0: hp");
                Debug.Log(data[0]);
                Debug.Log("data 5: hp");
                Debug.Log(data[5]);

                callbackOnFinish(data);
                yield return null;
            }
        }
    }
    public static IEnumerator GetEirbeesLevel(string wallet, System.Action<string[]> callbackOnFinish)

    {
        WWWForm form = new WWWForm();
        form.AddField("user_wallet", wallet);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3001/game/dataEirbeesLevel",form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success){
                Debug.Log(www.error);
            }
            else
            {
                var res = www.downloadHandler.text;
                EirbeesDataLevel eirbeesData = JsonUtility.FromJson<EirbeesDataLevel>(res);

                string[] data= new string [6];
                data[0] = eirbeesData.lvl0.ToString();
                data[1] = eirbeesData.lvl1.ToString();
                data[2] = eirbeesData.lvl2.ToString();
                data[3] = eirbeesData.lvl3.ToString();
                data[4] = eirbeesData.lvl4.ToString();
                data[5] = eirbeesData.lvl5.ToString();
                Debug.Log("data 0: lvl");
                Debug.Log(data[0]);
                Debug.Log("data 5: lvl");
                Debug.Log(data[5]);

                callbackOnFinish(data);
                yield return null;
            }
        }
    }

     public static IEnumerator GetEirbeesImages(string wallet, System.Action<string[]> callbackOnFinish)

    {
        WWWForm form = new WWWForm();
        form.AddField("user_wallet", wallet);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3001/game/dataEirbeesImages",form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success){
                Debug.Log(www.error);
            }
            else
            {
                var res = www.downloadHandler.text;
                EirbeesDataImage eirbeesData = JsonUtility.FromJson<EirbeesDataImage>(res);

                string[] data= new string [6];
                data[0] = eirbeesData.img0.ToString();
                data[1] = eirbeesData.img1.ToString();
                data[2] = eirbeesData.img2.ToString();
                data[3] = eirbeesData.img3.ToString();
                data[4] = eirbeesData.img4.ToString();
                data[5] = eirbeesData.img5.ToString();
                Debug.Log("data 0: img");
                Debug.Log(data[0]);
                Debug.Log("data 5: img");
                Debug.Log(data[5]);

                callbackOnFinish(data);
                yield return null;
            }
        }
    }



}
