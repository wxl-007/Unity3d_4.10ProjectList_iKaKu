using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : IComparable
{
    public uint id;
    /// <summary>
    /// 呢称.
    /// </summary>//
    public string name;
    /// <summary>
    /// 等级.
    /// </summary>
    public byte level;
    public int exp;
    public int exp_max
    {
        get { return PlayerPool.GetPlayerLevelUpExp(level); }
    }
    //头像.
    public string icon;
    //性别.
    public byte sex;
    //vip等级.
    public byte vipLevel;
    //体力.
    public short energy;
    public short energy_max
    {
        get { return (short)(50 + (level - 1) * 2); }
    }
    //统治力.
    public short cost
    {
        get { return (short)(20 + (level - 1) * 2); }
        set { }
    }
    //钻石.
    public int gold;
    //游戏币.
    public int silver;
    //荣誉.
    public int honor;
    //pvp积分.
    public int pvpScore;
	//pvp排行.
    public int pvpRanking;
    //赌计.
    public int bet;
    //派系.
    public SchoolType school;
    //战斗力.
    public short fightPower;
    protected List<IController> iControllers;

    public Player()
    {

    }
    //初始化.
    public void Init()
    {
        RegistControllers();
    }
    /// <summary>
    /// 获得某模块控制器.
    /// </summary>
    /// <returns>
    /// The controller.
    /// </returns>
    /// <param name='controllerType'>
    /// 模块类型.
    /// </param>
    public IController GetController(ControllerTypeInfo controllerType)
    {
        foreach (IController controller in iControllers)
        {
            if (controller.GetControllerType() == controllerType)
            {
                return controller;
            }
        }
        return null;
    }
	public void SetControllerList(List<IController> tControllers){
		iControllers = tControllers;
	}
    /// <summary>
    /// 注册所有模块.
    /// </summary>
    public void RegistControllers()
    {
        iControllers = new List<IController>();
        IController shipforPlayController = new ShipForPlayerController();
        shipforPlayController.Init(this);
        iControllers.Add(shipforPlayController);
//        IController guankaController = new GuankaController();
//        guankaController.Init(this);
//        iControllers.Add(guankaController);
//        IController equipmentController = new EquipmentController();
//        equipmentController.Init(this);
//        iControllers.Add(equipmentController);
//        IController cardGroupController = new CardGroupController();
//        cardGroupController.Init(this);
//        iControllers.Add(cardGroupController);
//        IController currencyController = new CurrencyController();
//        currencyController.Init(this);
//        iControllers.Add(currencyController);
//        IController cardStoreController = new CardStoreController();
//        cardStoreController.Init(this);
//        iControllers.Add(cardStoreController);
//        IController fightController = new FightController();
//        fightController.Init(this);
//        iControllers.Add(fightController);
//        //道具
//        IController propsController = new PropsCottroller();
//        propsController.Init(this);
//        iControllers.Add(propsController);
//        //成就
//        IController gloryController = new GloryController();
//        gloryController.Init(this);
//        iControllers.Add(gloryController);
//
//        ///<summer>
//        ///chat
//        ///</summer>
//        IController chatController = new ChatController();
//        chatController.Init(this);
//        iControllers.Add(chatController);
//
//        ///<summer>
//        ///mail
//        ///</summer>
//        //IController mailController = new MailController();
//        //mailController.Init(this);
//        //iControllers.Add(mailController);
//		IController pvpController = new PvpController();
//        pvpController.Init(this);
//        iControllers.Add(pvpController);
    }

	#region IComparable implementation
	public int CompareTo (object obj)
	{
		Player tPlayer = (Player)obj;
		return pvpScore.CompareTo(tPlayer.pvpRanking);
	}
	#endregion
}