using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(NetworkObject))]
public class otherPlayerInfo : SingletonNetwork<otherPlayerInfo>
{
    public GameObject infoPrefab;
    public Dictionary<ulong, Slider> datas = new Dictionary<ulong, Slider>();

    [ClientRpc]
    public void SpawnInfoClientRpc(Vector3 pos, NetworkObjectReference playerObj, ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("client call itself rpc");
            return;
        }

        playerObj.TryGet(out NetworkObject net);
        GameObject Obj = net.gameObject;
        var info = Obj.GetComponent<playerInfo>();

        var infoObj = itemDropPooling.Instance.TakeOut("otherinfo") ?? Instantiate(infoPrefab, transform);
        infoObj.GetComponentInChildren<Text>().text = "Player " + clientID;
        datas.Add(clientID, infoObj.GetComponentInChildren<Slider>());
        info.hp.OnValueChanged += (v1, v2) =>
        {
            updateValueInfo(v2 / (float)info.maxHP, clientID);
        };
    }
    [ClientRpc]
    public void despawnClientRpc(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("client call itself rpc");
            return;
        }
        GameObject infoObj = datas[clientID].gameObject.transform.parent.gameObject;
        itemDropPooling.Instance.PushIn("otherinfo", infoObj);

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
