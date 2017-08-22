using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using UnityEngine;

class ResourcesManager
{
    public static Weapon LoadWeapon(int rid)
    {
        Weapon weapon = new Weapon();
        string weaponFileContent = Resources.Load<TextAsset>(R.GetResourcesNameById(rid)).text;
        var config = FileReader.LoadConfigFile(weaponFileContent);
        
        return null;
    }

    public static GameObject CreateByRid(int rid, Transform parent = null)
    {
        var go = GameObject.Instantiate(Resources.Load(R.GetResourcesNameById(rid))) as GameObject;
        if (go == null)
        {
            Debug.LogError($"加载资源 {R.GetResourcesNameById(rid)} 出错！未找到！");
            return null;
        }
        if (parent != null)
            go.transform.SetParent(parent);
        return go;
    }
}
