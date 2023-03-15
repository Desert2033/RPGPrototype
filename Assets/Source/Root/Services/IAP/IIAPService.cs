using Source.Root.Services;
using System;
using System.Collections.Generic;

public interface IIAPService : IService
{
    public List<ProductDescription> Products();
    
    public bool IsInitialized { get; }
    
    public event Action Initialized;
 
    public void Init();
    public void StartPurchase(string productId);
}