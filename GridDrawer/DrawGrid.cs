using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// UnityEditorでグリッドを描画しオブジェクトをスナップさせるクラス
/// </summary>
[CustomEditor(typeof(Transform))]
public class DrawGrid : Editor {
	
	// グリッドの間隔
	public const float gridDistance = 0.1f;

	void OnSceneGUI()
	{
		// グリッドを非表示にする場合は処理を止める
		if (!GridEditWindow.isGridEnabled)
		{
			return;
		}
		
		// グリッドの色。
		Color color;

		// グリッド描画の開始地点をGridEditWindowから参照
		Vector3 originPos = GridEditWindow.originPos;
		
		// グリッドの本数　＝　面積÷間隔
		float numX = GridEditWindow.gridAreaY / gridDistance;
		float numY = GridEditWindow.gridAreaY / gridDistance;

		// 縦線を描画。開始位置から1本ずつ右に描画していく。
		for(int x = 0; x <= numX; x++)
		{
			// 線の開始位置を計算。
			Vector3 pos = originPos + Vector3.right * x * gridDistance;
			
			// 5の倍数は白にして目立たせる
			if (x % 5 == 0)
			{
				// 0.7をかけて色を落ち着かせる
				color = Color.white * 0.7f;
			}
			// それ以外は水色
			else
			{
				color = Color.cyan * 0.7f;
			}

			// 線を描画
			Debug.DrawLine(pos, pos + Vector3.up * GridEditWindow.gridAreaY, color);
		}

		// 横線を描画。開始位置から1本ずつ上に描画していく。
		for (int y = 0; y <= numY; y++)
		{
			Vector3 pos = originPos + Vector3.up * y * gridDistance;
			
			if (y % 5 == 0)
			{
				color = Color.white * 0.7f;
			}
			else
			{
				color = Color.cyan * 0.7f;
			}

			Debug.DrawLine(pos, pos + Vector3.right * GridEditWindow.gridAreaY, color);
		}

		// グリッドにぴったりと合わせるオブジェクトのトランスフォームを取得
		Transform objectTransform = target as Transform;

		// オブジェクトをスナップさせる処理
		Vector3 objectPosition = objectTransform.transform.position;
		objectPosition.x = Mathf.Floor(objectPosition.x / gridDistance) * gridDistance;
		objectPosition.y = Mathf.Floor(objectPosition.y / gridDistance) * gridDistance;
		objectTransform.transform.position = objectPosition;

		// 再描画
		EditorUtility.SetDirty(target);
	}

	private void OnDisable()
	{
		// 再描画
		EditorUtility.SetDirty(target);
	}
}
