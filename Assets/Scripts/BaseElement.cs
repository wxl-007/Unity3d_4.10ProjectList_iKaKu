//#define EDIT
using UnityEngine;
using System.Collections;

public class BaseElement : MonoBehaviour {
    public LayerMask EnemyLayer;    //敌人所在层
    public Team team;
    public string PlayerID;
    public string GetPlayerID() { 
        return PlayerID;
    }
    public int boundOfMove;
    public void SetPlayerID(string id){
        PlayerID = id;
    }

    /// <summary>
    /// 设置阵营所属
    /// 改变自身所在层,自动计算敌人层.
    /// </summary>
    /// <param name="team">阵营</param>
    public void SetTeam(Team team){
        this.team = team;
        switch (team){
            case Team.Alliance:
                ChangeChildLayer(transform, LayerMask.NameToLayer("Alliance"));
                EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");
                break;
            case Team.Enemy:
                ChangeChildLayer(transform, LayerMask.NameToLayer("Enemy"));
                EnemyLayer = (1 << LayerMask.NameToLayer("Alliance")) + (1 << LayerMask.NameToLayer("Self"));
                break;
            case Team.Self:
                ChangeChildLayer(transform, LayerMask.NameToLayer("Self"));
                EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");
                break;
        }
    }
    void ChangeChildLayer(Transform father,int layer) {
        if (father.collider && father.name != "AllComponents"){
            father.gameObject.layer = layer;
        }
        for (int i = 0; i < father.childCount; i++){
            ChangeChildLayer(father.GetChild(i), layer);
        }
    }

#if EDIT
    [HideInInspector]
    public Team preTeam;
    public void OnDrawGizmosSelected() {
        print("注释掉最上方的#define EDIT");
        if (preTeam != team) {
            preTeam = team;
            print("阵营改变");
            SetTeam(team);
        }
    }
#endif
}
