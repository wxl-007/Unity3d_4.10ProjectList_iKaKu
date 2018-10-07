using UnityEngine;
using System.Collections;

//namespace RabitFun
//{
	public class RabitFun 
	{
	
		//支持中文  
		public static void InitLabel(UILabel label, string str)
		{
			if(label != null)
			{
				label.text = str;
			}
			else
			{
				Debug.LogError("label is empty");
			}
		}
		public static void RefreshActivePanel(string panelname)
		{
			BasePanel bp = PanelManager.GetInstance().GetPanel(panelname);
		
			if(bp != null)
			{
				if(bp.gameObject.activeSelf)
				{
					bp.Refresh();
				}
			}
		}
	}
	
//}