using System.Collections.Generic;
using System;

[Serializable]
public class PurchaseData
{
    public List<BoughtIAP> BoughtIAPs = new List<BoughtIAP>();

    public event Action Changed;

    public void AddPurchase(string id)
    {
        BoughtIAP boughtIAP = ProductById(id);

        if (boughtIAP != null)
            boughtIAP.Count++;
        else
            BoughtIAPs.Add(new BoughtIAP { IdIAP = id, Count = 1 });

        Changed?.Invoke();
    }

    private BoughtIAP ProductById(string id) =>
        BoughtIAPs.Find(x => x.IdIAP == id);
}
