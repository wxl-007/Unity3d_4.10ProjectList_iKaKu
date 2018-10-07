using UnityEngine;
using System.Collections;
/// <summary>
/// 矿
/// </summary>
public class OreIsland:Island{
    float[] fireInterval;

    void Start() {
        fireInterval = new float[cannon.Length];
    }

    Collider[] nearShips;
    void Update() {
        //nearShips = Physics.OverlapSphere(transform.position,)
        for (int i = 0; i < fireInterval.Length; i++){
            fireInterval[i] += Time.deltaTime;
            if (fireInterval[i] > cannon[i].info.interval){
                fireInterval[i] -= cannon[i].info.interval;
                cannon[i].OnFire(hitTarget);
            }
        }
    }
}