using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void DoPressureGameOverStuff()
    {
        Debug.Log("You lost you dumb fuck (PRESSURE)");
    }

    public void DoOutOfLivesGameOverStuff()
    {
        Debug.Log("You ran out of lives you dumb fuck (LIVES)");
    }
}
