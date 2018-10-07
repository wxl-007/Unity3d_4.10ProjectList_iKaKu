using System;
using UnityEngine;
using System.Collections;
/// <summary>
/// 游戏管理.
/// </summary>
public class GameManager : MonoBehaviour
{
    private GameState gameState = GameState.INIT;
    private enum GameState
    {
        INIT = 1,
        INITING = 2,
        GAME = 3,
    }
    private static GameManager instance;
    /// <summary>
    /// 玩家.
    /// </summary>
    private Player player;
    public static GameManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        instance = this;
    }
    void Update()
    {
        switch (gameState)
        {
            case GameState.INIT:
                StartCoroutine(Init());
                break;
            case GameState.INITING:
                Init();
                break;
            case GameState.GAME:
                break;
        }
    }
    IEnumerator Init()
    {
        gameState = GameState.INITING;
        yield return StartCoroutine(Config.LoadConfig());
        yield return StartCoroutine(InitPools());
//        PanelManager.GetInstance().ShowPanel(Config.LoginPanel);
		NewPlayer();
		player.Init();
        gameState = GameState.GAME;
    }
    void OnGUI()
    {
		
    }
    /// <summary>
    /// 获得当前玩家
    /// </summary>
    /// <returns>
    /// 当前玩家.
    /// </returns>
    public Player GetPlayer()
    {
        return player;
    }
    public Player NewPlayer()
    {
        if (player == null)
        {
            player = new Player();
        }
        return player;
    }
    #region init pool
    public IEnumerator InitPools()
    {
        yield return StartCoroutine(new PlayerPool().Init());
        yield return StartCoroutine(new WeaponPool().Init());
		yield return StartCoroutine(new ShipPool().Init());
//		Application.LoadLevelAsync("test");
//		while(Application.isLoadingLevel){
//			yield return 1;
//		}
//		Application.GetStreamProgressForLevel("test");
//        yield return StartCoroutine(new GuankaPool().Init());
//        ResManager.GetInstance().AddInitIndex();
//        yield return StartCoroutine(new EquipmentPool().Init());
//        ResManager.GetInstance().AddInitIndex();
//        //道具.
//        yield return StartCoroutine(new PropsPool().Init());
//        ResManager.GetInstance().AddInitIndex();
//        //商城.
//        yield return StartCoroutine(new MallPool().Init());
//        ResManager.GetInstance().AddInitIndex();
//        //成就.
//        yield return StartCoroutine(new GloryPool().Init());
//        ResManager.GetInstance().AddInitIndex();
//
//        yield return StartCoroutine(new CardStorePool().Init());
//        ResManager.GetInstance().AddInitIndex();
//
//        yield return StartCoroutine(new MonsterPool().Init());
//        ResManager.GetInstance().AddInitIndex();
//        yield return StartCoroutine(new BuffPool().Init());
//        ResManager.GetInstance().AddInitIndex();
//        yield return StartCoroutine(new SpellPool().Init());
//        ResManager.GetInstance().AddInitIndex();
//        ///<summer>
//        ///静态文字资源.
//        ///</summer>
//        yield return StartCoroutine(Config.InitConfigStaticString());
//        ResManager.GetInstance().AddInitIndex();
//
//        Debug.Log("InitDataPoolEnd");
        yield break;
    }
    #endregion
}


