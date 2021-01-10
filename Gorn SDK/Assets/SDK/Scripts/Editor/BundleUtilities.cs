using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class BundleUtilities
{
    /// <summary>
    /// Exports manifest content to a chosen location.
    /// </summary>
    public static void ExportManifest(this string manifestContent, string path)
    {
        File.WriteAllText(path, manifestContent);
    }

    /// <summary>
    /// Exports all the selected assets into bundles.
    /// </summary>
    public static void ExportSelectedBundles(out string output, string overrideExtension = null, params string[] files)
    {
        ClearBundlesFolder();
        AssetDatabase.SaveAssets();

        output = Path.Combine(Application.dataPath, "AssetBundles");
        Directory.CreateDirectory(output);

        AssetBundleBuild[] bundles = GetBundleBuilds(files, overrideExtension);

        BuildPipeline.BuildAssetBundles(output, bundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        ClearFolderExcess(Path.Combine(Application.dataPath, "AssetBundles"));
    }

    /// <summary>
    /// Moves all files that contain the keyword to the given directory.
    /// </summary>
    public static void GroupFilesToFolder(string directory, string newDirectory, string fileContains)
    {
        fileContains = fileContains.ToLowerInvariant();

        string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
        Directory.CreateDirectory(newDirectory);

        foreach (string file in files)
        {
            FileInfo fileInfo = new FileInfo(file);

            if (fileInfo.Name.ToLowerInvariant().Contains(fileContains))
            {
                File.Move(file, Path.Combine(newDirectory, fileInfo.Name));
            }
        }

        ClearFolderExcess(newDirectory);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Removes any unneeded build files.
    /// </summary>
    public static void ClearFolderExcess(string directory)
    {
        string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);

            if (file.Name.Contains("AssetBundles")
             || file.Name.EndsWith(".meta")
             || file.Name.Split('.').Length > 2)
            {
                File.Delete(files[i]);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Removes all files from the bundles folder.
    /// </summary>
    public static void ClearBundlesFolder()
    {
        string bundleFolder = Path.Combine(Application.dataPath, "AssetBundles");

        Directory.Delete(bundleFolder, true);
        Directory.CreateDirectory(bundleFolder);
    }

    private static AssetBundleBuild[] GetBundleBuilds(string[] files, string overrideExtension = null)
    {
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

        foreach (string path in files)
        {
            AssetImporter importer = AssetImporter.GetAtPath(path);

            if (importer == null)
            {
                continue;
            }
            FileInfo file = new FileInfo(path);

            importer.SetAssetBundleNameAndVariant(file.Name.Split('.').First().Replace(" ", ""), "armor");

            string bundleName = importer.assetBundleName;
            string bundleVariant = importer.assetBundleVariant;
            string fullName = $"{bundleName}.{bundleVariant}";

            AssetBundleBuild build = new AssetBundleBuild
            {
                assetBundleName = bundleName,
                assetBundleVariant = bundleVariant,
                assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(fullName)
            };

            builds.Add(build);
        }

        return builds.ToArray();
    }
}