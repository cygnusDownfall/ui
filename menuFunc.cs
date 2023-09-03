using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuFunc : Singleton< MonoBehaviour>
{
    string joincode;
    [SerializeField] GameObject menuCam;
    public async void EnterRoom(GameObject room)
    {
        joincode = GameObject.Find("JoinCodeInput").GetComponent<TMPro.TMP_InputField>().text;
        try
        {
            if ((joincode == null) || (joincode == ""))
            {
                Debug.Log("Please Enter a Join Code");
                joincode = await GameObject.FindGameObjectWithTag("net").GetComponent<ConectController>().createRelay();
                Debug.Log(joincode);
            }
            else
            {
                GameObject.FindGameObjectWithTag("net").GetComponent<ConectController>().joinRelay(joincode);

            }
            room.SetActive(true);
            room.transform.GetChild(2).GetComponent<TMP_Text>().text=joincode;
            
        }
        catch (UnityException e)
        {
            Debug.Log(e);
        }

    }

    public void toogleGameMenu(bool isMenu)
    {
        //SceneManager.LoadScene("gameScene");
        Camera.current.gameObject.transform.GetChild(0).gameObject.SetActive(isMenu);
        Camera.current.gameObject.transform.GetChild(1).gameObject.SetActive(!isMenu);
        menuCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority=isMenu?100:0;

    }


    public void Exit()
    {
        Application.Quit();
    }
}
