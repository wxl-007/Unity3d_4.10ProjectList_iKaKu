using UnityEngine;
using System.Collections;

public class JhDragDropRoot : MonoBehaviour {

	static public Transform root;

	void Awake () { root = transform; }
}
