using UnityEngine ;
using System . Collections . Generic ;
using System ;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager                  Instance { get ; private set ; }
    private       Dictionary < ResourceType , int > inventory = new Dictionary < ResourceType , int > ( ) ;
    public event Action                             OnInventoryChanged ;

    void Awake ( )
    {
        if ( Instance != null
          && Instance != this )
        {
            Destroy ( gameObject ) ;
            return ;
        }

        Instance = this ;
        InitializeInventory ( ) ;
    }

    void InitializeInventory ( )
    {
        inventory . Clear ( ) ;

        foreach ( ResourceType type in Enum . GetValues ( typeof ( ResourceType ) ) )
        {
            if ( type != ResourceType . None )
            {
                if ( ! inventory . ContainsKey ( type ) )
                {
                    inventory . Add ( type , 0 ) ;
                }
            }
        }

        Debug . Log ( "Inventory Initialized." ) ;
        OnInventoryChanged ? . Invoke ( ) ;
    }

    public void AddResource ( ResourceType resource , int amount )
    {
        if ( resource == ResourceType . None
          || amount   <= 0 ) return ;

        if ( inventory . ContainsKey ( resource ) )
        {
            inventory [ resource ] += amount ;
        }
        else
        {
            inventory . Add ( resource , amount ) ;
            Debug . LogWarning ( $"Resource type {resource} was not pre-initialized. Added now." ) ;
        }

        Debug . Log ( $"Added {amount} {resource}. New total: {inventory [ resource ]}" ) ;
        OnInventoryChanged ? . Invoke ( ) ;
    }

    public bool RemoveResource ( ResourceType resource , int amount )
    {
        if ( resource == ResourceType . None
          || amount   <= 0 ) return false ;

        if ( inventory . ContainsKey ( resource )
          && inventory [ resource ] >= amount )
        {
            inventory [ resource ] -= amount ;
            Debug . Log ( $"Removed {amount} {resource}. Remaining: {inventory [ resource ]}" ) ;
            OnInventoryChanged ? . Invoke ( ) ;
            return true ;
        }
        else
        {
            Debug . LogWarning ( $"Not enough {resource} to remove {amount}. Have: {GetResourceCount ( resource )}" ) ;
            return false ;
        }
    }

    public bool HasEnoughResource ( ResourceType resource , int amount )
    {
        if ( resource == ResourceType . None
          || amount   <= 0 ) return true ;

        return inventory . ContainsKey ( resource ) && inventory [ resource ] >= amount ;
    }


    public int GetResourceCount ( ResourceType resource )
    {
        if ( inventory . TryGetValue ( resource , out int count ) )
        {
            return count ;
        }

        return 0 ;
    }

    public IReadOnlyDictionary < ResourceType , int > GetInventory ( )
    {
        return inventory ;
    }
}
