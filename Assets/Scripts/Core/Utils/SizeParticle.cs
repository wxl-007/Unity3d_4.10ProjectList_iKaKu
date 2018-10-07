using UnityEngine;
using System.Collections;

public class SizeParticle : MonoBehaviour {
    public float startSizeMultiple;
    public bool ApplySizeMultiple;

    public float startSpeedMultiple;
    public bool ApplySpeedMultiple;

    public float startLifeTimeMultiple;
    public bool ApplyLifeTimeMultiple;

    void OnDrawGizmos() {
        if (ApplySizeMultiple) {
            ApplySizeMultiple = false;
            SetParticle(gameObject, true);
        }
        if (ApplySpeedMultiple) {
            ApplySpeedMultiple = false;
            SetParticle(gameObject, false, true);
        }
        if (ApplyLifeTimeMultiple) {
            ApplyLifeTimeMultiple = false;
            SetParticle(gameObject, false, false, true);
        }
    }

    void SetParticle(GameObject target, bool size = false, bool speed = false, bool lifetime = false){
        if (target.particleSystem){
            if (size)
                target.particleSystem.startSize *= startSizeMultiple;
            if (speed)
                target.particleSystem.startSpeed *= startSpeedMultiple;
            if (lifetime)
                target.particleSystem.startLifetime *= startLifeTimeMultiple;
        }
        else if (target.particleEmitter) {
            if (size)
                target.particleEmitter.maxSize *= startSizeMultiple;
            if (speed)
                target.particleEmitter.maxEnergy *= startSpeedMultiple;
            //if (lifetime)
            //    target.particleEmitter.ve *= startLifeTimeMultiple;
        }
        for (int i = 0; i < target.transform.childCount; i++){
            SetParticle(target.transform.GetChild(i).gameObject, size, speed, lifetime);
        }
    }
}