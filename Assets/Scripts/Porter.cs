using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//detecto si la bola atraviesa la porteria
	void OnTriggerEnter2D(Collider2D ball) {

		if (ball.name == "Ball"){

			//Si es la portería izquierda
			if (this.name == "Left"){
				//Cuento el gol y reinicio la bola
				ball.GetComponent<Ball>().reiniciarBola("Right");	
			}
			//Si es la portería derecha
			else if (this.name == "Right"){
				//Cuento el gol y reinicio la bola
				ball.GetComponent<Ball>().reiniciarBola("Left");
			}
		}
	}
}
