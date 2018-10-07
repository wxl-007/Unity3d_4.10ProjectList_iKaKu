#define Editor
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AutoMesh : MonoBehaviour{
    public Vector2 externt;             //面片宽高
    public MeshRenderMode rendermode;   //面片法线方向

	void Start () {
        GetComponent<MeshFilter>().mesh = GetMesh();
	}

    Mesh GetMesh() {
        Mesh m = new Mesh();
        switch (rendermode) { 
            case MeshRenderMode.normal_Up:
                m.vertices = new Vector3[]{
                    new Vector3(-externt.x,.1f,-externt.y),
                    new Vector3(-externt.x,.1f,externt.y),
                    new Vector3(externt.x,.1f,externt.y),
                    new Vector3(externt.x,.1f,-externt.y)
                };
                break;
            case MeshRenderMode.normal_Back:
                m.vertices = new Vector3[]{
                    new Vector3(-externt.x,-externt.y,0),
                    new Vector3(-externt.x,externt.y,0),
                    new Vector3(externt.x,externt.y,0),
                    new Vector3(externt.x,-externt.y,0)
                };
                break;
            case MeshRenderMode.normal_Left:
                m.vertices = new Vector3[]{
                    new Vector3(0,-externt.y,externt.x),
                    new Vector3(0,externt.y,externt.x),
                    new Vector3(0,externt.y,-externt.x),
                    new Vector3(0,-externt.y,-externt.x)
                };
                break;
            case MeshRenderMode.normal_Right:
                m.vertices = new Vector3[]{
                    new Vector3(0,-externt.y,-externt.x),
                    new Vector3(0,externt.y,-externt.x),
                    new Vector3(0,externt.y,externt.x),
                    new Vector3(0,-externt.y,externt.x)
                };
                break;
        }
        m.uv = new Vector2[]{
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0)
        };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        return m;
    }

    //public void SetPosition(Vector3 start,Vector3 end) {
    //    float dis = Vector3.Distance(start, end) * .5f;
    //    transform.forward = end - start;
    //    transform.position = (start + end) * .5f;
    //    transform.localScale = new Vector3(1, 1, dis);
    //}
#if Editor
    #region 编译删除
    MeshRenderMode tmp_mrm;
    Vector2 tmp_externt;
    void OnDrawGizmos() {
        if (Application.isPlaying)
            return;
        print("编译删除");
        if (tmp_externt != externt || tmp_mrm != rendermode || !GetComponent<MeshFilter>().sharedMesh){
            tmp_externt = externt;
            tmp_mrm = rendermode;
            GetComponent<MeshFilter>().sharedMesh = GetMesh();
        }
    }
    //public Transform A;
    //public Transform B;
    #endregion
#endif
}
public enum MeshRenderMode {
    normal_Up,
    normal_Back,
    normal_Left,
    normal_Right
}