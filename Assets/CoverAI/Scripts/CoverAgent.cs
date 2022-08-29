using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CoverAgentDelegates))]
public class CoverAgent : MonoBehaviour
{
    public float AITimeFrame;
    public float rangedAttackRange;
    public float meleeAttackRange;
    public float meleeChargeRange;
    public float gunHeightComparedToPivot;
    public float gunCrouchHeightComparedToPivot;
    public float pivotHeight;

    private FSM fsm;
    private CoverAgentDelegates delegates;
    private Vector3? cover = null;

    // Start is called before the first frame update
    void Start()
    {
        this.delegates = GetComponent<CoverAgentDelegates>();
        this.CreateFSM();
        this.fsm.OnEntry(this);
        StartCoroutine(this.UpdateFSMCoroutine());
    }

    public Vector3? GetCover()
    {
        if(this.cover == null)
            return null;
        Vector3 coverPosition = this.cover.Value;
        return new Vector3(coverPosition.x, coverPosition.y, coverPosition.z);
    }

    public void SetCover(Vector3? coverPoint)
    {
        if(coverPoint == null)
        {
            this.cover = null;
        }
        else
        {
            Vector3 coverPosition = coverPoint.Value;
            this.cover = new Vector3(coverPosition.x, coverPosition.y, coverPosition.z);
        }
    }

    public CoverAgentDelegates GetDelegates()
    {
        return this.delegates;
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

    private void OnDrawGizmos()
    {
        Vector3 localGunStart = new Vector3(0, this.gunHeightComparedToPivot / this.transform.lossyScale.y, 0);
        Vector3 localGunEnd = new Vector3(0, this.gunHeightComparedToPivot / this.transform.lossyScale.y, 2.5f);
        Vector3 worldGunStart = this.transform.localToWorldMatrix.MultiplyPoint(localGunStart);
        Vector3 worldGunEnd = this.transform.localToWorldMatrix.MultiplyPoint(localGunEnd);
        Vector3 localCrouchGunStart = new Vector3(0, this.gunCrouchHeightComparedToPivot / this.transform.lossyScale.y, 0);
        Vector3 localCrouchGunEnd = new Vector3(0, this.gunCrouchHeightComparedToPivot / this.transform.lossyScale.y, 2.5f);
        Vector3 worldCrouchGunStart = this.transform.localToWorldMatrix.MultiplyPoint(localCrouchGunStart);
        Vector3 worldCrouchGunEnd = this.transform.localToWorldMatrix.MultiplyPoint(localCrouchGunEnd);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(worldGunStart, worldGunEnd);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(worldCrouchGunStart, worldCrouchGunEnd);
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        Gizmos.DrawSphere(this.transform.position + new Vector3(0, -this.pivotHeight, 0), 0.5f);
    }
}
