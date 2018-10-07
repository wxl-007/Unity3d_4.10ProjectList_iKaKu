using UnityEngine;
using System.Collections;
using System.Xml;

namespace JianghuUtils{
	public class XMLReader
	{
		XmlDocument xmldocument = new XmlDocument ();
		
		public XMLReader(){
		}
		
		public XMLReader(string path){
			this.xmldocument.Load(path);
		}
		
		public string GetAttribute(string name){
			//XmlElement System_Settings = (XmlElement)xmldocument.SelectSingleNode("System_Settings");
			XmlNode System_Settings = xmldocument.SelectSingleNode(name);
			//Debug.Log(System_Settings.InnerXml);
			return System_Settings.InnerText;
		}
		
	}
}
