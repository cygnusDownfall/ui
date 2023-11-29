using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ConectController : Singleton<ConectController>
{
    [SerializeField] int maxPlayer = 4;
    override public async void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        await UnityServices.InitializeAsync();
        await ChatSystem.Instance.InitializeAsync();

    }
    async void Start()
    {
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

        //
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
            NetworkManager.Singleton.StartClient();
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


