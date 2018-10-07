using System;

public class ShipForPlayer
{
	public int id;
	public ShipInfo shipInfo{
		get{return ShipPool.GetInfo(id);}
	}
	/// <summary>
	/// 血量.
	/// </summary>
	public float health;
	/// <summary>
	/// 血量上限.
	/// </summary>
	public float healthVolume{
		get{return shipInfo.healthVolume;}
	}
    /// <summary>
    /// 移动速度
    /// </summary>
	public float moveSpeed{
		get{return shipInfo.moveSpeed;}
	}
    /// <summary>
    /// 检测半径
    /// </summary>
	public float range{
		get{return shipInfo.range;}
	}
	/// <summary>
	/// 损毁度.
	/// </summary>
    public float damageDegree;

	public static implicit operator bool(ShipForPlayer data) {
        return data != null;
    }
}


