using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFieldOfViewController))]
[CanEditMultipleObjects]
public class FieldOfViewVisualizer : Editor {

	void OnSceneGUI () {
		EnemyFieldOfViewController enemyFOV = target as EnemyFieldOfViewController;
		
		Handles.color = Color.grey;
		Handles.DrawWireArc (enemyFOV.transform.position, Vector3.up, Vector3.forward, 360, enemyFOV.viewRadius);

		Handles.color = Color.black;
		Vector3 angleLineA = enemyFOV.CalculateDirectionFromAngle( -enemyFOV.viewAngle/2 );
		Vector3 angleLineB = enemyFOV.CalculateDirectionFromAngle( enemyFOV.viewAngle/2 );
		Handles.DrawLine(enemyFOV.transform.position, enemyFOV.transform.position + angleLineA * enemyFOV.viewRadius);
		Handles.DrawLine(enemyFOV.transform.position, enemyFOV.transform.position + angleLineB * enemyFOV.viewRadius);
		Handles.color = new Color(1, 1, 1, .2f);
		Handles.DrawSolidArc(enemyFOV.transform.position, Vector3.up, angleLineA, enemyFOV.viewAngle, enemyFOV.viewRadius);
	}
}
