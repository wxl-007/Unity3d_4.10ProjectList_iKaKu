using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 战舰控制类
/// </summary>
public class ShipController : MonoBehaviour {
    public static List<Ship> MyShip = new List<Ship>();
    public static List<Ship> MyShipDead = new List<Ship>();
    public static List<Ship> EnemyShip = new List<Ship>();
    public static List<Ship> EnemyShipDead = new List<Ship>();

    public static Ship CommaderShip;    //主舰

    //添加战舰(实例化)
    public void AddShip(GameObject shipToInit,Vector3 Position,Quaternion rotation) {
        Ship ship = (Instantiate(shipToInit, Position, rotation) as GameObject).GetComponent<Ship>();
        MyShip.Add(ship);
    }

    //改造战舰
    public static void UpgradeShip(Ship target,ShipForPlayer upgradeData) {
        target.shipData = upgradeData;
        target.OnUpgrade(upgradeData);
    }

    //销毁己方战舰
    public static void DestroyMyShip(Ship target) {
        MyShip.Remove(target);
        target.gameObject.SetActive(false);
        MyShipDead.Add(target);
        //TODO
        //判断是否全部都被销毁(胜利失败判断)
    }

    //销毁敌人战舰
    public static void DestroyEnemyShip(Ship target) {
        EnemyShip.Remove(target);
        target.gameObject.SetActive(false);
        EnemyShipDead.Add(target);
        //TODO
        //判断是否销毁全部敌舰(胜利失败判断)
    }

    //出售战舰
    public static void SellShip(Ship target) {
        MyShip.Remove(target);
    }

    //获取战舰数据
    public static ShipForPlayer GetShipData(int index){
        return MyShip[index].shipData;
    }

    //获取所有战舰速度(木桶原理)
    public float GetMoveSpeed() {
        float speed = 0;
        return speed;
    }

    void OnGUI() {
        for (int i = 0; i < MyShip.Count; i++) {
            if (GUILayout.Button(MyShip[i].name)) {
                CameraControl.CameraMoveTo(MyShip[i].transform);
                Event.current.Use();
            }
        }
        GUILayout.BeginArea(new Rect(0, Screen.height*.6f, Screen.width*.3f, Screen.height * .5f), "敌船");
        for (int i = 0; i < EnemyShip.Count; i++) {
            if (GUILayout.Button(EnemyShip[i].name)) {
                CameraControl.CameraMoveTo(EnemyShip[i].transform);
                Event.current.Use();
            }
        }
        GUILayout.EndArea();
    }

//    //获取战舰数据
//    public static ShipInfo GetShipData(string shipName) {
//        //根据战舰名称获取相应数据
//        ShipInfo data = new ShipInfo(8, Team.Allies, 1, 20, ShipType.aircraftCarrier, 120, 100, 1);
//        return data;
//    }
}