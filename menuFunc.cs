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
    public Canvas aimcanvas;
    private void Start()
    {
        gameplayCanvas.SetActive(false);
        loadingWait(1);
    }
    public async void loadingWait(int secconds)
    {
        loadingUI.Instance.Show();
        await Task.Delay(secconds * 1000);
        loadingUI.Instance.Hide();
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
                joincode = await ConectController.Instance.createRelay();
                Debug.LogWarning("create room:" + joincode);
            }
            else
            {
                await ConectController.Instance.GetComponent<ConectController>().joinRelay(joincode);

            }
            worldID.text = joincode;

            //loadingUI.Instance.Show();
            SceneEventProgressStatus status;
            //SceneManager.LoadScene(1, LoadSceneMode.Additive);
            if (NetworkManager.Singleton.IsServer)
            {
                status = NetworkManager.Singleton.SceneManager.LoadScene("base", LoadSceneMode.Additive);
            }
            NetworkManager.Singleton.SceneManager.OnSceneEvent += (ev) => Debug.Log("scene event: " + ev + "\ntype:" +
            ev.SceneEventType + "\nsceneName:" + ev.SceneName + "\nclient complete: " + ev.ClientsThatCompleted);

            for (int time = 0, maxtime = 10; !NetworkManager.Singleton.IsConnectedClient && !NetworkManager.Singleton.IsServer && time < maxtime; time++)
            {
                Debug.Log("waiting for client to connect:  " + Time.time);
                await Task.Delay(2000);
            }

            PlayerController.Instance.loadPlayer();
            //join channel with joincode 
            // _ = ChatSystem.Instance.JoinEchoChannelAsync();
            toogleGameMenu(false);
            aimcanvas.enabled = true;
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
        PlayerController.Instance.toogleEvent(!isMenu);
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
