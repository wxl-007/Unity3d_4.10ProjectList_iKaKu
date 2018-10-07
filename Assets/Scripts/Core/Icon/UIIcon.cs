using UnityEngine;
using System.Collections;
using System.Threading;

public class UIIcon : MonoBehaviour
{
    public string url;
    public UITexture icon;
    public UISprite frame;
    public UISprite back;
    public GameObject label0;
    public GameObject label1;
    public GameObject label2;
    public GameObject label3;

    public UISprite iconSprite;

    public enum FORM_TYPE
    {
        RECT = 0,
        CIRCLE = 1,
        Offwhite = 2,
    }
    public FORM_TYPE formType;
    void Start()
    {

    }

    void InitMaterial()
    {
        if (icon != null && icon.material == null)
        {
            if (formType == FORM_TYPE.CIRCLE)
            {
                icon.material = GameObject.Instantiate(ResManager.LoadExist("Material/IconCircleMaterial")) as Material;
            }
            else if (formType == FORM_TYPE.Offwhite)
            {
                icon.material = GameObject.Instantiate(ResManager.LoadExist("Material/IconOffwhiteMaterial")) as Material;
            }
            else
            {
                icon.material = GameObject.Instantiate(ResManager.LoadExist("Material/IconMaterial")) as Material;
            }
        }
    }
    public void ReInitMaterial()
    {
        icon.material = null;
        InitMaterial();
    }
    public void SetPlayerIcon(uint pid, string iconName)
    {
        if (back != null)
        {
            NGUITools.SetActive(back.gameObject, false);
        }
        //		if(url == null || url.Equals("") || !url.Equals(pid + "/" + iconName)){

        if (iconName != null && !iconName.Equals(""))
        {
            if (icon != null)
            {
                NGUITools.SetActive(icon.gameObject, true);
            }
            if (icon != null)
            {
                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(LoadPlayerIcon(pid, iconName));
                    url = pid + "/" + iconName;
                }
            }

        }
        else
        {
            if (icon != null)
            {
                NGUITools.SetActive(icon.gameObject, false);
            }
        }
        //		}
        SetPos(-1, -1, -1, -1, -1);
    }
    public IEnumerator LoadPlayerIcon(uint pid, string iconName)
    {
        if (icon != null)
        {
            InitMaterial();
            ResGameObject resObj = new ResGameObject();
            resObj.resType = ResGameObject.TYPE_TEXTURE;
            yield return StartCoroutine(ResManager.GetInstance().LoadPlayerIcon(pid, "default", resObj));
            icon.material.mainTexture = (Texture2D)resObj.obj;
            yield return StartCoroutine(ResManager.GetInstance().LoadPlayerIcon(pid, iconName, resObj));
            icon.material.mainTexture = (Texture2D)resObj.obj;
            icon.MarkAsChanged();
        }
        yield break;
    }
    public IEnumerator LoadIcon(string fileName)
    {
        if (icon != null)
        {
            InitMaterial();
            ResGameObject resObj = new ResGameObject();
            resObj.resType = ResGameObject.TYPE_TEXTURE;
            yield return StartCoroutine(ResManager.GetInstance().Load("Icon/" + fileName, resObj));
            icon.material.mainTexture = (Texture2D)resObj.obj;
            icon.MarkAsChanged();
        }
        yield break;
    }
    public static UIIcon CreateIcon(Transform trans)
    {
        GameObject iconObject = GameObject.Instantiate(ResManager.LoadExist(Config.ComponentItemPath + Config.UIIcon), trans.position, trans.rotation) as GameObject;
        iconObject.transform.parent = trans.parent;
        iconObject.transform.localScale = trans.localScale;
        iconObject.GetComponent<UIIcon>().InitMaterial();
        return iconObject.GetComponent<UIIcon>();
    }
    public static UIIcon CreateIcon(Transform trans, FORM_TYPE type)
    {
        GameObject iconObject = GameObject.Instantiate(ResManager.LoadExist(Config.ComponentItemPath + Config.UIIcon), trans.position, trans.rotation) as GameObject;
        iconObject.transform.parent = trans.parent;
        iconObject.transform.localScale = trans.localScale;
        iconObject.GetComponent<UIIcon>().formType = type;
        iconObject.GetComponent<UIIcon>().InitMaterial();
        return iconObject.GetComponent<UIIcon>();
    }
    public void SetIcon(string iconName, string backName, int frameType, int pos0, int pos1, int pos2, int pos3)
    {


        SetIcon(iconName, frameType, pos0, pos1, pos2, pos3);
        if (back != null)
        {
            NGUITools.SetActive(back.gameObject, true);
            back.spriteName = backName;
        }

    }

    public void SetIcon(string iconName, int frameType, int pos0, int pos1, int pos2, int pos3)
    {
        if (back != null)
        {
            NGUITools.SetActive(back.gameObject, false);
            back.spriteName = "bg_05";
        }
		if (icon != null)
        {
            NGUITools.SetActive(icon.gameObject, true);
        }
        if (url == null || url.Equals("") || !url.Equals(iconName) || icon.material.mainTexture == null)
        {

            if (iconName != null && !iconName.Equals(""))
            {
                if (icon != null)
                {
                    NGUITools.SetActive(icon.gameObject, true);
                }
                if (iconSprite != null)
                {
                    NGUITools.SetActive(iconSprite.gameObject, true);
                }

                if (icon != null)
                {
                    if (gameObject.activeInHierarchy)
                    {
						NGUITools.SetActive(icon.gameObject, true);
                        StartCoroutine(LoadIcon(iconName));
                        url = iconName;
                    }
                }
                if (iconSprite != null)
                {
                    iconSprite.spriteName = iconName;
                }

            }
            else
            {
                if (icon != null)
                {
                    NGUITools.SetActive(icon.gameObject, false);
                }
                if (iconSprite != null)
                {
                    NGUITools.SetActive(iconSprite.gameObject, false);
                }
                if (frame != null)
                {
                    NGUITools.SetActive(frame.gameObject, false);
                }

            }
        }
        if (frame != null)
        {
            NGUITools.SetActive(frame.gameObject, true);
            if (frameType >= 0)
            {
                if (frameType >= 20)
                {
                    if (formType == FORM_TYPE.CIRCLE)
                    {
                        frame.spriteName = "frame_circle_buff";
                    }
                    else
                    {
                        frame.spriteName = "frame_buff";
                    }
                }
                else if (frameType >= 10)
                {
                    if (formType == FORM_TYPE.CIRCLE)
                    {
                        frame.spriteName = "frame_circle_npc" + (frameType - 10).ToString();
                    }
                    else
                    {
                        frame.spriteName = "frame_npc" + (frameType - 10).ToString();
                    }
                }
                else
                {
                    if (formType == FORM_TYPE.CIRCLE)
                    {

                        frame.spriteName = "frame_" + frameType.ToString();
                    }
                    else
                    {
                        frame.spriteName = "frame_" + frameType.ToString();
                    }
                }
                NGUITools.SetActive(frame.gameObject, true);
            }
            else
            {
                NGUITools.SetActive(frame.gameObject, false);
            }
        }
        SetPos(frameType, pos0, pos1, pos2, pos3);
    }
    public void SetPos(int frameType, int pos0, int pos1, int pos2, int pos3)
    {
        if (label0 != null)
        {
            if (pos0 > 0)
            {
                NGUITools.SetActive(label0, true);
                UILabel label = label0.GetComponentInChildren<UILabel>();
                if (label != null)
                {
                    label.text = pos0.ToString();
                    if (frameType >= 0)
                        label0.GetComponentInChildren<UISprite>().spriteName = "icon_sign_" + frameType.ToString();
                }
            }
            else
            {
                NGUITools.SetActive(label0, false);
            }
        }
        if (label1 != null)
        {
            if (pos1 >= 0)
            {
                NGUITools.SetActive(label1, true);
                label1.GetComponentInChildren<UILabel>().text = pos1.ToString();
                if (frameType >= 0)
                    label1.GetComponentInChildren<UISprite>().spriteName = "icon_sign_" + frameType.ToString();
            }
            else
            {
                NGUITools.SetActive(label1, false);
            }
        }
        if (label2 != null)
        {
            if (pos2 >= 0)
            {
                NGUITools.SetActive(label2, true);
                UISprite sprite = label2.GetComponentInChildren<UISprite>();
                sprite.spriteName = "icon_chip_" + pos2.ToString();
            }
            else
            {
                NGUITools.SetActive(label2, false);
            }
        }
        if (label3 != null)
        {
            if (pos3 > 0)
            {
                NGUITools.SetActive(label3, true);
                UISprite sprite = label3.GetComponentInChildren<UISprite>();
                if (sprite != null)
                {
                    NGUITools.SetActive(sprite.gameObject, false);
                }
                if (label3.GetComponentInChildren<UILabel>() != null)
                {
                    label3.GetComponentInChildren<UILabel>().text = pos3.ToString();
                }
            }
            else
            {
                NGUITools.SetActive(label3, false);
            }
        }
    }
    public void SetSpriteColor(Color color)
    {
        if (iconSprite != null)
        {
            iconSprite.color = color;
        }
    }
    //	public void AddDragDrop(IItemValue item){
    //		JhDragDropItem dragDropItem = gameObject.AddComponent<JhDragDropItem>();
    //		dragDropItem.item = item;
    //		BoxCollider box = gameObject.AddComponent<BoxCollider>();
    //		box.size = new Vector3(128,128,1);
    //		box.center = new Vector3(0,0,-20);
    //	}
#if UNITY_EDITOR

    /// <summary>
    /// Draw a visible blue outline for the clipped area.
    /// </summary>
    void OnDrawGizmos()
    {
        Vector2 size = new Vector2(0.0f, 0.0f);
        if (icon != null)
        {
            size = new Vector2(icon.transform.localScale.x, icon.transform.localScale.y);
        }
        if (iconSprite != null)
        {
            size = new Vector2(iconSprite.transform.localScale.x, iconSprite.transform.localScale.y);
        }

        if (size.x == 0f) size.x = 128;
        if (size.y == 0f) size.y = 128;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        if (iconSprite != null)
        {
            Gizmos.DrawWireCube(new Vector2(iconSprite.cachedTransform.localPosition.x, iconSprite.cachedTransform.localPosition.y), size);
        }
        if (icon != null)
        {
            Gizmos.DrawWireCube(new Vector2(icon.cachedTransform.localPosition.x, icon.cachedTransform.localPosition.y), size);
        }
    }
#endif
//    public void SetIcon(object obj)
//    {
//        if (obj == null)
//        {
////            SetIcon(Config.HeroIconPath + "default", 1, -1, -1, -1, -1);
//            			SetIcon("", 1, -1, -1, -1, -1);
//        }
//        else
//            if (obj is Hero)
//            {
//                Hero hero = (Hero)obj;
//                SetIcon(Config.HeroIconPath + hero.heroInfo.icon, hero.heroInfo.quality, hero.level, -1, -1, -1);
//            }
//            else if (obj is Guanka)
//            {
//                Guanka guanka = (Guanka)obj;
//			
//                SetIcon(Config.HeroIconPath + guanka.monster.GetAllHero()[0].heroInfo.icon, guanka.quality, -1, -1, -1, -1);
//            }
//            else if (obj is Story)
//            {
//                Story story = (Story)obj;
//                SetIcon(Config.StoryIconPath + story.icon, -1, -1, -1, -1, -1);
//            }
//            else if (obj is Props)
//            {
//                Props props = (Props)obj;
//                SetIcon(Config.PropsIconPath + props.propsInfo.icon, -1, -1, -1, -1, props.Count);
//            }
//            else if (obj is PropsInfo)
//            {
//                PropsInfo props = (PropsInfo)obj;
//                SetIcon(Config.PropsIconPath + props.icon, -1, -1, -1, -1, -1);
//            }
//            else if (obj is Glory)//成就.
//            {
//                Glory glory = (Glory)obj;
//                SetIcon(Config.GloryIconPath + glory.gloryInfo.icon, -1, -1, -1, -1, -1);
//            }
//    }
//    public void SetBigIcon(object obj)
//    {
//		if (obj == null)
//        {
////            SetIcon(Config.HeroIconPath + "default", 1, -1, -1, -1, -1);
//            			SetIcon("", 1, -1, -1, -1, -1);
//        }else
//        if (obj is Hero)
//        {
//            Hero hero = (Hero)obj;
//            SetIcon(Config.HeroBigIconPath + hero.heroInfo.photo, hero.heroInfo.quality, hero.level, -1, -1, -1);
//        } if (obj is Guanka)
//        {
//            Guanka guanka = (Guanka)obj;
//            SetIcon(Config.GuankaIconPath + guanka.icon, guanka.quality, -1, -1, -1, -1);
//        }
//        else if (obj is Props)
//        {
//            Props props = (Props)obj;
//            SetIcon(Config.PropsIconPath + props.propsInfo.name, -1, -1, -1, -1, -1);
//        }
//        else if (obj is BuffInfo)
//        {
//            BuffInfo buffInfo = (BuffInfo)obj;
//            SetIcon(Config.BuffIconPath + buffInfo.icon, -1, -1, -1, -1, -1);
//        }
//    }
}
