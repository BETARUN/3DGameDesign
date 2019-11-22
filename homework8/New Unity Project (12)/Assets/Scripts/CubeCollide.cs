using UnityEngine;

public class CubeCollide : MonoBehaviour {
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ethan")
        {
            collision.gameObject.GetComponentInChildren<HealthControl>().ReduceHealth();
        }
    }
}