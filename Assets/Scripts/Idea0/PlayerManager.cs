using UnityEngine;

public class PlayerManager : MonoBehaviour
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius,mask);
        if (hitColliders.Length > 0)
        {
            
            
            DirectorMaster.instance.PauseAction(hitColliders[0].transform.position);
            hitColliders[0].gameObject.SetActive(false);
        }
        else {

            if (DirectorMaster.instance.playableDirector.state == UnityEngine.Playables.PlayState.Paused) {
                DirectorMaster.instance.playableDirector.Play();
            }
        }
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("Encontrado:" + hitCollider.name);

            }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
