using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadLevel(UnityEngine.SceneManagement.Scene scene)
    {
        // Logic to load the specified scene
        Debug.Log($"Loading scene: {scene.name}");
        SceneManager.LoadScene(scene.buildIndex);
    }
    
}
