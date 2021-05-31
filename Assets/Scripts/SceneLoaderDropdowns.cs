using UnityEditor;
namespace KingdomOfNight
{
    public partial class SceneLoader
    {
#if UNITY_EDITOR
        [MenuItem("Scenes/4 players")]
        public static void Load4players() { OpenScene("Assets/scenes/4 players.unity"); }
        [MenuItem("Scenes/EndScene")]
        public static void LoadEndScene() { OpenScene("Assets/scenes/EndScene.unity"); }
        [MenuItem("Scenes/FIrebaseLogin")]
        public static void LoadFIrebaseLogin() { OpenScene("Assets/scenes/FIrebaseLogin.unity"); }
        [MenuItem("Scenes/GameScene")]
        public static void LoadGameScene() { OpenScene("Assets/scenes/GameScene.unity"); }
        [MenuItem("Scenes/LoadingScene 1")]
        public static void LoadLoadingScene1() { OpenScene("Assets/scenes/LoadingScene 1.unity"); }
        [MenuItem("Scenes/LoadingScene")]
        public static void LoadLoadingScene() { OpenScene("Assets/scenes/LoadingScene.unity"); }
        [MenuItem("Scenes/MainScene")]
        public static void LoadMainScene() { OpenScene("Assets/scenes/MainScene.unity"); }
#endif
    }
}