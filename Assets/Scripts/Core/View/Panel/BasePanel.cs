using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// panel基类.
/// </summary>
public abstract class BasePanel : MonoBehaviour, IRefresh
{
	public enum PanelType{
		/// <summary>
		/// 正常.
		/// </summary>
		Normal = 0,
		/// <summary>
		/// 显示在其他panel上面.
		/// </summary>
		Cover = 1,
	}
    /// <summary>
    /// panel 名字.
    /// </summary>
    public string windowTittle;
    private Animation mAnimation;
    //	private ModuleLimit[] limitList; 

    void Awake()
    {
        //		InitAnimation();
       
    }
    /// <summary>
    /// 初始化动画.
    /// </summary>
    protected void InitAnimation()
    {
        mAnimation = gameObject.GetComponent<Animation>();
        if (mAnimation == null)
        {
            mAnimation = gameObject.AddComponent<Animation>();
            mAnimation.playAutomatically = false;
        }
        AnimationClip clipWindowLeftAppear = GameObject.Instantiate(ResManager.LoadExist("Animations/Common/JhWindowLeftAppear")) as AnimationClip;
        mAnimation.AddClip(clipWindowLeftAppear, "JhWindowLeftAppear");
        AnimationClip clipWindowLeftLeave = GameObject.Instantiate(ResManager.LoadExist("Animations/Common/JhWindowLeftLeave")) as AnimationClip;
        mAnimation.AddClip(clipWindowLeftLeave, "JhWindowLeftLeave");
        AnimationClip clipWindowRightAppear = GameObject.Instantiate(ResManager.LoadExist("Animations/Common/JhWindowRightAppear")) as AnimationClip;
        mAnimation.AddClip(clipWindowRightAppear, "JhWindowRightAppear");
        AnimationClip clipWindowRightLeave = GameObject.Instantiate(ResManager.LoadExist("Animations/Common/JhWindowRightLeave")) as AnimationClip;
        mAnimation.AddClip(clipWindowRightLeave, "JhWindowRightLeave");
    }
    /// <summary>
    /// 获得Panel名字.
    /// </summary>
    /// <returns>
    /// The window title.
    /// </returns>
    public virtual string GetWindowTitle()
    {
        return windowTittle;
    }
    /// <summary>
    /// 刷新Panel.
    /// </summary>
    public virtual void Refresh()
    {
        OnRefresh();
//		UILayout layout = GetComponent<UILayout>();
//		if(layout != null){
//			layout.Adaptive = true;
//		}
    }
    public virtual IEnumerator Init()
    {
        InitAnimation();
        yield break;
    }
	public virtual PanelType GetPanelType()
    {
        return PanelType.Normal;
    }
    public void OnRefresh()
    {
        //		if(limitList == null){
        //			limitList = gameObject.GetComponentsInChildren<ModuleLimit>();
        //		}
        //		foreach(ModuleLimit limit in limitList){
        //			limit.OnRefresh();
        //		}
        BasePanel panel = this;
//        UILayout layout = panel.GetComponent<UILayout>();
//        if (layout != null)
//        {
//            layout.UpdateWHS();
//
//            panel.gameObject.transform.localScale *= layout.m_scale;
//          
//            layout.Adaptive = true;
//        }
    }
    /// <summary>
    /// 从左出现动画.
    /// </summary>
    public IEnumerator PlayLeftAppearAnimation()
    {
        NGUITools.SetActive(gameObject, true);
        mAnimation.Play("JhWindowLeftAppear");
        yield break;
        //        ActiveAnimation.Play(an, "JhWindowLeftAppear", AnimationOrTween.Direction.Forward, AnimationOrTween.EnableCondition.DoNothing, AnimationOrTween.DisableCondition.DoNotDisable);
    }
    /// <summary>
    /// 向左离开动画.
    /// </summary>
    public IEnumerator PlayLeftLeaveAnimation()
    {
        mAnimation.Play("JhWindowLeftLeave");
        while (mAnimation.isPlaying)
        {
            yield return 1;
        }
        NGUITools.SetActive(gameObject, false);
        //        Animation an = gameObject.GetComponent<Animation>(); ActiveAnimation.Play(an, "JhWindowLeftLeave", AnimationOrTween.Direction.Forward, AnimationOrTween.EnableCondition.EnableThenPlay, AnimationOrTween.DisableCondition.DisableAfterForward);

    }
    /// <summary>
    /// 从右出现动画.
    /// </summary>
    public IEnumerator PlayRightAppearAnimation()
    {
        NGUITools.SetActive(gameObject, true);
        mAnimation.Play("JhWindowRightAppear");
        yield break;
        //        Animation an = gameObject.GetComponent<Animation>(); ActiveAnimation.Play(an, "JhWindowRightAppear", AnimationOrTween.Direction.Forward, AnimationOrTween.EnableCondition.DoNothing, AnimationOrTween.DisableCondition.DoNotDisable);
    }
    /// <summary>
    /// 向右离开动画.
    /// </summary>
    public IEnumerator PlayRightLeaveAnimation()
    {
        mAnimation.Play("JhWindowRightLeave");
        while (mAnimation.isPlaying)
        {
            yield return 1;
        }
        NGUITools.SetActive(gameObject, false);
        //        Animation an = gameObject.GetComponent<Animation>();
        //        ActiveAnimation.Play(an, "JhWindowRightLeave", AnimationOrTween.Direction.Forward, AnimationOrTween.EnableCondition.EnableThenPlay, AnimationOrTween.DisableCondition.DisableAfterForward);

    }
    public void UnActiveGameObject(AnimationEvent event1)
    {
        NGUITools.SetActive(gameObject, false);
    }
    public static void DelAllItem(UIGrid grid)
    {
        int count = grid.transform.GetChildCount();
        List<GameObject> objList = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            objList.Add(grid.transform.GetChild(i).gameObject);

        }
        for (int i = 0; i < count; i++)
        {
            NGUITools.Destroy(objList[i]);
        }
    }
    public void ReturnBack()
    {
        PanelManager.GetInstance().ShowPreviousWindow();
    }
    public bool IsPlayAnimation()
    {
        return mAnimation.isPlaying;
    }
    public void ReturnBackNoPush()
    {
        PanelManager.GetInstance().ShowPreviousPanelNoPush(this);
    }
}

