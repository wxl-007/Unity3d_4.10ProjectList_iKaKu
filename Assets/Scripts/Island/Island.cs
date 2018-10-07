using UnityEngine;
using System.Collections;
/// <summary>
/// 岛
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Island : FightElement,ILife {
    public IslandCannon[] cannon;       //炮台
    public FightElement hitTarget;      //攻击目标

    public UISlider BloodShow;          //血量条
    public float Blood { set; get; }    //血量
    public ShipEffect particles;        //着火粒子效果发射管理器

    public bool isAlive { get { return Blood > 0; } }//是否被摧毁
    public GameObject[] BrokenState_Show;

    public void OnReceiveDamage(CannonBall cannonball){
        if (cannonball.data.belongTo == transform) {
            return;
        }
        //TODO
        BrokenState(0); //根据损坏程度,显示不同的毁坏状态
    }

    public void OnDie(){
        //TODO
        //播放摧毁动画
    }

    /// <summary>
    /// 自动根据血量冒烟,相应的烟火粒子/模型展示
    /// </summary>
    public void OnCatchingFire(Unit component,int count){
        component.OnCatchingFire(count);
    }

    public void BrokenState(int state) {
        if (state > BrokenState_Show.Length || state < 0) {
            Debug.LogError("损毁状态为各个状态索引,不可小于0,不可大于状态总数,当前State = " + state, gameObject);
            return;
        }
        for (int i = 0; i < BrokenState_Show.Length; i++) {
            if (state == i) {
                BrokenState_Show[i].SetActive(true);
            }else{
                BrokenState_Show[i].SetActive(false);
            }
        }
    }
}