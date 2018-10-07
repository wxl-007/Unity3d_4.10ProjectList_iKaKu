using UnityEngine;
using System.Collections;

public class LockTargetEffect : MonoBehaviour {
    FightElement skillReleaser;  //释放者
    FightElement lockTarget;     //被锁定者
    float lifeTime;
    public void SetTarget(FightElement target, float lifeTime){
        lockTarget = target;
        this.lifeTime = lifeTime;
        gameObject.SetActive(true);
        StartCoroutine(LockTarget());
    }
    public void SetTarget(FightElement self, FightElement target, float lifeTime){
        this.skillReleaser = self;
        lockTarget = target;
        this.lifeTime = lifeTime;
        gameObject.SetActive(true);
        StartCoroutine(LockTarget());
    }
    public void FallowTarget(FightElement target, float lifeTime){
        lockTarget = target;
        this.lifeTime = lifeTime;
        gameObject.SetActive(true);
        StartCoroutine(Fallow());
    }

    IEnumerator Fallow() {
        float t = 0;
        while (t < lifeTime) {
            t += Time.deltaTime;
            yield return null;
            transform.position = lockTarget.transform.position;
        }
        gameObject.SetActive(false);
    }

    IEnumerator LockTarget() { 
        float t = 0;
        LineRenderer selfLine = GetComponent<LineRenderer>();
        if (!selfLine) {
            throw new System.Exception("Component:LineRenderer is not attached to this gameobject!");
        }
        while (t < lifeTime) {
            t += Time.deltaTime;
            yield return null;
            selfLine.SetPosition(0, skillReleaser.transform.position);
            selfLine.SetPosition(1, lockTarget.transform.position);
        }
        gameObject.SetActive(false);
    }
}