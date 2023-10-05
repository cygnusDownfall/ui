using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ConectController : MonoBehaviour
{
    [SerializeField] int maxPlayer = 4;
    async void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        await UnityServices.InitializeAsync();

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

        }catch(AuthenticationException e){
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
            playerLoaded();
            return joincode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("relay error:" + e);
            return "";
        }

    }
    public async void joinRelay(string joincode)
    {
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joincode);

            RelayServerData sd = new RelayServerData(allocation, "dtls");
            GetComponent<UnityTransport>().SetRelayServerData(sd);
            NetworkManager.Singleton.StartClient();
            playerLoaded();

        }
        catch (RelayServiceException e)
        {
            Debug.LogError("relay error:" + e);
        }
    }
    public void playerLoaded(){
        playerController.Instance.loadPlayer();
    }
    
}
