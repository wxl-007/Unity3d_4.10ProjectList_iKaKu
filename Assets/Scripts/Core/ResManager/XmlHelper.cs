using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Xml;
using UnityEngine;

public class XmlHelper : MonoBehaviour
{
    private static XmlHelper instance;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        instance = this;
    }
    public static XmlHelper GetInstance()
    {
        return instance;
    }
    public XmlDocument xml;
    public List<object> alList;
    /// <summary>
    /// 获取xml文件中对应的字段值，并填充对象.
    /// </summary>
    /// <param name="obj">要填充的对象.</param>
    /// <param name="filePath">文件路径.</param>
    /// <returns></returns>
    public IEnumerator getContentsByFiledName(object obj, String filePath)
    {

        alList = new List<object>();
        xml = null;
        yield return StartCoroutine(LoadXml(filePath));
        XmlDocument xmlDoc = xml;

        if (xmlDoc == null)
        {

        }
        else
        {

            XmlElement rootElement = xmlDoc.DocumentElement;


            FieldInfo[] fieldList = obj.GetType().GetFields();


            XmlNodeList itemNodeList = rootElement.SelectNodes("item");

            for (int i = 0; i < itemNodeList.Count; i++)
            {
                //					string str = "";
                XmlNode oXmlNode = itemNodeList[i];



                object newObj = Activator.CreateInstance(obj.GetType());

                foreach (FieldInfo tmpFiled in fieldList)
                {
                    object fieldValue = getXmlNodeValue(oXmlNode, tmpFiled);
                    if (fieldValue != null)
                    {
                        newObj.GetType().GetField(tmpFiled.Name).SetValue(newObj, fieldValue);
                    }
                    //						str += ("--" + tmpFiled.Name +":"+fieldValue );
                }

                //					Debug.Log(str);
                //newObj.GetType().GetMethod(objMethodName, new Type[] { newObj.GetType() }).Invoke(newObj, newObj2);

                alList.Add(newObj);
            }
        }
        yield break;
    }


    /// <summary>
    /// 获取xml文件中对应的字段值，并填充对象，同时填充目标对象中包含的对象，并赋值给目标对象.
    /// </summary>
    /// <param name="obj">要填充的目标对象.</param>
    /// <param name="obj2">目标对象中的子对象.</param>
    /// <param name="objMethodName">目标对象中用于合并子对象的方法名.</param>
    /// <param name="filePath">文件路径.</param>
    /// <returns></returns>
    public IEnumerator getRelationContents(object obj, object obj2, String objMethodName, String filePath)
    {
        alList = new List<object>();
        xml = null;
        yield return StartCoroutine(LoadXml(filePath));
        XmlDocument xmlDoc = xml;
        if (xmlDoc == null)
        {

        }
        else
        {

            XmlElement rootElement = xmlDoc.DocumentElement;


            FieldInfo[] fieldList = obj.GetType().GetFields();
            FieldInfo[] fieldList2 = obj2.GetType().GetFields();


            XmlNodeList itemNodeList = rootElement.SelectNodes("item");

            for (int i = 0; i < itemNodeList.Count; i++)
            {
                XmlNode oXmlNode = itemNodeList[i];

                object newObj = Activator.CreateInstance(obj.GetType());

                foreach (FieldInfo tmpFiled in fieldList)
                {
                    object fieldValue = getXmlNodeValue(oXmlNode, tmpFiled);
                    if (fieldValue != null)
                    {
                        newObj.GetType().GetField(tmpFiled.Name).SetValue(newObj, fieldValue);
                    }
                }


                object newObj2 = Activator.CreateInstance(obj2.GetType());

                foreach (FieldInfo tmpFiled in fieldList2)
                {
                    object fieldValue = getXmlNodeValue(oXmlNode, tmpFiled);
                    if (fieldValue != null)
                    {
                        newObj2.GetType().GetField(tmpFiled.Name).SetValue(newObj2, fieldValue);
                    }
                }


                newObj.GetType().GetMethod(objMethodName, new Type[] { newObj2.GetType() }).Invoke(newObj, new object[] { newObj2 });

                alList.Add(newObj);
            }
        }
        yield break;
    }


    private static object getXmlNodeValue(XmlNode xmlNode, FieldInfo field)
    {
        XmlNodeList oXmlNodeList = xmlNode.ChildNodes;

        foreach (XmlNode tmpNode in oXmlNodeList)
        {

            if (tmpNode.Name.Equals(field.Name))
            {
                if (field.FieldType == typeof(System.UInt32))
                    return Convert.ToUInt32(tmpNode.InnerText);
                else if (field.FieldType == typeof(System.UInt16))
                    return Convert.ToUInt16(tmpNode.InnerText);
                else if (field.FieldType == typeof(System.UInt64))
                    return Convert.ToUInt64(tmpNode.InnerText);
                else if (field.FieldType == typeof(System.Int32))
                    return Convert.ToInt32(tmpNode.InnerText);
                else if (field.FieldType == typeof(System.Int16))
                    return Convert.ToInt16(tmpNode.InnerText);
                else if (field.FieldType == typeof(System.Int64))
                    return Convert.ToInt64(tmpNode.InnerText);
                else if (field.FieldType == typeof(System.Single))
                    return Convert.ToSingle(tmpNode.InnerText);
                else if (field.FieldType == typeof(System.Double))
                    return Convert.ToDouble(tmpNode.InnerText);
                else if (field.FieldType == typeof(System.Byte))
                    return Convert.ToByte(tmpNode.InnerText);
                else if (field.FieldType.IsEnum)
                    return System.Enum.Parse(field.FieldType, tmpNode.InnerText);
                else if (field.FieldType == typeof(System.String))
                    return tmpNode.InnerText;
                else
                    return tmpNode.InnerText;

            }
        }

        return null;
    }

    public IEnumerator LoadXml(string url)
    {
        //		Debug.Log("load xml:" + url);
        //		ResGameObject resObj = new ResGameObject();
        //		yield return StartCoroutine(ResManager.GetInstance().Load(url,resObj));
        ////		resObj.obj = Resources.Load(url);
        //		TextAsset ta = resObj.obj as TextAsset;
        //        xml = new XmlDocument();
        //		System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(ta.bytes);
        //		
        ////		byte[] datas = ResManager.LoadXmlData(url);
        ////        XmlDocument xml = new XmlDocument();
        ////		System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(datas);
        //		
        //		System.IO.StreamReader streamReader = new System.IO.StreamReader(memoryStream, true);
        //		string str = streamReader.ReadToEnd();
        //		streamReader.Close();
        //		memoryStream.Close();
        //        xml.LoadXml(str);

//        if (Application.isEditor || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
//        {
//			Test1.log = "load xml:" + url;
//            Debug.Log("load xml:" + url);
            ResGameObject resObj = new ResGameObject();
            yield return StartCoroutine(ResManager.GetInstance().Load(url, resObj));
            if (resObj.obj != null)
            {
                TextAsset ta = resObj.obj as TextAsset;
                xml = new XmlDocument();
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(ta.bytes);

                System.IO.StreamReader streamReader = new System.IO.StreamReader(memoryStream, true);
                string str = streamReader.ReadToEnd();
                streamReader.Close();
                memoryStream.Close();
                xml.LoadXml(str);
            }
            else
            {
                Debug.Log("Not Find " + url + "!");
            }
//        }
//        else
//        {
//			Test1.log = "load xml:" + url;
//            Debug.Log("load xml by LoadXmlData:" + url);
//
//            byte[] datas = ResManager.LoadXmlData(url);
//            if (datas != null)
//            {
//                xml = new XmlDocument();
//                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(datas);
//
//                System.IO.StreamReader streamReader = new System.IO.StreamReader(memoryStream, true);
//                string str = streamReader.ReadToEnd();
//                streamReader.Close();
//                memoryStream.Close();
//                xml.LoadXml(str);
//            }
//            else
//            {
//                Debug.Log("Not Find " + url + "!");
//            }
//        }
        yield break;
    }
}

