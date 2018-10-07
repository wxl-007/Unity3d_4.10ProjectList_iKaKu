/// <summary>
/// 岛上的炮
/// </summary>
public class IslandCannon:Island{
    public WeaponInfo info;

    void Start() {
        //TODO
        //这里需要修改
        info = ((ShipForPlayerController)GameManager.GetInstance().GetPlayer().GetController(ControllerTypeInfo.Ship)).GetShip(1).shipInfo.mainWeapon;
    }


    public void OnFire(FightElement target){
        //TODO
    }
}