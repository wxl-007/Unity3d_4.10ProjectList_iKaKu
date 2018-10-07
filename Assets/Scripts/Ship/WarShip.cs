//#define EDIT
using UnityEngine;
using System.Collections;
/// <summary>
/// 军舰
/// </summary>
public class WarShip:Ship{
    public float moveSpeed {
        get {
            return shipData.moveSpeed;
        }
    }
    public ShipAI selfAI;
    byte moveCount = 0;     //计算间隔帧
    ShipMoveState preState;
    void Update(){
        #region 移动/检测控制
        if (!canMove)
            return;
        switch (moveState){
            #region 停止移动
            case ShipMoveState.Stop:                /*当前设定为不可移动(停止移动,并向敌人开火)*/
                if (preState != ShipMoveState.Stop) {
                    preState = ShipMoveState.Stop;
                    //TODO
                }
                if (canHitTarget){
                    if (GetDisOfHorizontalPlane(transform.position, hitTarget.transform.position) < shipData.range) {//---如果目标在攻击范围内
                        canFire = true;
                    }else{//----------------------------------------------------------------------------------------------如果目标不在攻击范围内
                        canFire = false;
                        moveState = ShipMoveState.MoveToTarget;
                        return;
                    }
                }else{
                    canFire = false;
                    moveState = ShipMoveState.MoveByMainPath;
                    return;
                }
                break;
            #endregion
            #region 沿副路线移动
            case ShipMoveState.MoveByAssistantPath: /*当前设定为沿副路线移动*/
                if (preState != ShipMoveState.MoveByAssistantPath){
                    preState = ShipMoveState.MoveByAssistantPath;
                }
                if (!isCommaderShip) {
                    CheckNearObjects();
                    if (canHitTarget){
                        moveState = ShipMoveState.MoveToTarget;
                        break;
                    }
                }
                if (selfAI.assistantPath.SetMove(this)) { //副路径未到达终点,继续移动
                    if (selfAI.assistantPath.HaveNextNode() && IsStrumpAhead(transform.position, selfAI.assistantPath.GetNextNode())){ //如果遇到障碍物      重新计算路径
                        selfAI.GetAssistantPath(selfAI.assistantPath.GetEndNode());
                    }
                }else{  //副路径到达终点
                    if (selfAI.mainPath.SetMove(this)) {//主路径未到达终点,继续移动
                        selfAI.mainPath.turnInterval = 0;
                        moveState = ShipMoveState.MoveByMainPath;
                        return;
                    }else{                              //主路线到达终点,自由移动
                        moveState = ShipMoveState.MoveFree;
                        return;
                    }
                }
                break;
            #endregion
            #region 向设定目标移动
            case ShipMoveState.MoveToTarget:        /*当前设定为朝目标前进*/
                if (preState != ShipMoveState.MoveToTarget) {   //检测移动状态更改
                    preState = ShipMoveState.MoveToTarget;
                    if (hitTarget)
                        selfAI.GetAssistantPath(hitTarget.gameObject);
                }
                if (canHitTarget){     // = 如果攻击目标存在 = //
                    if (GetDisOfHorizontalPlane(transform.position, hitTarget.transform.position) < shipData.range){//如果目标在射程内-
                        if (!canFire) { //如果开火状态发生变化
                            StartCoroutine(FireAll());
                        }
                        canFire = true; //可开火
                    }else{//------------------------------------------------------------------------------------------如果目标不在射程内
                        canFire = false;//不可开火
                        if (selfAI.assistantPath.HaveNextNode() && IsStrumpAhead(transform.position, selfAI.assistantPath.GetNextNode())){
                            selfAI.GetAssistantPath(hitTarget.gameObject);
                        }
                        if(selfAI.assistantPath.SetMove(this)){     //沿路径,向目标移动
                            moveCount++;
                            if (moveCount == 64) {
                                moveCount = 0;
                                selfAI.GetAssistantPath(hitTarget.gameObject);
                            }
                        }else{
                            selfAI.GetAssistantPath(hitTarget.gameObject);
                        }
                    }
                }else {             // = 如果攻击目标被摧毁 = //
                    //计算副路线到主路线的下一个移动位置
                    if (selfAI.mainPath.HaveNextNode()){
                        //selfAI.GetAssistantPath(selfAI.mainPath.GetNextNode());//计算上次沿主路线上的下一个坐标点
                        SetPosition(selfAI.mainPath.GetNextNode());
                    }
                    else
                        moveState = ShipMoveState.MoveFree;
                }
                break;
            #endregion
            #region 沿主路线移动 (检测周围敌人,并停止移动,攻击敌人)
            case ShipMoveState.MoveByMainPath:      /*当前设定为沿主路线移动*/
                if (preState != ShipMoveState.MoveByMainPath) {
                    preState = ShipMoveState.MoveByMainPath;
                }
                if (!canHitTarget){     // = 如果未检测到攻击目标 = //
                    canFire = false;
                    if (selfAI.mainPath.SetMove(this)){ //主线未到最终点,继续移动
                        if (selfAI.mainPath.HaveNextNode()){
                            if (IsStrumpAhead(transform.position, selfAI.mainPath.GetNextNode())){ //移动过程中遇到障碍物
                                selfAI.mainPath.turnInterval = 0;
                                if (selfAI.mainPath.HaveNextNode())
                                    selfAI.GetAssistantPath(selfAI.mainPath.GetNextNode());//计算副路线
                                moveState = ShipMoveState.MoveByAssistantPath;//计算副路径,绕过障碍物
                                return;
                            }
                        }else{
                            moveState = ShipMoveState.MoveFree;
                        }
                    }else{             //主线到达最终点
                        moveState = ShipMoveState.MoveFree;
                        return;
                    }
                    CheckNearObjects(); //检测周围敌人
                }else{
                    moveState = ShipMoveState.MoveToTarget;
                }
                break;
            #endregion
            #region 向设定地点移动
            case ShipMoveState.MoveToPosition:      /*当前设定为向目标点移动*/
                if (preState != ShipMoveState.MoveToPosition) {
                    preState = ShipMoveState.MoveToPosition;
                }
                selfAI.GetAssistantPath(moveToPosition);
                moveState = ShipMoveState.MoveByAssistantPath;
                break;
            #endregion
            #region 自由移动
            case ShipMoveState.MoveFree:            /*当前设定为:主线移动到尽头,并没有次要路线*/
                //检测剩余敌人-向最近的一个开火
                if (preState != ShipMoveState.MoveFree) {
                    preState = ShipMoveState.MoveFree;
                }
                float dis = float.PositiveInfinity;
                if (team == Team.Self || team == Team.Alliance) {   //同盟,或者己方阵营
                    for (int i = 0; i < ShipController.EnemyShip.Count; i++) {
                        if (GetDisOfHorizontalPlane(transform.position, ShipController.EnemyShip[i].transform.position) < dis) {
                            dis = GetDisOfHorizontalPlane(transform.position, ShipController.EnemyShip[i].transform.position);
                            SetTarget(ShipController.EnemyShip[i]);
                        }
                    }
                }else if (team == Team.Enemy) {                     //敌方阵营
                    for (int i = 0; i < ShipController.MyShip.Count; i++) {
                        if (GetDisOfHorizontalPlane(transform.position, ShipController.MyShip[i].transform.position) < dis) {
                            dis = GetDisOfHorizontalPlane(transform.position, ShipController.MyShip[i].transform.position);
                            SetTarget(ShipController.MyShip[i]);
                        }
                    }
                }
                moveState = ShipMoveState.MoveToTarget;
                break;
            #endregion
        }
        #endregion

        #region 开火控制
        if (canFire && canHitTarget && GetDisOfHorizontalPlane(transform.position, hitTarget.transform.position) < shipData.range){ //如果可以开火
            isMoving = false;
            for (int i = 0; i < cannons.Length; i++){
                fireInterval[i] += Time.deltaTime;
                if (fireInterval[i] > cannons[i].fireInterval){
                    fireInterval[i] -= cannons[i].fireInterval;
                    cannons[i].OnFire(transform, hitTarget);
                }
            }
        }
        else {
            canFire = false;
        }
        #endregion
        OnMove();
    }

    /// <summary>
    /// 检测两点之间是否有障碍物
    /// </summary>
    /// <param name="prePos">上一个点</param>
    /// <param name="nextPos">下一个点</param>
    /// <returns></returns>
    bool IsStrumpAhead(Vector3 prePos,Vector3 nextPos) {//是否前方有障碍物
        Vector3 endpos = prePos + (nextPos - prePos).normalized * (boundOfMove << 1);
        #if EDIT
        Debug.DrawLine(prePos, endpos, Color.yellow);
        #endif
        endpos = nextPos;
        endpos.y = 0;
        return Physics.SphereCast(new Ray(prePos, endpos - prePos), .4f, (boundOfMove << 1));
    }

    Collider[] nearObjects;
    void CheckNearObjects() {
        Collider nearestObj = null;
        float dis = float.MaxValue;
        nearObjects = Physics.OverlapSphere(transform.position, shipData.range, EnemyLayer);
        if (nearObjects != null && nearObjects.Length != 0) {
            foreach (Collider obj in nearObjects) { //遍历所有检测到的碰撞体,并获取距离最近的一个.
                if (dis > GetDisOfHorizontalPlane(obj.transform.position, transform.position)){
                    dis = GetDisOfHorizontalPlane(obj.transform.position, transform.position);
                    nearestObj = obj;
                }
            }
            if (nearestObj)
                hitTarget = nearestObj.GetComponent<FightElement>();
        }
    }

#if EDIT
    void OnDrawGizmos() {
        print("编译时,请注释掉最上面的#define EDIT");
        if (string.IsNullOrEmpty(shipName)) {
            shipName = name;
        }
        Gizmos.color = new Color(1, 0, 0, .3f);
        if (shipData) {
            Gizmos.DrawSphere(transform.position, shipData.range);
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, boundOfMove);
        if (animBehaviour == null) {
            animBehaviour = transform.GetChild(0).GetComponent<Animation>();
        }
        if (animBehaviour){
            if (animBehaviour.gameObject.layer != LayerMask.NameToLayer("CheckPath")){
                animBehaviour.gameObject.layer = LayerMask.NameToLayer("CheckPath");
            }
        }
        if (!selfAI){
            selfAI = GetComponent<ShipAI>();
        }
    }
#endif
}