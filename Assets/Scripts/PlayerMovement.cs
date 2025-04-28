using UnityEngine ;


[ RequireComponent ( typeof ( Rigidbody2D ) ) ]
public class PlayerMovement : MonoBehaviour
{
    [ Header ( "Movement Settings" ) ]
    [ SerializeField ] private float baseMoveSpeed = 5f ;

    [ SerializeField ] private float currentMoveSpeed ;

    [ Header ( "Components" ) ]
    private Rigidbody2D rb ;

    private Vector2 movementInput ;
    public  int     MovementSpeedLevel { get ; private set ; } = 0 ;

    void Awake ( )
    {
        rb               = GetComponent < Rigidbody2D > ( ) ;
        currentMoveSpeed = baseMoveSpeed ;
    }

    void Update ( )
    {
        movementInput . x = Input . GetAxisRaw ( "Horizontal" ) ;
        movementInput . y = Input . GetAxisRaw ( "Vertical" ) ;
        movementInput . Normalize ( ) ;
    }

    void FixedUpdate ( )
    {
        rb . linearVelocity = movementInput * currentMoveSpeed ;
    }


    public void UpdateMovementSpeed ( float newSpeed , int level )
    {
        currentMoveSpeed   = newSpeed ;
        MovementSpeedLevel = level ;
        Debug . Log ( $"Movement Speed upgraded to Level {level}, Speed: {currentMoveSpeed}" ) ;
    }

    public float GetBaseMoveSpeed ( )
    {
        return baseMoveSpeed ;
    }
}
