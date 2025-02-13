using UnityEngine;

public class DiseaseManager : MonoBehaviour
{
    public static DiseaseManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }
}
