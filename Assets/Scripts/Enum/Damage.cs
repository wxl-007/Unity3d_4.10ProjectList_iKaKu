using UnityEngine;

//public struct Damage {
//    /// <summary>
//    /// 所受伤害位置
//    /// </summary>
//    public Vector3 hitPos;
//    /// <summary>
//    /// 所受伤害撞击方向
//    /// </summary>
//    public Vector3 hitDir;
//    /// <summary>
//    /// 伤害强度
//    /// </summary>
//    public float damage;
//    /// <summary>
//    /// 伤害类型
//    /// </summary>
//    public DamageType damageType;
//    /// <summary>
//    /// 伤害所属
//    /// </summary>
//    public FightElement damageBelong;

//    /// <summary>
//    /// 伤害结构构造函数
//    /// </summary>
//    /// <param name="hitPosition">伤害位置</param>
//    /// <param name="hitDirection">伤害物方向</param>
//    /// <param name="hitStrength">伤害强度</param>
//    /// <param name="damageBelong">伤害所属</param>
//    /// <param name="damageType">伤害类型</param>
//    public Damage(Vector3 hitPosition,Vector3 hitDirection,float hitStrength,FightElement damageBelong,DamageType damageType = DamageType.normal) {
//        hitPos = hitPosition;
//        hitDir = hitDirection;
//        damage = hitStrength;
//        this.damageBelong = damageBelong;
//        this.damageType = damageType;
//    }
//}

/// <summary>
/// 元素类型:无,战舰,岛屿,飞机,火炮,炮弹
/// </summary>
public enum FightElementType { 
    None        = -1,
    Ship        = 0,
    AirCraft    = 1,
    Island      = 2
}

///// <summary>
///// 伤害类型(普通,火焰弹攻击...etc)
///// </summary>
//public enum DamageType{
//    normal  = 0,     //普通攻击
//    fire    = 1,     //火焰弹
//    torpedo = 2,     //鱼雷
//    cannon  = 3,     //加农炮
//    airartillery = 4,//防空炮
//}

/// <summary>
/// 战船,基地,子弹,飞机 的 所属阵营
/// 判断是否为敌对关系,或者同盟关系
/// </summary>
public enum Team {
    Self     = 0,   //己方阵营
    Enemy    = 1,   //敌方阵营
    Alliance = 2    //同盟阵营
}

/// <summary>
/// 基地类型(可开矿,可产能源...etc)
/// </summary>
public enum IslandType{ 
    //矿
    //能源
    //
    //...
}

/// <summary>
/// 子弹类型,炮弹类型
/// </summary>
public enum BulletType { 
    normal  = 0,
    fire    = 1,
    torpedo = 2,
    cannon  = 3,
    airartillery =4
}

/// <summary>
/// 爆炸类型(子弹,炮弹,战船,飞机?)
/// </summary>
public enum ExploseType { 
    normal  = 0,    //炮弹爆炸
    fire    = 1,    //燃烧弹爆炸
    water   = 2,    //击中水面
    ship    = 3,    //战船摧毁
    aircraft= 4,    //飞机摧毁
    island  = 5,    //矿岛爆炸
    Count   = 6     //掌管爆炸类型数量(统计功能)
}

/// <summary>
/// 战舰移动状态
/// </summary>
public enum ShipMoveState{
    MoveToTarget,       //向目标移动
    MoveToPosition,     //向点移动
    MoveByMainPath,     //沿主路径移动
    MoveByAssistantPath,//沿副路线移动
    MoveFree,           //自由形态
    Stop                //停止移动
}

/// <summary>
/// 火炮类型
/// </summary>
public enum CannonType { 
    /// <summary>
    /// 高射炮 Flac
    /// </summary>
    高射炮                = 0,//高射炮
    /// <summary>
    /// 主炮 Armament
    /// </summary>
    主炮            = 1,//主炮
    /// <summary>
    /// 组合炮 Dual_purposeCannon
    /// </summary>
    组合炮  = 2,//组合炮,
    /// <summary>
    /// 副炮 Turret
    /// </summary>
    副炮              = 3,//副炮
}