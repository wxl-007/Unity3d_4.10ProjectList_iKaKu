using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;
using System.Reflection;
using System.IO;
using System.Xml;
public class ResManager : MonoBehaviour
{
	public bool isResourcesPath;
	
//	class ResData{
//		public string url;
//		public int version;
//	}
	public enum UpdateState{
		NoUpdata = 0,
		DynamiUpdate = 1,
		ForceUpdate = 2,
	}
	public class Version{
		public int version1 = 1;
		public int version2 = 0;
		public int version3 = 1;
		public string ToMyString(){
			return version1 + "." + version2 + "." + version3;
		}
	}
	public static string AssetbundleBaseURL
	{
	    get
	    {
			if(Application.platform == RuntimePlatform.IPhonePlayer)  
			{  
				return Application.dataPath + "/Raw/AssetBundle/";
			}  
			else if(Application.platform == RuntimePlatform.Android)  
			{  
				return "jar:file://" + Application.dataPath + "!/assets/AssetBundle/";
//			 	return Application.streamingAssetsPath + "/AssetBundle/";  
			}
			else if (Application.isEditor)
            {
				return  "" + Application.dataPath + "/StreamingAssets/AssetBundle/";
            }else
			{  
				return "" + Application.dataPath + "/StreamingAssets/AssetBundle/";
			}
	    }
	}
	public static string FileWriteBaseUrl
	{
	    get
	    {
			if(Application.platform == RuntimePlatform.IPhonePlayer)  
			{  
				// Strip "/Data" from path
				string path = Application.dataPath.Substring (0, Application.dataPath.Length - 5);
				// Strip application name
				path = path.Substring(0, path.LastIndexOf('/'));
				return path + "/Documents/AssetBundle/";
//				return Application.dataPath + "/Raw/AssetBundle/Resources/";
			}  
			else if(Application.platform == RuntimePlatform.Android)  
			{  
				return "jar:file://" + Application.dataPath + "!/assets/AssetBundle/";
//			 	return Application.streamingAssetsPath + "/AssetBundle/";  
			}  
			else
			{  
				return Application.dataPath + "/StreamingAssets/AssetBundle/";
			}
		}
	}
	
//    private string fileSeverUrl = "http://60.207.196.58/upload/";
	private string resPath = "/StreamingAssets/AssetBundle/";

    private static ResManager Instance;

    Dictionary<string, WWW> m_Downloads = new Dictionary<string, WWW>();
	Dictionary<string, Texture2D> m_IconList = new Dictionary<string, Texture2D>();
	Dictionary<string, UnityEngine.Object> m_Resources = new Dictionary<string, UnityEngine.Object>();
	Dictionary<string, UnityEngine.AssetBundleCreateRequest> m_Resource_Requests = new Dictionary<string, UnityEngine.AssetBundleCreateRequest>();
	
	
	// Use this for initialization
	void Start ()
    {
        Instance = this;
//		PlayerPrefs.DeleteAll();
	}

	// Update is called once per frame
	void Update ()
    {
	
	}
	public static ResManager GetInstance(){
		return Instance;
	}

    public IEnumerator CoroutineDownload(Version version,string resUrl)
    {
		String url = "";
        // When the level is being downloaded already, wait for it to finish.
        if (m_Downloads.ContainsKey(resUrl))
        {
            while (!m_Downloads[resUrl].isDone)
            {
                yield return null;
            }

            yield break;
        }
        // Otherwise start a new download.
        else
        {
            // Wait for the Caching system to be ready
            while (!Caching.ready)
            {
                yield return null;
            }

            // Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
//            if (Application.isEditor)
//            {
//				url = "file://" + Application.dataPath + "/NetResources/" + resUrl;
//            }
//            else
//            {
                url = Config.FileServerURL + Config.FileDownInterface  + "Version" + version.ToMyString() + resPath + resUrl;
//            }
			Debug.Log(url);
            WWW www = new WWW(url);
			curDownloadWWW = www;
            yield return www;
            if (www.error != null)
            {
                throw new Exception("WWW download had an error:" + www.error);
            }
            else
            {
				
				Instance.m_Downloads[resUrl] = www;
				
				Debug.Log("download" + resUrl + " succes!");
				FileHelper file = new FileHelper();
				
				string filePath = FileWriteBaseUrl + resUrl;
				Debug.Log(filePath);
				int index = filePath.LastIndexOf('/');
				string dirPath = filePath.Substring(0,index);
				if(!Directory.Exists(dirPath)){
					Directory.CreateDirectory(dirPath);
				}
				file.WriteNewFile(www.bytes,filePath);
            }
            yield break;
        }
    }
	public static byte[] LoadXmlData(string resUrl){
		string filePath = "" + FileWriteBaseUrl + resUrl + ".xml";
		byte[] datas = null;
		try{
			datas = FileHelper.ReadFile(filePath);
		}catch(Exception e){
			Debug.Log(e.ToString());
		}
//		if(datas == null){
//			datas = (Resources.Load(resUrl) as TextAsset).bytes;
//		}
		return datas;
	}
	public IEnumerator Load(string resUrl,ResGameObject target){
//		WWW www = null;
//       
//		if (m_Resources.ContainsKey(resUrl))
//        {
//			target.obj = m_Resources[resUrl];
//			yield break;
//        }
//        else
//        {
//			string filePath = FileWriteBaseUrl + resUrl;
			//res in Resources or assetBundle
			
			//1-------------www load assetBundle
//			if(target.resType == ResGameObject.TYPE_BOUNLD){
//				filePath += ".unity3d";
//			}else if(target.resType == ResGameObject.TYPE_TEXTURE){
//				filePath += ".png";
//			}
//			Debug.Log(filePath);
//			
//			www = new WWW(filePath);
//			
//			yield return www;
//			if (www == null || www.error != null)
//	        {
//				target.obj = Resources.Load(resUrl);
//			}else{
//				if(target.resType == ResGameObject.TYPE_BOUNLD){
//					AssetBundle bundle = www.assetBundle;
//					String[] name = resUrl.Split('/');
//					
//					target.obj = bundle.Load(name[name.Length - 1]);
//				}else if(target.resType == ResGameObject.TYPE_TEXTURE){
//					target.obj = www.texture;
//				}
//			}
			
			//2-------------resources load res 
//			target.obj = Resources.Load(resUrl);
			
		//3-------------assetBundle from file
		if(!isResourcesPath)  
		{ 
			AssetBundleCreateRequest request = null;
			if (m_Resource_Requests.ContainsKey(resUrl))
	        {
				if(m_Resource_Requests[resUrl] != null){
		            while (!m_Resource_Requests[resUrl].isDone)
		            {
		                yield return null;
		            }
				}
	        }
	        else
	        {
				yield return StartCoroutine(InitRes(resUrl));
			}
			request = m_Resource_Requests[resUrl];
			if(request != null){
				AssetBundle bundle = request.assetBundle;
	
				target.obj = bundle.mainAsset;
			}else{
				target.obj = null;
			}
			
			yield break;

		}else{
			target.obj = Resources.Load(resUrl);
		}
	}
//	Texture2D mTexture;
//	public void OnGUI(){
//		if(mTexture != null){
//			GUI.DrawTexture(new Rect(10,10,200,200),mTexture);
//		}
//	}
	public IEnumerator LoadPlayerIcon(uint pid,string iconName,ResGameObject target){
		if(iconName.Equals("default") || iconName.Equals("nan1")){
			yield return StartCoroutine( Load("Icon/" + iconName,target));
		}else{
			string url = pid + "/" + iconName;
			string localUrl =  FileWriteBaseUrl + "/img/" + url + ".png";
			try{
				Texture2D tex = new Texture2D(1, 1);
				byte[] rawJPG = File.ReadAllBytes(localUrl);
				tex.LoadImage(rawJPG);
				
				target.obj = tex;
				m_IconList[url] = tex;
			}catch(Exception e){
				Debug.Log(e);
				target.obj = null;
			}
			if(target.obj == null){
				string netUrl = Config.FileServerURL + Config.FileUploadInterface + ""+ url;
				WWW www = new WWW(netUrl);
				yield return www;
				if (www.error != null)
		        {
		            www = null;
		        }
		        else
		        {
					Debug.Log("download" + url + " succes!");
					SavePlayerIcon(pid,iconName,www.bytes);
					target.obj = www.texture;
				}
			}

		}
	}
	public void SavePlayerIcon(uint pid,string iconName,byte[] datas){
		string path = FileWriteBaseUrl+"img/" + pid + "/";
		if(!Directory.Exists(path)){
			Directory.CreateDirectory(path);
		}
		FileHelper file = new FileHelper();
		string filePath = path + iconName + ".png";
		file.WriteNewFile(datas,filePath);
	}
	private IEnumerator InitRes(string resUrl){
		if(!isResourcesPath)  
		{ 
			if(!m_Resource_Requests.ContainsKey(resUrl)){
				string filePath = FileWriteBaseUrl + resUrl;
				
				filePath += ".unity3d";
//				Debug.Log(filePath);
				byte[] datas = null;
				datas = FileHelper.ReadFile(filePath);

				if(datas == null){
					filePath = AssetbundleBaseURL + resUrl;
					
					filePath += ".unity3d";
					Debug.Log(filePath);
					datas = FileHelper.ReadFile(filePath);
				}
				AssetBundleCreateRequest request = null;
				if(datas != null){
					request = AssetBundle.CreateFromMemory(datas);
					m_Resource_Requests[resUrl] = request;
				}else{
					m_Resource_Requests[resUrl] = null;
				}
				yield return request;
			}
		}
	}
	public void UnLoadAllBundle(){
		foreach(AssetBundleCreateRequest request in m_Resource_Requests.Values){
			request.assetBundle.Unload(true);
		}
		m_Resources.Clear();
	}
	private WWW curDownloadWWW = null;
	private bool isDownloadEnd = false;
	public WWW GetCurDownloadWWW(){
		return curDownloadWWW;
	}
	public bool IsDownloadEnd(){
		return isDownloadEnd;
	}
	int downloadResCount;
	int downloadIndex;
	public float GetDownloadPro(){
		float pro = 0.0f;
		if(curDownloadWWW != null){
			pro = (downloadIndex * 1.0f * curDownloadWWW.progress)/(downloadResCount * 1.0f);
		}
		if(pro > 1.0f){
			pro = 1.0f;
		}
		return pro;
	}
	Version serverVersion = new Version();
	Version clientVersion = new Version();
	public IEnumerator JudgeVersion(){
//		PlayerPrefs.DeleteAll();
		
		if(!isResourcesPath)  
		{ 
			Debug.Log("JudgeVersion!");
			
			clientVersion = new Version();
			clientVersion.version1 = PlayerPrefs.GetInt("version1",1);
			clientVersion.version2 = PlayerPrefs.GetInt("version2",0);
			clientVersion.version3 = PlayerPrefs.GetInt("version3",1);
			yield return StartCoroutine(GetServerVersion());
			
			UpdateState state = JudUpdateState();
			Debug.Log("Version=" + serverVersion.ToMyString());
			if(state == UpdateState.DynamiUpdate){
				clientVersion = serverVersion;
				isDownloadEnd = false;
				
				yield return StartCoroutine(CoroutineDownload(clientVersion,"Version/newRes_"+clientVersion.ToMyString()+".unity3d"));
				ResUrl newObj = new ResUrl();
	//			List<object> datas = XmlHelper.getContentsByFiledName(newObj,"Version/newRes_" + severVersion);
				XmlHelper xmlHelper = XmlHelper.GetInstance();
				yield return StartCoroutine(xmlHelper.getContentsByFiledName(newObj,"Version/newRes_" + serverVersion.ToMyString()));
				List<object> datas = xmlHelper.alList;
				downloadResCount = datas.Count;
				downloadIndex = 0;
				foreach(object obj in datas){
					newObj = (ResUrl)obj;
					yield return StartCoroutine(CoroutineDownload(clientVersion,newObj.url+ ".unity3d"));
					downloadIndex++;
				}
				PlayerPrefs.SetInt("version1",clientVersion.version1);
				PlayerPrefs.SetInt("version2",clientVersion.version2);
				PlayerPrefs.SetInt("version3",clientVersion.version3);
				isDownloadEnd = true;
				curDownloadWWW = null;
			}else if(state == UpdateState.ForceUpdate){
				isDownloadEnd = true;
				curDownloadWWW = null;
			}
			else{
				isDownloadEnd = true;
				curDownloadWWW = null;
			}
		}else{
			isDownloadEnd = true;
			curDownloadWWW = null;
			yield break;
		}
	}
	
	public IEnumerator GetServerVersion(){
		WWW www = new WWW(Config.LoginServerURL + Config.GetVersionInterface);
		yield return www;
		if(www.error != null && !www.error.Equals("")){
			Debug.Log(www.error);
//			TipManager.GetInstance().ShowTip(20054);
		}else{
			LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject( www.text);
			serverVersion.version1 = (byte)jsonData["v1"];
			serverVersion.version2 = (byte)jsonData["v2"];
			serverVersion.version3 = (short)jsonData["v3"];
		}
	}
	
	public UpdateState JudUpdateState(){
		if(clientVersion.version1 < serverVersion.version1){
			return UpdateState.ForceUpdate;
		}
		if(clientVersion.version2 < serverVersion.version2){
			return UpdateState.ForceUpdate;
		}
		if(clientVersion.version3 < serverVersion.version3){
			return UpdateState.DynamiUpdate;
		}
		return UpdateState.NoUpdata;
	}
	int copyResCount;
	int copyIndex;
	public float GetCopyPro(){
		float pro = 0.0f;
		pro = (copyIndex * 1.0f)/(copyResCount * 1.0f);
		if(pro > 1.0f){
			pro = 1.0f;
		}
		return pro;
	}
	public IEnumerator CopyAssetBundleToDocuments(){
//		PlayerPrefs.DeleteAll();
		if(!isResourcesPath)  
		{
//			if(!Directory.Exists(FileWriteBaseUrl)){
//				Directory.CreateDirectory(FileWriteBaseUrl);
//			}
//			yield return StartCoroutine(CopyFileToFile(AssetbundleBaseURL + "" + "resXml.unity3d",FileWriteBaseUrl + ""+ "resXml.unity3d"));
//			ResUrl newObj = new ResUrl();
//			
//			XmlHelper xmlHelper = XmlHelper.GetInstance();
//			yield return StartCoroutine(xmlHelper.getContentsByFiledName(newObj,"resXml"));
//			List<object> datas = xmlHelper.alList;
			copyResCount = 1900;
//			Debug.Log("copyIndex" + copyIndex);
//			m_Resource_Requests.Remove("resXml");
			initIndex = 0;
			int isCopyFile = PlayerPrefs.GetInt("isCopyFile",0);
			if(isCopyFile == 0){
				isCopyFileEnd = false;
				yield return StartCoroutine(CopyPathToPath(AssetbundleBaseURL + "",FileWriteBaseUrl + ""));
				isCopyFileEnd = true;
				isCopyFile = 1;
				PlayerPrefs.SetInt("isCopyFile",isCopyFile);
			}else{
				isCopyFileEnd = true;
				yield break;
			}
		}else{
			isCopyFileEnd = true;
		}
	}
	public IEnumerator CopyPathToPath(string orgPath,string newDir){
		if(!Directory.Exists(newDir)){
			Directory.CreateDirectory(newDir);
		}
		string[] fileEntries = Directory.GetFiles(orgPath);
		string[] fileDirectorys = Directory.GetDirectories(orgPath);
		
		foreach(string fileName in fileEntries){
            string filePath = fileName.Replace("\\", "/");
//            int index = filePath.LastIndexOf("/");
//            filePath = filePath.Substring(index);
			string path = filePath.Substring(orgPath.Length);
            string localPath = newDir + path;
            yield return StartCoroutine(CopyFileToFile(filePath,localPath));
			copyIndex ++;
		}
		foreach(string fileDir in fileDirectorys){
			string fileDirPath = fileDir.Replace("\\", "/");
			string pathDir = fileDirPath.Substring(orgPath.Length);
			string localDirPath = newDir + pathDir;
			yield return StartCoroutine(CopyPathToPath(fileDirPath,localDirPath));
		}
	}
	public IEnumerator CopyFileToFile(string orgFile,string newFile){
		if(FilterFile(orgFile)){
//			yield return true;
		}else{
			Debug.Log("Copy:"+orgFile+"-To-"+newFile);
	//		byte[] datas = null;
	//		datas = FileHelper.ReadFile(orgFile);
			
			curCopyFileWWW = new WWW("file:" + orgFile);
	        yield return curCopyFileWWW;
	        if (curCopyFileWWW.error != null)
	        {
	            throw new Exception("WWW download had an error:" + curCopyFileWWW.error);
	        }
	        else
	        {
				FileHelper file = new FileHelper();
				file.WriteNewFile(curCopyFileWWW.bytes,newFile);
	        }
			curCopyFileWWW = null;
	        yield break;
		}
	}
	private WWW curCopyFileWWW = null;
	public WWW GetCurCopyFileWWW(){
		return curCopyFileWWW;
	}
	private bool isCopyFileEnd = false;
	public bool IsCopyFileEnd(){
		return isCopyFileEnd;
	}
	static string[] filters = new string[]
    {//过滤不打包的文件类型.
        ".log",
        ".db",
		".db",
		".meta",
    };
	static bool FilterFile(string fileName){
		
		//过滤文件类型.
        foreach (string s in filters)
        {
            if (fileName.EndsWith(s, System.StringComparison.OrdinalIgnoreCase))
            {
				return true;
            }
        }
		return false;
	}
	class ResUrl{
		public string url = "";
	}
	int initResCount;
	int initIndex;
	public float GetInitPro(){
		float pro = 0.0f;
		pro = (initIndex * 1.0f)/(initResCount * 1.0f);
		if(pro > 1.0f){
			pro = 1.0f;
		}
		return pro;
	}
	public void SetInitResCount(int count){
		initResCount = count;
		initIndex = 0;
	}
	public void SetInitIndex(int index){
		initIndex = index;
	}
	public void AddInitIndex(){
		initIndex++;
	}
	public IEnumerator InitAtlasAndPrefabs(){
		initResCount = 20;
		initIndex = 0;
//		if(!isResourcesPath)  
//		{
//			ResUrl newObj = new ResUrl();
//				
//			XmlHelper xmlHelper = XmlHelper.GetInstance();
//			yield return StartCoroutine( xmlHelper.getContentsByFiledName(newObj,"AtlasAndPrefabs"));
//			List<object> datas = xmlHelper.alList;
//			initResCount = datas.Count + 20;
//			initIndex = 0;
//			foreach(object obj in datas){
//				newObj = (ResUrl)obj;
//				yield return StartCoroutine(InitRes(newObj.url));
//				initIndex ++;
//			}
//		}
		yield break;
	}


//	public IEnumerator InitLoadAtalsAndPrefabs(){
//		if(!isResourcesPath)  
//		{
//			yield return StartCoroutine(InitRes("Atlas/Icon/IconAtlas"));
//			yield return StartCoroutine(InitRes("Atlas/Loading/LoadingAtlas"));
//			yield return StartCoroutine(InitRes("Atlas/UI/UIAtlas"));
//			yield return StartCoroutine(InitRes("Fonts/3698/3699Font"));
//			yield return StartCoroutine(InitRes("Animations/Common/JhWindowLeftAppear"));
//			yield return StartCoroutine(InitRes("Animations/Common/JhWindowLeftLeave"));
//			yield return StartCoroutine(InitRes("Animations/Common/JhWindowRightAppear"));
//			yield return StartCoroutine(InitRes("Animations/Common/JhWindowRightLeave"));
//			yield break;
//		}
//	}
	public static UnityEngine.Object LoadExist(string resUrl){
		/*if(!Instance.isResourcesPath)  
		{ 
			AssetBundle bundle = Instance.m_Resource_Requests[resUrl].assetBundle;
			return bundle.mainAsset;
		}else{*/
			return Resources.Load(resUrl);
		//}
	}
	public static UnityEngine.Object LoadExistByCatch(string resUrl){
//		if(!Instance.isResourcesPath)  
//		{ 
//			AssetBundle bundle = Instance.m_Resource_Requests[resUrl].assetBundle;
//			return bundle.mainAsset;
//		}else{
			return Resources.Load(resUrl);
//		}
	}
}
