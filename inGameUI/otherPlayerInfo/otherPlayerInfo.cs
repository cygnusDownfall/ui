using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
[RequireComponent(typeof(NetworkObject))]
public class otherPlayerInfo : SingletonNetwork<otherPlayerInfo>
{
    public GameObject infoPrefab;
    public List<playerInfo> datas;

    [ClientRpc]
    public void SpawnInfoClientRpc(Vector3 pos, string name, NetworkObjectReference playerObj)
    {
        var go = Instantiate(infoPrefab, pos, Quaternion.identity);

        playerObj.TryGet(out NetworkObject net);
        //playerInfo info=net.gameObject.GetComponent<ControllReceivingSystem>().curCharacterControl.gameObject
        datas.Add(null);
    }

}
