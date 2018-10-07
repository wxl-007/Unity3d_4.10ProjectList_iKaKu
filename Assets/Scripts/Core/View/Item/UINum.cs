using UnityEngine;
using System.Collections;

public class UINum : MonoBehaviour {
	public const int MAX = 6;
	public int num{
		get{return m_num;}
		set{
			m_num = value;
			int tNum = m_num;
			for(int i = MAX;i > 0;i--){
				int n = (int)Mathf.Pow(10f,i);
				if(tNum >= n){
					if(spriteList.Length > i){
						NGUITools.SetActive(spriteList[i].gameObject,true);
						spriteList[i].spriteName = (tNum/n).ToString();
					}
					tNum = tNum%n;
				}else{
					if(spriteList.Length > i){
						NGUITools.SetActive(spriteList[i].gameObject,false);
					}
				}
			}
			NGUITools.SetActive(spriteList[0].gameObject,true);
			spriteList[0].spriteName = (tNum).ToString();
		}
	}
	public UISprite[] spriteList;
	private int m_num;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
