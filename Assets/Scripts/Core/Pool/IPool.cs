using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// 配置表数据.
/// </summary>
public abstract class IPool
{
	/// <summary>
	/// 初始化.
	/// </summary>
	public abstract IEnumerator Init();
	/// <summary>
	/// 重新读取.
	/// </summary>
	public abstract IEnumerator Reload();
}


