using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(NetworkObject))]
public class otherPlayerInfo : SingletonNetwork<otherPlayerInfo>
{
    public GameObject infoPrefab;
    public List<playerInfo> datas = new List<playerInfo>();

    [ClientRpc]
    public void SpawnInfoClientRpc(Vector3 pos, string name, NetworkObjectReference playerObj)
    {
        var go = Instantiate(infoPrefab, pos, Quaternion.identity);

        playerObj.TryGet(out NetworkObject net);
        playerInfo info = net.gameObject.GetComponent<ControllReceivingSystem>().curCharacterControl.gameObject.GetComponent<playerInfo>();
        info.hp.OnValueChanged += (o, n) =>
        {
            go.GetComponentInChildren<Slider>().value = n / (float)info.maxHP;
        };
        datas.Add(info);
    }

}
