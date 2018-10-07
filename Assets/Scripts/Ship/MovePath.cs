#define EDIT
using System;
using UnityEngine;
public class MovePath : MonoBehaviour {
    public Vector3[] pathNode;  //移动结点
    public bool isDebug;        //是否显示线路
    //float curMoveSpeed;         //当前移动速度
    //float minMoveSpeed;         //最小转向移动速度

    #region 移动数据
    [HideInInspector]
    public int nextNode = 0;     //路线锚点

    [HideInInspector]
    public float turnInterval = 0;  /*转向控制*/
    //Vector3 orgDir;
    Vector3 tarDir;
    Quaternion orgRotation;
    Quaternion tarRotation;
    #endregion

    public bool SetMove(Ship target){
        if (pathNode == null || nextNode == pathNode.Length) {
            target.isMoving = false;
            return false;
        }
        if (GetDistance(target.transform.position, pathNode[nextNode]) < 10){
            nextNode++;      //移向下一个锚点
            turnInterval = 0;
            if (nextNode == pathNode.Length){
                target.isMoving = false;
                return false;
            }
        }
        #region 船调头的功能
        if (turnInterval < 1){
            tarDir = pathNode[nextNode] - target.transform.position;
            turnInterval += Time.deltaTime;
            tarDir.y = 0;   //避免Y方向上的移动
            target.transform.forward = Vector3.Slerp(target.transform.forward, tarDir, turnInterval);
        }
        #endregion

        #region 船移动
        target.transform.position += target.transform.forward * Time.deltaTime * target.shipData.moveSpeed;
        target.isMoving = true;
        return true;
        #endregion
    }

    /// <summary>
    /// Before you use this function, check if HaveNextNode()
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextNode() {
        if (nextNode >= pathNode.Length) {
            Debug.LogError("数组越界!");//如果数组越界,说明已经移动到终点位置了.|| 或者数组根本未定义
            throw new System.Exception("未找到下一个结点,请先用HaveNextNode()检查下一个结点是否存在");
        }
        return pathNode[nextNode];
    }

    /// <summary>
    /// 判断是否拥有下一个路径点(是否到达结尾)
    /// </summary>
    /// <returns></returns>
    public bool HaveNextNode() {
        return nextNode < pathNode.Length;
    }

    public Vector3 GetEndNode() {
        return pathNode[pathNode.Length - 1];
    }

    /// <summary>
    /// 获取水平面的距离(不计算高度差)
    /// </summary>
    /// <param name="pos_1">坐标</param>
    /// <param name="pos_2">坐标</param>
    /// <returns></returns>
    float GetDistance(Vector3 pos_1, Vector3 pos_2) {
        Vector2 p_1 = new Vector2(pos_1.x, pos_1.z);
        Vector2 p_2 = new Vector2(pos_2.x, pos_2.z);
        return Vector2.Distance(p_1, p_2);  
    }

    /// <summary>
    /// 插入一个路径点到数组中
    /// </summary>
    /// <param name="index">插入点</param>
    /// <param name="Node">插入的路径点坐标</param>
    public void Insert(int index, Vector3 Node) {
        Vector3[] pathTemp = new Vector3[pathNode.Length + 1];
        Array.Copy(pathNode, pathTemp, index);
        pathTemp[index] = Node;
        Array.Copy(pathNode, index, pathTemp, index + 1, pathNode.Length - index);
        pathNode = pathTemp;
    }

    /// <summary>
    /// 移除给出下标(包含)之后的所有路径点(保留终点)
    /// </summary>
    /// <param name="startIndex">移除起始位置</param>
    public void RemoveToEnd(int startIndex) {
        if (startIndex < 0 || pathNode.Length < startIndex)
            throw new Exception("Index out of array Exception");
        Vector3[] tmpNode = new Vector3[Mathf.Max(startIndex + 1, 2)];
        Array.Copy(pathNode, tmpNode, Mathf.Max(startIndex,2));
        tmpNode[tmpNode.Length - 1] = pathNode[pathNode.Length - 1];
        pathNode = tmpNode;
    }

    /// <summary>
    /// 清空路径点
    /// </summary>
    public void Clear() {
        pathNode = new Vector3[0];
    }

#if EDIT    //发布时,不会编译
    void OnDrawGizmos() {
        if (pathNode != null) {
            for (int i = 0; i < pathNode.Length; i++) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(pathNode[i], 5f);
                Gizmos.color = Color.green;
                if (i + 1 < pathNode.Length)
                    Gizmos.DrawLine(pathNode[i], pathNode[i + 1]);
            }
        }
        if (!isDebug) {
            return;
        }
    }
#endif
}