using UnityEngine;
using UnityEngine.SceneManagement;

public class menuFunc : Singleton<menuFunc>
{
    public TMPro.TMP_Text worldID;
    public string joincode;
    [SerializeField] GameObject menuCam;
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
            toogleGameMenu(false);
            worldID.text = joincode;

            //loadingUI.Instance.Show();
            await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            PlayerController.Instance.loadPlayer();

            //join channel with joincode 
            await ChatSystem.Instance.JoinEchoChannelAsync();

            menuCam.SetActive(false);
        }
        catch (UnityException e)
        {
            Debug.LogError(e);
        }
    }

    public void toogleGameMenu(bool isMenu)
    {
        menuCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = isMenu ? 100 : 0;

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
