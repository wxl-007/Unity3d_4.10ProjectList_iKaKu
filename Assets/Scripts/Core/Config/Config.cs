using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using JianghuUtils;
using System.Xml;

public static class Config
{
    public const string ConfigFileName = "Data/config";

    public static float FontSize_VeryBig = 36f;
    public static float FontSize_Big = 34f;
    public static float FontSize_Normal = 30f;
    public static float FontSize_Small = 26f;
    public static float FontSize_VerySmall = 24f;

    public static float PictureSize_VeryBig = 128f;
    public static float PictureSize_Big = 96f;
    public static float PictureSize_Formal = 64f;
    public static float PictureSize_Small = 32f;

    public static string Color_Yellow = "ffb400";

    public static float Float_Tip_Lasts_Time = 3f;

    public static string LoginServerURL = "";
    public static string LoginInterface = "handleLogin";
    public static string LoginInterface91 = "91/handleLogin";
    public static string RegistInterface = "handleRegister";
    public static string SetPlayerSexInterface = "account/usex";
    public static string SetPlayerIconInterface = "account/uavatar";
    public static string SetPlayerAreaInterface = "account/uaddress";
    public static string GetVersionInterface = "account/version";

    public static string FileServerURL = "";
    public static string FileUploadInterface = "up/";
    public static string FileDownInterface = "down/";
    /*
     * app status
     * */
    public static int status;

    /*
     * is mute
     * */
    public static bool mute;

    /*
     * static constructor
     * */
    //	static Config(){
    //		LoadConfig();
    //	}

    	public static string EffectPath = "Prefabs/Effect/";
    //	public static string MainSceneLogic = "MainLogic";
    //	
    //	//Panel prefabs resource folder.
    public static string PanelPath = "Prefabs/Panels/";
    //	
    //	/**
    //	 * PanelName;
    //	 */
    /// <summary>
    /// 玩家.
    /// </summary>
    public static string PlayerPanel = "PlayerPanel";
    /// <summary>
    /// 登录.
    /// </summary>
    public static string LoginPanel = "LoginPanel";
    /// <summary>
    /// 剧情.
    /// </summary>
    public static string StoryPanel = "StoryPanel";
    /// <summary>
    /// 英雄.
    /// </summary>
    public static string HeroPanel = "HeroPanel";
    /// <summary>
    /// 英雄详细页面.
    /// </summary>
    public static string HeroDetailedPanel = "HeroDetailedPanel";
    /// <summary>
    /// 市集
    /// </summary>
    public static string CardStoryPanel = "CardStoryPanel";
    public static string RandomCardPanel = "RandomCardPanel";
    public static string RandomSingelPanel = "RandomSingelPanel";

    public static string FightPanel = "FightPanel";
    /// <summary>
    /// 道具
    /// </summary>
    public static string PropsPanel = "Props/PropsPanel";
    ///<sunmmary>
    ///英雄培养
    ///<summary>
    public static string HeroTrainPanel = "HeroTrainPanel";
    ///<sunmmary>
    ///英雄选择
    ///<summary>
    public static string FoodHeroPanle = "FoodHeroPanle";
    /// <summary>
    /// 成就
    /// </summary>
    public static string GloryPanel = "Glory/GloryPanel";
    ///<sunmmary>
    ///聊天界面
    ///</sunmmary>
    public static string ChatPanel = "ChatPanel";
    ///<sunmmary>
    ///聊天菜单界面
    ///</sunmmary>
    public static string ChatMenuPanel = "ChatMenuPanel";
    ///<sunmmary>
    ///邮件界面
    ///</sunmmary>summary>
    public static string MailPanel = "MailPanel";
    ///<sunmmary>
    ///邮件内容界面
    ///</sunmmary>
    public static string MaiItemlPanel = "MaiItemlPanel";
	
	public static string GuanKaPanel = "GuanKaPanel";
	public static string PvpPanel = "PvpPanel";
    //	public static string PlayerPanel = "PlayerPanel";
    //	public static string ActorPanel = "ActorPanel";
    //	public static string ActorInformationPanel = "ActorInformationPanel";
    //	public static string BottomTab = "BottomTab";
    //	public static string EquipmentOperationPanel = "EquipmentOperationPanel";
    //	public static string EquipmentPanel = "EquipmentPanel";
    //	public static string TelentPanel = "TelentPanel";
    //	public static string WugongOperationPanel = "WugongOperationPanel";
    //	public static string WugongPanel = "WugongPanel";
    //	public static string XinfaOperationPanel = "XinfaOperationPanel";
    //	public static string XinfaPanel = "XinfaPanel";
    //	public static string SocialPanel = "SocialPanel";
    //	public static string JianghuPanel = "JianghuPanel";
    //	public static string ActivityPanel = "ActivityPanel";
    //	public static string SettingPanel = "SettingPanel";
    //	public static string TalentPanel = "TalentPanel";
    //	public static string HuoYueDuPanel = "HuoYuePanel";
    //	public static string TaskPanel = "MissionPanel";
    //	public static string TaskDetailPanel = "MissionDetailPanel";
    //	public static string MaiyiPanel = "MaiyiPanel";
    //	public static string XiwuPanel = "XiWuPanel";
    //	public static string CaikuangPanel = "";
    //	public static string ZhanlingPanel = "";
    //	public static string LoginPanel = "LoginPanel";
    //	public static string Login91Panel = "Login91Panel";
    //	
    //	public static string ItemPanel = "ItemPanel";
    ////	public static string MonsterListPanel = "MonsterListPanel";
    //	public static string MessagePanel = "MessagePanel";
    //	public static string MessageListPanel = "MessageListPanel";
    //	public static string BiGuanPanel = "BiGuanPanel";
    //	public static string StoryPanel = "StoryPanel";
    //	public static string ScenePanel = "ScenePanel";	
    //	public static string SocialDetailPanel = "SocialDetailPanel";	
    //	public static string RemarkPanel = "RemarkPanel";	
    //	public static string AddFriendPanel = "AddFriendPanel";
    //	public static string StorePanel = "StorePanel";
    //	public static string LeiTaiPanel = "LeiTaiPanel";
    //	public static string RookieGuidePanel = "RookieGuidePanel";
    //	public static string PlayerSetPanel = "PlayerSetPanel";
    //	
    public static string ComponentItemPath = "Prefabs/Items/";
    //	
    //	/*
    //	 * Component prefab path; 
    //	 */
    //	public static string StoreItemLine = "StoreItemLine";
    //	public static string WugongItemLine = "WugongItemLine";
    //	public static string UseWugong = "UseWuGong";
    public static string UIIcon = "UIIcon";
    //	public static string EquipmentItemLine = "EquipmentItemLine";
    //	public static string EquipmentItemSmall = "EquipmentItemSmall";
    //	public static string XinfaItemLine = "XinfaItemLine";
    //	public static string ItemLine = "ItemLine";
    public static string GuankaItem = "GuankaItem";
    //	public static string TalentLine = "TalentLine";
    public static string StoryItem = "StoryItem";
	public static string MonsterItem = "MonsterItem";
    //	public static string StorySign = "StorySign";
    //	public static string SceneItemLine = "SceneItemLine";
    //	public static string UIIconWithRotation = "UIIconWithRotation";
    //	public static string PlayerLine = "PlayerLine";	
    public static string FloatTip = "FloatTip";
    public static string StaticTip = "StaticTip";
    //	public static string MoreButton = "ZMoreButton";
    //	public static string MessageListBlock = "MessageListBlock";
    //	public static string PartnerLine = "PartnerLine";
    //	public static string MaiyiInteractionLine = "MaiyiInteractionLine";
    //	
    //	public static string Message_Friend = "Message_Friend";
    //	public static string MessageLine_Left = "MessageLine_Left";
    //	public static string MessageLine_Right = "MessageLine_Right";
    //	public static string LeiTaiEnemyLine = "LeiTaiEnemyLine";
    //	public static string Message_Time = "Message_Time";
    //	public static string Message_BiGuanInvitation = "Message_BiGuanInvite";
    //	
    //	public static string MissionLine = "MissionLine";
    //	public static string MissionAwardItem = "MissionAwardItem";
    //	public static string MissionRefreshLine = "MissionRefreshLine";
    //	public static string HuoyueLine = "HuoyueLine";
    //	
    //	public static string PurchaseConfirm = "PurchaseConfirm";
    //	
    //	public static string ActorButton = "Actor";
    //	public static string MessageButton = "Message";
    //	public static string SocialButton = "Social";
    //	public static string ActivityButton = "Activity";
    //	public static string JianghuButton = "Jianghu";

    public static string CardPositionItem = "CardPositionItem";
    public static string HeroItem = "HeroItem";
    public static string SwitchCardGroupItem = "SwitchCardGroupItem";
    public static string CardGroupTabItem = "CardGroupTabItem";
    public static string FightHeroItem = "FightHeroItem";
    public static string HeroTrainFoodItem = "HeroTrainFoodItem";
    public static string FoodHeroItem = "FoodHeroItem";
    public static string SpellInfoItem = "SpellInfoItem";
    public static string ChatLineItem = "ChatLineItem";
    public static string MailItem = "MailItem";
    public static string ChannelItem = "ChannelItem";
	public static string OtherPlayerHeroItem = "OtherPlayerHeroItem";
	public static string PvpRankingItem = "PvpRankingItem";
	public static string MapItem = "MapItem";
    public static string HeroIconPath = "Hero/";
    public static string GuankaIconPath = "Guanka/";
    public static string StoryIconPath = "Story/";
    public static string PropsIconPath = "Props/";
    public static string BuffIconPath = "Buff/";
    public static string HeroBigIconPath = "HeroBig/";

    public static string GloryIconPath = "Glory/";//成就 

    public static Dictionary<int, string> ConfigString = new Dictionary<int, string>();
    class ConfigStringInfo
    {
        public int id;
        public string str;
    }

    public enum e_color
    {
        white,
        black,
    }

    public static IEnumerator LoadConfig()
    {
        XmlHelper xmlHelperTmp = XmlHelper.GetInstance();
        yield return GameManager.GetInstance().StartCoroutine(xmlHelperTmp.LoadXml(Config.ConfigFileName));
        XmlDocument configXml = xmlHelperTmp.xml;
        //connection initialization
        LoginServerURL = configXml.SelectSingleNode("root/Configration/Connnection/LoginServerURL").InnerText;
        FileServerURL = configXml.SelectSingleNode("root/Configration/Connnection/FileServerURL").InnerText;
        LoginInterface = configXml.SelectSingleNode("root/Configration/Connnection/LoginInterface").InnerText;
        RegistInterface = configXml.SelectSingleNode("root/Configration/Connnection/RegistInterface").InnerText;

        //font size initialization
        FontSize_VeryBig = (float)XmlConvert.ToDouble(configXml.SelectSingleNode("root/Configration/System_Settings/FontSize/VeryBig").InnerText);
        FontSize_Big = (float)XmlConvert.ToDouble(configXml.SelectSingleNode("root/Configration/System_Settings/FontSize/Big").InnerText);
        FontSize_Normal = (float)XmlConvert.ToDouble(configXml.SelectSingleNode("root/Configration/System_Settings/FontSize/Normal").InnerText);
        FontSize_Small = (float)XmlConvert.ToDouble(configXml.SelectSingleNode("root/Configration/System_Settings/FontSize/Small").InnerText);
        FontSize_VerySmall = (float)XmlConvert.ToDouble(configXml.SelectSingleNode("root/Configration/System_Settings/FontSize/VerySmall").InnerText);

        //font size
        string isMute = configXml.SelectSingleNode("root/Configration/System_Settings/Mute").InnerText;
        if (isMute.Equals("1"))
        {
            mute = true;
        }
        else if (isMute.Equals("0"))
        {
            mute = false;
        }
    }
    static public string GetStringWhithColor(string str, e_color color)
    {
        string colorstr = null;
        colorstr = GetColorString(color);
        str = colorstr + str + "[-]";
        return str;
    }
    static public string GetColorString(e_color color)
    {
        string colorstr = null;
        switch (color)
        {
            default:
                colorstr = "[FFFFFF]";
                break;
        }
        return colorstr;
    }
    static public int GetColorInt(e_color color)
    {
        int colorstr;
        switch (color)
        {
            default:
                colorstr = 0xFFFFFF;
                break;
        }
        return colorstr;
    }
//    static public string GetChannelNameWithStyle(ChatPool.e_ChannelStyle style)
//    {
//        string str = null;
//        switch (style)
//        {
//            case ChatPool.e_ChannelStyle.All:
//                str = ConfigString[1];
//                break;
//            case ChatPool.e_ChannelStyle.Guild:
//                str = ConfigString[4];
//                break;
//            case ChatPool.e_ChannelStyle.Private:
//                str = ConfigString[3];
//                break;
//            case ChatPool.e_ChannelStyle.Team:
//                str = ConfigString[5];
//                break;
//            case ChatPool.e_ChannelStyle.World:
//                str = ConfigString[2];
//                break;
//            default:
//                Debug.LogError("no name");
//                break;
//        }
//        return str;
//    }
//    static public IEnumerator InitConfigStaticString()
//    {
//        ConfigString.Clear();
//
//        XmlHelper xmlHelper = XmlHelper.GetInstance();
//        ConfigStringInfo strinfo = new ConfigStringInfo();
//        yield return GameManager.GetInstance().StartCoroutine(xmlHelper.getContentsByFiledName(strinfo, "Data/string/string"));
//        List<object> datas = xmlHelper.alList;
//
//        for (int i = 0; i < datas.Count; i++)
//        {
//            ConfigString.Add((datas[i] as ConfigStringInfo).id, (datas[i] as ConfigStringInfo).str);
//        }
//        yield break;
//    }
}
