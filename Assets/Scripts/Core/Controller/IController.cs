using System;
public interface IController
{
    /**
     * 获取属性表.
     * @return
//		 */
    //		FightPropertyTable GetFightPropertyTable();
    //
    //		/**
    //		 * 重置清空属性.
    //		 */
    //		void ResetProperty();

    /**
     * 获取控制器类型.
     * @return
     */
    ControllerTypeInfo GetControllerType();
    /**
     * 初始化.
     * @param oPlayer
     */
    void Init(Player oPlayer);
    /// <summary>
    /// 是否初始化结束
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is init finish; otherwise, <c>false</c>.
    /// </returns>
    bool IsInitFinish();
}


