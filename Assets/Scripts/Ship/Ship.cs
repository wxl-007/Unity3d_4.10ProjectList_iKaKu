#define Editor
using UnityEngine;
using System.Collections;
/// <summary>
/// 所有战舰父类
/// </summary>
//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ShipAI))]
public class Ship : FightElement,ILife{
    public string shipName;
    public Cannon[] cannons;        //大炮
    public bool isCommaderShip;     //是否为主舰

    #region 锅炉,瞭望塔数据
    public Unit WatchTower;  //瞭望塔
    public Unit Boiler;      //锅炉
    public Unit Radar;       //雷达
    #endregion

    #region 效果展示
    public Animation animBehaviour; //(伤害/死亡)动画
    public ShipEffect waves;        //海浪特效
    public ShipEffect ripple;       //波纹特效
    public ShipEffect vortex;       //漩涡(沉没特效)
    #endregion

    #region 技术数据
    public ShipForPlayer shipData;  //自身数据
    [HideInInspector]
    public FightElement hitTarget;   //攻击目标
    /// <summary>
    /// 判断是否目标存在并存活
    /// </summary>
    public bool canHitTarget { 
        get {
            if (!hitTarget)
                return false;
            if (hitTarget.GetComponent<Ship>()) {
                return hitTarget.GetComponent<Ship>().isAlive;
            }else if (hitTarget.GetComponent<Island>()) {
                return hitTarget.GetComponent<Island>().isAlive;
            }else if (hitTarget.GetComponent<AirCraft>()) {
                return hitTarget.GetComponent<AirCraft>().isAlive;
            }
            return false;
        }
    }
    [HideInInspector]
    public Vector3 moveToPosition;  //玩家设定移动位置
    [HideInInspector]
    public ShipMoveState moveState = ShipMoveState.MoveByMainPath;
    [HideInInspector]
    public bool canMove;            //是否可移动
    [HideInInspector]
    public bool isMoving = false;   //是否正在移动
    [HideInInspector]
    public bool canFire = false;    //是否可开火
    [HideInInspector]
    public float[] fireInterval;    //开火时间间隔计算
    public bool isAlive { get { return shipData.health > 0; } }   //是否存活
    /// <summary>
    /// 是否有相机渲染次物体
    /// </summary>
    public bool isVisible {
        get{
            if (renderer && renderer.isVisible){
                return true;
            }
            for (int i = 0; i < cannons.Length; i++) {
                if (cannons[i].isVisible) {
                    return true;
                }
            }
            return false;
        }
    }   //是否渲染
    #endregion

    IEnumerator Start(){
        canMove = false;    //设置移动状态为不可移动.(防止在读取数据的时候战船使用未赋值的数据引发异常)

        #region 测试
       // print("这里为测试域");
        if (isCommaderShip){
            ShipController.CommaderShip = this;
        }
        #endregion

        yield return new WaitForSeconds(2.0f);

        #if Editor
        if (!shipData) {    //获取自身数据
            try{
                GetShipData();
            }catch (System.Exception e) {
                Debug.LogError(e.Message);
            }
        }
        #endif

        if (shipData) {     //读取完战船数据之后再次检查是否读取成功
            fireInterval = new float[cannons.Length];
        }else{
            Debug.LogWarning("Ship Data is not assigned! But you are still trying to access it!", gameObject);
        }
        if (team == Team.Enemy)
            ShipController.EnemyShip.Add(this);
        else if (team == Team.Self)
            ShipController.MyShip.Add(this);
        else
            Debug.LogError("请为当前战舰设置阵营!", gameObject);
        canMove = true;     //战船可以移动
    }

    //预留(更改船的数据)
    void SetShipData(ShipForPlayer data) {
        this.shipData = data;
    }

    void GetShipData() {
        //TODO
        //获取当前战船数据并赋值给shipData.
		ShipForPlayerController shipForPlayerController = (ShipForPlayerController)GameManager.GetInstance().GetPlayer().GetController(ControllerTypeInfo.Ship);
        shipData = shipForPlayerController.AddShip(1);  //通知战船总控制,添加ID
        shipData.health = shipData.healthVolume;        //初始化血量
        for (int i = 0; i < cannons.Length; i++){
            cannons[i].InitData(shipData.shipInfo.mainWeapon);
        }
    }

    /// <summary>
    /// 判断攻击点是否在战船左边
    /// </summary>
    /// <param name="hitPos"></param>
    /// <returns></returns>
    bool IsHitLeft(Vector3 hitPos) {
        Vector3 dir = hitPos - transform.position;
        return Vector3.Cross(dir, transform.forward).y > 0;
    }

    #region 预留接口
    //鼠标或手指点击
    public void OnMouseEvent() { 
        //TODO
        //响应鼠标事件
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void OnStopMove() {
        canMove = false;
    }

    /// <summary>
    /// 开始移动
    /// </summary>
    public void OnStartMove() {
        canMove = true;
    }

    /// <summary>
    /// 遇到敌人
    /// </summary>
    public void OnEnemy() {
        OnStopMove();
    }

    //当升级战舰
    public void OnUpgrade(ShipForPlayer upgradeData) {
        shipData = upgradeData;
        //TODO
        //服务器同步
    }

    /// <summary>
    /// 船移动特效,水花显示,分水纹显示
    /// </summary>
    public void OnMove() {
        if(!waves){
            return;
        }
        if (isMoving) {
            waves.SetShow();
        }else{
            waves.SetHide();
        }
    }
    #endregion

    //死亡(播放动画,不可攻击,不可被攻击,销毁)
    public void OnDie() {
        canMove = false;
        canFire = false;
        isMoving = false;
        if (vortex)
            vortex.SetShow(); //播放船沉没漩涡
        if (animBehaviour)
            animBehaviour.Play("ShipDie");
        else
            Debug.LogWarning("animBehaviour is not assigned!");
        if (team == Team.Self) {
            ShipController.MyShip.Remove(this);
            ShipController.MyShipDead.Add(this);
        }else if (team == Team.Enemy) {
            ShipController.EnemyShip.Remove(this);
            ShipController.EnemyShipDead.Add(this);
        }
        StartCoroutine(WaitToSink());
    }
    IEnumerator WaitToSink(){//等待战舰死亡动画播放完毕后隐藏
        while (animBehaviour.IsPlaying("ShipDie")) {
            yield return null;
        }
        gameObject.SetActive(false);
    }

   /// <summary>
   /// 受到炮弹伤害
   /// </summary>
   /// <param name="cannonball">实施伤害炮弹</param>
    public void OnReceiveDamage(CannonBall cannonball) { //当受到伤害
        if (cannonball.data.belongTo == transform){
            return;
        }
        cannonball.OnHitEnemy();//通知炮弹攻击到目标,自动隐藏
        shipData.health -= cannonball.data.damage;
        if (isAlive) {      //判断是否存活
            if (!isVisible) //如果相机看不到,那就别忙活了(效果可以省略了).
                return;
            ParticleController.Play(cannonball.transform.position, Quaternion.identity, ExploseType.fire); //播放爆炸特效
            if (IsHitLeft(cannonball.transform.position)){
                if (isVisible && animBehaviour)
                    animBehaviour.Play("HitLeft");
            }else {
                if (isVisible && animBehaviour)
                    animBehaviour.Play("HitRight");
            }
            if (ripple)
                ripple.SetShow(); //播放波纹
        }else{
            OnDie();
        }
    }

    /// <summary>
    /// 当血量小于一定程度之后战舰着火动画
    /// </summary>
    public void OnCatchingFire(Unit component,int count){
        component.OnCatchingFire(count);
    }

    public bool SetTarget(FightElement target){ //设定移动目标
        switch (team){      //判断自己所在阵营
            case Team.Self:
            case Team.Alliance:
                if (target.team == Team.Enemy) {
                    hitTarget = target;
                    moveState = ShipMoveState.MoveToTarget;
                    return true;
                }
                break;
            case Team.Enemy:
                if (target.team != team) {
                    hitTarget = target;
                    moveState = ShipMoveState.MoveToTarget;
                    return true;
                }
                break;
        }
        return false;
    }

    public void SetPosition(Vector3 position){ //设定移动位置
        moveToPosition = position;
        moveState = ShipMoveState.MoveToPosition;
    }

    /// <summary>
    /// 计算出两个坐标点之间的水平面距离(忽略高度落差)
    /// </summary>
    /// <param name="P_1">坐标点</param>
    /// <param name="P_2">坐标点</param>
    /// <returns></returns>
    public float GetDisOfHorizontalPlane(Vector3 P_1, Vector3 P_2)
    {
        Vector2 p_1 = new Vector2(P_1.x, P_1.z);
        Vector2 p_2 = new Vector2(P_2.x, P_2.z);
        return Vector2.Distance(p_1, p_2);
    }

    public void Move(Vector3 Dir){
        //if(transform.forward != Dir)
    }

    //void OnCollisionEnter(Collision other){
    //    if (other.collider.GetComponent<CannonBall>()){ //被子弹打击到
    //        if (other.collider.GetComponent<CannonBall>().data.belongTo == transform){
    //            Debug.LogWarning("队长! 别开枪!自己人!", other.gameObject);
    //            return;
    //        }
    //        OnReceiveDamage(other.collider.GetComponent<CannonBall>());
    //        other.collider.GetComponent<CannonBall>().OnHitEnemy();
    //    }else if (other.collider.GetComponent<Ship>()){//被战舰撞击到
    //        //TODO
    //    }
    //}

    /// <summary>
    /// 火力全开
    /// </summary>
    /// <returns></returns>
    public IEnumerator FireAll(){
        if (hitTarget){
            Vector3 orgF = transform.forward;
            Vector3 tarF = Vector3.Cross(Vector3.up, hitTarget.transform.position - transform.position).normalized;
            float t = 0;
            while (t < 1){
                t += Time.deltaTime;
                transform.forward = Vector3.Lerp(orgF, tarF, t);
                yield return null;
            }
        }else{
            yield break;
        }
    }
}