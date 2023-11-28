using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gacchaCardUI : MonoBehaviour
{
    gacchaSystem<cardModel> gacchaSys;
    public List<cardPack> packList; //sau nay co the phat trien lay tu api 
    public byte indexOfCurrentPack;
    bool toogleState = false;
    [Header("-------------------Ref UI Result-------------------")]
    [SerializeField] private GameObject cardUIResultTemplate;
    [SerializeField] private GameObject gacchaResultPanel;
    [SerializeField] private Transform gacchaResultContentZone;
    [SerializeField] private GameObject packUITemplate;
    [Header("------------Ref UI Gaccha Card-----------------")]
    [SerializeField] private GameObject gacchaPanel;
    [SerializeField] private GameObject leftsidePackPanel;
    [SerializeField] private GameObject rightsideContentPanel;
    [SerializeField] private GameObject buttonGacchaX1, buttonGacchaXAll;

    [Header("-----------------Ref Info Pack-------------------")]
    [SerializeField] private TMP_Text namePackUI, timeRemainUI, noteUI;
    [SerializeField] private Image illustrationPackUI;
    [Header("---------------Infor---------------")]
    public int cardPoint;
    public byte prizeForGaccha;


    public void toogleUI()
    {
        toogleState = !toogleState;
        if (toogleState)
        {
            loadWindowGacchaCard();
            gacchaPanel.SetActive(true);
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
    }
    void loadPackList()
    {
        var leftTrans = leftsidePackPanel.transform;
        if (leftTrans.childCount > 0 && leftTrans.childCount != packList.Count)
        {
            for (int i = 0; i < leftTrans.childCount; i++)
            {
                Destroy(leftTrans.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < packList.Count; i++)
        {
            var packUI = Instantiate(packUITemplate, leftsidePackPanel.transform);
            packUI.GetComponentInChildren<TMP_Text>().text = packList[i].namePack;
            //packUI.GetComponent<Image>().sprite = packList[i].borderImage;
            packUI.GetComponent<Button>().onClick.AddListener(() => SelectPack((byte)i));
        }
    }
    /// <summary>
    /// set a button with the value of max value posible to gaccha 
    /// </summary>
    void loadMaxGacchaNumber()
    {
        cardPoint = PlayerController.Instance.playerInfo.cardPoint;
        int res = cardPoint / prizeForGaccha;

        buttonGacchaXAll.GetComponent<Button>().onClick.AddListener(() => clickToGaccha((byte)res));
    }
    void loadInfoPack()
    {
        namePackUI.text = packList[indexOfCurrentPack].namePack;
        timeRemainUI.text = packList[indexOfCurrentPack].timeRemain;
        noteUI.text = packList[indexOfCurrentPack].notes;
        illustrationPackUI.sprite = packList[indexOfCurrentPack].illustration;
    }
    #endregion
    public void SelectPack(byte index)
    {
        if (indexOfCurrentPack == index) return;
        loadInfoPack();
        loadMaxGacchaNumber();
        indexOfCurrentPack = index;
    }
    public void clickToGaccha(byte num)
    {
        List<cardModel> res = new List<cardModel>();
        gacchaSys ??= new gacchaSystem<cardModel>();
        Debug.Log("danh sach bai da nhan:");
        for (int i = 0; i < num; i++)
        {
            //gaccha ra cardModel
            packList[indexOfCurrentPack].copyTo(gacchaSys);
            var resGaccha = gacchaSys.gaccha();
            res.Add(resGaccha);
            Debug.Log(resGaccha);
        }

        //xu ly UI phan thuong nhan duoc 
        StartCoroutine(showResultGaccha(res));
    }
    IEnumerator showResultGaccha(List<cardModel> res)
    {
        gacchaResultPanel.SetActive(true);
        for (int i = 0; i < res.Count; i++)
        {
            var cardUI = gacchaResultContentZone.GetChild(i).gameObject ?? Instantiate(cardUIResultTemplate, gacchaResultContentZone);
            if (gacchaResultContentZone.GetChild(i).gameObject != null) cardUI.SetActive(true);
            cardUI.GetComponent<Image>().sprite = res[i].icon;
            yield return new WaitForSeconds(0.5f);
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
    private void Start()
    {
        //loadWindowGacchaCard();

    }
    #endregion
}