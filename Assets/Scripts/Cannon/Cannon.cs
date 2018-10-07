#define EDIT
using UnityEngine;
using System.Collections;
/// <summary>
/// 大炮(发射子弹所用)
/// 当前脚本添加到有renderer组件的物体上,因为:
/// OnBecameInvisible 和 OnBecameVisible 需要渲染组件
/// </summary>
public class Cannon : BaseElement,IFire {
    public int cannon_ID;
    public CannonType cannonType;
    public WeaponInfo data;
    public bool canFire { set; get; }   //是否可开火
    public float fireInterval { set { data.interval = value; } get { return data.interval; } }  //开火间隔
    public float fireRange { get { return data.range; } set { data.range = value; } }           //开火有效射程

    public Transform CannonBarrel;      //炮管
    public Animation recoilAnim;        //后坐力展示动画

    /// <summary>
    /// 是否有相机渲染此物体
    /// </summary>
    public bool isVisible{                     //是否相机可见(哎呦我艹,管用!!)
        get {
            if(renderer && renderer.isVisible){
                return true;
            }
            for (int i = 0; i < transform.childCount; i++) {
                if (transform.GetChild(i).renderer && transform.GetChild(i).renderer.isVisible) {
                    return true;
                }
            }
            return false;
        }
    }
    public Transform[] firePosition;    //每个炮弹打出位置(炮管口)

    public CannonBall bulletPrefs;  //发射子弹预设
    Transform belongTo;             //所属

    #region 发射角度问题_Begin
    public Vector3 leftSightAxis;   //视角左侧最大轴向|
    public Vector3 righSightAxis;   //视角右侧最大轴向| 这两个轴会夹出一个角度,扇形覆盖区域(由绿到红)就是可发射区域.

    [HideInInspector]
    public Vector3 leftAxis;
    [HideInInspector]
    public Vector3 RighAxis;
    /// <summary>
    /// 判断是否视野≥180°
    /// (≥ 180° = true)
    /// (＜ 180° = false)
    /// </summary>
    bool isObtuseAngle { get { return Vector3.Cross(leftSightAxis, righSightAxis).y <= 0; } }  //判断当前视角 ≥ 180°,返回true, 否则返回false.

    public CannonAxis axisType = CannonAxis.普通;
    public RotateType rotateType;
    #endregion 发射角度问题_End   已经完美解决

    void Start() {
        canFire = false;
    }

    //这里初始化数据
    public void InitData(WeaponInfo tData){
        data = tData;
    }

    public void OnDie()//预留.按需更改
    {
        //销毁还是隐藏,这是个问题.
        Destroy(gameObject);
        //gameObject.SetActive(false
    }

    //发射炮弹入口(内部处理旋转,角度问题)
    public void OnFire(Transform belongTo, FightElement target){
        this.belongTo = belongTo;
        if (!CanFire(target))   //首先判断是否可攻击
            return;
        Ship hitTarget;
        if ((hitTarget = target.GetComponent<Ship>()) && hitTarget.isMoving) {  //判断目标是否正在移动
            #region 来吧!计算一个提前量!!!!!!
            Vector3 hitPos = GetPointInBounds(hitTarget);
            float t = Vector3.Distance(hitPos, transform.position) / bulletPrefs.data.moveSpeed;
            Vector3 targetPos = hitTarget.shipData.moveSpeed * target.transform.forward * (t + fireInterval * .25f) + hitPos;//最终目标中心点
            StartCoroutine(RotateTo(targetPos));
            #endregion
        }else{
            if (target.GetComponent<FightElement>())
                StartCoroutine(RotateTo(GetPointInBounds(target.GetComponent<FightElement>())));
            else
                StartCoroutine(RotateTo(target.transform.position));
        }
    }
    
    Vector3 GetSpeed(Vector3 firePosition, Vector3 hitPosition, float bulletSpeed)//获取初始速度(矢量)
    {
        //距离
        float dis = Vector3.Distance(firePosition, hitPosition);
        //方向
        Vector3 dir = (hitPosition - firePosition).normalized;
        //速度
        //xz
        float t = dis / bulletSpeed;
        dir *= bulletSpeed;
        //y
        dir.y -= Physics.gravity.y * t * .5f;
        return dir;
    }

    public void OnMouseEvent()
    {
        print("OnMouseEvent");
    }

    public void OnReceiveDamage()
    {
        print("OnReceiveDamage");
    }

    public IEnumerator RotateTo(Vector3 targetPosition){
        canFire = false;
        float rotateSpeed = 4 / fireInterval;   //调整旋转速度为发射间隔的4倍
        float t = 0;
        Vector3 orgForward = transform.forward; //---------------------------------------------------------------初始前方朝向轴
        Vector3 tarForward = GetSpeed(firePosition[0].position, targetPosition, bulletPrefs.data.moveSpeed);   //目的前方朝向轴

        float angle_X_org = CannonBarrel.localEulerAngles.x;
        float angle_X_tar = Vector3.Angle(tarForward, new Vector3(tarForward.x, 0, tarForward.z));
        if (tarForward.y > 0)   //判断是否向上转动X夹角
            angle_X_tar = -angle_X_tar;//设置为负
        tarForward.y = 0;
        
        #region 摄像机未渲染
        if(!isVisible) { //摄像机没看你,别忙活了.哈哈哈哈.
            CannonBarrel.localEulerAngles = new Vector3(angle_X_tar, 0, 0);
            transform.forward = tarForward;
            Fire(GetSpeed(firePosition[0].position, targetPosition, bulletPrefs.data.moveSpeed));
            yield break;
        }
        #endregion

        #region 当判断可能会转出预设视角时(夹角大于180°).
        if (isObtuseAngle && IsNeedLerp(orgForward, tarForward)){
            Vector3 lerpForward = -(orgForward + tarForward) * .5f; //添加一个中间量,过度到这个位置之后再向下一个位置前进.
            while (t < 1){
                t += Time.deltaTime * rotateSpeed;
                if (t < 0.5f)
                    transform.forward = Vector3.Slerp(orgForward, lerpForward, t * 2);
                else
                    transform.forward = Vector3.Slerp(lerpForward, tarForward, t * 2 - 1);
                CannonBarrel.localEulerAngles = new Vector3(Mathf.LerpAngle(angle_X_org, angle_X_tar, t), 0, 0);
                yield return null;
            }
        }
        #endregion
        #region 当判断不会转出预设视角时(夹角小于180°).
        else{
            while (t < 1){
                t += Time.deltaTime * rotateSpeed;
                transform.forward = Vector3.Slerp(orgForward, tarForward, t);
                CannonBarrel.localEulerAngles = new Vector3(Mathf.LerpAngle(angle_X_org, angle_X_tar, t), 0, 0);
                yield return null;
            }
        }
        #endregion

        Fire(GetSpeed(firePosition[0].position, targetPosition, bulletPrefs.data.moveSpeed));
    }

    /// <summary>
    /// 判断是否需要转向的时候中间插值(避免大炮转向不可能的角度)
    /// </summary>
    /// <param name="D">自身当前朝向</param>
    /// <param name="T">目标朝向</param>
    /// <returns></returns>
    bool IsNeedLerp(Vector3 D, Vector3 T){
        Vector3 forwardAxis = transform.parent.rotation * ((leftSightAxis + righSightAxis) * .5f);
        if ((Vector3.Cross(D, T).y > 0 && (Vector3.Cross(D, forwardAxis).y > 0 && Vector3.Cross(forwardAxis, T).y > 0)) || (Vector3.Cross(D, T).y < 0 && (Vector3.Cross(D, forwardAxis).y < 0 && Vector3.Cross(forwardAxis, T).y < 0))){//嘿嘿!不要修改这里了.能用就行.
            return true;
        }
        return false;
    }

    /// <summary>
    /// 发射炮弹,后坐力展示
    /// </summary>
    /// <param name="cannonballSpeed"></param>
    /// <returns></returns>
    void Fire(Vector3 cannonballSpeed){//发射,炮弹在这里出膛!
        for (int i = 0; i < firePosition.Length; i++) {
            CannonBall canball = ParticleController.FireConnonBall(firePosition[i].position, cannonballSpeed, BulletType.normal, data.deviation);      //再注释回来
            //canball.SetCannonBall(cannonballSpeed, .3f);
            canball.data.belongTo = belongTo;
        }
        if (isVisible){
            ParticleController.Play(firePosition[0].position, CannonBarrel.rotation, ExploseType.normal);
            if (recoilAnim){
                recoilAnim.Play();
            }
        }
        canFire = true;
    }
    
    /// <summary>
    /// 判断攻击目标是否在可攻击视角内
    /// </summary>
    /// <param name="target">攻击目标</param>
    /// <returns></returns>
    public bool CanFire(FightElement target){
        switch (cannonType) { 
            case CannonType.主炮:   //主炮
                if (!(target.fightElementType == FightElementType.Ship || target.fightElementType == FightElementType.Island)) {//攻击目标不为战舰或者岛,不可攻击
                    return false;
                }
                break;
            case CannonType.组合炮://组合炮 //攻击目标为全部,无需判断目标,直接进行下面的角度判断.
                break;
            case CannonType.高射炮:       //高射炮
                if (target.fightElementType != FightElementType.AirCraft) {  //攻击目标不为飞机,不可攻击
                    return false;
                }
                break;
            case CannonType.副炮:     //副炮
                if (target.fightElementType != FightElementType.Island) {    //攻击目标不为岛,不可攻击
                    return false;
                }
                break;
        }
        Vector3 targetDir = belongTo.worldToLocalMatrix.MultiplyVector(target.transform.position - transform.position);
        targetDir.y = 0;
        if (isObtuseAngle) {  //如果视角 ≥ 180°
            if (Vector3.Cross(righSightAxis, targetDir).y > 0 && Vector3.Cross(targetDir, leftSightAxis).y > 0){//判断在右轴与左轴之间.
                return false;
            }else{
                return true;
            }
        }else {             //如果视角 ＜ 180°
            if (Vector3.Cross(leftSightAxis, targetDir).y > 0 && Vector3.Cross(targetDir, righSightAxis).y > 0){//判断在左轴与右轴之间.
                return true;
            }else{
                return false;
            }
        }
    }

    /// <summary>
    /// 获取他妈的碰撞盒之内的一个他妈的随机点,擦!
    /// </summary>
    /// <param name="target">拥有他妈的碰撞盒的他妈的战斗元素</param>
    /// <returns></returns>
    public Vector3 GetPointInBounds(FightElement target){
        Vector3 externts = target.externts;
        return target.transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(Random.Range(-externts.x, externts.x), externts.y, Random.Range(-externts.z, externts.z)));
    }

#if EDIT
    #region  这一部分在编译时请删除掉.(仅留作测试调试用).
    //public Ship shipTest;
    //public int density = 20;
    public FightElement testTarget;

    public bool isDebug;

    public float bulletMoveSpeed = 100;
    public float shipMoveSpeed = 10;
    void OnDrawGizmos() {
        if (testTarget) {
            if (CanFire(testTarget)) {
                Debug.DrawLine(transform.position, testTarget.transform.position, Color.green);
            }
        }

        for (int i = 0; firePosition != null && i < firePosition.Length; i++) {
            if (firePosition[i]){
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(firePosition[i].position, .3f);
            }
        }
        if (leftSightAxis.y != 0) leftSightAxis.y = 0;
        if (righSightAxis.y != 0) righSightAxis.y = 0;

        if (Application.isPlaying){
            Debug.DrawLine(transform.position, transform.position + transform.parent.rotation * leftSightAxis, Color.blue);
            Debug.DrawLine(transform.position, transform.position + transform.parent.rotation * righSightAxis, Color.black);
        }else{
            if (!CannonBarrel) {
                for (int i = 0; i < transform.childCount; i++) {
                    if (transform.GetChild(i).name.Contains("CannonBarrel"))
                        CannonBarrel = transform.GetChild(i);
                }
                if (!CannonBarrel) {
                    for (int i = 0; i < transform.childCount; i++) {
                        for (int j = 0; j < transform.GetChild(i).childCount; j++) {
                            if (transform.GetChild(i).GetChild(j).name.Contains("CannonBarrel")) {
                                CannonBarrel = transform.GetChild(i).GetChild(j);
                            }
                        }
                    }
                }
            }
            Debug.DrawLine(transform.position, transform.position + leftSightAxis * 2, Color.green);
            Debug.DrawLine(transform.position, transform.position + righSightAxis * 2, Color.red);
        }
        
        if (CannonBarrel.transform.parent != transform) {
            CannonBarrel.transform.parent = transform;
        }
    }

    void OnDrawGizmosSelected() {
        //base.OnDrawGizmosSelected();
        if (!isDebug)
            return;
        print(name + "  isObtuseAngle = " + isObtuseAngle);
        float t = 0;
        if (!Application.isPlaying){
            if (isObtuseAngle){
                Vector3 mid = -(leftSightAxis + righSightAxis).normalized;
                //Debug.DrawLine(transform.position, transform.position + mid, Color.magenta);
                while (t < 1){
                    t += 0.01f;
                    if (t < .5f)
                        Debug.DrawLine(transform.position, transform.position + Vector3.Slerp(leftSightAxis.normalized, mid, t * 2).normalized * 10, Color.Lerp(Color.green, Color.red, t));
                    else
                        Debug.DrawLine(transform.position, transform.position + Vector3.Slerp(mid, righSightAxis.normalized, t * 2 - 1).normalized * 10, Color.Lerp(Color.green, Color.red, t));
                }
            }else{
                while (t < 1){
                    t += 0.01f;
                    Debug.DrawLine(transform.position, transform.position + Vector3.Slerp(leftSightAxis.normalized, righSightAxis.normalized, t) * 10, Color.Lerp(Color.green, Color.red, t));
                }
            }
        }else{
            if (isObtuseAngle) {
                Vector3 mid = -(transform.parent.rotation * (leftSightAxis + righSightAxis)).normalized;
                while (t < 1){
                    t += 0.01f;
                    if (t < .5f)
                        Debug.DrawLine(transform.position, transform.position + Vector3.Slerp(transform.parent.rotation * leftSightAxis.normalized, mid, t * 2).normalized * 10, Color.Lerp(Color.green, Color.red, t));
                    else
                        Debug.DrawLine(transform.position, transform.position + Vector3.Slerp(mid, transform.parent.rotation * righSightAxis.normalized, t * 2 - 1).normalized * 10, Color.Lerp(Color.green, Color.red, t));
                }
            }else{
                while (t < 1){
                    t += 0.01f;
                    Debug.DrawLine(transform.position, transform.position + Vector3.Slerp(transform.parent.rotation * leftSightAxis, transform.parent.rotation * righSightAxis, t) * 10, Color.Lerp(Color.green, Color.red, t));
                }
            }
        }
    }

    /// <summary>
    /// 画一条虚线
    /// </summary>
    /// <param name="start">起点</param>
    /// <param name="end">终点</param>
    /// <param name="density">密度</param>
    /// <param name="c">颜色</param>
    void DrawImaginaryLine(Vector3 start, Vector3 end, int density, Color color){
        Vector3[] Pos = new Vector3[density << 1 + 2];
        Vector3 dir = (end - start) / (density << 1);
        for (int i = 0; i < Pos.Length; i++){
            Pos[i] = start + dir * i;
        }
        for (int i = 0; i < density << 1; i += 2){
            Debug.DrawLine(Pos[i], Pos[i + 1], color);
        }
    }
    #endregion
#endif
}
public enum CannonAxis {
    普通      = 0,
    增强      = 1
}
//有些炮需要转动炮基,有些炮不需要转动炮基.有些炮需要额外两个轴确定角度.
public enum RotateType { 
    炮基转动,
    仅炮管动,
    全部不动
}