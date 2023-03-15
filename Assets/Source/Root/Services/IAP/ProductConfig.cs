using UnityEngine.Purchasing;
using System;

[Serializable]
public class ProductConfig
{
    public string Id;
    public string Price;
    public string Icon;
    public ProductType ProductType;
    public ItemType ItemType;
    public int MaxPurchaseCount;
    public int Quantity;
}
