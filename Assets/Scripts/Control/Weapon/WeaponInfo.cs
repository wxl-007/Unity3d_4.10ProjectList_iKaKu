public class WeaponInfo {
	/// <summary>
	/// 序号.
	/// </summary>
	public int id;
	/// <summary>
	/// 名字.
	/// </summary>
	public string name;
	/// <summary>
	/// 图标.
	/// </summary>
	public string icon;
	/// <summary>
	/// 详细描述.
	/// </summary>
	public string describe;
    /// <summary>
    /// 射程(米).
    /// </summary>
    public float range;
	/// <summary>
	/// 射速(秒).
	/// </summary>
    public float interval;
	/// <summary>
	/// 精准度0-1.
	/// </summary>
    public float deviation;
	/// <summary>
	/// 口径(米).
	/// </summary>
	public float caliber;

//    //阵营.
//    public int team;
//    //类型.
//    public int type;
//    //损毁度.
//    public float health;
//    //重力值(负数,绝对值越大,重力值越大).
//    public float gravity;
}