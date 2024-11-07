using UnityEngine;

public class RotateOnDegree : MonoBehaviour
{
    Movement movement;
    GameManager gameManager;

    void Start()
    {
        movement = GameObject.FindGameObjectWithTag("Bonine").GetComponent<Movement>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Rotates Bonine to the specified direction for `timer` seconds
    public void Rotate(float degree, float timer)
    {
        if (gameManager.pausePanel.activeInHierarchy) return;
        movement.rotationTimer = timer;

        if (degree > 330 || degree <= 60)
        {
            movement.ChangeAnimationState(Movement.Animation.Bonine_Walk_Right);
            movement.idleState = Movement.Animation.Bonine_Idle_Right;
        }

        else if (degree > 60 && degree < 150)
        {
            movement.ChangeAnimationState(Movement.Animation.Bonine_Walk_Back);
            movement.idleState = Movement.Animation.Bonine_Idle_Back;
        }

        else if (degree >= 150 && degree < 240)
        {
            movement.ChangeAnimationState(Movement.Animation.Bonine_Walk_Left);
            movement.idleState = Movement.Animation.Bonine_Idle_Left;
        }

        else if (degree > 240 && degree < 330)
        {
            movement.ChangeAnimationState(Movement.Animation.Bonine_Walk_Front);
            movement.idleState = Movement.Animation.Bonine_Idle;
        }
    }
}