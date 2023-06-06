using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public List<Phases> phases;

    int phaseCounter = 0;

    void SummonObjects()
    {
        if(!phases[phaseCounter].isStagger)
        {
            foreach (GameObject curr in phases[phaseCounter].objectsToSummon)
            {
                curr.SetActive(true);
            }
        }

        else
        {
            StartCoroutine(StaggerRoutine(phases[phaseCounter].staggerInterval));
        }
       

        ++phaseCounter;
    }

    IEnumerator StaggerRoutine(float staggerTime)
    {
        foreach (GameObject curr in phases[phaseCounter].objectsToSummon)
        { 
            if(curr != null)
            curr.SetActive(true);

            yield return new WaitForSeconds(staggerTime);
        }
    }
}
