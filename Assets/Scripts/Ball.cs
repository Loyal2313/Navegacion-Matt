using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{	

	//Velocidad
	public float velocidad = 30.0f;

	//Audio Source
	AudioSource fuenteDeAudio;

	//Clips de audio
	[SerializeField]
	private AudioClip audioGol, audioRaqueta, audioRebote;

	//Contadores de goles
	private int golesIzquierda = 0;
	private int golesDerecha = 0;

	//Cajas de texto de los contadores
	[SerializeField]
	private Text contadorIzquierda, contadorDerecha;

	//Caja de texto del resultado
	public Text result;

	//Caja de texto para el temporizador
	public Text temporizador;

	//variables para mostrar el tiempo
	private string minutos, segundos;

	//contador de tiempo
	private float tiempo=10;
	
    // Start is called before the first frame update
    void Start()
    {
        //Velocidad inicial hacia la derecha
		GetComponent<Rigidbody2D>().velocity = Vector2.right * velocidad;

		//Recupero el componente audio source;
		fuenteDeAudio = GetComponent<AudioSource>();

		//Pongo los contadores a 0
		contadorIzquierda.text = golesIzquierda.ToString();
		contadorDerecha.text = golesDerecha.ToString();

		//Desactivo la caja de resultado
		result.enabled = false;

		//Quito la pausa
		Time.timeScale = 1;
    }

	//Se ejecuta al colisionar
	void OnCollisionEnter2D(Collision2D micolision){

		//Col contiene toda la información de la colisión
		//Si la bola colisiona con la raqueta:
		//   micolision.gameObject es la raqueta
		//   micolision.transform.position es la posición de la raqueta

		//Si choca con la raqueta izquierda
		if (micolision.gameObject.name == "Left Raquet"){

			//Valor de x
			int x = 1;

			//Valor de y
			int y = direccionY(transform.position, micolision.transform.position);

			//Calculo dirección
			Vector2 direccion = new Vector2(x, y);

			//Aplico velocidad
			GetComponent<Rigidbody2D>().velocity = direccion * velocidad;

			//Reproduzco el sonido de la raqueta
			fuenteDeAudio.clip = audioRaqueta;
			fuenteDeAudio.Play();
		}

		//Si choca con la raqueta derecha
		else if (micolision.gameObject.name == "Right Raquet"){

			//Valor de x
			int x = -1;

			//Valor de y
			int y = direccionY(transform.position, micolision.transform.position);

			//Calculo dirección (normalizada para que de 1 o -1)
			Vector2 direccion = new Vector2(x, y);

			//Aplico velocidad
			GetComponent<Rigidbody2D>().velocity = direccion * velocidad;

			//Reproduzco el sonido de la raqueta
			fuenteDeAudio.clip = audioRaqueta;
			fuenteDeAudio.Play();
		}

		//Para el sonido del rebote
		if (micolision.gameObject.name == "Up" || micolision.gameObject.name == "Down"){

			//Reproduzco el sonido del rebote
			fuenteDeAudio.clip = audioRebote;
			fuenteDeAudio.Play();

		}
	}

	//Direccion Y
	int direccionY(Vector2 posicionBola, Vector2 posicionRaqueta){

		if (posicionBola.y > posicionRaqueta.y){
			return 1;
		}
		else if (posicionBola.y < posicionRaqueta.y){
			return -1;
		}
		else{
			return 0;
		}
	}

	//Reinicio la posición de la bola
	public void reiniciarBola(string direccion){

		//Posición 0 de la bola
		transform.position = Vector2.zero;
		//Vector2.zero es lo mismo que new Vector2(0,0);

		//Velocidad inicial de la bola
		velocidad = 30;

		//Velocidad y dirección
		if (direccion == "Right"){
			//Incremento goles al de la derecha
			golesDerecha++;
			//Lo escribo en el marcador
			contadorDerecha.text = golesDerecha.ToString();
			//Reinicio la bola (si no ha llegado a 5)
			if (!comprobarFinal()){
				GetComponent<Rigidbody2D>().velocity = Vector2.left * velocidad;
				//Vector2.right es lo mismo que new Vector2(-1,0)
			}	
			//Vector2.right es lo mismo que new Vector2(1,0)
		}
		else if (direccion == "Left"){
			//Incremento goles al de la izquierda
			golesIzquierda++;
			//Lo escribo en el marcador
			contadorIzquierda.text = golesIzquierda.ToString();
			//Reinicio la bola (si no ha llegado a 5)
			if (!comprobarFinal()){
				GetComponent<Rigidbody2D>().velocity = Vector2.left * velocidad;
				//Vector2.right es lo mismo que new Vector2(-1,0)
			}	
			GetComponent<Rigidbody2D>().velocity = Vector2.left * velocidad;
			//Vector2.right es lo mismo que new Vector2(-1,0)
		}

		//Reproduzco el sonido del gol
		fuenteDeAudio.clip = audioGol;
		fuenteDeAudio.Play();
	}

	void Update () {

		//Incremento la velocidad de la bola
		velocidad = velocidad + 0.1f;

		//Decremento el tiempo
		tiempo -= Time.deltaTime;
		
		//Escribo el tiempo
		if(!comprobarFinal()){
			minutosSegundos(tiempo);
		}
		else{
			minutosSegundos(0);
		}
	}

	void minutosSegundos(float tiempo){

		//Minutos
		if (tiempo > 120){
			minutos = "02";
		}
		else if (tiempo >= 60){
			minutos = "01";
		}
		else{
			minutos = "00";
		}

		//Segundos
		int numSegundos = Mathf.RoundToInt(tiempo % 60);
		if (numSegundos > 9){
			segundos = numSegundos.ToString();
		}
		else{
			segundos = "0" + numSegundos.ToString();
		}

		//Escribo en la caja de texto
		temporizador.text = minutos + ":" + segundos;
	}

	//Compruebo si alguno ha llegado a 5 goles (o se ha acabado el tiempo)
	bool comprobarFinal(){
		
		//Compruebo si se ha acabado el tiempo
		if (tiempo <= 0){
			//Compruebo quién ha ganado
			if (golesIzquierda > golesDerecha){
				//Escribo y muestro el resultado
				result.text = "¡Jugador Izquierda GANA!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
			}
			else if (golesDerecha > golesIzquierda){
				//Escribo y muestro el resultado
				result.text = "¡Jugador Derecha GANA!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
			}
			else{
				//Escribo y muestro el resultado
				result.text = "¡EMPATE!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
			}
			//Muestro el resultado, pauso el juego y devuelvo true
			result.enabled = true;
			Time.timeScale = 0; //Pausa
			return true;
		}

		//Si el de la izquierda ha llegado a 5
		if (golesIzquierda == 5){
			//Escribo y muestro el resultado
			result.text = "¡Jugador Izquierda GANA!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
			//Muestro el resultado, pauso el juego y devuelvo true
			result.enabled = true;
			Time.timeScale = 0; //Pausa
			return true;
		}
		//Si el de le aderecha a llegado a 5
		else if (golesDerecha == 5){
			//Escribo y muestro el resultado
			result.text = "¡Jugador Derecha GANA!\nPulsa I para volver a Inicio\nPulsa P para volver a jugar";
			//Muestro el resultado, pauso el juego y devuelvo true
			result.enabled = true;
			Time.timeScale = 0; //Pausa
			return true;
		}
		//Si ninguno ha llegado a 5, continúa el juego
		else{
			return false;
		}
	}
}
