using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PanelManager : MonoBehaviour
{

    private static PanelManager instance;
    /// <summary>
    /// panel集合.
    /// </summary>
    private Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();
    public Dictionary<string, BasePanel> Panels
    {
        get
        {
            return panels;
        }
    }
    /// <summary>
    /// The panel history.
    /// </summary>
    private Stack<BasePanel> panelHistory = new Stack<BasePanel>();
    public Stack<BasePanel> PanelHistory
    {
        get
        {
            return panelHistory;
        }
    }
    /// <summary>
    ///  上一个panel名字.
    /// </summary>
    string previousPanelName;
    //	/// <summary>
    //	/// 当前panel名字.
    //	/// </summary>
    //	string currentPanelName;
    /// <summary>
    /// 当前panel.
    /// </summary>
    private BasePanel currentPanel;
    /// <summary>
    /// panel父物体.
    /// </summary>
    public GameObject baseWindow;
    public UICamera uicamera;

    private BasePanel tmpPanel;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        baseWindow = GameObject.Find("BaseWindow");
        GameObject camera = GameObject.Find("Camera");
        uicamera = camera.GetComponent<UICamera>();

    }
    public void InitPanel()
    {
        //		NGUITools.SetActive( CreatePanel(Config.StoryPanel),false);
        //		NGUITools.SetActive( CreatePanel(FightFactory.TUI_TU_PVE_RES),false);
    }
    public void OnLogin()
    {
        ShowPanel(Config.PlayerPanel);
        panelHistory.Clear();
        DeletePanel(Config.LoginPanel);
    }
    /// <summary>
    /// 显示panel.
    /// </summary>
    /// <param name='windowName'>
    /// Window name.
    /// </param>
    public void ShowPanel(string panelName)
    {
        StartCoroutine(ShowPanelCoroutine(panelName));
       
    }
    private IEnumerator ShowPanelCoroutine(string panelName)
    {
        if (tmpPanel != null)
        {
            NGUITools.SetActive(tmpPanel.gameObject, false);
            tmpPanel = null;
        }
        //等待panel创建.
        yield return StartCoroutine(CreatePanel(panelName));
        BasePanel panel = GetPanel(panelName);
        if (panel != currentPanel)
        {
            BasePanel tempPanel = null;
			BasePanel perPanel = null;
            if (panelHistory.Count > 0)
            {
                tempPanel = panelHistory.Peek();
            }
            if (currentPanel != null)
            {
				if(panel.GetPanelType() == BasePanel.PanelType.Normal){
	                HidePanel(currentPanel);
	                StartCoroutine( currentPanel.PlayLeftLeaveAnimation());
					if(currentPanel.GetPanelType() == BasePanel.PanelType.Cover){
		                HidePanel(currentPanel);
						StartCoroutine( currentPanel.PlayLeftLeaveAnimation());
						BasePanel tPanel = panelHistory.Peek();
		                if(tPanel != null){
							HidePanel(tPanel);
							StartCoroutine(tPanel.PlayLeftLeaveAnimation());
						}
					}
				}
				if (tempPanel != panel)
                {
                    panelHistory.Push(currentPanel);
                }
				perPanel = currentPanel;
            }


            currentPanel = panel;
            //			currentPanelName = panelName;
            NGUITools.SetActive(currentPanel.gameObject, true);
            StartCoroutine( currentPanel.PlayLeftAppearAnimation());
			
        }
        currentPanel.Refresh();
    }
    /// <summary>
    /// 显示panel不堆栈.
    /// </summary>
    /// <param name='panelName'>
    /// Panel name.
    /// </param>
    public void ShowPanelNotPush(string panelName)
    {
        StartCoroutine(ShowPanelNotPushCoroutine(panelName));
    }
    private IEnumerator ShowPanelNotPushCoroutine(string panelName)
    {
        //等待panel创建.
        yield return StartCoroutine(CreatePanel(panelName));
        BasePanel panel = GetPanel(panelName);
        panel.Refresh();
        StartCoroutine(panel.PlayLeftAppearAnimation());
        tmpPanel = panel;
        NGUITools.SetActive(currentPanel.gameObject, false);
    }
    public void ShowPreviousPanelNoPush(BasePanel panel)
    {
        HidePanel(panel);
        StartCoroutine( panel.PlayRightLeaveAnimation());
        NGUITools.SetActive(currentPanel.gameObject, true);
//        currentPanel.PlayRightAppearAnimation();
        currentPanel.Refresh();
    }
	/// <summary>
	/// 创建一个panel，显示在原来的panel上面.
	/// </summary>
	public void ShowPanelCover(string panelName){
		StartCoroutine(ShowPanelCoverCoroutine(panelName));
	}
	private IEnumerator ShowPanelCoverCoroutine(string panelName)
    {
        //等待panel创建.
        yield return StartCoroutine(CreatePanel(panelName));
        BasePanel panel = GetPanel(panelName);
		NGUITools.SetActive(panel.gameObject, true);
        panel.Refresh();
        StartCoroutine( panel.PlayLeftAppearAnimation());
        tmpPanel = panel;
    }
    /// <summary>
    /// 创建一个panel.
    /// </summary>
    /// <returns>
    /// The panel.
    /// </returns>
    /// <param name='panelName'>
    /// Panel name.
    /// </param>
    public IEnumerator CreatePanel(string panelName)
    {
        BasePanel panel = null;
        if (panels.TryGetValue(panelName, out panel))
        {
            //			NGUITools.SetActive(panel.gameObject, true);
        }
        else
        {
            ResGameObject obj = new ResGameObject();
            yield return StartCoroutine(ResManager.GetInstance().Load(Config.PanelPath + panelName, obj));
            GameObject gameObj = NGUITools.AddChild(baseWindow, obj.obj as GameObject);
            panel = gameObj.GetComponent<BasePanel>();
            yield return StartCoroutine(panel.Init());
            panels.Add(panelName, panel);
        }
    }
    /// <summary>
    /// 删除一个panel.
    /// </summary>
    /// <param name='panelName'>
    /// Panel name.
    /// </param>
    private void DeletePanel(string panelName)
    {
        BasePanel panel = null;
        if (panels.TryGetValue(panelName, out panel))
        {
            panels.Remove(panelName);
            NGUITools.Destroy(panel.gameObject);
        }
    }

    public void HidePanel(BasePanel go)
    {
        //		NGUITools.SetActive(go, false);
        //		go.PlayLeftLeaveAnimation();
        Resources.UnloadUnusedAssets();
    }
    /// <summary>
    /// 获得panel.
    /// </summary>
    /// <returns>
    /// The panel.
    /// </returns>
    /// <param name='windowName'>
    /// Window name.
    /// </param>
    public BasePanel GetPanel(string windowName)
    {
        if (panels.ContainsKey(windowName))
        {
            return panels[windowName];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 刷新当前正在显示的panel.
    /// </summary>
    public void RefrushCurrentPanel()
    {
        if (currentPanel != null && currentPanel.gameObject.activeInHierarchy)
        {
            currentPanel.Refresh();
        }
        if (tmpPanel != null && tmpPanel.gameObject.activeInHierarchy)
        {
            tmpPanel.Refresh();
        }
    }
    /// <summary>
    /// 刷新一个panel.
    /// </summary>
    /// <param name='windowName'>
    /// Window name.
    /// </param>
    public void RefrushPanel(string windowName)
    {
        BasePanel panel = GetPanel(windowName);
        if (panel != null && panel.gameObject.activeInHierarchy)
        {
            panel.Refresh();
        }
    }
    /// <summary>
    /// 返回上一个panel.
    /// </summary>
    public void ShowPreviousWindow()
    {
        HidePanel(currentPanel);
        StartCoroutine( currentPanel.PlayRightLeaveAnimation());
        currentPanel = panelHistory.Pop();
        NGUITools.SetActive(currentPanel.gameObject, true);
        StartCoroutine( currentPanel.PlayRightAppearAnimation());
        currentPanel.Refresh();

    }

    public static PanelManager GetInstance()
    {
        if (instance != null)
            return instance;
        return null;
    }
}
