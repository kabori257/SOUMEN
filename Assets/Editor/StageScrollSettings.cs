using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageScroll))]
public class StageScrollSettings : Editor
{
    private StageScroll _target;

    SerializedProperty debugMode;
    SerializedProperty goalPosCount;
    SerializedProperty speed;
    SerializedProperty startAreaRange;
    SerializedProperty cornerMargin;
    SerializedProperty cornerSetting;
    SerializedProperty centerChopStickRow;
    SerializedProperty centerChopStickColumn;
    SerializedProperty centerDispertion;
    SerializedProperty cornerChopStickRow;
    SerializedProperty cornerChopStickColumn;
    SerializedProperty cornerDispertion;
    SerializedProperty chopStickRowNum;
    SerializedProperty chopStickColumnNum;
    SerializedProperty dispertion;
    SerializedProperty cornerRate;

    private void Awake()
    {
        _target = target as StageScroll;
        debugMode = serializedObject.FindProperty("debugMode");
        goalPosCount = serializedObject.FindProperty("goalPosCount");
        speed = serializedObject.FindProperty("speed");
        startAreaRange = serializedObject.FindProperty("startAreaRange");
        cornerMargin = serializedObject.FindProperty("cornerMargin");
        cornerSetting = serializedObject.FindProperty("cornerSetting");
        centerChopStickRow = serializedObject.FindProperty("centerChopStickRow");
        centerChopStickColumn = serializedObject.FindProperty("centerChopStickColumn");
        centerDispertion = serializedObject.FindProperty("centerDispertion");
        cornerChopStickRow = serializedObject.FindProperty("cornerChopStickRow");
        cornerChopStickColumn = serializedObject.FindProperty("cornerChopStickColumn");
        cornerDispertion = serializedObject.FindProperty("cornerDispertion");
        chopStickRowNum = serializedObject.FindProperty("chopStickRowNum");
        chopStickColumnNum = serializedObject.FindProperty("chopStickColumnNum");
        dispertion = serializedObject.FindProperty("dispertion");
        cornerRate = serializedObject.FindProperty("cornerRate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        debugMode.boolValue = EditorGUILayout.ToggleLeft("デバッグモード", debugMode.boolValue);
        goalPosCount.intValue = EditorGUILayout.IntField("ゴールの長さ", goalPosCount.intValue);
        speed.floatValue = EditorGUILayout.FloatField("進む速さ", speed.floatValue);
        startAreaRange.floatValue = EditorGUILayout.Slider("最初に箸が来ない範囲", startAreaRange.floatValue, 0f, 300f);
        cornerMargin.floatValue = EditorGUILayout.Slider("角の大きさ(割合)", cornerMargin.floatValue, 0f, 1f);
        cornerSetting.boolValue = EditorGUILayout.ToggleLeft("角を別に設定する", cornerSetting.boolValue);

        if (cornerSetting.boolValue)
        {
            centerChopStickRow.intValue = EditorGUILayout.IntField("真ん中の箸の行数", centerChopStickRow.intValue);
            centerChopStickColumn.intValue = EditorGUILayout.IntField("真ん中の箸の列数", centerChopStickColumn.intValue);
            centerDispertion.floatValue = EditorGUILayout.Slider("真ん中縦方向のばらつき", centerDispertion.floatValue, 0f, 10f);

            cornerChopStickRow.intValue = EditorGUILayout.IntField("角の箸の行数", cornerChopStickRow.intValue);
            cornerChopStickColumn.intValue = EditorGUILayout.IntField("角の箸の列数", cornerChopStickColumn.intValue);
            cornerDispertion.floatValue = EditorGUILayout.Slider("角縦方向のばらつき", cornerDispertion.floatValue, 0f, 10f);
        }
        else
        {
            chopStickRowNum.intValue = EditorGUILayout.IntField("箸の行数", chopStickRowNum.intValue);
            chopStickColumnNum.intValue = EditorGUILayout.IntField("箸の列数", chopStickColumnNum.intValue);
            dispertion.floatValue = EditorGUILayout.Slider("縦方向のばらつき", dispertion.floatValue, 0f, 10f);
            cornerRate.floatValue = EditorGUILayout.Slider("角の箸の割合", cornerRate.floatValue, 0f, 1f);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
