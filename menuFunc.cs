using UnityEngine;

public class menuFunc : Singleton< MonoBehaviour>
{
    [SerializeField] TMPro.TMP_Text worldID;
    string joincode;
    [SerializeField] GameObject menuCam;
    public async void EnterRoom()
    {
        joincode = GameObject.Find("JoinCodeInput").GetComponent<TMPro.TMP_InputField>().text;
        Debug.LogWarning("join code:"+joincode);
        try
        {
            if ((joincode == null) || (joincode == ""))
            {
                Debug.Log("Please Enter a Join Code");
                joincode = await GameObject.FindGameObjectWithTag("net").GetComponent<ConectController>().createRelay();
                Debug.LogWarning("create room:"+joincode);
            }
            else
            {
                GameObject.FindGameObjectWithTag("net").GetComponent<ConectController>().joinRelay(joincode);

            }
            toogleGameMenu(false);
            worldID.text = joincode;

            loadingUI.Instance.Show();
        }
        catch (UnityException e)
        {
            Debug.LogError(e);
        }
    }

    public void toogleGameMenu(bool isMenu)
    {
        menuCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority=isMenu?100:0;

    }

    public void Exit()
    {
        Application.Quit();
    }
}
