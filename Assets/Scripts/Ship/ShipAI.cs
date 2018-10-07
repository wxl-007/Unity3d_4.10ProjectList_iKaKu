//#define Editor
using UnityEngine;

/// <summary>
/// 战舰 AI(控制战船路径)
/// </summary>
public class ShipAI : MonoBehaviour {
    public MovePath mainPath;       //主路径(编辑器赋值)
    public MovePath assistantPath;  //副路径
    LayerMask pathLayer = 1 << 12;

    public int radius;              //自身所占范围半径
    void Start() {
        radius = GetComponent<BaseElement>().boundOfMove;
        GameObject tmp_assistant = new GameObject("AssistantPath" + name);
        tmp_assistant.transform.parent = transform;
        assistantPath = tmp_assistant.AddComponent<MovePath>();
    }

    /// <summary>
    /// 更改主路径点
    /// </summary>
    /// <param name="endPosition"></param>
    public void GetMainPath(Vector3 endPosition){
        CalculatePath(mainPath, transform.position, endPosition);
    }

    /// <summary>
    /// 根据目标点,计算副路径
    /// </summary>
    /// <param name="endPosition">目的地</param>
    public void GetAssistantPath(Vector3 endPosition){
        assistantPath.pathNode = new Vector3[2] { transform.position, endPosition };
        assistantPath.nextNode = 0;
        assistantPath.turnInterval = 0;
		O = 0;
        CalculatePath(assistantPath, transform.position, endPosition);
    }

    /// <summary>
    /// 根据给出的目标计算出路径
    /// </summary>
    /// <param name="target">目标</param>
    public void GetAssistantPath(GameObject target) {
        Vector3 endPosition = target.transform.position;
        RaycastHit[] hits = Physics.RaycastAll(new Ray(transform.position, target.transform.position - transform.position));
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider == target.collider) {
                endPosition = hits[i].point;
                break;
            }
        }
        assistantPath.pathNode = new Vector3[2] { transform.position, endPosition };
        assistantPath.nextNode = 0;
        assistantPath.turnInterval = 0;
		O = 0;
        CalculatePath(assistantPath, transform.position, endPosition);
    }

    RaycastHit hit;
    int O = 0;      //复杂度累计(超过3个点之后不计算.计算个蛋球).
    void CalculatePath(MovePath path, Vector3 start, Vector3 end){
        if (O > 3) return;
        O++;
        collider.enabled = false;
        if (Physics.SphereCast(new Ray(start, end - start), radius, out hit, GetDistace(start, end))){
            if (hit.collider.bounds.Contains(end)) {
                collider.enabled = true;
                return;
            }
            path.Insert(path.pathNode.Length - 1, GetPoit(path, hit, end - start));//插入倒数第二个位置一个计算好的点
            CalculatePath(path, path.pathNode[path.pathNode.Length - 2], path.pathNode[path.pathNode.Length - 1]);//计算下一个点,直到没有障碍物
        }else{
            for (int i = 0; i < path.pathNode.Length - 1; i++){
                if (!Physics.Raycast(path.pathNode[i], path.pathNode[path.pathNode.Length - 1] - path.pathNode[i], GetDistace(path.pathNode[i], path.pathNode[path.pathNode.Length - 1]))){
                    path.RemoveToEnd(i + 1);
                    break;
                }
            }
        }
        collider.enabled = true;
    }

    float GetDistace(Vector3 startPos, Vector3 endPos) {
        Vector2 stp = new Vector2(startPos.x, startPos.z);
        Vector2 edp = new Vector2(endPos.x, endPos.z);
        return Vector2.Distance(stp, edp) - .01f;
    }

    /// <summary>
    /// 根据给出的障碍物,和移动方向计算出下一个路径点
    /// </summary>
    /// <param name="hit">障碍物</param>
    /// <param name="dir">当前点到终点方向</param>
    /// <returns>路径点</returns>
    Vector3 GetPoit(MovePath path, RaycastHit hit, Vector3 dir){
        #region CurCode
        Vector3 point = Vector3.zero;
        Vector3 hitCenter = new Vector3(hit.transform.position.x, path.pathNode[0].y, hit.transform.position.z);
        //法向量(根据其特征判断左侧还是右侧寻找路径点)(特征:垂直于水平面/平行于Y轴)
        Vector3 normal = Vector3.Cross(dir, hitCenter - hit.point);

        //dir和normal的法向量(路径经过侧向)
        Vector3 step = Vector3.Cross(dir, normal).normalized;
        //获取最外围的一个点
        if (hit.collider.GetComponent<BaseElement>())
            point = hit.collider.transform.position + step * (hit.collider.GetComponent<BaseElement>().boundOfMove + radius);
        Collider[] nearObjs = Physics.OverlapSphere(point, radius);
        if (nearObjs.Length > 0){  //周围还有其他障碍物
            int stepBy = 1;
            while (stepBy < 300){
                stepBy++;
                if (Physics.OverlapSphere(point + step * stepBy, radius).Length == 0){
                    point += stepBy * step;
                    break;
                }else if (Physics.OverlapSphere(point - step * stepBy, radius).Length == 0){
                    point -= stepBy * step;
                    break;
                }
            }
        }
        point.y = path.pathNode[0].y;
        return point;
        #endregion

        #region OrgCode_2
        //Vector3 point = Vector3.zero;
        //Vector3 hitCenter = new Vector3(hit.transform.position.x, path.pathNode[0].y, hit.transform.position.z);
        ////法向量(根据其特征判断左侧还是右侧寻找路径点)(特征:垂直于水平面/平行于Y轴)
        //Vector3 normal = Vector3.Cross(dir, hitCenter - hit.point);

        ////dir和normal的法向量(路径经过侧向)
        //Vector3 step = Vector3.Cross(dir, normal).normalized;
        ////获取最外围的一个点
        //point = hit.point + step * radius;
        //if (hit.collider.GetComponent<BaseElement>())
        //    point = hit.collider.transform.position + step * (hit.collider.GetComponent<BaseElement>().boundOfMove + radius);
        //else
        //    print("Error");

        ////Collider[] nearObj = Physics.OverlapSphere(point, 50);
        ////if (nearObj != null && nearObj.Length != 0){
        ////    print("路径点半径50米内有其他障碍物");
        ////    if (nearObj.Length == 1 && nearObj[0] == collider) { }
        ////}
        //point.y = path.pathNode[0].y;
        //if (!Application.isPlaying){
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawLine(hit.collider.transform.position, point);
        //}
        //return point;
        #endregion

        #region OrgCode_1
        //Vector3 point = Vector3.zero;
        //if (hit.collider.GetComponent<Ship>()) {
        //    if (hit.collider.GetComponent<Ship>().isMoving) {
        //        return hit.collider.transform.position - hit.collider.transform.forward * (radius << 1);
        //    }
        //}
        //if (hit.collider.GetComponent<BaseElement>()) { }
        //Vector3 hitCenter = new Vector3(hit.transform.position.x, path.pathNode[0].y, hit.transform.position.z);
        ////法向量(根据其特征判断左侧还是右侧寻找路径点)(特征:垂直于水平面/平行于Y轴)
        //Vector3 normal = Vector3.Cross(dir, hitCenter - hit.point);

        ////dir和normal的法向量(路径经过侧向)
        //Vector3 step = Vector3.Cross(dir, normal).normalized;
        ////获取最外围的一个点
        //point = hit.collider.ClosestPointOnBounds(step * 20 + hit.point) + step * 100;

        ////Collider[] nearObj = Physics.OverlapSphere(point, 50);
        ////if (nearObj != null && nearObj.Length != 0){
        ////    print("路径点半径50米内有其他障碍物");
        ////    if (nearObj.Length == 1 && nearObj[0] == collider) { }
        ////}
        //point.y = path.pathNode[0].y;
        //return point;
        #endregion
    }

#if Editor
    void OnDrawGizmos() {
        if (mainPath == null) {
            //animation.cullingType = AnimationCullingType.BasedOnRenderers;
            //AnimationCurve t = new AnimationCurve();
            //t.keys[0].time = .3f;
            GameObject pathMain = new GameObject("MainPath_" + name);
            pathMain.transform.parent = transform;
            mainPath = pathMain.AddComponent<MovePath>();
        }
    }
#endif
}