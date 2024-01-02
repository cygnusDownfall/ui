using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
public class otherPlayerInfo : SingletonNetwork<otherPlayerInfo>
{
    public GameObject infoPrefab;
    public Dictionary<ulong, Slider> datas = new Dictionary<ulong, Slider>();

    public void SpawnInfo(playerInfo info, ulong clientID)
    {
        var infoObj = itemPooling.Instance.TakeOut("otherinfo") ?? Instantiate(infoPrefab, transform);
        infoObj.GetComponentInChildren<TMPro.TMP_Text>().text = "Player " + clientID;
        datas.Add(clientID, infoObj.GetComponentInChildren<Slider>());
        info.hp.OnValueChanged += (v1, v2) =>
        {
            updateValueInfo(v2 / (float)info.maxHP, clientID);
        };
    }
    public void despawnInfo(ulong clientID)
    {

        GameObject infoObj = datas[clientID].gameObject.transform.parent.gameObject;
        itemPooling.Instance.PushIn("otherinfo", infoObj);

    }

    private void updateValueInfo(float value, ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("client call itself rpc");
            return;
        }
        datas[clientID].value = value;
    }


}
