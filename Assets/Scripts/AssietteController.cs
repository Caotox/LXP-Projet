using UnityEngine;

public class AssietteController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isTomate;
    public bool isSalade;
    public bool isPain;
    public bool isViande;
    public bool isCut;
    //public bool isCutting = GameObject.GetValue(PiggController<isCutting>);
    public bool isAssiette;
    public bool isvide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTomate = false;
        isSalade = false;
        isPain = false;
        isViande = false;
        isCut = false;
        isAssiette = true;
        isvide = true;

    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
