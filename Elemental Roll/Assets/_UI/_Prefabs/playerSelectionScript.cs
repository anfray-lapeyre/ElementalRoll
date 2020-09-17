using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerSelectionScript : MonoBehaviour
{
    public GameObject FireSelected;
    public GameObject IcePlaceholder;
    public GameObject IcePlayer;
    public TMP_Text IceText;
    public TMP_Text IceMaxText;
    public Material IceMaterial;
    public GameObject IceSelected;
    public GameObject EarthPlaceholder;
    public GameObject EarthPlayer;
    public TMP_Text EarthText;
    public TMP_Text EarthMaxText;
    public Material EarthMaterial;
    public GameObject EarthSelected;
    public GameObject DeathPlaceholder;
    public GameObject DeathPlayer;
    public TMP_Text DeathText;
    public TMP_Text DeathMaxText;
    public Material DeathMaterial;
    public GameObject DeathSelected;

    private int selected = 0;
    private int slimecount = 0;

    public void UpdateProgress()
    {
        FireSelected.SetActive(false);
        IceSelected.SetActive(false);
        EarthSelected.SetActive(false);
        DeathSelected.SetActive(false);
        slimecount = ActualSave.actualSave.getSlimesCollected();
        if (isIceUnlocked(slimecount)) //Ice is unlocked
        {
            IcePlaceholder.SetActive(false);
            IcePlayer.SetActive(true);
            if (isEarthUnlocked(slimecount)) //Earth is unlocked
            {
                EarthPlaceholder.SetActive(false);
                EarthPlayer.SetActive(true);

                if (isDeathUnlocked(slimecount)) // Death is unlocked
                {
                    DeathPlaceholder.SetActive(false);
                    DeathPlayer.SetActive(true);
                }
                else //Death is locked, show its infos
                {
                    UpdateDeath(slimecount);

                }
            }
            else//Earth and death are locked, show their infos
            {
                UpdateEarth(slimecount);
                UpdateDeath(slimecount);
            }
        }
        else//Ice,Earth and death are locked, show their infos
        {

            UpdateIce(slimecount);
            UpdateEarth(slimecount);
            UpdateDeath(slimecount);
        }
    }

    private void UpdateIce(int slimecount)
    {
        IceText.text = slimecount + "";
        IceMaxText.text = "/"+ActualSave.actualSave.slimesToUnlock(1);
        IceMaterial.SetFloat("Progress", 100f * slimecount / ActualSave.actualSave.slimesToUnlock(1));//We update the slimematerial, frome 0% to 100%
    }

    private void UpdateEarth(int slimecount)
    {
        EarthText.text = slimecount + "";
        EarthMaxText.text = "/" + ActualSave.actualSave.slimesToUnlock(2);
        EarthMaterial.SetFloat("Progress", 100f * slimecount / ActualSave.actualSave.slimesToUnlock(2)); //We update the slimematerial, frome 0% to 100%
    }

    private void UpdateDeath(int slimecount)
    {
        DeathText.text = slimecount + "";
        DeathMaxText.text = "/" + ActualSave.actualSave.slimesToUnlock(3);
        DeathMaterial.SetFloat("Progress", 100f * slimecount / ActualSave.actualSave.slimesToUnlock(3)); //We update the slimematerial, frome 0% to 100%
    }


    private bool isIceUnlocked(int slimecount)
    {
        return slimecount >= ActualSave.actualSave.slimesToUnlock(1);
    }

    private bool isEarthUnlocked(int slimecount)
    {
        return slimecount >= ActualSave.actualSave.slimesToUnlock(2);
    }

    private bool isDeathUnlocked(int slimecount)
    {
        return slimecount >= ActualSave.actualSave.slimesToUnlock(3);
    }

    public void confirmChoice()
    {
        ActualSave.actualSave.chosenPlayer = selected;
        SaveSystem.SaveGame(ActualSave.actualSave,ActualSave.saveSlot);

        switch (selected)
        {
            case 1:
                IceSelected.LeanCancel();
                IceSelected.LeanScale(Vector3.one * 1.2f, 0.1f).setLoopPingPong(1);
                Invoke("burstIce",0.1f);
                break;
            case 2:
                EarthSelected.LeanCancel();
                EarthSelected.LeanScale(Vector3.one * 1.2f, 0.1f).setLoopPingPong(1);
                Invoke("burstEarth", 0.1f);

                break;
            case 3:
                DeathSelected.LeanCancel();
                DeathSelected.LeanScale(Vector3.one * 1.2f, 0.1f).setLoopPingPong(1);
                Invoke("burstDeath", 0.1f);

                break;
            default:
                FireSelected.LeanCancel();
                FireSelected.LeanScale(Vector3.one * 1.2f, 0.1f).setLoopPingPong(1);
                Invoke("burstFire", 0.1f);

                break;
        }
    }

    public void burstFire()
    {
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
        FireSelected.GetComponent<ParticleSystem>().Emit(emitOverride, 30);
    }

    public void burstIce()
    {
        ParticleSystem.EmitParams emitOverrideIce = new ParticleSystem.EmitParams();
        IceSelected.GetComponent<ParticleSystem>().Emit(emitOverrideIce, 30);
    }

    public void burstEarth()
    {
        ParticleSystem.EmitParams emitOverrideEarth = new ParticleSystem.EmitParams();
        EarthSelected.GetComponent<ParticleSystem>().Emit(emitOverrideEarth, 30);
    }

    public void burstDeath()
    {
        ParticleSystem.EmitParams emitOverrideDeath = new ParticleSystem.EmitParams();
        DeathSelected.GetComponent<ParticleSystem>().Emit(emitOverrideDeath, 30);
    }

    public void goLeft()
    {

        switch (selected)
        {
            case 1:
                FireSelected.SetActive(true);
                FireSelected.transform.localScale = Vector3.zero;
                FireSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                IceSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                Invoke("deactivateIce", 0.3f);
                selected = 0;
                break;
            case 2:
                IceSelected.SetActive(true);
                IceSelected.transform.localScale = Vector3.zero;
                IceSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                EarthSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                Invoke("deactivateEarth", 0.3f);
                selected = 1;
                break;
            case 3:
                EarthSelected.SetActive(true);
                EarthSelected.transform.localScale = Vector3.zero;
                EarthSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                DeathSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                Invoke("deactivateDeath", 0.3f);
                selected = 2;
                break;
            default:
                if (isDeathUnlocked(slimecount))
                {
                    DeathSelected.SetActive(true);
                    DeathSelected.transform.localScale = Vector3.zero;
                    DeathSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                    FireSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                    Invoke("deactivateFire", 0.3f);
                    selected = 3;
                }else if (isEarthUnlocked(slimecount))
                {
                    EarthSelected.SetActive(true);
                    EarthSelected.transform.localScale = Vector3.zero;
                    EarthSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                    FireSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                    Invoke("deactivateFire", 0.3f);
                    selected = 2;
                }else if (isIceUnlocked(slimecount))
                {
                    IceSelected.SetActive(true);
                    IceSelected.transform.localScale = Vector3.zero;
                    IceSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                    FireSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                    Invoke("deactivateFire", 0.3f);
                    selected = 1;
                }
                else
                {
                    if(!FireSelected.LeanIsTweening())
                        FireSelected.LeanScale(Vector3.one * 0.8f, 0.2f).setLoopPingPong(1);
                }
                break;
        }
    }

    public void goRight()
    {
        switch (selected)
        {
            case 1:
                if (isEarthUnlocked(slimecount))
                {
                    EarthSelected.SetActive(true);
                    EarthSelected.transform.localScale = Vector3.zero;
                    EarthSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                    IceSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                    Invoke("deactivateIce", 0.3f);
                    selected = 2;
                }
                else
                {
                    FireSelected.SetActive(true);
                    FireSelected.transform.localScale = Vector3.zero;
                    FireSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                    IceSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                    Invoke("deactivateIce", 0.3f);
                    selected = 0;
                }
                break;
            case 2:
                if (isDeathUnlocked(slimecount))
                {
                    DeathSelected.SetActive(true);
                    DeathSelected.transform.localScale = Vector3.zero;
                    DeathSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                    EarthSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                    Invoke("deactivateEarth", 0.3f);
                    selected = 3;
                }
                else
                {
                    FireSelected.SetActive(true);
                    FireSelected.transform.localScale = Vector3.zero;
                    FireSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                    EarthSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                    Invoke("deactivateEarth", 0.3f);
                    selected = 0;
                }
                break;
            case 3:
                FireSelected.SetActive(true);
                FireSelected.transform.localScale = Vector3.zero;
                FireSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                DeathSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                Invoke("deactivateDeath", 0.3f);
                selected = 0;
                break;
            default:
                if (isIceUnlocked(slimecount))
                {
                    IceSelected.SetActive(true);
                    IceSelected.transform.localScale = Vector3.zero;
                    IceSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                    FireSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                    Invoke("deactivateFire", 0.3f);
                    selected = 1;
                }
                else
                {
                    if (!FireSelected.LeanIsTweening())
                        FireSelected.LeanScale(Vector3.one * 0.8f, 0.2f).setLoopPingPong(1);
                }
                break;
        }

    }

    public void QuitSelection()
    {
        switch (selected)
        {
            case 1:
                IceSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                Invoke("deactivateIce", 0.3f);
                break;
            case 2:
                EarthSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                Invoke("deactivateEarth", 0.3f);
                break;
            case 3:
                DeathSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                Invoke("deactivateDeath", 0.3f);
                break;
            default:
                FireSelected.transform.LeanScale(Vector3.zero, 0.3f).setEaseInExpo();
                Invoke("deactivateFire", 0.3f);
                break;
        }
        selected = -1;

    }

    public void deactivateFire()
    {
        if(selected != 0)
            FireSelected.SetActive(false);
    }

    public void deactivateIce()
    {
        if (selected != 1)
            IceSelected.SetActive(false);
    }

    public void deactivateEarth()
    {
        if (selected != 2)
            EarthSelected.SetActive(false);
    }

    public void deactivateDeath()
    {
        if (selected != 3)
            DeathSelected.SetActive(false);
    }

    public void EnterSelection()
    {
        selected = ActualSave.actualSave.chosenPlayer;
        switch (selected)
        {

            case 1:
                IceSelected.SetActive(true);
                IceSelected.transform.localScale = Vector3.zero;
                IceSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                break;
            case 2:
                EarthSelected.SetActive(true);
                EarthSelected.transform.localScale = Vector3.zero;
                EarthSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                break;
            case 3:
                DeathSelected.SetActive(true);
                DeathSelected.transform.localScale = Vector3.zero;
                DeathSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                break;
            default:
                FireSelected.SetActive(true);
                FireSelected.transform.localScale = Vector3.zero;
                FireSelected.transform.LeanScale(Vector3.one, 0.5f).setEaseOutExpo();
                break;
        }
    }
}
