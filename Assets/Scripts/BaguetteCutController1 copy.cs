using UnityEngine;

public class BaguetteCutController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isTomate;
    public bool isSalade;
    public bool isPain;
    public bool isViande;

    public bool isCut;
    public bool isCooked;
    public bool isAssiette;
    public string currentAction = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTomate = false;
        isSalade = false;
        isPain = true;
        isViande = false;
        isCut = true;
        isAssiette = true;
        isCooked = false;


    }

    public void CutBaguette()
    {
        if (!isCut)
        {
            Debug.Log("Baguette coupée !");
            isCut = true;
        }
        else
        {
            Debug.Log("La Baguette est déjà coupée.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
