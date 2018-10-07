public interface ILife{

    //血量
    //死亡
    //维修
    //float Blood { set; get; }
    /// <summary>
    /// 收到伤害时调用这里
    /// </summary>
    void OnReceiveDamage(CannonBall cannonball);

    bool isAlive { get; }//是否已被击沉

    /// <summary>
    /// 死亡时调用这里
    /// </summary>
    void OnDie();

    /// <summary>
    /// 着火离子发射
    /// </summary>
    void OnCatchingFire(Unit obj,int count);
}