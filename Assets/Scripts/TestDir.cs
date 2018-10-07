using UnityEngine;
using System.Collections;

public class TestDir : MonoBehaviour {
    public Transform cannon;
    public Transform A_0;

    public Transform target;
    public Transform B_0;

    public Bounds b;
    public bool GetBounds;
    public FightElement fe;
    public float radius = .1f;
    public bool useMatrix = false;


    public Vector3 left;
    public Vector3 right;
    void OnDrawGizmos() {
        if (cannon && A_0 && target){
            Vector3 dir = transform.worldToLocalMatrix.MultiplyVector(target.position - cannon.position);
            Debug.DrawLine(cannon.position, cannon.position + dir, Color.magenta);
            Debug.DrawLine(cannon.position, cannon.position + left, Color.magenta * .5f);
            Debug.DrawLine(cannon.position, cannon.position + right, Color.magenta * .5f);

            dir = target.position - cannon.position;
            Debug.DrawLine(cannon.position, cannon.position + dir, Color.cyan);
            //left
            dir = transform.localToWorldMatrix.MultiplyVector(left);
            Debug.DrawLine(cannon.position, cannon.position + dir, Color.cyan * .5f);
            //right
            dir = transform.localToWorldMatrix.MultiplyVector(right);
            Debug.DrawLine(cannon.position, cannon.position + dir, Color.cyan * .5f);
        }
        print("删除本脚本");

        //if (A_0 && A && B && B_0){
        //    B_0.localPosition = B.rotation * (A_0.position - A.position);//A.worldToLocalMatrix.MultiplyPoint3x4(A_0.position);
        //}

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.localToWorldMatrix.MultiplyPoint3x4(A.localPosition));


        //return;
        //if (collider){
            if (GetBounds || b == null){
                GetBounds = false;
                b = collider.bounds;
            }
        //    DrawBounds(transform, b);
        //}
        //Gizmos.DrawSphere(Random.Range(collider.bounds.IntersectRay))

        //Gizmos.DrawSphere(A.position, 1);
        //Gizmos.DrawSphere(B.position, 1);
        //Gizmos.DrawLine(A.position, B.position);
        //Vector3 dir = B.position - A.position;
        //dir.y = 0;

        //Gizmos.DrawLine(A.position, A.position + dir * 2);

        if (fe) {
            for (int i = 0; i < 100;i++)
                Gizmos.DrawSphere(GetPointInBounds(fe), radius);
        }
    }

    public static void DrawBounds(Transform belongTo,Bounds aabb){
        Vector3 externts = aabb.extents;
        Debug.Log("externts = " + externts + "      aabb.center = " + aabb.center);
        Vector3 originPos = belongTo.position;
        Quaternion rotation = belongTo.rotation;
        Matrix4x4 matrix = belongTo.localToWorldMatrix;
        Vector3 leftTopF = originPos + rotation * new Vector3(-externts.x, externts.y, externts.z); /*matrix.MultiplyPoint3x4(new Vector3(-externts.x, externts.y, externts.z))*/;
        Vector3 leftTopB = originPos + rotation * new Vector3(-externts.x, externts.y, -externts.z);
        Vector3 leftDownF = originPos + rotation * new Vector3(-externts.x, -externts.y, externts.z);
        Vector3 leftDownB = originPos + rotation * new Vector3(-externts.x, -externts.y, -externts.z);
        Vector3 rightTopF = originPos + rotation * new Vector3(externts.x, externts.y, externts.z);
        Vector3 rightTopB = originPos + rotation * new Vector3(externts.x, externts.y, -externts.z);
        Vector3 rightDownF = originPos + rotation * new Vector3(externts.x, -externts.y, externts.z);
        Vector3 rightDownB = originPos + rotation * new Vector3(externts.x, -externts.y, -externts.z);
        //左边
        Debug.DrawLine(leftTopF, leftTopB);
        Debug.DrawLine(leftTopF, leftDownF);
        Debug.DrawLine(leftDownB, leftTopB);
        Debug.DrawLine(leftDownB, leftDownF);
        //中间
        Debug.DrawLine(leftTopF, rightTopF);
        Debug.DrawLine(leftTopB, rightTopB);
        Debug.DrawLine(leftDownF, rightDownF);
        Debug.DrawLine(leftDownB, rightDownB);
        //右边
        Debug.DrawLine(rightTopF, rightTopB);
        Debug.DrawLine(rightTopF, rightDownF);
        Debug.DrawLine(rightDownB, rightTopB);
        Debug.DrawLine(rightDownB, rightDownF);
    }
    /// <summary>
    /// 获取他妈的碰撞盒之内的一个他妈的随机点,擦!
    /// </summary>
    /// <param name="target">拥有他妈的碰撞盒的他妈的战斗元素</param>
    /// <returns></returns>
    public Vector3 GetPointInBounds(FightElement target){
        Vector3 externts = target.externts;
        return target.transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(Random.Range(-externts.x, externts.x), externts.y, Random.Range(-externts.z, externts.z)));
        return target.transform.rotation * (new Vector3(Random.Range(-externts.x, externts.x), Random.Range(-externts.y, externts.y), Random.Range(-externts.z, externts.z)) + target.transform.worldToLocalMatrix.MultiplyPoint3x4(target.collider.bounds.center + target.transform.position));
    }
}