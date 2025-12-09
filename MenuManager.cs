using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void IniciarJuegoArkanoid()
    {
    
        SceneManager.LoadScene("SampleScene");
    }

    public void IniciarJuegoGalaga()
    {
       
        SceneManager.LoadScene("galagascene");
    }

    public void SalirDelJuego()
    {
        
        Application.Quit();

        
    }
}