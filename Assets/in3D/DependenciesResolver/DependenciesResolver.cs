using System.IO;
using UnityEditor;
using UnityEngine;

namespace DependenciesResolver
{
    [InitializeOnLoad]
    public class DependenciesResolver
    {
        static DependenciesResolver()
        {
            AssetDatabase.importPackageCompleted += OnImportPackageCompleted;
        }

        private static void OnImportPackageCompleted(string packagename)
        {
#if !IN3D_SDK_DEPENDENCIES_RESOLVED
            var projectManifestPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName,
                                                   "Packages/manifest.json");
            var depsManifestPath = Path.Combine(Application.dataPath, "in3D/DependenciesResolver/manifest.json");

            ManifestEditor.AddDependencies(depsManifestPath, projectManifestPath);
#if UNITY_2020_1_OR_NEWER
            UnityEditor.PackageManager.Client.Resolve();
#endif
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone,
                                                             "IN3D_SDK_DEPENDENCIES_RESOLVED");
            AssetDatabase.Refresh();
            Debug.Log($"Imported package: {packagename}");
            AssetDatabase.DeleteAsset("Assets/in3D/DependenciesResolver");
#endif
        }
    }
}