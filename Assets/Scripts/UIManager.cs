using UnityEngine ;
using TMPro ;
using System . Collections . Generic ;

public class UIManager : MonoBehaviour
{
    [ Header ( "Inventory UI References" ) ]
    [ SerializeField ] private TextMeshProUGUI ironCountText ;

    [ SerializeField ] private TextMeshProUGUI redGemCountText ;
    [ SerializeField ] private TextMeshProUGUI blueGemCountText ;

    [ Header ( "Manager References" ) ]
    [ SerializeField ] private InventoryManager inventoryManager ;

    void Start ( )
    {
        if ( inventoryManager == null )
        {
            Debug . LogError ( "InventoryManager reference not set in UIManager!" ) ;
            inventoryManager = InventoryManager . Instance ;
            if ( inventoryManager == null ) return ;
        }

        inventoryManager . OnInventoryChanged += UpdateInventoryUI ;
        UpdateInventoryUI ( ) ;
    }

    void OnDestroy ( )
    {
        if ( inventoryManager != null )
        {
            inventoryManager . OnInventoryChanged -= UpdateInventoryUI ;
        }
    }

    void UpdateInventoryUI ( )
    {
        if ( inventoryManager == null ) return ;
        if ( ironCountText != null )
            ironCountText . text = $"Iron: {inventoryManager . GetResourceCount ( ResourceType . Iron )}" ;
        else
            Debug . LogWarning ( "Iron Count Text is not assigned in UIManager." ) ;
        if ( redGemCountText != null )
            redGemCountText . text = $"Red Gems: {inventoryManager . GetResourceCount ( ResourceType . RedGem )}" ;
        else
            Debug . LogWarning ( "Red Gem Count Text is not assigned in UIManager." ) ;
        if ( blueGemCountText != null )
            blueGemCountText . text = $"Blue Gems: {inventoryManager . GetResourceCount ( ResourceType . BlueGem )}" ;
        else
            Debug . LogWarning ( "Blue Gem Count Text is not assigned in UIManager." ) ;
    }
}
