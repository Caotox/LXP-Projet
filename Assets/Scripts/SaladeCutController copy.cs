using UnityEngine;

public class SaladeCutController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isTomate;
    public bool isSalade;
    public bool isPain;
    public bool isViande;
    public bool isCut;
    //public bool isCutting = GameObject.GetValue(PiggController<isCutting>);
    public bool isAssiette;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTomate = false;
        isSalade = true;
        isPain = false;
        isViande = false;
        isCut = true;
        isAssiette = true;

    }
    
    public void CutSalade()
    {
        if (!isCut)
        {
            Debug.Log("Salade coupée !");
            isCut = true;
        }
        else
        {
            Debug.Log("La Salade est déjà coupée.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
