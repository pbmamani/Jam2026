using UnityEngine;

public class BotonPieza : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject GridSnap;
    private void OnMouseDown()
    {
        GetComponent<Renderer>().material.color = Color.blue;
        GridSnap.SetActive(true);
    }
}
