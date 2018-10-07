using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetsSettingsEditor : AssetPostprocessor{
    void OnPreprocessTexture()
    {
        if (assetPath.Contains("Texture/Auto")) {
            TextureImporter textureImporter = assetImporter as TextureImporter;
		    textureImporter.textureType = TextureImporterType.Advanced;
            textureImporter.npotScale = TextureImporterNPOTScale.None;
            textureImporter.isReadable = false;
            textureImporter.generateMipsInLinearSpace = false;
            textureImporter.textureFormat = TextureImporterFormat.RGBA32;
            textureImporter.mipmapEnabled = false;
            textureImporter.filterMode = FilterMode.Bilinear;
        }
    }
    void OnPreprocessAudio(){
        if (assetPath.Contains("2DSound")) { 
            AudioImporter audioImporter = assetImporter as AudioImporter;
            audioImporter.threeD = false;
            audioImporter.loadType = AudioImporterLoadType.CompressedInMemory;
        }
    }
}