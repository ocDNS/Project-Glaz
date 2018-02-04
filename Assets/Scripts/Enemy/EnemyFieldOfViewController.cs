using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldOfViewController : MonoBehaviour {

	public delegate void PlayerInRangeDelegate();

	public PlayerInRangeDelegate PlayerInRangeAction;
	public float viewRadius;
	public float viewAngle;
	public LayerMask playerMask;
	public LayerMask obstacleMask;
	public float checkFrequency = .5f;
	public bool isPlayerInSight = false;
	public Transform playerTransform;
	

	void Start () {
		StartCoroutine(StartCheckForPlayer(checkFrequency));
	}

	public Vector3 CalculateDirectionFromAngle (float angle) {
		angle += transform.rotation.eulerAngles.y;
		return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
	}

	public IEnumerator StartCheckForPlayer (float waitTime) {
		while(true) {
			CheckForPlayer();
			yield return new WaitForSeconds(waitTime);
		}
	}

	void CheckForPlayer () {
		Collider[] hits;

		hits = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
		if(hits == null){
			isPlayerInSight = false;
			return;
		}

		foreach(Collider hit in hits) {
			if(hit.tag == "Player"){
				//Debug.Log(transform.name + " player in range");
				Vector3 dirToPlayer = (hit.transform.position - transform.position).normalized;

				if(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle/2){
					float distanceToPlayer = Vector3.Distance(transform.position, hit.transform.position);
					//Debug.Log("player in viewing range");
					if(!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask)){
						//Debug.Log("player in sight");
						Debug.DrawLine(transform.position, hit.transform.position, Color.red, checkFrequency);

						isPlayerInSight = true;
						playerTransform = hit.gameObject.transform;
					}
					else{
						isPlayerInSight = false;
					}
				}
				else{
					isPlayerInSight = false;
				}
			}
		}
	}
}
