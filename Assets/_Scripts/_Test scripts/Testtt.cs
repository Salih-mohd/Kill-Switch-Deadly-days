using UnityEngine;

public class Testtt : MonoBehaviour
{
    public LayerMask bulletMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, 999, bulletMask))
            {
                Debug.DrawRay(transform.position, transform.forward, Color.green);

               
                var playerHealth=hit.collider.GetComponent<IDamage>();
                //Debug.Log("hitted collider is "+hit.collider.name);


                //playerHealth.Damage(10);
                //Debug.Log("called player health scripts damage");
            }
        }
        Debug.DrawRay(transform.position,transform.forward,Color.red);
    }
}
