using UnityEngine;
using System.Collections;

public class ParticleFX : MonoBehaviour {

    public float lifeTime = .3f;
    float lifetime = 0;
    void OnEnable() {
        lifetime = 0;
    }

	void Update () {
        lifetime += Time.deltaTime;
        if (lifetime > lifeTime) {
            gameObject.SetActive(false);
        }
	}

#if UNITY_EDITOR
    void OnDrawGizmos(){
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).particleEmitter && transform.GetChild(i).particleEmitter.GetComponent<ParticleAnimator>().autodestruct){
                transform.GetChild(i).particleEmitter.GetComponent<ParticleAnimator>().autodestruct = false;
            }
        }
    }
#endif
}