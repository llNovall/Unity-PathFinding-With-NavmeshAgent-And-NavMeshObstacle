using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _navMeshAgent;
    [SerializeField]
    private NavMeshObstacle _navMeshObstacle;

    [SerializeField]
    private bool _hasDestination = false; 

    [SerializeField]
    private bool _isDestinationSet = false;

    [SerializeField]
    private Vector3 _currentDestination;

    [SerializeField]
    private float _acceptableDistanceFromTarget = 2;

    [SerializeField]
    private float _secondsToWait = 2;

    [SerializeField]
    private bool _isWaiting = false; 

    private void Awake()
    {
        _navMeshAgent.enabled = true;
        _navMeshObstacle.enabled = false;
    }

    void Update()
    {
        if (!_isWaiting)
        {
            if (!_hasDestination)
            {
                _currentDestination = FindDestination();
                _hasDestination = true;
            }
            else
            {
                if (!_isDestinationSet)
                    _isDestinationSet = _navMeshAgent.SetDestination(_currentDestination);

                if (!_navMeshAgent.pathPending)
                {
                    if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                    {
                        _navMeshAgent.isStopped = false;

                        if ((gameObject.transform.position - _currentDestination).sqrMagnitude <= Mathf.Pow(_acceptableDistanceFromTarget, 2))
                        {                          
                            _isWaiting = true;
                            _navMeshAgent.enabled = false;
                            _navMeshObstacle.enabled = true;
                            StartCoroutine(ChangeWaitStatus(false));
                        }
                    }
                    else if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid || _navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial)
                    {
                        _isDestinationSet = false;
                        _hasDestination = false;
                    }
                }
            }
        }
    }

    public IEnumerator ChangeWaitStatus(bool value)
    {
        yield return new WaitForSeconds(_secondsToWait);

        _navMeshObstacle.enabled = false;
        _navMeshAgent.enabled = true;
        _isWaiting = false;

        yield return new WaitForFixedUpdate();
        _isDestinationSet = false;
        _hasDestination = false;
    }

    public Vector3 FindDestination()
    {
        NavMeshHit navMeshHit;
        Vector3 dir = Random.insideUnitSphere * 50;
        NavMesh.SamplePosition(dir, out navMeshHit, 200, 1);
  
        return navMeshHit.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(gameObject.transform.position, _currentDestination);           
    }
}
