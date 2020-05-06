using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public UINumController bulletNum;
    public UINumController energyNum;


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

    // Update is called once per frame
    void Update()
    {
        
    }
}
