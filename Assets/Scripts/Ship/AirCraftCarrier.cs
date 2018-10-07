using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 航母(继承自Ship)
/// </summary>
public class AirCraftCarrier : Ship{
    public List<AirCraft> airCrafts = new List<AirCraft>();
    public ShipAI shipAI;
    void Update(){
        for (int i = 0; i < fireInterval.Length; i++){
            fireInterval[i] += Time.deltaTime;
            if (fireInterval[i] > cannons[i].data.interval){
                fireInterval[i] -= cannons[i].data.interval;
                cannons[i].OnFire(transform,hitTarget);
            }
        }
    }

    public void Destroy(AirCraft toDestroy) {
        airCrafts.Remove(toDestroy);
        //toDestroy.gameObject.SetActive(false);//隐藏 或者 消失
        Destroy(toDestroy.gameObject);
    }

    public void ReleaseAirCraft() {
        foreach (AirCraft ac in airCrafts) { 
            //ac
        }
    }
}