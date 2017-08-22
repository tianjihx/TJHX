using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class MenuTools
{
    private static HashSet<string> ignoreList = new HashSet<string>()
    {
        "DOTweenSettings"
    };

    [MenuItem("工具/生成R")]
    public static void GenerateR()
    {
        string resourcesRootPath = (Application.dataPath + "/Resources");
        string RDirPath = Application.dataPath + "/Scripts/R/";
        Dictionary<string, string> RNameList = new Dictionary<string, string>();
        string RContent = TranverseDir2Class(resourcesRootPath, 1, 1, "", RNameList);
        if (!Directory.Exists(RDirPath))
            Directory.CreateDirectory(RDirPath);
        string RidPath = RDirPath + "RID.cs";
        string RmapPath = RDirPath + "RMap.cs";
        File.WriteAllText(RidPath, RContent);
        File.WriteAllText(RmapPath, GenerateRMap(RNameList).Replace(resourcesRootPath + "/", ""));
        Debug.Log("<color=blue>生成R文件成功！</color>");
    }

    private static string GenerateRMap(Dictionary<string, string> RNameList)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"using System.Collections.Generic;

partial class R {
    public static Dictionary<int, string> map = new Dictionary<int, string>()
    {
");
        foreach (var pair in RNameList)
        {
            var prefix = pair.Value;
            var name = pair.Key;
            sb.Append(string.Format("        {{{0}, \"{1}\"}},\n", prefix + name, prefix.Replace('.', '/') + name));
        }
        sb.Remove(sb.Length - 2, 2);
        sb.Append("\n    };\n}");
        return sb.ToString();
    }

    private static string TranverseDir2Class(string dirpath, int depth, int dirNo, string prefix, Dictionary<string, string> RNameList)
    {
        StringBuilder sb = new StringBuilder();
        if (depth != 1)
        {
            sb.Append(Indent(depth) + "public class " + Path.GetFileName(dirpath) + "\n" + Indent(depth) + "{\n");
            prefix += Path.GetFileName(dirpath) + ".";
        }
        else
        {
            sb.Append("partial class R\n{\n");
        }
        

        var filepaths = Directory.GetFiles(dirpath);
        int fileCount = 0;
        foreach (var filepath in filepaths)
        {
            if (isIgnore(Path.GetFileNameWithoutExtension(filepath)))
                continue;
            if (Path.GetExtension(filepath) == ".meta")
                continue;
            if (Directory.Exists(filepath))
                continue;
            sb.Append(string.Format(
                Indent(depth + 1) + "public static int {0,-32} = {1}{2:00}{3:0000};\n", 
                regularClassName(Path.GetFileNameWithoutExtension(filepath)), depth, dirNo, ++fileCount
                ));
            RNameList.Add(Path.GetFileNameWithoutExtension(filepath), prefix);
        }

        var dirs = Directory.GetDirectories(dirpath);
        if (dirs.Length != 0)
        {
            if (fileCount != 0)
                sb.Append('\n');
            int dirCount = 0;
            foreach (var dir in dirs)
            {
                sb.Append(TranverseDir2Class(dir, depth + 1, ++dirCount, prefix, RNameList));
            }
            sb.Append("\n");
        }
        sb.Append(Indent(depth) + "}");
        return sb.ToString();
    }

    private static bool isIgnore(string checkname)
    {
        return ignoreList.Contains(checkname);
    }

    private static string regularClassName(string name)
    {
        if (name[0] > '0' && name[0] < '9')
            name = '_' + name;
        return name.Replace(' ', '_').Replace('.', '_');
    }

    private static string Indent(int indentDepth)
    {
        indentDepth--;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < indentDepth * 4; ++i)
        {
            sb.Append(' ');
        }
        return sb.ToString();
    }
}
