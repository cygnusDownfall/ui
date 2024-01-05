using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class loadingUI : Singleton<loadingUI>
{
    public Sprite[] sprites;
    [SerializeField] byte deltaChangeSprite = 1;
    [SerializeField] bool isLoop;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        Debug.Log("loading ...." + Time.time);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("unloading...." + Time.time);
        gameObject.SetActive(false);
    }
}
