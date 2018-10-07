using UnityEngine;
using System.Collections;

public interface IFire{
    float fireInterval { set; get; }
    float fireRange { set; get; }
    bool canFire { set; get; }
    void OnFire(Transform belongTo, FightElement target);
    IEnumerator RotateTo(Vector3 targetPos);
    bool CanFire(FightElement target);
}
