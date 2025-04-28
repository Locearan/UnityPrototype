using UnityEngine ;
using System . Collections ;

public class MiningController : MonoBehaviour
{
    [ Header ( "Mining Settings" ) ]
    [ SerializeField ]
    private float
        miningReach = 0.6f ;

    [ SerializeField ] private KeyCode mineKey = KeyCode . Space ;

    [ SerializeField ]
    private LayerMask diggableLayer ;

    [ Header ( "References" ) ]
    [ SerializeField ] private TilemapManager tilemapManager ;

    [ SerializeField ] private InventoryManager inventoryManager ;


    [ Header ( "State" ) ]
    private bool isMining = false ;

    private Vector3Int currentMiningTarget ;
    private float      currentDigTimeRemaining ;

    public  int   DrillSpeedLevel { get ; private set ; } = 0 ;
    private float drillSpeedMultiplier = 1.0f ;

    void Update ( )
    {
        if ( Input . GetKeyDown ( mineKey )
          && ! isMining )
        {
            TryStartMining ( ) ;
        }

        if ( isMining && Input . GetKeyUp ( mineKey ) )
        {
        }
    }


    void TryStartMining ( )
    {
        Vector2 moveInput = new Vector2 ( Input . GetAxisRaw ( "Horizontal" ) , Input . GetAxisRaw ( "Vertical" ) ) ;
        Vector3 direction = Vector3 . zero ;

        if ( moveInput . magnitude > 0.1f )
        {
            if ( Mathf . Abs ( moveInput . x ) > Mathf . Abs ( moveInput . y ) )
                direction = new Vector3 ( Mathf . Sign ( moveInput . x ) , 0 , 0 ) ;
            else
                direction = new Vector3 ( 0 , Mathf . Sign ( moveInput . y ) , 0 ) ;
        }
        else
        {
            return ;
        }


        StartMiningProcess ( direction ) ;
    }

    void TryStartMining ( Vector3 direction )
    {
        StartMiningProcess ( direction . normalized ) ;
    }


    void StartMiningProcess ( Vector3 direction )
    {
        if ( tilemapManager   == null
          || inventoryManager == null )
        {
            Debug . LogError ( "MiningController is missing references (TilemapManager or InventoryManager)." ) ;
            return ;
        }

        Vector3          checkPosition = transform . position + direction * miningReach ;
        Vector3Int       targetCell    = tilemapManager . WorldToCell ( checkPosition ) ;
        DiggableTileData tileData      = tilemapManager . GetTileData ( targetCell ) ;

        if ( tileData != null
          && tileData . isDiggable )
        {
            StartCoroutine ( MineTileCoroutine ( targetCell , tileData ) ) ;
        }
        else
        {
        }
    }

    private IEnumerator MineTileCoroutine ( Vector3Int targetCell , DiggableTileData tileData )
    {
        isMining            = true ;
        currentMiningTarget = targetCell ;
        float actualDigTime = tileData . baseDiggingTime / drillSpeedMultiplier ;
        currentDigTimeRemaining = actualDigTime ;

        Debug . Log
            (
             $"Started mining {tileData . name} at {targetCell}. Base time: {tileData . baseDiggingTime}, Actual time: {actualDigTime}"
            ) ;

        while ( currentDigTimeRemaining > 0 )
        {
            currentDigTimeRemaining -= Time . deltaTime ;

            yield return null ;
        }

        Debug . Log ( $"Finished mining {tileData . name} at {targetCell}" ) ;

        tilemapManager . RemoveTile ( targetCell ) ;

        if ( tileData . resourceType   != ResourceType . None
          && tileData . resourceAmount > 0 )
        {
            inventoryManager . AddResource ( tileData . resourceType , tileData . resourceAmount ) ;
            Debug . Log ( $"Collected {tileData . resourceAmount} {tileData . resourceType}" ) ;
        }

        isMining = false ;
    }

    public void UpdateDrillSpeed ( float multiplier , int level )
    {
        drillSpeedMultiplier = multiplier ;
        DrillSpeedLevel      = level ;
        Debug . Log ( $"Drill Speed upgraded to Level {level}, Multiplier: {drillSpeedMultiplier}" ) ;
    }

    void OnDrawGizmosSelected ( )
    {
        Gizmos . color = Color . yellow ;
        Gizmos . DrawLine ( transform . position , transform . position + Vector3 . up    * miningReach ) ;
        Gizmos . DrawLine ( transform . position , transform . position + Vector3 . down  * miningReach ) ;
        Gizmos . DrawLine ( transform . position , transform . position + Vector3 . left  * miningReach ) ;
        Gizmos . DrawLine ( transform . position , transform . position + Vector3 . right * miningReach ) ;
    }
}
