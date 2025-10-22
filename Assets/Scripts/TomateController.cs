using UnityEngine;

public class TomateController : MonoBehaviour
{
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
        isTomate = true;
        isSalade = false;
        isPain = false;
        isViande = false;
        isCut = false;
        isAssiette = false;

    }
    
    public void CutTomato()
    {
        if (!isCut)
        {
            Debug.Log("Tomate coupée !");
            isCut = true;
        }
        else
        {
            Debug.Log("La tomate est déjà coupée.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
