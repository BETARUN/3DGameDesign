using UnityEngine;

public class LoadResources : MonoBehaviour {
    private void OnEnable()
    {
        if(gameObject.name == "Ethan")
        {
            Canvas canvas = Instantiate(Resources.Load("Prefabs/UGUI_HealthBar", typeof(Canvas)), Vector3.zero, Quaternion.identity, null) as Canvas;
            canvas.gameObject.SetActive(true);
            canvas.gameObject.transform.SetParent(this.transform);
        }
        else
        {
            GameObject gameObject = Instantiate(Resources.Load("Prefabs/IMGUI_HealthBar", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
            gameObject.SetActive(true);
        }
    }
}