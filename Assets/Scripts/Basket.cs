// Basket.cs
using UnityEngine;
using System.Linq;

public class Basket : MonoBehaviour
{
    public int ItemCount { get; private set; }

    // Example method to update ItemCount
    public void AddItems(int count)
    {
        ItemCount += count;
    }

    public void RemoveItems(int count)
    {
        int removed = 0;
        ItemCount = Mathf.Max(0, ItemCount - count);
         var fruits = transform.Cast<Transform>()
                              .Where(t => t.CompareTag("PickUp"))
                              .ToList();
         foreach (var fruit in fruits)
        {
            if (removed >= count) break;
            Destroy(fruit.gameObject);
            removed++;
        }
         Debug.Log($"[Basket] Removed {removed} items from the basket. Remaining items: {transform.childCount}");
    }
    /// <summary>
    /// Smaže z tohoto GameObjectu count potomků, kteří mají tag "PickUp".
    /// </summary>
    

       
    
}
