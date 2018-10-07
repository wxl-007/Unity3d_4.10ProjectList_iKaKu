using System;
using System.Collections;
using System.Collections.Generic;
/**
 * 玩家配置表.
 */
public class PlayerPool : IPool
{
	class PlayerLevelUpExp{
		public int level = 0;
		public int experience = 0;
	}
	/// <summary>
	/// <玩家等级，玩家升级经验>.
	/// </summary>
	private static Dictionary<int,int> playerLevelUpExp = new Dictionary<int,int>();
	
    public override IEnumerator Init() {
		playerLevelUpExp.Clear();
		yield return GameManager.GetInstance().StartCoroutine(InitPlayerLevelUpExp());
		yield break;
    }
	/// <summary>
	/// 读取玩家升级经验配置表.
	/// </summary>
	private IEnumerator InitPlayerLevelUpExp(){
		PlayerLevelUpExp newObj = new PlayerLevelUpExp();
		XmlHelper xmlHelper = XmlHelper.GetInstance();
		yield return GameManager.GetInstance().StartCoroutine(xmlHelper.getContentsByFiledName(newObj,"Data/playerExpLevel"));
		List<object> datas = xmlHelper.alList;
		foreach(object obj in datas){
			newObj = (PlayerLevelUpExp)obj;
			playerLevelUpExp.Add(newObj.level,newObj.experience);
		}
	}
	/// <summary>
	/// 获得玩家某一等级升级所需经验.
	/// </summary>
	/// <returns>
	/// 所需经验.
	/// </returns>
	/// <param name='level'>
	/// 玩家等级.
	/// </param>
	public static int GetPlayerLevelUpExp(int level){
		if (playerLevelUpExp.ContainsKey(level))
            return playerLevelUpExp[level];
        return int.MaxValue;
	}
	
    public override IEnumerator Reload() {
        if(playerLevelUpExp != null)
            playerLevelUpExp.Clear();
        yield return GameManager.GetInstance().StartCoroutine(Init());
    }
}


