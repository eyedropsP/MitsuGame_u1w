using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public UINumController bulletNum;
    public UINumController energyNum;

    public InputField field;

    public GameObject debugParts;


    public void AddBullet(int num)
    {
        bulletNum.AddNum(num);
    }

    public void SubBullet(int num)
    {
        bulletNum.SubNum(num);
    }

    
    public void AddEnergy(int num)
    {
        energyNum.AddNum(num);
    }

    public void SubEnergy(int num)
    {
        energyNum.SubNum(num);
    }

    public void SetBullet() {
        if (field != null)
        {
            var temp = int.Parse (field.text);
            bulletNum.SetNum(temp);
        }
    }

    public void SetEnergy() {
        if (field != null)
        {
            var temp = int.Parse (field.text);
            energyNum.SetNum(temp);
        }
    }

    public void ToggleDebugUI() {
        if (debugParts != null) {
            if (GetComponent<Toggle>() != null) {
                debugParts.SetActive(GetComponent<Toggle>().isOn);
            }
        }
    }

}
