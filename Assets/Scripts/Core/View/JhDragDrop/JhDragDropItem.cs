using System;
using UnityEngine;
public class JhDragDropItem : MonoBehaviour
{
	public GameObject dropItem;
	Transform mTrans;
	bool mIsDragging = false;
	Transform mParent;
//	public IItemValue item;
	Vector3 mOrgPos;
	
	private GameObject newGameObject;
//	private 
	/// <summary>
	/// Update the table, if there is one.
	/// </summary>

	void UpdateTable ()
	{
		UITable table = NGUITools.FindInParents<UITable>(gameObject);
		if (table != null) table.repositionNow = true;
		UIGrid grid = NGUITools.FindInParents<UIGrid>(gameObject);
		if (grid != null) grid.repositionNow = true;
	}

//	/// <summary>
//	/// Drop the dragged object.
//	/// </summary>
//
//	void Drop ()
//	{
//		// Is there a droppable container?
//		Collider col = UICamera.lastHit.collider;
//		DragDropContainer container = (col != null) ? col.gameObject.GetComponent<DragDropContainer>() : null;
//
//		if (container != null)
//		{
//			// Container found -- parent this object to the container
//			mTrans.parent = container.transform;
//
//			Vector3 pos = mTrans.localPosition;
//			pos.z = 0f;
//			mTrans.localPosition = pos;
//		}
//		else
//		{
//			// No valid container under the mouse -- revert the item's parent
//			mTrans.parent = mParent;
//			mTrans.localPosition = mOrgPos;
//		}
//
//		// Notify the table of this change
//		UpdateTable();
//
//		// Make all widgets update their parents
//		NGUITools.MarkParentAsChanged(gameObject);
//	}
//
//	/// <summary>
//	/// Cache the transform.
//	/// </summary>
//
//	void Awake () { mTrans = transform; }
//
//	/// <summary>
//	/// Start the drag event and perform the dragging.
//	/// </summary>
//
//	void OnDrag (Vector2 delta)
//	{
//		if (enabled && UICamera.currentTouchID > -2)
//		{
//			if (!mIsDragging)
//			{
//				mOrgPos = mTrans.localPosition;
//				mIsDragging = true;
//				mParent = mTrans.parent;
//				mTrans.parent = JhDragDropRoot.root;
//				
//				Vector3 pos = mTrans.localPosition;
//				pos.z = 0f;
//				mTrans.localPosition = pos;
//				
//				NGUITools.MarkParentAsChanged(gameObject);
//			}
//			else
//			{
//				mTrans.localPosition += (Vector3)delta;
//			}
//		}
//	}
	
	void Drop ()
	{
		// Is there a droppable container?
		Collider col = UICamera.lastHit.collider;
		DragDropContainer container = (col != null) ? col.gameObject.GetComponent<DragDropContainer>() : null;

		if (container != null)
		{
			// Container found -- parent this object to the container
			newGameObject.transform.parent = container.transform;

			Vector3 pos = newGameObject.transform.localPosition;
			pos.z = 0f;
			newGameObject.transform.localPosition = pos;
		}
		else
		{
//			// No valid container under the mouse -- revert the item's parent
//			mTrans.parent = mParent;
//			mTrans.localPosition = mOrgPos;
			Destroy(newGameObject);
		}

		// Notify the table of this change
		UpdateTable();

		// Make all widgets update their parents
		NGUITools.MarkParentAsChanged(gameObject);
	}
	void OnDrag (Vector2 delta)
	{
		if (enabled && UICamera.currentTouchID > -2)
		{
			if (!mIsDragging)
			{
				mIsDragging = true;
				if(dropItem == null){
					dropItem = gameObject;
				}
				newGameObject = NGUITools.AddChild(JhDragDropRoot.root.gameObject,dropItem);
				NGUITools.SetActive(newGameObject,true);
				newGameObject.transform.position = dropItem.transform.position;
				Vector3 pos = newGameObject.transform.localPosition;
				pos.z = 0f;
				newGameObject.transform.localPosition = pos;
				
				NGUITools.MarkParentAsChanged(newGameObject);
			}
			else
			{
				newGameObject.transform.localPosition += ((Vector3)delta*960/Screen.height);
			}
		}
	}
	
	/// <summary>
	/// Start or stop the drag operation.
	/// </summary>

	void OnPress (bool isPressed)
	{
		if (enabled)
		{
			mIsDragging = false;
			Collider col = collider;
			if (col != null) col.enabled = !isPressed;
			if (!isPressed) Drop();
		}
	}
}

