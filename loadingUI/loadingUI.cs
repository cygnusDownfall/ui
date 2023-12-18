using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class loadingUI : Singleton<loadingUI>
{
    byte loadingProcess=0;
    public Sprite[] sprites;
    [SerializeField] byte deltaChangeSprite=1;
    [SerializeField]bool isLoop;

    void SetLoadingProcess(byte process)
    {
        loadingProcess = process;
        GetComponent<Image>().sprite = sprites[process];
    }
    //public IEnumerator run(){
    //    while(loadingProcess<sprites.Length){
    //        yield return new WaitForSeconds(deltaChangeSprite);
    //        SetLoadingProcess((byte)(loadingProcess+1));
    //        if(isLoop&&(loadingProcess==sprites.Length-1)){
    //            loadingProcess=0;
    //        }
    //    }
    //}

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        Debug.Log("loading ...."+Time.time);
        gameObject.SetActive(true);
       // StartCoroutine(run());
    }

    public void Hide()
    {
        Debug.Log("unloading...."+Time.time);
        gameObject.SetActive(false);
       // StopCoroutine(run());
    }
}
