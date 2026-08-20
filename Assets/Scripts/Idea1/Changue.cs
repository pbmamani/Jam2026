using UnityEngine;
using UnityEngine.UI;

public enum TIPOS { 
    CAMBIO,
    FINAL
}

public class Changue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float radius;
    [SerializeField] LayerMask mask;
    [SerializeField] TIPOS tipo_actual;

    bool Uso = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Uso == false) {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, mask);

            if (hitColliders.Length > 0)
            {
                switch (tipo_actual)
                {
                    case TIPOS.CAMBIO:
                        hitColliders[0].GetComponent<PlayerMove>().ChangueDestination(1);
                        MasterManager.Instance.EjecutarEncuentro(gameObject.name);
                        Uso = true;
                        break;

                    case TIPOS.FINAL:
                        MasterManager.Instance.EjecutarFinal(gameObject.name);
                        Uso = true;
                        break;

                    default:

                        break;
                }

                //gameObject.SetActive(false);
            }
            
        }

        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
