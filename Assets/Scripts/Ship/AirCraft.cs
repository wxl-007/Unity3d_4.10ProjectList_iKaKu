using UnityEngine;
using System.Collections;
/// <summary>
/// 飞机(轰炸技能)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AirCraft : FightElement,ILife,IFire{
    public MovePath flyPath;            //飞行路径
    public AirCraftCarrier BelongTo;    //所属航母
    public Vector3 attackPosition;      //攻击目标点
    public Transform aircraftBody;      //飞机本体

    public bool canFire { set; get; }   //是否可攻击
    public float Blood { get; set; }    //血量
    public bool isAlive { get { return Blood > 0; } }   //是否被摧毁
    public float fireRange { get; set; }//攻击半径
    public float fireInterval { set; get; }//攻击间隔(预留)

    float speed = 20;
    [HideInInspector]
    public Vector3 moveSpeed;           //移动速度
    bool bend = false;                  //是否转向
    Vector3 curSpeed;                   //当前速度
    AirCraftState nowState;             //当前飞机移动状态

    public Vector3 breedPos;    //飞机出生点
    public float flyHeight;     //飞行高度
    public Vector3[] pathNode; //飞行节点 
    public bool isDebug;
    public Vector3 shipHead; // 甲板尽头
    public void OnFire(Transform belongTo, FightElement target){
        //TODO
        //投弹
    }

    public void OnReceiveDamage(CannonBall cannonball) {
        Blood -= cannonball.data.damage;    //减少血量
    }

    void Start() {
        if (rigidbody) {
            rigidbody.useGravity = false;   //禁用物理重力
            rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else
            Debug.LogError("物体没有添加刚体组件!", gameObject);
        Blood = 1;
      
        aircraftBody = gameObject.transform;
        breedPos = gameObject.transform.localPosition;
        attackPosition = flyPath.pathNode[pathNode.Length];
        Debug.Log(pathNode.Length);
        moveSpeed = (attackPosition - breedPos) * 10;
        nowState = AirCraftState.go;
    }
    
    void Update() {
        if (isAlive){
            switch (nowState){
                case AirCraftState.go:
                    aircraftBody.transform.position += moveSpeed * Time.deltaTime;
                    if (aircraftBody.transform.position.y <= 150)
                    {
                        aircraftBody.transform.position += new Vector3(0, flyHeight, 0);

                    }
                    
                    break;
                case AirCraftState.back:
                    break;
                case AirCraftState.off:
                    break;
                case AirCraftState.land:
                    break;
            }
        }
    }

    void FindPath(Vector3 targetPos) {
        
        //从飞机生成点到跑道尽头
        //从跑道尽头到敌舰的区域
        //从敌舰区域到外围区域消失

    }

    public void OnDie(){
        BelongTo.Destroy(this);
    }

    public IEnumerator RotateTo(Vector3 targetPos)
    {
        Vector3 orgDir = transform.forward;
        Vector3 endDir = (targetPos - transform.position).normalized;
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime;
            transform.forward = Vector3.Lerp(orgDir, endDir, t);
            yield return null;
        }
        throw new System.NotImplementedException();
    }

    void OnDrawGizmos()
    {
        if (pathNode != null)
        {
            for (int i = 0; i < pathNode.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(pathNode[i], 5f);
                Gizmos.color = Color.yellow;
                if (i + 1 < pathNode.Length)
                    Gizmos.DrawLine(pathNode[i], pathNode[i + 1]);
            }
        }
        if (!isDebug)
        {
            return;
        }
    }


    public bool CanFire(FightElement target){
        return true;
        //TODO
    }

    public void SetTarget(FightElement target) {
        //TODO
        pathNode[0] = breedPos;
        pathNode[1] = shipHead;
        pathNode[2] = attackPosition;



    }

    public void SetHitPosition(Vector3 attackPosition) {
        this.attackPosition = attackPosition;
    }


    public void OnCatchingFire(Unit component,int count)
    {
          component.OnCatchingFire(count);
    }
}
public enum AirCraftState {
    off  = 0,
    go   = 1,
    back = 2,
    land = 3
}