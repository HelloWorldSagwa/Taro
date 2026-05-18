using Arcanum.App;
using Arcanum.Scenes;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Arcanum.EditorTools
{
    public static class ArcanumSprint1Builder
    {
        [MenuItem("Arcanum/Build Sprint 1 PC Scenes")]
        public static void Build()
        {
            Directory.CreateDirectory("Assets/Arcanum");
            Directory.CreateDirectory("Assets/Arcanum/Scenes");

            PlayerSettings.companyName = "HelloWorldSagwa";
            PlayerSettings.productName = "ARCANUM STAGE";
            PlayerSettings.defaultScreenWidth = 1920;
            PlayerSettings.defaultScreenHeight = 1080;
            PlayerSettings.defaultIsNativeResolution = false;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.resizableWindow = false;
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Standalone, "com.HelloWorldSagwa.ArcanumStage");

            var bootScenePath = CreateScene("Boot", typeof(BootController));
            var homeTableScenePath = CreateScene("HomeTable", typeof(HomeTableController));
            var ritualScenePath = CreateScene("Ritual", typeof(RitualController));
            var readingResultScenePath = CreateScene("ReadingResult", typeof(ReadingResultController));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            EditorBuildSettings.scenes = new[]
            {
                CreateBuildSettingsScene(bootScenePath),
                CreateBuildSettingsScene(homeTableScenePath),
                CreateBuildSettingsScene(ritualScenePath),
                CreateBuildSettingsScene(readingResultScenePath)
            };

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static string CreateScene(string sceneName, System.Type controllerType)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var scenePath = $"Assets/Arcanum/Scenes/{sceneName}.unity";

            var cameraObject = new GameObject("Main Camera", typeof(Camera));
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.035f, 0.035f, 0.065f);
            camera.orthographic = true;
            camera.orthographicSize = 5f;

            new GameObject(sceneName + "Controller", controllerType);
            EditorSceneManager.SaveScene(scene, scenePath);
            AssetDatabase.ImportAsset(scenePath, ImportAssetOptions.ForceSynchronousImport);

            return scenePath;
        }

        private static EditorBuildSettingsScene CreateBuildSettingsScene(string scenePath)
        {
            var sceneGuid = AssetDatabase.GUIDFromAssetPath(scenePath);
            if (sceneGuid.Empty())
            {
                throw new FileNotFoundException($"Scene asset was not imported before build settings update: {scenePath}");
            }

            return new EditorBuildSettingsScene(scenePath, true);
        }
    }
}
