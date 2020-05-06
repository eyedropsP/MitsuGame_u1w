using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINumController : MonoBehaviour
{
    // Start is called before the first frame update

    private int currentNum;

    [SerializeField]
    Sprite[] numbers = new Sprite[10];

    [SerializeField]
    GameObject tensNumObj;

    [SerializeField]
    GameObject onesNumObj;

    Image tensImg, onesImg;


    void Start()
    {
        tensImg = tensNumObj.GetComponent<Image>();
        onesImg = onesNumObj.GetComponent<Image>();
    }

    public void AddNum(int num)
    {
        if (currentNum + num <= 99)
        {
            currentNum += num;
        }
        else
        {
            currentNum = 99;
        }
        ApplyNum();
    }

    public void SubNum(int num)
    {
        if (currentNum - num >= 0) {
            currentNum -= num;
        }
        else 
        {
            currentNum = 0;
        }
        ApplyNum();
    }

    public void SetNum(int num)
    {
        if (num <= 99)
        {
            currentNum = num;
        }
        ApplyNum();
    }


    void ApplyNum() {
        int tens, ones;
        tens = (int)Mathf.Floor(currentNum / 10);
        ones = (int)(currentNum % 10);
        tensImg.sprite = numbers[tens];
        onesImg.sprite = numbers[ones];
    }
}
