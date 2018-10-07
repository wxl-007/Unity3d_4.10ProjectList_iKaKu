using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 武器配置表.
/// </summary>
public class WeaponPool : IPool
{
	
	private static Dictionary<int, WeaponInfo> tableInfo = new Dictionary<int, WeaponInfo>();
	#region implemented abstract members of IPool
	public override IEnumerator Init ()
	{
		tableInfo.Clear();
		WeaponInfo newObj = new WeaponInfo();
		XmlHelper xmlHelper = XmlHelper.GetInstance();
		yield return GameManager.GetInstance().StartCoroutine(xmlHelper.getContentsByFiledName(newObj,"Data/weapon"));
		List<object> datas = xmlHelper.alList;
		foreach(object obj in datas){
			newObj = (WeaponInfo)obj;
			tableInfo.Add(newObj.id,newObj);
		}
		yield break;
	}
	#endregion
	public static WeaponInfo GetInfo(int id){
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




