using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctionSystem : Singleton<UIFunctionSystem>
{
    public TMPro.TMP_Text worldID;
    public string joincode;
    [SerializeField] GameObject menuCam;
    [SerializeField] GameObject gameplayCanvas;
    private void Start()
    {
        gameplayCanvas.SetActive(false);
    }
    public async void EnterRoom()
    {
        joincode = GameObject.Find("JoinCodeInput").GetComponent<TMPro.TMP_InputField>().text;
        Debug.LogWarning("join code:" + joincode);
        try
        {
            if ((joincode == null) || (joincode == ""))
            {
                Debug.Log("Please Enter a Join Code");
                joincode = await GameObject.FindGameObjectWithTag("net").GetComponent<ConectController>().createRelay();
                Debug.LogWarning("create room:" + joincode);
            }
            else
            {
                await GameObject.FindGameObjectWithTag("net").GetComponent<ConectController>().joinRelay(joincode);

            }
            worldID.text = joincode;

            //loadingUI.Instance.Show();
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            while (!NetworkManager.Singleton.IsConnectedClient)//&& !NetworkManager.Singleton.IsHost)
            {
                Debug.Log("waiting for client to connect:  " + Time.time);
                await Task.Delay(1000);
            }
            PlayerController.Instance.loadPlayer();
            //join channel with joincode 
            _ = ChatSystem.Instance.JoinEchoChannelAsync();
            toogleGameMenu(false);
        }
        catch (UnityException e)
        {
            Debug.LogError(e);
        }
    }

    public void toogleGameMenu(bool isMenu)
    {
        menuCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = isMenu ? 100 : 0;
        menuCam.SetActive(isMenu);
        PlayerController.Instance.setPlayerControllable(!isMenu);
        gameplayCanvas.SetActive(!isMenu);
    }
    public void toogle(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);

    }

    public void rot(GameObject obj, Vector3 axis, float rot)
    {
        obj.transform.Rotate(axis, rot);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
