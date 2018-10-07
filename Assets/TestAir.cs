using UnityEngine;
using System.Collections;

public class TestAir : MonoBehaviour {
    public MovePath path;
    AirCraftType curType;
    /*状态*/              /*时间*/
    //加速                2s
    //拉杆,抬头           1s
    //上升                
    //到达预定高度         
    //平稳                 
    //转向                 
    //丢炸弹               
    //返回                 
    //降落                 

    public float tarHeght = 100;
    public float speed = 300;
    public Vector3 curSpeed;
    public Vector3 hitPos;
    public float accelerate = 9.8f;
                           
                           
    //距离目标距离         
    //距离目标高度         
    //当前飞行速度         
    //携带弹药量           
    //
    void Update(){
        switch (curType) { 
            case AirCraftType.加速:
                speed += accelerate * Time.deltaTime;
                curSpeed = transform.forward * speed;
                transform.forward = curSpeed;
                transform.position += curSpeed * Time.deltaTime;                
                break;
            case AirCraftType.上升:
                break;
            case AirCraftType.平稳:
                break;
            case AirCraftType.轰炸:
                break;
            case AirCraftType.轰炸结束:
                break;
            case AirCraftType.返回:
                break;
        }
    }

    void SpeedUp() { 
        //TODO
    }

    void SpeedDown() { 
        //TODO
    }
    void AirAttack() { 
        //TODO
    }

    public FightElement belongTo;
    public FightElement hitTarget;
    public void SetTarget(FightElement belongTo,FightElement target) {
        hitTarget = target;
        this.belongTo = belongTo;
        path.Insert(0, belongTo.transform.position);
        path.Insert(1, belongTo.transform.position + belongTo.transform.forward * 100);
        path.Insert(2, target.transform.position);
    }
}
public enum AirCraftType { 
    加速,
    上升,
    平稳,
    轰炸,
    轰炸结束,
    返回
}