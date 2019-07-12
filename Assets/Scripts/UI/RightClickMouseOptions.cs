using UnityEngine;

public class RightClickMouseOptions : MonoBehaviour
{
    public GameObject boxPrefab;

    public GameObject mouseOptionsBox;
    
    private void LateUpdate() {
        if (Input.GetMouseButtonDown(1)) { CheckAndOpenMouseOptions(); }

    }

    private void CheckAndOpenMouseOptions() {
        
        Debug.Log("MouseRightClick");
    }
}
