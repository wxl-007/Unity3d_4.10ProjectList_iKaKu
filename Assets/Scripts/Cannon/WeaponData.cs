public class WeaponData {
    //射程
    public float range;
    //射速
    public float interval;
    //精准度
    public float deviation;
    //阵营
    public int team;
    //类型
    public int type;
    //损毁度
    public float health;
    //重力值(负数,绝对值越大,重力值越大)
    public float gravity;

    public static WeaponData GetData(string name) {
        return new WeaponData();
    }
}