//#define Editor
using UnityEngine;
using System.Collections;
/// <summary>
/// 战舰水面特效(波纹,浪花,分水线,漩涡)
/// </summary>
public class ShipEffect : MonoBehaviour {
    public ParticleSystem[] particles_1;
    public float[] Emission_PS;
    public ParticleEmitter[] particles_2;
    public float[] maxEmission_PE;
    public float[] minEmission_PE;
    void Start() {
        Emission_PS = new float[particles_1.Length];
        for (int i = 0; i < Emission_PS.Length; i++) {
            Emission_PS[i] = particles_1[i].emissionRate;
        }

        maxEmission_PE = new float[particles_2.Length];
        minEmission_PE = new float[particles_2.Length];
        for (int i = 0; i < minEmission_PE.Length; i++){
            minEmission_PE[i] = particles_2[i].minEmission;
            maxEmission_PE[i] = particles_2[i].maxEmission;
        }
    }

    /// <summary>
    /// 设置粒子发射器停止
    /// </summary>
    public void SetHide() {
        if (particles_1.Length != 0){
            for (int i = 0; i < particles_1.Length; i++){
                particles_1[i].emissionRate = 0;
            }
        }
        if (particles_2.Length != 0){
            for (int i = 0; i < particles_2.Length; i++){
                particles_2[i].minEmission = particles_2[i].maxEmission = 0;
            }
        }
    }
    public void SetHideImmediate(){ 
        for (int i = 0; i < particles_1.Length; i++) {
            particles_1[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < particles_2.Length; i++) {
            particles_2[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置显示所有
    /// </summary>
    public void SetShow() {
        SetShowImmediate();
        if (particles_1.Length != 0) {
            for (int i = 0; i < particles_1.Length; i++){
                particles_1[i].emissionRate = Emission_PS[i];
            }
        }

        if (particles_2.Length != 0){
            for (int i = 0; i < particles_2.Length; i++){
                particles_2[i].minEmission = minEmission_PE[i];
                particles_2[i].maxEmission = maxEmission_PE[i];
            }
        }
    }

    void SetShowImmediate() {
        for (int i = 0; i < particles_1.Length; i++) {
            particles_1[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < particles_2.Length; i++) {
            particles_2[i].gameObject.SetActive(true);
        }
    }
#if Editor
    ArrayList ps = new ArrayList();
    ArrayList pe = new ArrayList();
    public bool Get;
    public bool hide;
    void OnDrawGizmos() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<ParticleSystem>()) {
                ps.Add(transform.GetChild(i));
            }else if (transform.GetChild(i).GetComponent<ParticleEmitter>()) {
                pe.Add(transform.GetChild(i));
            }
        }
        particles_1 = new ParticleSystem[ps.Count];
        particles_2 = new ParticleEmitter[pe.Count];
        for (int i = 0; i < particles_1.Length; i++) {
            particles_1[i] = ((Transform)ps[i]).GetComponent<ParticleSystem>();
        }
        for (int i = 0; i < particles_2.Length; i++) {
            particles_2[i] = ((Transform)pe[i]).GetComponent<ParticleEmitter>();
        }
        ps.Clear();
        pe.Clear();

        if (Get){
            Get = false;
            Emission_PS = new float[particles_1.Length];
            for (int i = 0; i < Emission_PS.Length; i++)
            {
                Emission_PS[i] = particles_1[i].emissionRate;
            }

            maxEmission_PE = new float[particles_2.Length];
            minEmission_PE = new float[particles_2.Length];
            for (int i = 0; i < maxEmission_PE.Length; i++)
            {
                minEmission_PE[i] = particles_2[i].minEmission;
                maxEmission_PE[i] = particles_2[i].maxEmission;
            }
        }
    }   
#endif
}
public enum ParticleType { 
    waves,
    ripple,
    vortex
}