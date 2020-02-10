using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raquet : MonoBehaviour
{	
	//Velocidad
	[SerializeField]
	private float speed = 30;

	//Eje Vertival
	[SerializeField]
	private string eje;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Capturo el valor del eje vertical para la raqueta
		float v = Input.GetAxis(eje);

		//Modifico la velocidad de la raqueta
		GetComponent<Rigidbody2D>().velocity = new Vector2(0, v * speed);
    }
}
