using UnityEngine;
using System.Collections;

/// <summary>
/// 玩家信息 panel.
/// </summary>
public class PlayerPanel : BasePanel
{
    public UINum lbNum;

    //头像.
    public UIIcon icon;
    //呢称.
    public UILabel nameLabel;
    //派别.
    public UILabel schoolLabel;
    // 体力条.
    public UISlider tiliSlider;
    //经验条.
    public UISlider expSlider;
    //经验值.
    public UILabel expLabel;
    //统治力.
    public UILabel costLabel;
    //金币.
    public UILabel silverLabel;
    //钻石.
    public UILabel diamondLabel;
    //荣誉.
    public UILabel honorLabel;
    //击杀数.
    public UILabel killCountLabel;
    //赌计.
    public UILabel betLabel;
    public override void Refresh()
    {
        base.Refresh();
        Player player = GameManager.GetInstance().GetPlayer();
        icon.SetPlayerIcon(player.id, player.icon);
        nameLabel.text = player.name;
        if (player.school == SchoolType.angel)
        {
            schoolLabel.text = "侠义同盟";
        }
        else
        {
            schoolLabel.text = "黑暗势力";
        }
        tiliSlider.sliderValue = player.energy * 1.0f / player.energy_max * 1.0f;
        expSlider.sliderValue = player.exp * 1.0f / player.exp_max * 1.0f;
        if (expLabel != null)
        {
            expLabel.text = player.exp + "/" + player.exp_max;
        }
        costLabel.text = player.cost.ToString();
        silverLabel.text = player.silver.ToString();
        diamondLabel.text = player.gold.ToString();
        honorLabel.text = player.honor.ToString();
        killCountLabel.text = player.pvpScore.ToString();
        betLabel.text = player.bet.ToString();
        lbNum.num = player.level;//显示等级
    }
//    public void OnPve()
//    {
//        PanelManager.GetInstance().ShowPanel(Config.StoryPanel);
//    }
//    public void OnHero()
//    {
//        PanelManager.GetInstance().ShowPanel(Config.HeroPanel);
//    }
//    public void OnStore()
//    {
//        PanelManager.GetInstance().ShowPanel(Config.CardStoryPanel);
//    }
//
//    /// <summary>
//    /// 道具
//    /// </summary>
//    public void OnProps()
//    {
//        PanelManager.GetInstance().ShowPanelCover(Config.PropsPanel);
//    }
//    /// <summary>
//    /// 成就
//    /// </summary>
//    public void OnGlory() { PanelManager.GetInstance().ShowPanel(Config.GloryPanel); }
//
//    public void OnTrain()
//    {
//        PanelManager.GetInstance().ShowPanel(Config.HeroTrainPanel);
//    }
//    public void OnChat()
//    {
//        PanelManager.GetInstance().ShowPanel(Config.ChatPanel);
//    }
//    public void OnPvp()
//    {
//        PanelManager.GetInstance().ShowPanel(Config.PvpPanel);
//
//        PvpAction.GetInstance().ApplyPvpRankingPlayer();
//        PvpAction.GetInstance().ApplyFreeSearchOtherPlayer();
//    }

//    /// <summary>
//    /// 召唤武馆界面
//    /// </summary>
//    public void OnApplyMartial()
//    {
//        PanelManager.GetInstance().ShowPanel(Config.MartialPanel);
//    }
}
