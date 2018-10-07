using System;
using UnityEngine;
public class UIDraggablePanelExtend : MonoBehaviour
{
	private UIDraggablePanel mDraggablePanel = null;
	private UIPanel mPanel = null;
	private Vector2 orgPosition = new Vector2();
	private Vector4 orgClipRange = new Vector4();
	private UITable mTable = null;
	private UIGrid mGrid = null;
	void Start(){
		Init();
	}
	public void Init(){
		if(mPanel == null){
			mPanel = GetComponent<UIPanel>();
			mDraggablePanel = GetComponent<UIDraggablePanel>();
			orgPosition = new Vector2(mDraggablePanel.transform.localPosition.x,mDraggablePanel.transform.localPosition.y);
			orgClipRange = new Vector4(mPanel.clipRange.x,mPanel.clipRange.y,mPanel.clipRange.z,mPanel.clipRange.w);
			mTable = gameObject.GetComponentInChildren<UITable>();
			mGrid = gameObject.GetComponentInChildren<UIGrid>();
		}
	}
	public void SetPositionY(float y){
		if(mPanel != null){
			mPanel.transform.localPosition = new Vector3(orgPosition.x,orgPosition.y + y,mPanel.transform.localPosition.z);
			mPanel.clipRange = new Vector4(orgClipRange.x,orgClipRange.y - y,orgClipRange.z,orgClipRange.w);
		}
	}
	public void SetPositionX(float x){
		if(mPanel != null){
			mPanel.transform.localPosition = new Vector3(orgPosition.x - x,mPanel.transform.localPosition.y ,mPanel.transform.localPosition.z);
			mPanel.clipRange = new Vector4(orgClipRange.x + x,mPanel.clipRange.y,mPanel.clipRange.z,mPanel.clipRange.w);
		}
	}
	public void SetItemInPanel(int index){
		Transform itemTransform = mTable.transform.GetChild(index);
		Bounds itemBounds = NGUIMath.CalculateRelativeWidgetBounds(itemTransform);
		float itemW = itemBounds.size.x * itemTransform.localScale.x;
//		float itemH = itemBounds.size.y * itemTransform.localScale.y;
		
		float x = itemTransform.localPosition.x - itemBounds.size.x/2;
//		float y = itemTransform.localPosition.y - itemW/2;
		if(mPanel.clipRange.x - orgClipRange.x > x){
			SetPositionX(x);
		}
		if(mPanel.clipRange.x - orgClipRange.x + orgClipRange.z < x + itemW){
			SetPositionX(x + itemW - orgClipRange.z);
		}
		if(mTable != null){
			
		}
	}
	public void SetItemInCenterX(int index){
		if(mPanel != null){
			float itemH = 0;
			int itemCount = 0;
			if(mTable != null){
				itemH = mTable.padding.x;
				itemCount = mTable.transform.GetChildCount();
			}else if(mGrid != null){
				itemH = mGrid.cellWidth;
				itemCount = mGrid.transform.GetChildCount();
			}
			float x = (index* itemH - (orgClipRange.z - itemH)/2);
			if(x < 0){
				x = 0;
			}else if(x > ((itemCount * itemH) - orgClipRange.z)){
				x = ((itemCount * itemH) - orgClipRange.z);
			}
			SetPositionX(x);
			mPanel.transform.localPosition = new Vector3(orgPosition.x - x,orgPosition.y,mPanel.transform.localPosition.z);
		}
	}
//	public void SelectItem(int index){
//		float itemW = 0;
//		float itemH = 0;
//		int itemCount = 0;
//		int columns = 0;
//		if(mTable != null){
//			itemH = mTable.padding.y;
//			itemCount = mTable.transform.GetChildCount();
//			columns = mTable.columnsp;
//			int offx = 0;
//			int offy = 0;
//			if(columns == 0){
//				offx = 
//			}else{
//			}
//		}
//		else if(mGrid != null){
//			itemH = mGrid.cellHeight;
//			itemCount = mGrid.transform.GetChildCount();
//			columns = mGrid.maxPerLine;
//		}
//	}
	public void SetItemInCenter(int index){
		if(mPanel != null){
			float itemH = 0;
			int itemCount = 0;
			if(mTable != null){
				itemH = mTable.padding.y;
				itemCount = mTable.transform.GetChildCount();
			}else if(mGrid != null){
				itemH = mGrid.cellHeight;
				itemCount = mGrid.transform.GetChildCount();
			}
			float y = (index* itemH);
			if(y < 0){
				y = 0;
			}else if(y > ((itemCount * itemH) - orgClipRange.w)){
				y = ((itemCount * itemH) - orgClipRange.w);
			}
			SetPositionY(y);
			mPanel.transform.localPosition = new Vector3(orgPosition.x,orgPosition.y + y,mPanel.transform.localPosition.z);
		}
	}
	public void ReSetPosition(){
		mPanel.transform.localPosition = new Vector3(orgPosition.x,orgPosition.y,mPanel.transform.localPosition.z);
		mPanel.clipRange = new Vector4(orgClipRange.x,orgClipRange.y,orgClipRange.z,orgClipRange.w); 
	}
}

