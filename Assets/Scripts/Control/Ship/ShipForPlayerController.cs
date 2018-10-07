using System;
using System.Collections.Generic;
using UnityEngine;
public class ShipForPlayerController : IController
{
	List<ShipForPlayer> shipList = new List<ShipForPlayer>();
	Player player;
	#region IController implementation
	public ControllerTypeInfo GetControllerType ()
	{
		return ControllerTypeInfo.Ship;
	}

	public void Init (Player oPlayer)
	{
		player = oPlayer;
	}

	public bool IsInitFinish ()
	{
		return true;
	}
	#endregion
	/// <summary>
	/// 获取玩家所有的船.
	/// </summary>
	public List<ShipForPlayer> GetShipList(){
		return shipList;
	}
	/// <summary>
	/// 添加一个船.
    /// 这里是否应该加上,出现位置/方向,所属阵营?
	/// </summary>
	public ShipForPlayer AddShip(int id){
		ShipForPlayer ship = new ShipForPlayer();
		ship.id = id;
		shipList.Add(ship);
		return ship;
	}
	/// <summary>
	/// 获得某个船.
	/// </summary>
	public ShipForPlayer GetShip(int id){
		foreach(ShipForPlayer ship in shipList){
			if(ship.id == id){
				return ship;
			}
		}
		return null;
	}
}

