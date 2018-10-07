using UnityEngine;
using System.Collections;

public class TestAir : MonoBehaviour {
    public MovePath path;
    AirCraftType curType;
    /*״̬*/              /*ʱ��*/
    //����                2s
    //����,̧ͷ           1s
    //����                
    //����Ԥ���߶�         
    //ƽ��                 
    //ת��                 
    //��ը��               
    //����                 
    //����                 

    public float tarHeght = 100;
    public float speed = 300;
    public Vector3 curSpeed;
    public Vector3 hitPos;
    public float accelerate = 9.8f;
                           
                           
    //����Ŀ�����         
    //����Ŀ��߶�         
    //��ǰ�����ٶ�         
    //Я����ҩ��           
    //
    void Update(){
        switch (curType) { 
            case AirCraftType.����:
                speed += accelerate * Time.deltaTime;
                curSpeed = transform.forward * speed;
                transform.forward = curSpeed;
                transform.position += curSpeed * Time.deltaTime;                
                break;
            case AirCraftType.����:
                break;
            case AirCraftType.ƽ��:
                break;
            case AirCraftType.��ը:
                break;
            case AirCraftType.��ը����:
                break;
            case AirCraftType.����:
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
    ����,
    ����,
    ƽ��,
    ��ը,
    ��ը����,
    ����
}