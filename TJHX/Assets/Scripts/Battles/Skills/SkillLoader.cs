using System;
using System.Collections.Generic;
using UnityEngine;

class SkillLoader
{
    public static string EFF_SKILL_XIAOHUIFU = "Effect/Skill/eff_skill_xiaohuifu";

    public static void Load(string prefabPath, Transform parent = null)
    {
        GameObject instance = GameObject.Instantiate(Resources.Load(prefabPath)) as GameObject;
        if (parent != null)
            instance.transform.SetParent(parent);
        instance.transform.position = Vector3.zero;
        instance.transform.localScale = Vector3.one;
    }
}
