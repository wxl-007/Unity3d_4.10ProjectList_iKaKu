using System;
using UnityEngine;
public class PlayerItem : MonoBehaviour, IItemValue
{
	//头像.
	public UIIcon icon;
	//呢称.
	public UILabel nameLabel;
	//等级.
	public UILabel levelLabel;
	//等级.
	public UINum levelUINum;
	//派别.
	public UILabel schoolLabel;
	//体力条.
	public UISlider tiliSlider;
	//体力值.
	public UILabel tiliLabel;
	//经验条.
	public UISlider expSlider;
	//经验值.
	public UILabel expLabel;
	//统治力.
	public UILabel costLabel;
	//金币.
	public UILabel goldLabel;
	//钻石.
	public UILabel diamondLabel;
	//荣誉.
    public UILabel honorLabel;
	//pvp积分.
	public UILabel killCountLabel;
	//赌计.
	public UILabel betLabel;
	//pvp排行.
	public UILabel pvpRankingLabe;
	//战斗力.
	public UILabel fightPowerLabel;
	public UISprite bgSprite;
	public UISprite iconFrameSprite;
	#region IItemValue implementation
	public void SetItemValue (object item)
	{
		Player player = (Player)item;
		if(player != null){
			if(icon != null){
				icon.SetPlayerIcon(player.id,player.icon);
			}
			if(nameLabel != null){
				nameLabel.text = player.name;
			}
			if(levelLabel != null){
				levelLabel.text = player.level.ToString();
			}
			if(levelUINum != null){
				levelUINum.num = player.level;
			}
			if(schoolLabel != null){
				if(player.school == SchoolType.angel){
					schoolLabel.text = "侠义同盟";
				}else{
					schoolLabel.text = "黑暗势力";
				}
			}
			if(tiliSlider != null){
				tiliSlider.sliderValue = player.energy * 1.0f / player.energy_max * 1.0f;
			}
			if(tiliLabel != null){
				tiliLabel.text = player.energy + "/" + player.energy_max;
			}
			if(expSlider != null){
				expSlider.sliderValue = player.exp * 1.0f / player.exp_max * 1.0f;
			}
			if(expLabel != null){
				expLabel.text = player.exp + "/" + player.exp_max;
			}
			if(costLabel != null){
//				CardGroupController cardGroupContoller = (CardGroupController)player.GetController(ControllerTypeInfo.CARDGROUP);
//				costLabel.text = cardGroupContoller.GetSelectCardGroup().GetCost().ToString() +"/"+ player.cost.ToString();
			}
			if(goldLabel !=null){
				goldLabel.text = player.gold.ToString();
			}
			if(diamondLabel != null){
				diamondLabel.text = player.silver.ToString();
			}
			if(honorLabel != null){
				honorLabel.text = player.honor.ToString();
			}
			if(killCountLabel != null){
				killCountLabel.text = player.pvpScore.ToString();
			}
			if(betLabel != null){
				betLabel.text = player.bet.ToString();
			}
			if(pvpRankingLabe != null){
				pvpRankingLabe.text = player.pvpRanking.ToString();
			}
			if(bgSprite != null){
				bgSprite.color = new Color(1,1,1,1);
			}
			if(fightPowerLabel != null){
				fightPowerLabel.text = player.fightPower.ToString();
			}
		}
	}

	public object GetItemValue ()
	{
		return GameManager.GetInstance().GetPlayer();
	}
	#endregion
	
	public void SetBgColor(Color color){
		if(bgSprite != null){
			bgSprite.color = color;
		}
	}
	public void SetBgSprite(string str){
		if(bgSprite != null){
			bgSprite.spriteName = str;
		}
	}
	public void SetFrameSprite(string str){
		if(iconFrameSprite != null){
			iconFrameSprite.spriteName = str;
		}
	}
}


