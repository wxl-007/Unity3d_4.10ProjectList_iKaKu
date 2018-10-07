using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// 对文件和文件夹的操作.
    /// </summary>
    public class FileHelper
    {

//        #region bool Check_File(string FilePath,string File_Folder,bool Create) 检查文件或者文件目录是否存在，根据参数判断是否创建文件目录[不创建文件]
//        /// <summary>
//        /// 检查文件||文件目录是否存在， 根据参数判断是否创建文件||文件目录
//        /// </summary>
//        /// <param name="FilePath">路径</param>
//        /// <param name="File_Folder"> file || folder </param>
//        /// <param name="Create">true 创建 false 不创建</param>
//        /// <returns></returns>
//        public bool Check_File(string FilePath, string File_Folder, bool Create)
//        {
//            StringHelper m_clsStringHelper = new StringHelper();
//
//            if (m_clsStringHelper.CheckNullstr(FilePath) == false)        //如果路径为空
//                return false;
//            else
//            {
//                try
//                {
//                    if (File_Folder == "file")                      //如果是判断文件是否存在
//                    {
//                        if (File.Exists(FilePath) == false)         //文件不存在
//                            return false;
//                        else
//                            return true;
//                    }
//                    else                                                //文件夹是否存在
//                    {
//                        if (Directory.Exists(FilePath) == false)         //文件不存在
//                        {
//                            if (Create == true)
//                            {
//                                Directory.CreateDirectory(FilePath);        //要创建一个
//                                return true;
//                            }
//                            else
//                                return false;
//                        }
//                        else
//                            return true;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    return false;
//                }
//            }
//        }
//        #endregion
//
//        #region void WriteFileFromFile(string strReadFilePath,string strWriteFilePath) 从一个文件读数据写入另一个新文件中
//        /// <summary>
//        /// 从一个文件读数据写入另一个新文件中
//        /// </summary>
//        /// <param name="strReadFilePath">要读取得文件路径和文件名</param>
//        /// <param name="strWriteFilePath">要写入的文件路径和文件名</param>
//        public void WriteFileFromFile(string strReadFilePath, string strWriteFilePath)
//        {
//            FileInfo fileRead = new FileInfo(strReadFilePath);
//            FileStream objRead = fileRead.OpenRead();
//
//            long lBytesLength = objRead.Length;
//            byte[] ByteArray = new byte[lBytesLength];
//            int nBytesRead = objRead.Read(ByteArray, 0, (int)lBytesLength);
//
//            FileInfo fileWrite = new FileInfo(strWriteFilePath);
//
//            FileStream objWrite = null;
//
//            if (fileWrite.Exists)
//            {
//                fileWrite.Delete();
//            }
//
//            objWrite = fileWrite.OpenWrite();
//
//            objWrite.Write(ByteArray, 0, (int)lBytesLength);
//
//            objWrite.Flush();
//
//            objWrite.Close();
//            objRead.Close();
//        }
//
//        #endregion

        #region void WriteNewFile(string strContent,string strWriteFilePath) 将字符串的内容写入到新的文件中
        /// <summary>
        /// 将字符串的内容写入到新的文件中.
        /// </summary>
        /// <param name="ByteArray">要写入的内容.</param>
        /// <param name="strWriteFilePath">要写入的文件路径和文件名.</param>
        public void WriteNewFile(byte[] ByteArray, string strWriteFilePath)
        {
//            System.Text.Encoding Code = System.Text.Encoding.GetEncoding("gb2312");
            //			Encoding Code = Encoding.Unicode ;

//            byte[] ByteArray = Code.GetBytes(strContent);

            int iBytesLength = ByteArray.Length;

            FileInfo fileWrite = new FileInfo(strWriteFilePath);
            FileStream objWrite = null;

            if (fileWrite.Exists)
            {
                fileWrite.Delete();
            }
            objWrite = fileWrite.OpenWrite();

            objWrite.Write(ByteArray, 0, iBytesLength);

            objWrite.Flush();
            objWrite.Close();

        }
		public void WriteNewFile(string strContent, string strWriteFilePath)
        {
            System.Text.Encoding Code = System.Text.Encoding.GetEncoding("utf-8");

            byte[] ByteArray = Code.GetBytes(strContent);
			WriteNewFile(ByteArray,strWriteFilePath);
		}

        #endregion

        #region void WriteAppendFile(string strContent,string strWriteFilePath) 将字符串的内容继续添加到已有文件中
        /// <summary>
        /// 将字符串的内容继续添加到已有文件中.
        /// </summary>
        /// <param name="strContent">要写入的内容.</param>
        /// <param name="strWriteFilePath">要写入的文件路径和文件名.</param>
        public static void WriteLog(string strContent, string strWriteFilePath)
        {
            System.Text.Encoding Code = System.Text.Encoding.GetEncoding("gb2312");
            //			Encoding Code = Encoding.Unicode ;

            byte[] ByteArray = Code.GetBytes(DateTime.Now.ToString() + " …… " + strContent + " \r\n");

            int iLength = ByteArray.Length;

            FileStream fs = new FileStream(strWriteFilePath, FileMode.Append);

            for (int i = 0; i < iLength; i++)
            {
                fs.WriteByte(ByteArray[i]);
            }

            fs.Flush();
            fs.Close();

        }

        #endregion

        #region bool UploadFile(FileUpload FileUpload1, string filePath, out string terminateFileName, out string resultDesc, bool isRename) 上传文件

//        /// <summary>
//        /// 上传文件
//        /// </summary>
//        /// <param name="FileUpload1">FileUpload控件</param>
//        /// <param name="filePath">文件上传的虚拟路径</param>
//        /// <param name="terminateFileName">上传后文件所在的服务器文件名</param>
//        /// <param name="resultDesc">生成结果描述</param>
//        /// <param name="isRename">是否改名</param>
//        /// <returns>执行结果信息</returns>
//        public bool UploadFile(FileUpload FileUpload1, string filePath, out string terminateFileName, out string resultDesc, bool isRename)
//        {
//            terminateFileName = "";
//
//            if (FileUpload1.HasFile)
//            {
//                string clientFilePath = FileUpload1.PostedFile.FileName.Trim();     // 客户端文件路径
//
//                #region 判断上传的文件是否可支持的文件
//
//                bool bUp = false;
//                string[] arUpload = Functions.GetAllowUploadFileType().ToLower().Split('|');
//
//                for (int i = 0; i < arUpload.Length; i++)
//                {
//                    if (arUpload[i].IndexOf(clientFilePath.Substring(clientFilePath.Length - 4).ToLower()) != -1)
//                    {
//                        bUp = true;
//                        break;
//                    }
//                }
//
//                if (!bUp)
//                {
//                    resultDesc = "系统只能支持" + arUpload.ToString() + "类型的文件上传";
//
//                    return false;
//                }
//
//                #endregion
//
//                FileInfo file = new FileInfo(clientFilePath);
//
//                if (isRename)   //改名
//                {
//                    Random rand = new Random();
//                    terminateFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + rand.Next(1000, 2000).ToString() + clientFilePath.Substring(clientFilePath.Length - 4);     //使用随即唯一的文件名
//                }
//                else
//                    terminateFileName = file.Name;                                    //文件名称
//                string fileName = terminateFileName;
//
//                StringHelper m_StringHelper = new StringHelper();
//
//                string webFilePath = filePath + fileName;        // 服务器端文件路径
//
//                if (!Directory.Exists(filePath))
//                    Directory.CreateDirectory(filePath);
//
//                if (!File.Exists(webFilePath))
//                {
//                    try
//                    {
//                        FileUpload1.SaveAs(webFilePath);                                // 使用 SaveAs 方法保存文件
//
//                        resultDesc = "提示：文件“" + fileName + "”成功上传";
//                    }
//                    catch (Exception ex)
//                    {
//                        resultDesc = "提示：文件上传失败，失败原因：" + ex.Message;
//
//                        return false;
//                    }
//                }
//                else
//                {
//                    resultDesc = "提示：文件已经存在，请重命名后上传";
//                }
//
//            }
//            else
//            {
//                resultDesc = "";
//                return false;
//            }
//
//            return true;
//        }
//
//        #endregion
//
//        #region bool UploadPicture(FileUpload FileUpload1, string filePath, out string terminateFileName, out string resultDesc, bool isRename, bool isAddShuiYin) 上传图片
//
//        /// <summary>
//        /// 上传图片
//        /// </summary>
//        /// <param name="FileUpload1">上传控件</param>
//        /// <param name="filePath">文件全路径</param>
//        /// <param name="terminateFileName">生成的文件名</param>
//        /// <param name="resultDesc">生成结果描述</param>
//        /// <param name="isRename">是否改名</param>
//        /// <param name="isAddShuiYin">是否生成带水印的图</param>
//        /// <returns>String 生成结果</returns>
//        public bool UploadPicture(FileUpload FileUpload1, string filePath, out string terminateFileName, out string resultDesc, bool isRename, bool isAddShuiYin)
//        {
//            terminateFileName = "";
//
//            if (FileUpload1.HasFile)
//            {
//                if (!Directory.Exists(filePath))
//                    Directory.CreateDirectory(filePath);
//
//                string clientFilePath = FileUpload1.PostedFile.FileName.Trim();     // 客户端图片路径
//
//                #region 判断上传的文件是否可支持的文件
//
//                bool bUp = false;
//                string[] arUpload = Functions.GetAllowUploadPictureType().ToLower().Split('|');
//
//                for (int i = 0; i < arUpload.Length; i++)
//                {
//                    if (arUpload[i].IndexOf(clientFilePath.Substring(clientFilePath.Length - 4).ToLower()) != -1)
//                    {
//                        bUp = true;
//                        break;
//                    }
//                }
//
//                if (!bUp)
//                {
//                    resultDesc = "系统只能支持" + arUpload.ToString() + "类型的文件上传";
//                    return false;
//                }
//
//                #endregion
//
//                FileInfo file = new FileInfo(clientFilePath);
//
//                if (isRename)
//                {
//                    Random rand = new Random();
//                    terminateFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + rand.Next(100, 200).ToString() + clientFilePath.Substring(clientFilePath.Length - 4);     //使用随即唯一的文件名
//                }
//                else
//                    terminateFileName = file.Name;
//
//                string fileName = terminateFileName;                                    //文件名成 
//                string fileName_s = "s_" + fileName;                           // 缩略图文件名称
//                string fileName_sy = "sy_" + fileName;                         // 水印图文件名称（文字）
//                string fileName_syp = "syp_" + fileName;                       // 水印图文件名称（图片）
//                string fileName_sypf = "shuiyin.jpg";
//
//
//
//                string webFilePath = filePath + fileName;
//                string webFilePath_s = filePath + fileName_s;
//                string webFilePath_sy = filePath + fileName_sy;
//                string webFilePath_syp = filePath + fileName_syp;
//                string webFilePath_sypf = filePath + fileName_sypf;
//
//                //if (!File.Exists(webFilePath))
//                //{
//                try
//                {
//                    FileUpload1.SaveAs(webFilePath);                // 使用 SaveAs 方法保存文件
//
//                    resultDesc = "提示：文件“" + fileName + "”成功上传，文件大小为：" + FileUpload1.PostedFile.ContentLength + "B.";
//
//                    if (isAddShuiYin)
//                    {
//                        AddShuiYinWord(webFilePath, webFilePath_sy);
//                        AddShuiYinPic(webFilePath, webFilePath_syp, webFilePath_sypf);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    resultDesc = "提示：文件上传失败，失败原因：" + ex.Message;
//
//                    return false;
//                }
//                //}
//                //else
//                //{
//                //    return "提示：文件已经存在，请重命名后上传";
//                //}
//            }
//            else
//            {
//                resultDesc = "没有选择上传的文件!";
//                return false;
//            }
//
//            return true;
//        }
//
//        #endregion
//
//        #region void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode) 生成缩略图
//        /**/
//        /// <summary>
//        /// 生成缩略图
//        /// </summary>
//        /// <param name="originalImagePath">源图路径（物理路径）</param>
//        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
//        /// <param name="width">缩略图宽度</param>
//        /// <param name="height">缩略图高度</param>
//        /// <param name="mode">生成缩略图的方式 HW指定高宽缩放（可能变形），W 指定宽，高按比例，H 指定高，宽按比例，Cut 指定高宽裁减（不变形）</param>    
//        public void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
//        {
//            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
//
//            int towidth = width;
//            int toheight = height;
//
//            int x = 0;
//            int y = 0;
//            int ow = originalImage.Width;
//            int oh = originalImage.Height;
//
//            switch (mode)
//            {
//                case "HW"://指定高宽缩放（可能变形）
//                    break;
//                case "W"://指定宽，高按比例                    
//                    toheight = originalImage.Height * width / originalImage.Width;
//                    break;
//                case "H"://指定高，宽按比例
//                    towidth = originalImage.Width * height / originalImage.Height;
//                    break;
//                case "Cut"://指定高宽裁减（不变形）
//                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
//                    {
//                        oh = originalImage.Height;
//                        ow = originalImage.Height * towidth / toheight;
//                        y = 0;
//                        x = (originalImage.Width - ow) / 2;
//                    }
//                    else
//                    {
//                        ow = originalImage.Width;
//                        oh = originalImage.Width * height / towidth;
//                        x = 0;
//                        y = (originalImage.Height - oh) / 2;
//                    }
//                    break;
//                default:
//                    break;
//            }
//
//
//            //新建一个bmp图片
//            System.Drawing.Image bitmap_old = new System.Drawing.Bitmap(towidth, toheight);
//            //复制新的bitmap，防止被锁死
//            System.Drawing.Image bitmap = new Bitmap(bitmap_old.Width, bitmap_old.Height);
//
//            //新建一个画板
//            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
//
//            //设置高质量插值法
//            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
//
//            //设置高质量,低速度呈现平滑程度
//            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
//
//            //清空画布并以透明背景色填充
//            g.Clear(System.Drawing.Color.Transparent);
//
//
//
//            //在指定位置并且按指定大小绘制原图片的指定部分
//            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
//                new System.Drawing.Rectangle(x, y, ow, oh),
//                System.Drawing.GraphicsUnit.Pixel);
//
//
//            //Rectangle expansionRectangle = new Rectangle(135, 10, 60, 60);
//            //Rectangle compressionRectangle = new Rectangle(300, 10, 60, 60);
//            //g.DrawImage(originalImage, 10, 10);
//            //g.DrawImage(originalImage, expansionRectangle);
//            //g.DrawImage(originalImage, compressionRectangle);
//
//
//
//            try
//            {
//                //以jpg格式保存缩略图
//                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png);
//            }
//            catch (System.Exception e)
//            {
//                throw e;
//            }
//            finally
//            {
//                originalImage.Dispose();
//                bitmap.Dispose();
//                g.Dispose();
//            }
//        }
//
//        #endregion
//
//        #region void AddShuiYinWord(string Path, string Path_sy) 在图片上增加文字水印
//        /**/
//        /// <summary>
//        /// 在图片上增加文字水印
//        /// </summary>
//        /// <param name="Path">原服务器图片路径</param>
//        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
//        private void AddShuiYinWord(string Path, string Path_sy)
//        {
//            string addText = "测试水印";
//            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
//            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
//            g.DrawImage(image, 0, 0, image.Width, image.Height);
//            System.Drawing.Font f = new System.Drawing.Font("Verdana", 16);
//            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
//
//            g.DrawString(addText, f, b, 15, 15);
//            g.Dispose();
//
//            image.Save(Path_sy);
//            image.Dispose();
//        }
//
//        #endregion
//
//        #region void AddShuiYinPic(string Path, string Path_syp, string Path_sypf) 在图片上生成图片水印
//        /**/
//        /// <summary>
//        /// 在图片上生成图片水印
//        /// </summary>
//        /// <param name="Path">原服务器图片路径</param>
//        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
//        /// <param name="Path_sypf">水印图片路径</param>
//        protected void AddShuiYinPic(string Path, string Path_syp, string Path_sypf)
//        {
//            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
//            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
//            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
//            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
//            g.Dispose();
//
//            image.Save(Path_syp);
//            image.Dispose();
//        }
//
//        #endregion
//
//        #region void MakeBlackWhitePicture(string originalImagePath, string thumbnailPath)
//        /// <summary>
//        /// 将彩色图片转换成黑白图片
//        /// </summary>
//        /// <param name="originalImagePath">原彩图的路径</param>
//        /// <param name="thumbnailPath">要生成的黑白图片</param>
//        public void MakeBlackWhitePicture(string originalImagePath, string thumbnailPath)
//        {
//            Bitmap source = new Bitmap(originalImagePath);
//
//            Bitmap bm = new Bitmap(source.Width, source.Height);
//
//            for (int y = 0; y < bm.Height; y++)
//            {
//                for (int x = 0; x < bm.Width; x++)
//                {
//                    Color c = source.GetPixel(x, y);
//
//                    int luma = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
//
//                    bm.SetPixel(x, y, Color.FromArgb(luma, luma, luma));
//                }
//            }
//
//            try
//            {
//                //以jpg格式保存缩略图
//                bm.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Icon);
//            }
//            catch (System.Exception e)
//            {
//                throw e;
//            }
//            finally
//            {
//                bm.Dispose();
//            }
//            //return bm; 
//        }
//
//        #endregion
//
//        #region static void EidtName(string path, string oldname, string newname, int type) 修改文件名(文件夹)名称（静态方法,专为新闻发布控件使用）
//        /// <summary>
//        /// 修改文件名(文件夹)名称
//        /// </summary>
//        /// <param name="path">路径</param>
//        /// <param name="oldname">原始名称</param>
//        /// <param name="newname">新名称</param>
//        /// <param name="type">0为文件夹,1为文件</param>
//        /// <returns>成功返回1</returns>
//
//        public static void EidtName(string path, string oldname, string newname, int type)
//        {
//            if (type == 0)
//            {
//                if (Directory.Exists(path + "\\" + oldname))
//                {
//                    try
//                    {
//                        Directory.Move(path + "\\" + oldname, path + "\\" + newname.Replace(".", ""));
//                    }
//                    catch (IOException e)
//                    {
//                        throw new Exception(e.ToString());
//                    }
//                }
//                else
//                {
//                    throw new Exception("参数传递错误!");
//                }
//            }
//            else
//            {
//                if (File.Exists(path + "\\" + oldname))
//                {
//                    try
//                    {
//                        File.Move(path + "\\" + oldname, path + "\\" + newname);
//                    }
//                    catch (Exception e)
//                    {
//                        throw new Exception(e.ToString());
//                    }
//                }
//                else
//                {
//                    throw new Exception("参数传递错误!");
//                }
//            }
//        }
//
//        #endregion
//
//        #region static void Del(string path, int type) 删除文件或文件夹（静态方法,专为新闻发布控件使用）
//        /// <summary>
//        /// 删除文件或文件夹
//        /// </summary>
//        /// <param name="path">路径</param>
//        /// <param name="filename">名称</param>
//        /// <param name="type">0代表文件夹,1代表文件</param>
//        /// <returns>返回值</returns>
//        public static void Del(string path, int type)
//        {
//            if (type == 0)
//            {
//                if (Directory.Exists(path))
//                {
//                    try
//                    {
//                        Directory.Delete(path, true);
//                    }
//                    catch (Exception e)
//                    {
//                        throw new IOException(e.ToString());
//                    }
//                }
//                else
//                {
//                    throw new IOException("参数错误!" + path);
//                }
//            }
//            else
//            {
//                if (File.Exists(path))
//                {
//                    try
//                    {
//                        File.Delete(path);
//                    }
//                    catch (Exception e)
//                    {
//                        throw new IOException(e.ToString());
//                    }
//                }
//                else
//                {
//                    throw new IOException("参数错误!");
//                }
//            }
//        }
//
//        #endregion
//
//        #region static void AddDir(string path, string filename) 添加文件夹（静态方法,专为新闻发布控件使用）
//        /// <summary>
//        /// 添加文件夹
//        /// </summary>
//        /// <param name="path">当前路径</param>
//        /// <param name="filename">文件夹名称</param>
//        /// <returns></returns>
//
//        public static void AddDir(string path, string filename)
//        {
//            if (Directory.Exists(path + "\\" + filename))
//            {
//                throw new IOException("此文件夹已存在!");
//            }
//            else
//            {
//                try
//                {
//                    Directory.CreateDirectory(path + "\\" + filename.Replace(".", ""));
//                }
//                catch (IOException e)
//                {
//                    throw new IOException(e.ToString());
//                }
//            }
//        }
//
//        #endregion
//
//        #region string[] GetDirectoryFileList(string path, string searchPattern) 获得指定目录下的文件列表
//        /// <summary>
//        /// 获得指定目录下的文件列表
//        /// </summary>
//        /// <param name="path">路径</param>
//        /// <param name="searchPattern">搜索的模糊文件名,如template_*.htm</param>
//        /// <returns></returns>
//        public string[] GetDirectoryFileList(string path, string searchPattern)
//        {
//            if (!Directory.Exists(path))
//                return new string[0];
//
//            DirectoryInfo dirInfo = new DirectoryInfo(path);
//            FileInfo[] fileInfos = dirInfo.GetFiles(searchPattern);
//            string[] result = new string[fileInfos.Length];
//            for (int i = 0; i < fileInfos.Length; i++)
//            {
//                result[i] = fileInfos[i].Name;
//            }
//
//            return result;
//        }
//
        #endregion
		/// <summary>
		/// 读取一个文件到字节流.
		/// </summary>
		/// <returns>
		/// 字节流.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='filename'>
		/// Filename.
		/// </param>
		public static byte[] ReadFile(string strReadFilePath)
		{
			byte[] ByteArray = null;
			try{
			FileInfo fileRead = new FileInfo(strReadFilePath);
            FileStream objRead = fileRead.OpenRead();

            long lBytesLength = objRead.Length;
            ByteArray = new byte[lBytesLength];
//            int nBytesRead =
					objRead.Read(ByteArray, 0, (int)lBytesLength);
			}catch(Exception e){
				Debug.Log(e.ToString());
				return null;
			}
			return ByteArray;
		}
    }
}
