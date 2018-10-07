using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 爆炸粒子效果控制中心
/// 子弹控制中心
/// </summary>
public class ParticleController : MonoBehaviour
{
    void Awake() {
        //调用顺序赋值
        useIndex_Particle = new int[(int)ExploseType.Count];
        //托管赋值
        Play = PlayParticle;
        FireConnonBall = Fire;
    }

    void Start(){
        useIndex_Particle = new int[initCount_Particle.Length];
        EXP_Particle = new GameObject[initCount_Particle.Length][];
        for (int i = 0; i < initCount_Particle.Length; i++){
            EXP_Particle[i] = new GameObject[initCount_Particle[i]];
        }

        useIndex_CannonBall = new int[initCount_CannonBall.Length];
        cannonBall = new CannonBall[initCount_CannonBall.Length][];
        for (int i = 0; i < initCount_CannonBall.Length; i++) {
            cannonBall[i] = new CannonBall[initCount_CannonBall[i]];
        }
    }

    #region Particle Control Data
    /// <summary>
    /// 爆炸效果演示
    /// </summary>
    /// <param name="position">爆炸位置</param>
    /// <param name="direction">爆炸朝向</param>
    /// <param name="type">爆炸类型</param>
    public delegate void _PlayExplose(Vector3 position,Quaternion direction,ExploseType type);
    public static _PlayExplose Play;

    public GameObject[] particles;
    GameObject[][] EXP_Particle;        //缓存所有爆炸效果

    /// <summary>
    /// 使用索引,下标为类型值
    /// </summary>
    int[] useIndex_Particle;                     //使用索引
    /// <summary>
    /// 每种子弹类型所实例化最大数量
    /// </summary>
    public int[] initCount_Particle;           //给定每种特效初始数量
    void PlayParticle(Vector3 pos, Quaternion direction, ExploseType type) {
        return;
        int index = (int)type;

        GameObject curParticle;
        if (EXP_Particle[index][useIndex_Particle[index]]) {
            curParticle = EXP_Particle[index][useIndex_Particle[index]];
            if (curParticle.activeInHierarchy){     //当前特效正在被使用
                curParticle = InitNewParticles(type);   //找到未被利用的,或者实例化一个新的
            }
        }else{
            curParticle = EXP_Particle[index][useIndex_Particle[index]] = Instantiate(particles[index]) as GameObject;
        }
        curParticle.transform.position = pos;
        curParticle.transform.rotation = direction;
        curParticle.SetActive(true);

        useIndex_Particle[index]++;
        if (useIndex_Particle[index] == initCount_Particle[index]) {
            useIndex_Particle[index] = 0;
        }
    }

    GameObject InitNewParticles(ExploseType type){ 
        int index = (int)type;
        GameObject newParticle = null;
        for (int i = 0; i < initCount_Particle[index]; i++) {
            if (EXP_Particle[index][i]) {
                if (!EXP_Particle[index][i].activeInHierarchy) {
                    newParticle = EXP_Particle[index][i];
                    break;
                }
            }else{
                newParticle = EXP_Particle[index][i] = Instantiate(particles[index]) as GameObject;
                break;
            }
        }
        if (newParticle == null) {
            GameObject[] newParticles = new GameObject[initCount_Particle[index] + 8];
            Array.Copy(EXP_Particle[index], newParticles, initCount_Particle[index]);
            EXP_Particle[index] = newParticles; //赋值给缓存
            useIndex_Particle[index] = initCount_Particle[index];
            initCount_Particle[index] += 8;
        }
        newParticle = EXP_Particle[index][useIndex_Particle[index]] = Instantiate(particles[index]) as GameObject;
        return newParticle;
    }

    #endregion Particle Control Data

    #region CannonBall Control Data
    public delegate CannonBall FireCannonBalls(Vector3 position, Vector3 moveSpeed, BulletType type, float deviation);
    public static FireCannonBalls FireConnonBall;

    public CannonBall[] cannonballs;
    CannonBall[][] cannonBall;
    int[] useIndex_CannonBall;
    public int[] initCount_CannonBall;

    CannonBall Fire(Vector3 position, Vector3 moveSpeed, BulletType type, float deviation){
        int index = (int)type;
        CannonBall newCannonBall;
        if (cannonBall[index][useIndex_CannonBall[index]]) {
            newCannonBall = cannonBall[index][useIndex_CannonBall[index]];
            if (newCannonBall.gameObject.activeInHierarchy){ //不足?实例化新元素
                newCannonBall = InitNewCannonBall(type);
            }
        }else{
            newCannonBall = cannonBall[index][useIndex_CannonBall[index]] = Instantiate(cannonballs[index]) as CannonBall;
        }

        newCannonBall.transform.position = position;
        newCannonBall.SetCannonBall(moveSpeed, deviation);
        newCannonBall.gameObject.SetActive(true);
        useIndex_CannonBall[index]++;
        if (useIndex_CannonBall[index] == initCount_CannonBall[index]) {
            useIndex_CannonBall[index] = 0;
        }
        return newCannonBall;
    }

    /// <summary>
    /// 当数组中炮弹不够使用时,实例化新的炮弹,并赋值到数组中
    /// </summary>
    /// <param name="ball"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    CannonBall InitNewCannonBall(BulletType type){
        int index = (int)type;
        CannonBall newCannonBall = null;
        //遍历数组找到未被利用的子弹
        for (int i = 0; i < initCount_CannonBall[index]; i++) {
            if (cannonBall[index][i]) { //当前引用不为空
                if (!cannonBall[index][i].gameObject.activeInHierarchy) {    //当前未被使用
                    newCannonBall = cannonBall[index][i];
                    break;
                }
            }else{                      //当前引用为空!
                newCannonBall = cannonBall[index][i] = Instantiate(cannonballs[index]) as CannonBall;//赋值
                break;
            }
        }
        //如果所有子弹全部都在被利用,建立新的数组,并扩大存储上限
        if (null == newCannonBall) {
            CannonBall[] newCannonBalls = new CannonBall[initCount_CannonBall[index] + 8];
            Array.Copy(cannonBall[index], newCannonBalls, initCount_CannonBall[index]);
            useIndex_CannonBall[index] = initCount_CannonBall[index];
            initCount_CannonBall[index] += 8;
            cannonBall[index] = newCannonBalls; //赋值给缓存
        }
        newCannonBall = cannonBall[index][useIndex_CannonBall[index]] = Instantiate(cannonballs[index]) as CannonBall;
        return newCannonBall;
    }
    #endregion  CannonBall Control Data
}