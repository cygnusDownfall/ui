using System.Threading.Tasks;
using UnityEngine.UI;

public class hpBar : Singleton<hpBar>
{
    /// <summary>
    /// value from 0 to 1
    /// </summary>
    public float Value
    {
        get => GetComponent<Image>().material.GetFloat("_value");
        set
        {
            if (value > 1) Value = 1;
            if (value < 0) Value = 0;
            GetComponent<Image>().material.SetFloat("_value", value);
        }
    }

    public void load()
    {
        var info =PlayerController.Instance.playerInfo;

        Value = info.hp / info.maxHP;

    }
}
