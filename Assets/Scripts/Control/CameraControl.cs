#define EDIT
using UnityEngine;
using System.Collections;

/// <summary>
/// 此脚本绑定在摄像机上
/// </summary>
public class CameraControl : MonoBehaviour {
    delegate void PlatForm();
    PlatForm OnUpdate;
    public delegate void SetCameraMoveTo(Transform target);
    public static SetCameraMoveTo CameraMoveTo;

    bool isMouseClick = true;

    Vector3 DownPosition;
    Vector3 UpPosition;
    RaycastHit hit;
    /*
     * 按下:
     *      记录位置
     * 释放:
     *      点击事件?(按下释放时间间隔/按下释放位置)
     *          水面?
     *          战斗单位?
     *              岛?
     *              船?
     *      拖动事件?
     *      
     */

    #region Data of Camera
    public float MaxHeight = 80;    //视野最大高度
    public Quaternion MaxRotation;  //视野最大方向
    public float MaxViewField = 60; //视野最大视角

    public float MinHeight = 14;    //视野最小高度
    public Quaternion MinRotation;  //视野最小方向
    public float MinViewField = 44; //视野最小视角

    public float sensitive = 0;
    #endregion


    #region 战船指定目标效果
    public LockTargetEffect lockTarget;
    public LockTargetEffect showTarget;
    #endregion
#if EDIT
    public bool mouseDown;
    public bool touchDown;

#elif UNITY_IPHONE || UNITY_ANDROID //|| !EDIT
    public float orgDis;
    public float curDis;
#endif


    void Start(){
        CameraMoveTo = MoveTo;
#if EDIT
        OnUpdate = PC;
#elif UNITY_ANDROID || UNITY_IPHONE || !EDIT
        OnUpdate = TouchPad;
#endif
        Zoom(0.5f);
    }

    public float Axis;
    void Update(){
        OnUpdate();
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

#if EDIT
    Vector3 orgPos;
    Vector3 curPos;
    void PC(){
        if (Input.GetMouseButtonDown(0)){
            isMouseClick = true;
            mouseDown = true;
            orgPos = Input.mousePosition;
            curPos = orgPos;
            DownPosition = orgPos;
        }else if (Input.GetMouseButtonUp(0)){
            mouseDown = false;
            if (isMouseClick) {
                if (Physics.Raycast(camera.ScreenPointToRay(DownPosition), out hit)) {
                    if (hit.collider.GetComponent<FightElement>()){
                        if (ShipController.CommaderShip.SetTarget(hit.collider.GetComponent<FightElement>())) {
                            lockTarget.SetTarget(ShipController.CommaderShip, hit.collider.GetComponent<FightElement>(), 30);
                            showTarget.FallowTarget(hit.collider.GetComponent<FightElement>(), 35f);
                        }
                    }else{
                        ShipController.CommaderShip.SetPosition(hit.point);
                        lockTarget.gameObject.SetActive(false);
                        showTarget.gameObject.SetActive(false);
                    }
                }
            }
        }

        if (mouseDown){
            StopAllCoroutines();
            //Time.timeScale = 1;
            curPos = Input.mousePosition;
            if (curPos != orgPos)
                CameraDrag(orgPos - curPos);
            orgPos = curPos;
        }

        if (Input.GetAxis("ScrollWheel") != 0){
            StopAllCoroutines();
            //Time.timeScale = 1;
            Zoom(Input.GetAxis("ScrollWheel") * .3f);
        }
    }
#elif UNITY_ANDROID || UNITY_IPHONE //|| !EDIT
    void TouchPad() {
        if (Input.touchCount == 0)
            return;
        if (Input.touchCount == 1) {        //===========单指触控===========
            isMouseClick = true;
            if (Input.GetTouch(0).phase == TouchPhase.Moved) {       //手指移动.
                StopAllCoroutines();
                //if (Input.GetTouch(0).deltaPosition.magnitude < .1f) {
                //    return;
                //}
                CameraDrag(-Input.GetTouch(0).deltaPosition * .8f);
            }else if(Input.GetTouch(0).phase == TouchPhase.Ended){   //手指释放
                if (isMouseClick) {
                    if (Physics.Raycast(camera.ScreenPointToRay(Input.GetTouch(0).position), out hit)) {
                        if (hit.collider.GetComponent<FightElement>()) { //-点击到场景中的战斗元素.
                            ShipController.CommaderShip.SetTarget(hit.collider.GetComponent<FightElement>());
                        }else{      //-------------------------------------点击到场景中空白处.
                            ShipController.CommaderShip.SetPosition(hit.point);
                        }
                    }
                }
            }
        }else if (Input.touchCount == 2) {  //===========双指触控===========
            isMouseClick = false;   //双指按下,点击取消
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began) {
                StopAllCoroutines();
                //Time.timeScale = 1;
                orgDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                curDis = orgDis;
            }else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) {
                curDis = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                Zoom(curDis / orgDis - 1);
                orgDis = curDis;
            }
        }
    }
#endif

    void Zoom(float deltaZoom){
        isMouseClick = false;
        sensitive += deltaZoom;

        sensitive = Mathf.Clamp01(sensitive);
        camera.fieldOfView = Mathf.Lerp(MaxViewField, MinViewField, sensitive);
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(MaxHeight, MinHeight, sensitive), transform.position.z);
        transform.rotation = Quaternion.Lerp(MaxRotation, MinRotation, sensitive);
    }

    void CameraDrag(Vector3 deltaPos){
        isMouseClick = false;   //如果有拖动,点击事件取消.
        transform.position += new Vector3(deltaPos.x, 0, deltaPos.y) * Mathf.Lerp(1, 0.2f, sensitive);
    }

    /// <summary>
    /// 设定观察战舰
    /// </summary>
    /// <param name="ship"></param>
    void MoveTo(Transform ship)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothMoveTo(ship.position));
    }

    IEnumerator SmoothMoveTo(Vector3 Pos)   //平滑移动至目标点(观察)
    {
        Vector3 targetPosition = new Vector3(0, 0, -275) + Pos;
        float t = 0;
        //Time.timeScale = .3f;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPosition.x, t), Mathf.Lerp(transform.position.y, MinHeight, t), Mathf.Lerp(transform.position.z, targetPosition.z, t));
            transform.rotation = Quaternion.Lerp(transform.rotation, MinRotation, t);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, MinViewField, t);
            sensitive = Mathf.Lerp(sensitive, 1, t);
            yield return null;
        }
        //Time.timeScale = 1;
    }

    /// <summary>
    /// 己方主舰受到伤害后摄像机震动
    /// </summary>
    /// <param name="shakeStrength"></param>
    /// 震动幅度
    /// <param name="rate"></param>
    /// 震动频率
    /// <param name="shakeTime"></param>
    /// 震动时长
    /// <returns></returns>
    IEnumerator ShakeCamera(float shakeStrength = 0.2f,float rate = 14,float shakeTime = 0.4f) {
        float t = 0;
        float speed = 1 / shakeTime;
        Vector3 orgPosition = transform.position;
        while (t < 1) {
            t += Time.deltaTime * speed;
            transform.position = orgPosition + new Vector3(Mathf.Sin(rate * t), Mathf.Cos(rate * t), 0) * Mathf.Lerp(shakeStrength, 0, t);
            yield return null;
        }
        transform.position = orgPosition;
    }

#if EDIT
    public bool GetMinDataOfCamera;
    public bool GetMaxDataOfCamera;

    public bool isShowHorizontalPlane;
    public int dev = 10;
    public Vector3 OffsetPosition;
    public int WideAndHeight;
    public Color colr;
    void OnDrawGizmos() {
        if (isShowHorizontalPlane) {
            if (OffsetPosition.y != 0)
                OffsetPosition.y = 0;
            for (int i = 0; i < WideAndHeight; i += dev){
                Debug.DrawLine(OffsetPosition + new Vector3(i, 0, 0), OffsetPosition + new Vector3(i, 0, WideAndHeight), colr);
                Debug.DrawLine(OffsetPosition + new Vector3(0, 0, i), OffsetPosition + new Vector3(WideAndHeight, 0, i), colr);
            }
        }
        //print("编译时,请将此脚本最上方的#define EDIT注释掉");
        if (GetMinDataOfCamera) {
            GetMinDataOfCamera = false;
            MinHeight = transform.position.y;
            MinRotation = transform.rotation;
            MinViewField = camera.fieldOfView;
        }
        else if (GetMaxDataOfCamera) {
            GetMaxDataOfCamera = false;
            MaxHeight = transform.position.y;
            MaxRotation = transform.rotation;
            MaxViewField = camera.fieldOfView;
        }
    }
#endif
}