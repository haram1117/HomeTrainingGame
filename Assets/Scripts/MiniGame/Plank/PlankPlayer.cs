using UnityEngine;

public class PlankPlayer : MonoBehaviour
{
    public VNectModel model;
    
    public float GetPositionVectorMagnitude()
    {
        var modelJointPoints = model.JointPoints;
        var head = modelJointPoints[PositionIndex.head.Int()];
        var hip = modelJointPoints[PositionIndex.hip.Int()];
        var foot = (modelJointPoints[PositionIndex.rFoot.Int()].Pos3D + modelJointPoints[PositionIndex.lFoot.Int()].Pos3D) /2 ;

        Vector3 hip_to_head = head.Pos3D - hip.Pos3D;
        Vector3 hip_to_foot = foot - hip.Pos3D;

        hip_to_head = hip_to_head / hip_to_head.magnitude;
        hip_to_foot = hip_to_foot / hip_to_foot.magnitude;

        var result = (hip_to_head + hip_to_foot).magnitude;
        return result;
    }


    public int GetPositionScore()
    {
        float mag = GetPositionVectorMagnitude();
        float temp = Mathf.Abs(mag - 0.3f);
        if (temp < 0.15)
            return 5;
        if (temp < 0.3)
            return 4;
        if (temp < 0.45)
            return 3;
        if (temp < 0.6)
            return 2;
        return 0;

    }
    
    public void Start()
    {
        
    }

    public void Update()
    {
        
    }
}
