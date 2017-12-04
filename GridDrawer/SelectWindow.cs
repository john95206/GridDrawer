using UnityEngine;
using System.Collections;
using UnityEditor;

public class SelectWindow : EditorWindow {

	public static string targetTag;
	public static Object targetScript;
	public static bool canLimitSelectedObject = false;

	// エディタの上のメニュータブからウィンドウを呼び出せるようにする
	[MenuItem("BBC/SelectWindow")]
	static void Open()
	{
		EditorWindow.GetWindow<SelectWindow>("SelectWindow");
	}

	private void OnGUI()
	{
		GUILayout.Space(10);

		// タグのリスト
		targetTag = EditorGUILayout.TagField("タグ", targetTag);

		// 指定のスクリプト
		targetScript = EditorGUILayout.ObjectField("スクリプト", targetScript, typeof(MonoScript), true);

		GUILayout.Space(20);

		EditorGUILayout.LabelField("指定のタグ・スクリプトのオブジェクトのみ選択できるようにする");
		canLimitSelectedObject = GUILayout.Toggle(canLimitSelectedObject, "タグ検索を有効化");

		// 一括スナップさせるボタン
		if (GUILayout.Button("オブジェクト一括選択"))
		{
			DrawGrid drawGrid = ScriptableObject.CreateInstance<DrawGrid>();
			drawGrid.SelectObjects();
		}

		GUILayout.Space(20);

		if (GUILayout.Button("設定をクリア"))
		{
			ClearParametors();
		}
	}

	void ClearParametors()
	{
		targetTag = string.Empty;
		targetScript = null;
		canLimitSelectedObject = false;
	}

	private void OnDisable()
	{
		ClearParametors();
	}
}
