using UnityEngine;
using UnityEngine.UI;

public class Changue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float radius;
    [SerializeField] LayerMask mask;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, mask);

        if (hitColliders.Length > 0)
        {
            hitColliders[0].GetComponent<PlayerMove>().ChangueDestination(1);
            //gameObject.SetActive(false);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
