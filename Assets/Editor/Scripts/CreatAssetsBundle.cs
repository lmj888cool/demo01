using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreatAssetsBundle : Editor
{
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
        //添加自定义tag
        AddTag("Enemy");
        AddTag("Hero");
        AddTag("Bomb");
        AddTag("Way");
    }
    static void AddTag(string tag)
    {
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((asset != null) && (asset.Length > 0))
        {
            SerializedObject so = new SerializedObject(asset[0]);
            SerializedProperty tags = so.FindProperty("tags");

            for (int i = 0; i < tags.arraySize; ++i)
            {
                if (tags.GetArrayElementAtIndex(i).stringValue == tag)
                {
                    return;     // Tag already present, nothing to do.
                }
            }

            tags.InsertArrayElementAtIndex(tags.arraySize);
            tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
            so.ApplyModifiedProperties();
            so.Update();
        }
    }

    static void AddLayer(string layer)
    {
        if (!isHasLayer(layer))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name.StartsWith("User Layer"))
                {
                    if (it.type == "string")
                    {
                        if (string.IsNullOrEmpty(it.stringValue))
                        {
                            it.stringValue = layer;
                            tagManager.ApplyModifiedProperties();
                            return;
                        }
                    }
                }
            }
        }
    }

    static bool isHasTag(string tag)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tag))
                return true;
        }
        return false;
    }

    static bool isHasLayer(string layer)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer))
                return true;
        }
        return false;
    }
}
