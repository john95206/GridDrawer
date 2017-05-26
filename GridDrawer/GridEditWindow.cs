using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// グリッドのパラメータを制御するウィンドウ
/// </summary>
[CustomEditor(typeof(Transform))]
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

	// エディタの上のメニュータブからウィンドウを呼び出せるようにする
	[MenuItem("GridEditorWindow/GridEditor")]
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

		// 間をあける
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		// グリッドを描画するかどうかをチェックボックスで変更できるようにする
		isGridEnabled = EditorGUILayout.Toggle("Grid描画", isGridEnabled);
	}
}
