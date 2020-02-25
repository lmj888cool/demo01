using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class GetFBXAnimation : MonoBehaviour
{
    /// <summary> 遍历出的clips </summary>
    private Dictionary<string, AnimationClip> clips;

    private AnimatorController mAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoAction(string fbxPath, string fbxName)
    {
        AnimatorController animatorOverrideController = new AnimatorController();
        //设置AnimatorController
        //animatorOverrideController.runtimeAnimatorController = mAnimator;
        //获得AnimatorController下的clips
        List<KeyValuePair<AnimationClip, AnimationClip>> animationClip = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        //animatorOverrideController.GetOverrides(animationClip);
        clips = AnimLoad(fbxPath, fbxName);
        foreach (var actionName in clips.Keys)
        {
            if (clips.ContainsKey(actionName))
            {
                AnimationClip outClip;
                clips.TryGetValue(actionName, out outClip);
                //animatorOverrideController[actionName] = outClip;
            }
        }
        AssetDatabase.CreateAsset(animatorOverrideController, fbxPath + fbxName + ".overrideController");
    }
    /// <summary>
    /// 获取fbx中的动画文件
    /// </summary>
    /// <param name="fbxPath"></param>
    /// <returns></returns>
    Dictionary<string, AnimationClip> AnimLoad(string fbxPath, string fbxName)
    {
        Dictionary<string, AnimationClip> clips = new Dictionary<string, AnimationClip>();
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(fbxPath + fbxName);

        foreach (Object o in objs)
        {
            AnimationClip clip = o as AnimationClip;
            if (clip != null)
            {
                clips.Add(clip.name, clip);
            }
        }
        return clips;
    }
}
