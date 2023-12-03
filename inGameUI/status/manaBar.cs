using UnityEngine;
using UnityEngine.UI;


public class manaBar : Singleton<manaBar>
{
    public byte maxMana;
    public byte currentMana { get; private set; }
    [SerializeField] GameObject manaToken;
    [SerializeField] byte lowThreshold;
    [SerializeField] byte mediumThreshold;
    [SerializeField] byte highThreshold;
    [SerializeField] Material tokenMatLow;
    [SerializeField] Material tokenMatMedium;
    [SerializeField] Material tokenMatHigh;
    [SerializeField] Material tokenMatOff;
    [SerializeField] GameObject[] tokens;

    void Start()
    {

    }
    void getDataFromPlayer()
    {
        var info = PlayerController.Instance.playerInfo;
        maxMana = info.maxMP;
        currentMana = info.mp;
    }
    public void load()
    {
        getDataFromPlayer();
        tokens = new GameObject[maxMana];
        for (int i = 0; i < maxMana; i++)
        {
            GameObject token = Instantiate(manaToken, transform);
            tokens[i] = token;
            if (i > currentMana - 1) { token.GetComponent<Image>().material = tokenMatOff; continue; }
            setMat((byte)i, token);
            //continue
        }
    }
    /// <summary>
    /// set mat for active mana token
    /// </summary>
    /// <param name="i"></param>
    /// <param name="token"></param> 
    void setMat(byte i, GameObject token)
    {
        if (i < lowThreshold) token.GetComponent<Image>().material = tokenMatLow;
        else if (i < mediumThreshold) token.GetComponent<Image>().material = tokenMatMedium;
        else token.GetComponent<Image>().material = tokenMatHigh;
    }
    /// <summary>
    /// nhan duoc mana
    /// </summary>
    /// <param name="amount">the amount of mana increase</param> 
    public void increaseMana(byte amount)
    {
        currentMana += amount;
        if (currentMana >= maxMana)
        { currentMana = maxMana; }
        for (byte i = (byte)(currentMana - amount); i < currentMana; i++)
        {
            Debug.Log(i);
            setMat(i, tokens[i]);
        }
    }
    /// <summary>
    /// tieu hao mana 
    /// </summary>
    /// <param name="amount">the amount of mana to decrease</param>
    public void decreaseMana(byte amount)
    {
        currentMana -= amount;
        if (currentMana > maxMana)
        { currentMana = 0; }
        for (int i = currentMana + amount - 1; i >= currentMana; i--)
        {
            Debug.Log(i);
            Debug.Log(currentMana);
            tokens[i].GetComponent<Image>().material = tokenMatOff;
        }
    }
}
