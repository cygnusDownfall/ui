using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(NetworkObject))]
public class otherPlayerInfo : SingletonNetwork<otherPlayerInfo>
{
    public GameObject infoPrefab;
    public Dictionary<ulong, Slider> datas = new Dictionary<ulong, Slider>();

    [ServerRpc(RequireOwnership = false)]
    public void SpawnInfoServerRpc(NetworkBehaviourReference infoRef, ulong clientID)
    {
        infoRef.TryGet(out playerInfo info);
        var infoObj = itemPooling.Instance.TakeOut("otherinfo") ?? Instantiate(infoPrefab, transform);
        var infoNet = infoObj.GetComponent<NetworkObject>();
        infoNet.SpawnWithOwnership(clientID);
        infoNet.TrySetParent(transform);

        //set 
        infoObj.GetComponentInChildren<TMPro.TMP_Text>().text = "Player " + clientID;
        var hpUI = infoObj.GetComponentInChildren<Slider>();
        hpUI.value = 1;
        datas.Add(clientID, hpUI);

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
