using UnityEngine;

public class PlankPlayer : MonoBehaviour
{
    public VNectModel model;
    
    private float GetPositionVectorMagnitude()
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


    /// <summary>
    /// 얼마나 스쿼트 잘하고 있는지
    /// </summary>
    /// <returns>스쿼트 점수 2~5 그만두면 -1</returns>
    public int GetPositionScore()
    {
        float mag = GetPositionVectorMagnitude();
        float temp = Mathf.Abs(mag - 0.3f);
        if (temp < 0.1)
            return 5;
        if (temp < 0.2)
            return 4;
        if (temp < 0.3)
            return 3;
        if (temp < 0.4)
            return 2;
        return -1;
    }
    
}
