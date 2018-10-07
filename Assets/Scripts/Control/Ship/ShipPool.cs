using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 船配置表.
/// </summary>
public class ShipPool : IPool
{
	
	private static Dictionary<int, ShipInfo> tableInfo = new Dictionary<int, ShipInfo>();
	#region implemented abstract members of IPool
	public override IEnumerator Init ()
	{
		tableInfo.Clear();
		ShipInfo newObj = new ShipInfo();
		XmlHelper xmlHelper = XmlHelper.GetInstance();
		yield return GameManager.GetInstance().StartCoroutine(xmlHelper.getContentsByFiledName(newObj,"Data/ship"));
		List<object> datas = xmlHelper.alList;
		foreach(object obj in datas){
			newObj = (ShipInfo)obj;
			tableInfo.Add(newObj.id,newObj);
		}
		yield break;
	}
	#endregion
	public static ShipInfo GetInfo(int id){
		if(tableInfo.ContainsKey(id)){
			return tableInfo[id];
		}
		return null;
	}

	#region implemented abstract members of IPool
	public override IEnumerator Reload ()
	{
		tableInfo.Clear();
		yield return GameManager.GetInstance().StartCoroutine(Init());
	}
	#endregion
}

