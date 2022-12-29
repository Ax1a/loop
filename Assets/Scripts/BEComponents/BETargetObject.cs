using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BETargetObject : MonoBehaviour
{
    public List<BEBlock> beBlockGroupsList;
    public AudioSource beAudioSource;
    private BEController beController;
    public BEController BeController { get => beController; }

    //v1.1 -Enable programming env from target object inspector
    [HideInInspector]
    public BEProgrammingEnv beProgrammingEnv;
    [SerializeField]
    private bool enableProgrammingEnv = true;
    public bool EnableProgrammingEnv
    {
        get
        {
            return enableProgrammingEnv;
        }
        set
        {
            enableProgrammingEnv = value;
            SetEnableProgrammingEnv(value);
        }
    }

    private void OnDisable()
    {
        SetEnableProgrammingEnv(false);
    }

    private void OnDestroy()
    {
        try
        {
            Destroy(beProgrammingEnv.gameObject);
        }
        catch
        {
            // object already destroyed
        }
    }

    private void SetEnableProgrammingEnv(bool value)
    {
        if (beController == null)
        {
            beController = GetBeController();
        }
        try
        {
            beController.FindTargetObjects();
            if (beController.singleEnabledProgrammingEnv && value == true)
            {
                foreach (BETargetObject targetObject in BEController.beTargetObjectList)
                {
                    if (targetObject != this)
                    {
                        targetObject.EnableProgrammingEnv = false;
                    }
                }
            }
            if (beProgrammingEnv != null)
            {
                beProgrammingEnv.gameObject.SetActive(value);
            }
            else
            {
                GetProgrammingEnv(transform).gameObject.SetActive(value);
            }
        }
        catch
        {
            //exiting play mode
        }
    }

    // v1.1 -GetBeController method added to BETargetObject using FindObjectOfType, more suitable than tag=="GameController"
    private BEController GetBeController()
    {
        return FindObjectOfType<BEController>();
    }

    public BEProgrammingEnv GetProgrammingEnv(Transform parent)
    {
        BEProgrammingEnv progEnv = null;
        foreach (Transform child in parent)
        {
            if (child.GetComponent<BEProgrammingEnv>())
            {
                progEnv = child.GetComponent<BEProgrammingEnv>();
                break;
            }
            GetProgrammingEnv(child);
        }
        return progEnv;
    }

    private void OnValidate()
    {
        SetEnableProgrammingEnv(enableProgrammingEnv);
    }

    private void Awake()
    {
        beController = GetBeController();
    }

    void Start()
    {
        beBlockGroupsList = new List<BEBlock>();
        beAudioSource = GetComponent<AudioSource>();
    }
}