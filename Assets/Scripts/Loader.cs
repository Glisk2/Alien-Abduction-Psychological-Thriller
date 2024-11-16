using UnityEngine.SceneManagement;


public static class Loader 
{
    public enum Scene{
        GameScene, 
        MainMenuScene,
    }

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
