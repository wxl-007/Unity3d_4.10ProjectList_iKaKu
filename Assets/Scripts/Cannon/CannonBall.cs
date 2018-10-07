using UnityEngine;
using System.Collections;
/// <summary>
/// 导弹/炮弹
/// </summary>
public class CannonBall : BaseElement {
    CannonBallData cannonballinfo;
    public bool isVisible {
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
    public CannonBallData data{
        get{
            if (cannonballinfo == null){
                cannonballinfo = new CannonBallData();
                cannonballinfo.damage = 2;
                cannonballinfo.moveSpeed = 100;
                cannonballinfo.type = BulletType.cannon;
            }
            return cannonballinfo;
        }
    }
    public float gravity;
    public Vector3 moveSpeed;
    /// <summary>
    /// 为炮弹添加速度值
    /// </summary>
    /// <param name="moveSpeed">原始速度[度量单位:米/秒]</param>
    /// <param name="division">偏差(每百米偏差[度量单位:米])</param>
    public void SetCannonBall(Vector3 moveSpeed, float division) {
        //this.moveSpeed = moveSpeed + Random.onUnitSphere * division;
        Vector3 偏差 = Random.onUnitSphere * division;
        float 长度 = moveSpeed.magnitude;
        Vector3 原始方向 = moveSpeed.normalized * 100;
        Vector3 最终方向 = 原始方向 + 偏差;
        this.moveSpeed = 最终方向.normalized * 长度;
    }

    void Start() {
    }

    /// <summary>
    /// 打击到敌舰时调用此方法
    /// </summary>
    public void OnHitEnemy() {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 打击到水面时调用此方法
    /// </summary>
    public void OnHitWater() { 
        //击中水面特效
        ParticleController.Play(transform.position, Quaternion.identity, ExploseType.water);
        gameObject.SetActive(false);
    }
    
    void Update(){
        if (transform.position.y > 1){
            moveSpeed.y += Physics.gravity.y * Time.deltaTime;
            transform.forward = moveSpeed;
            transform.position += moveSpeed * Time.deltaTime;
            Collider[] other = Physics.OverlapSphere(transform.position ,8);
            if (other.Length != 0) {
                for (int i = 0; i < other.Length; i++){
                    if (other[i].GetComponent<Ship>()){
                        other[i].GetComponent<Ship>().OnReceiveDamage(this);
                    }else if (other[i].GetComponent<Island>()){
                        other[i].GetComponent<Island>().OnReceiveDamage(this);
                    }else if (other[i].GetComponent<AirCraft>()) {
                        other[i].GetComponent<AirCraft>().OnReceiveDamage(this);
                    }
                }
            }
        }else{
            //释放爆炸烟火效果
            OnHitWater();
        }
    }
}