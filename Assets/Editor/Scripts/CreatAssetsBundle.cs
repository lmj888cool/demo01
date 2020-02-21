using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreatAssetsBundle : Editor
{
    //[MenuItem("Assets/CreatAssetsBundle")]
    //static void f_BuilAB()
    //{
    //    BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    //}
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string dir = Application.streamingAssetsPath;// "AssetBundles";
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
#if UNITY_EDITOR
        //BuildPipeline.BuildAssetBundles(dir, //路径必须创建
        //    BuildAssetBundleOptions.ChunkBasedCompression, //压缩类型
        //    BuildTarget.StandaloneWindows64);//平台
#endif
#if UNITY_ANDROID
            BuildPipeline.BuildAssetBundles(dir, //路径必须创建
            BuildAssetBundleOptions.ChunkBasedCompression, //压缩类型
            BuildTarget.Android);//平台
#endif
#if UNITY_IOS
                BuildPipeline.BuildAssetBundles(dir, //路径必须创建
            BuildAssetBundleOptions.ChunkBasedCompression, //压缩类型
            BuildTarget.iPhone);//平台
#endif
    }
}
