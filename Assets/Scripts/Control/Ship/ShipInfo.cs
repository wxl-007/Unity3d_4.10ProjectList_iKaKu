using UnityEngine;
using System.Collections;

public class ShipInfo{
	/// <summary>
	/// 序号.
	/// </summary>
	public int id;
	/// <summary>
	/// 名字.
	/// </summary>
	public string name;
	/// <summary>
	/// 图标.
	/// </summary>
	public string icon;
	/// <summary>
	/// 详细描述.
	/// </summary>
	public string describe;
    /// <summary>
    /// 速度(米/秒).
    /// </summary>
    public float moveSpeed;
	/// <summary>
	/// 主炮ID.
	/// </summary>
    public int mainWeaponId;
	/// <summary>
	/// 副炮ID.
	/// </summary>
	public int auxiliaryWeaponId;
	/// <summary>
	/// 主炮.
	/// </summary>
	public WeaponInfo mainWeapon{
		get{return WeaponPool.GetInfo(mainWeaponId);}
	}
	/// <summary>
	/// 副炮.
	/// </summary>
	public WeaponInfo auxiliaryWeapon{
		get{return WeaponPool.GetInfo(mainWeaponId);}
	}
	/// <summary>
	/// 级别.
	/// </summary>
    public int level;
	/// <summary>
	/// 射程(米).
	/// </summary>
    public float range;
	/// <summary>
	/// 类型.
	/// </summary>
    public ShipType type;
    /// <summary>
    /// 血量上限.
    /// </summary>
    public float healthVolume;
    /// <summary>
    /// 防御.
    /// </summary>
    public float defense;
//    public ShipInfo(float moveSpeed,Team team,int level,float range,ShipType type,float health,float blood,float defense) {
//        this.moveSpeed = moveSpeed;
//        //this.team = team;
//        this.level = level;
//        this.type = type;
//        this.health = health;
//        this.blood = blood;
//        this.defense = defense;
//    }

    public static implicit operator bool(ShipInfo data) {
        return data != null;
    }
}