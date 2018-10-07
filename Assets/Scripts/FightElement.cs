//#define EDIT
using UnityEngine;
/// <summary>
/// 战斗元素(航母,战舰,飞机等父类)
/// 包含方法:
///     OnCatchingFire();//着火特效展示
/// </summary>
public class FightElement : BaseElement {
    //public Vector3[] hitPos;            //可攻击点
    public Vector3 externts;            //碰撞盒大小
    public FightElementType fightElementType;    //元素类型
}