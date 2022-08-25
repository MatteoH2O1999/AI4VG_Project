using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CoverAgentDelegates))]
public class CoverAgent : MonoBehaviour
{
    public float AITimeFrame;
    public float rangedAttackRange;

    private FSM fsm;

    // Start is called before the first frame update
    void Start()
    {
        this.CreateFSM();
        this.fsm.OnEntry(this);
        StartCoroutine(this.UpdateFSMCoroutine());
    }

    private void CreateFSM()
    {
        this.fsm = new CoverFSM();
    }

    IEnumerator UpdateFSMCoroutine()
    {
        while(true)
        {
            this.fsm.UpdateFSM(this);
            yield return new WaitForSeconds(this.AITimeFrame);
        }
    }
}
