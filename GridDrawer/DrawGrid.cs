using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Linq;

/// <summary>
/// UnityEditorでグリッドを描画しオブジェクトをスナップさせるクラス
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(Transform))]
public class DrawGrid : Editor {

	GameObject[] selectionObjects;

	bool IsFilterdSelection
	{
		get
		{
			return SelectWindow.canLimitSelectedObject || SelectWindow.targetScript != null;
		}
	}

	public void SelectObjects()
	{
		selectionObjects = FindObjectsOfType<GameObject>();

		if(selectionObjects == null)
		{
			Debug.Log("ゲームオブジェクトが一つもありません");
		}

		Selection.objects = selectionObjects;

		SetSelectObjects();
	}

	void SetSelectObjects()
	{
		SelectFilterByTag();

		SelectFilterByScripts();

		if (!IsFilterdSelection)
		{
			Selection.objects = Selection.objects != null ? Selection.objects : selectionObjects;
		}
	}

	void SelectFilterByTag()
	{
		if (!SelectWindow.canLimitSelectedObject || SelectWindow.targetTag == null)
		{
			return;
		}

		if (!string.IsNullOrEmpty(SelectWindow.targetTag))
		{
			var targetTag = SelectWindow.targetTag;

			Selection.objects = Selection.gameObjects.Where(o => o.gameObject.tag == targetTag).ToArray();

			if (Selection.objects == null)
			{
				Debug.Log("指定のタグのオブジェクトがありません");
			}
		}
	}

	void SelectFilterByScripts()
	{
		if(SelectWindow.targetScript != null)
		{
			MonoScript targetScript = SelectWindow.targetScript as MonoScript;

			Selection.objects = Selection.gameObjects.Where(o =>
			o.GetComponents<MonoBehaviour>()
			.FirstOrDefault(component =>
				component.GetType() == targetScript.GetClass()
			)).ToArray();
		}
	}

	public void SnapModifiedHeight(float marginY)
	{
		if(Selection.objects == null)
		{
			Debug.LogError("オブジェクトを選択してください");
			return;
		}

		foreach(GameObject o in Selection.gameObjects)
		{

			Transform trfm = o.transform;
			float modifiedHeight = 0;

			modifiedHeight = trfm.position.y * 0.1f;
			modifiedHeight = Mathf.Round(modifiedHeight);
			modifiedHeight = modifiedHeight * 10;

			trfm.position = new Vector3(trfm.position.x, modifiedHeight + marginY, trfm.position.z);
		}
	}

	void SnapDragedObjects()
	{
		Transform[] objectTransforms;

		SetSelectObjects();

		if(Selection.transforms == null)
		{
			return;
		}

		// グリッドにぴったりと合わせるオブジェクトのトランスフォームを取得
		objectTransforms = Selection.transforms as Transform[];

		// オブジェクトをスナップさせる処理
		foreach (Transform objectTransform in objectTransforms)
		{
			Vector3 objectPosition = objectTransform.transform.position;
			objectPosition.x = Mathf.Floor(objectPosition.x / GridEditWindow.gridDistance) * GridEditWindow.gridDistance;
			objectPosition.y = Mathf.Floor(objectPosition.y / GridEditWindow.gridDistance) * GridEditWindow.gridDistance;
			objectTransform.transform.position = objectPosition;
		}
	}

	void OnSceneGUI()
	{
		if (GridEditWindow.dragObjectSnapped)
		{
			SnapDragedObjects();
		}

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
		float numX = GridEditWindow.gridAreaX / GridEditWindow.gridDistance;
		float numY = GridEditWindow.gridAreaY / GridEditWindow.gridDistance;

		// 縦線を描画。開始位置から1本ずつ右に描画していく。
		for(int x = 0; x <= numX; x++)
		{
			// 線の開始位置を計算。
			Vector3 pos = originPos + Vector3.right * x * GridEditWindow.gridDistance;
			
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
			Vector3 pos = originPos + Vector3.up * y * GridEditWindow.gridDistance;
			
			if (y % 5 == 0)
			{
				color = Color.white * 0.7f;
			}
			else
			{
				color = Color.cyan * 0.7f;
			}

			Debug.DrawLine(pos, pos + Vector3.right * GridEditWindow.gridAreaX, color);
		}

		EditorUtility.SetDirty(target);
	}

	private void OnDisable()
	{
		EditorUtility.SetDirty(target);
	}
}
