using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gacchaCardUI : Singleton<gacchaCardUI>
{
    gacchaSystem<cardModel> gacchaSys;
    public List<cardPack> packList; //sau nay co the phat trien lay tu api 
    public byte indexOfCurrentPack = 255;
    bool toogleState = false;
    [Header("-------------------Ref UI Result-------------------")]
    [SerializeField] private GameObject cardUIResultTemplate;
    [SerializeField] private GameObject gacchaResultPanel;
    [SerializeField] private Transform gacchaResultContentZone;
    [SerializeField] private GameObject packUITemplate;
    [Header("------------Ref UI Gaccha Card-----------------")]
    [SerializeField] private GameObject gacchaPanel;
    public bool gacchaPanelActive { get => gacchaPanel.activeSelf; }
    [SerializeField] private GameObject leftsidePackPanel;
    [SerializeField] private GameObject rightsideContentPanel;
    [SerializeField] private GameObject buttonGacchaX1, buttonGacchaXAll;

    [Header("-----------------Ref Info Pack-------------------")]
    [SerializeField] private TMPro.TMP_Text namePackUI, timeRemainUI, noteUI;
    [SerializeField] private Image illustrationPackUI;
    [Header("---------------Infor---------------")]

    public byte prizeForGaccha;

    /// <summary>
    /// toogle gacchaPanel and load data 
    /// </summary> 
    public void toogleUI()
    {
        toogleState = !toogleState;
        Debug.Log(toogleState);
        if (toogleState)
        {
            gacchaPanel.SetActive(true);
            loadWindowGacchaCard();
        }
        else
        {
            gacchaPanel.SetActive(false);
        }
    }
    public void toogleUI(bool state)
    {
        toogleState = state;
        Debug.Log(toogleState);
        if (toogleState)
        {
            gacchaPanel.SetActive(true);
            loadWindowGacchaCard();
        }
        else
        {
            gacchaPanel.SetActive(false);
        }
    }
    #region loading UI
    public void loadWindowGacchaCard()
    {
        loadPackList();
        SelectPack(0);
        loadMaxGacchaNumber();

    }
    void loadPackList()
    {
        var leftTrans = leftsidePackPanel.transform;
        GameObject packUI;
        for (int i = 0; i < packList.Count; i++)
        {
            Debug.Log(i);
            packUI = (i < leftTrans.childCount) ? leftTrans.GetChild(i).gameObject : Instantiate(packUITemplate, leftsidePackPanel.transform);
            packUI.GetComponentInChildren<TMPro.TMP_Text>().text = packList[i].namePack;
            //packUI.GetComponent<Image>().sprite = packList[i].borderImage;
            int indexEventCall = i;
            packUI.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectPack(indexEventCall);
            });
        }
    }
    /// <summary>
    /// set a button with the value of max value posible to gaccha 
    /// </summary>
    void loadMaxGacchaNumber()
    {
        var point = playerGeneralInfo.Instance.cardPoint;
        int res = point / prizeForGaccha;
        byte tmp = (byte)res;
        Debug.Log(tmp);
        buttonGacchaXAll.GetComponent<Button>().onClick.AddListener(() => clickToGaccha(tmp));
    }
    void loadInfoPack()
    {
        Debug.Log("load info pack");
        namePackUI.text = packList[indexOfCurrentPack].namePack;
        timeRemainUI.text = packList[indexOfCurrentPack].timeRemain;
        noteUI.text = packList[indexOfCurrentPack].notes;
        illustrationPackUI.sprite = packList[indexOfCurrentPack].illustration;
    }
    #endregion
    public void SelectPack(int index)
    {
        if (indexOfCurrentPack == index) return;
        Debug.Log("select pack:" + index);

        indexOfCurrentPack = (byte)index;
        Debug.Log("byte index:" + indexOfCurrentPack);
        loadInfoPack();
    }
    public void clickToGaccha(int num)
    {
        if (num == 0) return;
        Debug.Log(num * prizeForGaccha);
        if (playerGeneralInfo.Instance.cardPoint < num * prizeForGaccha) return;
        List<cardModel> res = new List<cardModel>();
        gacchaSys ??= new gacchaSystem<cardModel>();
        Debug.Log("click gaccha pack index:" + indexOfCurrentPack);
        packList[indexOfCurrentPack].copyTo(gacchaSys);
        Debug.Log("danh sach bai da nhan:");
        for (int i = 0; i < num; i++)
        {
            //gaccha ra cardModel
            var resGaccha = gacchaSys.gaccha();
            res.Add(resGaccha);
            Debug.Log(resGaccha);
        }
        deckCard.Instance.addCard(res);
        //xu ly UI phan thuong nhan duoc 
        StartCoroutine(showResultGaccha(res));
        loadMaxGacchaNumber();

    }
    IEnumerator showResultGaccha(List<cardModel> res)
    {
        gacchaResultPanel.SetActive(true);
        Debug.Log("number of Result gaccha:" + res.Count);
        for (int i = 0; i < res.Count; i++)
        {
            var cardUI = gacchaResultContentZone.GetChild(i).gameObject ?? Instantiate(cardUIResultTemplate, gacchaResultContentZone);
            Debug.Log(cardUI);
            if (gacchaResultContentZone.GetChild(i).gameObject != null) cardUI.SetActive(true);
            cardUI.GetComponent<Image>().sprite = res[i].icon;
            yield return new WaitForSeconds(1f);
        }
    }
    public void clickToAcceptResult()
    {
        for (int i = 0; i < gacchaResultContentZone.childCount; i++)
        {
            gacchaResultContentZone.GetChild(i).gameObject.SetActive(false);
        }
        gacchaResultPanel.SetActive(false);
    }
    #region mono
    private void OnEnable()
    {
        //loadWindowGacchaCard();
    }
    private void Start()
    {

    }
    #endregion
}