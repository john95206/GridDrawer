using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// グリッドのパラメータを制御するウィンドウ
/// </summary>
public class GridEditWindow : EditorWindow
{
	// NOTE: 
	// 各変数はDrawGrid.csで参照するためstaticにする
	// グリッドを表示するかどうか
	public static bool isGridEnabled = false;
	// グリッドの開始地点
	public static Vector2 originPos = Vector2.zero;
	// グリッドの面積の横の長さ
	public static int gridAreaX = 0;
	// グリッドの面積の縦の長さ
	public static int gridAreaY = 0;
	// グリッドの間隔
	public static float gridDistance = 0.1f;
	// ドラッグしているオブジェクトをスナップするかどうか
	public static bool dragObjectSnapped =	true;
	public static string targetTag;
	float marginY;

	// エディタの上のメニュータブからウィンドウを呼び出せるようにする
	[MenuItem("BBC/GridEditor")]
	static void Open()
	{
		EditorWindow.GetWindow<GridEditWindow>("GridEditor");
	}

	private void OnGUI()
	{
		// グリッドの開始地点を編集できるようにする
		originPos = EditorGUILayout.Vector2Field("Position", originPos);
		// グリッドの横の長さを編集できるようにする
		gridAreaX = EditorGUILayout.IntField("描画するX座標の範囲", gridAreaX);
		// グリッドの縦の長さを編集できるようにする
		gridAreaY = EditorGUILayout.IntField("描画するY座標の範囲", gridAreaY);
		// ドラッグしたオブジェクトをスナップさせるかどうかをボタンで実装
		dragObjectSnapped = EditorGUILayout.Toggle("ドラッグしたオブジェクトをスナップさせる", dragObjectSnapped);

		GUILayout.Space(5);
		
		EditorGUILayout.LabelField("選択のオブジェクトを地面から指定pixelだけ離す");
		marginY = EditorGUILayout.FloatField("何pixel離す？", marginY);
		EditorGUILayout.LabelField("選択のオブジェクトを地面から" + marginY + "pixel離す");
		if (GUILayout.Button("Margin Snap"))
		{
			DrawGrid drawGrid = ScriptableObject.CreateInstance<DrawGrid>();
			drawGrid.SnapModifiedHeight(marginY * 0.01f);
		}

		// 間をあける
		GUILayout.Space(30);

		EditorGUILayout.LabelField("Grid描画ボタン");
		// グリッドを描画するかどうかをチェックボックスで変更できるようにする
		isGridEnabled = EditorGUILayout.Toggle("Grid描画", isGridEnabled);
	}
}
