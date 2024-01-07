using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ConectController : Singleton<ConectController>
{
    [SerializeField] int maxPlayer = 4;
    override public void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }
    async void Start()
    {
        await UnityServices.InitializeAsync();

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to sign in anonymously: " + task.Exception);
                }
                else
                {
                    Debug.Log("Signed in anonymously");
                }
            });
        }
        catch (AuthenticationException e)
        {
            Debug.Log("Authen error: " + e);
        }
      //  await ChatSystem.Instance.startSystem();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientDisconnected(ulong obj)
    {
        Debug.Log("client disconnected:" + obj);
    }

    private void OnClientConnect(ulong obj)
    {
        Debug.Log("client connected:" + obj);
    }

    public async Task<string> createRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayer);
            string joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData sd = new RelayServerData(allocation, "dtls");
            GetComponent<UnityTransport>().SetRelayServerData(sd);
            NetworkManager.Singleton.StartHost();
            return joincode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("relay error:" + e);
            return "";
        }

    }
    public async Task joinRelay(string joincode)
    {
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joincode);

            RelayServerData sd = new RelayServerData(allocation, "dtls");
            GetComponent<UnityTransport>().SetRelayServerData(sd);
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("relay error:" + e);
        }
    }

}


