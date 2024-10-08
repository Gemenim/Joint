using System.Collections;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private HingeJoint _joint;
    [SerializeField] private float _timeForce;

    public void Shake()
    {
        StartCoroutine(SwichMotor());
    }

    private IEnumerator SwichMotor()
    {
        WaitForSeconds time = new WaitForSeconds(_timeForce);

        _joint.useMotor = true;

        yield return time;

        _joint.useMotor = false;
    }
}
