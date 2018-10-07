using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SphereCollider))]
public class Unit : BaseElement{
    public ParticleEmitter[] Smoking;   //这个是冒烟粒子效果
    float[] maxRate;
    public int radius = 10;

    void Start() {
        InitMaxRate();
    }

    void InitMaxRate() { 
        maxRate = new float[Smoking.Length];
        for (int i = 0; i < maxRate.Length; i++) {
            maxRate[i] = Smoking[i].maxEmission;
            Smoking[i].maxEmission = 0;
        }
    }

    public void OnCatchingFire(int count) {
        for (int i = 0; i < Smoking.Length; i++) {
            Smoking[i].maxEmission = count;
        }
    }

    public void OnCatchingFire(float rate) {
        for (int i = 0; i < maxRate.Length; i++) {
            Smoking[i].maxEmission = maxRate[i] * rate;
        }
    }





    public Color c = new Color(1, 1, 1, .1f);
    void OnDrawGizmos() {
        if (!collider) {
            gameObject.AddComponent<SphereCollider>();
        }
        ((SphereCollider)collider).radius = radius;
        ((SphereCollider)collider).center = Vector2.zero;
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, radius);
    }
}