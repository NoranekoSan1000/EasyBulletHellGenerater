using System;
using UnityEditor;
using UnityEngine;

namespace EasyBulletHellGenerator
{
    [CustomEditor(typeof(BulletPattern))]
    public class BulletPatternEditor : Editor
    {
        private Texture2D image;

        public override void OnInspectorGUI()
        {
            BulletPattern bulletPattern = (BulletPattern)target;

            // Enum��\�����ĕύX�����m
            EditorGUI.BeginChangeCheck();
            bulletPattern.formation = (BulletsManager.ShotFormation)EditorGUILayout.EnumPopup("Formation", bulletPattern.formation);
            if (EditorGUI.EndChangeCheck())
            {
                // Enum�̕ύX���������ꍇ�ɉ摜���X�V
                image = null;
            }

            // �摜�\��
            viewImage(bulletPattern.formation.ToString());

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("---�e�̋����ݒ�---", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            

            EditorGUILayout.LabelField("�e�̃I�u�W�F�N�g", EditorStyles.boldLabel);
            bulletPattern.bulletObject = (GameObject)EditorGUILayout.ObjectField("bulletObject", bulletPattern.bulletObject, typeof(GameObject), true);

            EditorGUILayout.LabelField("�U��", EditorStyles.boldLabel);
            bulletPattern.isMissile = EditorGUILayout.Toggle("Is Missile", bulletPattern.isMissile);

            EditorGUILayout.LabelField("�����x", EditorStyles.boldLabel);
            bulletPattern.speed = EditorGUILayout.FloatField("Speed", bulletPattern.speed);

            EditorGUILayout.LabelField("�����x", EditorStyles.boldLabel);
            bulletPattern.acceleration = EditorGUILayout.FloatField("Acceleration", bulletPattern.acceleration);

            EditorGUILayout.LabelField("���ݎ���", EditorStyles.boldLabel);
            bulletPattern.existTime = EditorGUILayout.FloatField("Exist Time", bulletPattern.existTime);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("---�e���ݒ�---", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField("���s��", EditorStyles.boldLabel);
            bulletPattern.executionCount = EditorGUILayout.IntField("Execution Count", bulletPattern.executionCount);

            EditorGUILayout.LabelField("���s�Ԋu", EditorStyles.boldLabel);
            bulletPattern.interval = EditorGUILayout.FloatField("Interval", bulletPattern.interval);

            EditorGUILayout.LabelField("�P���s�Ŕ��˂���e��", EditorStyles.boldLabel);
            bulletPattern.numBullets = EditorGUILayout.IntField("Num Bullets", bulletPattern.numBullets);

            if (bulletPattern.formation == BulletsManager.ShotFormation.SpreadShot)
            {
                EditorGUILayout.LabelField("�g�U�p�x", EditorStyles.boldLabel);
                bulletPattern.spreadAngle = EditorGUILayout.FloatField("Spread Angle", bulletPattern.spreadAngle);
            }
            else if (bulletPattern.formation == BulletsManager.ShotFormation.SiegeShot)
            {
                EditorGUILayout.LabelField("��͔��a", EditorStyles.boldLabel);
                bulletPattern.siegeRadius = EditorGUILayout.FloatField("Siege Radius", bulletPattern.siegeRadius);
            }

            EditorUtility.SetDirty(bulletPattern);
        }

        private void viewImage(string fileName)
        {
            // �摜�̓ǂݍ��݂ƕ\��
            if (image == null)
            {
                string[] guids = AssetDatabase.FindAssets(fileName);
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    image = AssetDatabase.LoadMainAssetAtPath(path) as Texture2D;
                    if (image != null)
                    {
                        break;
                    }
                }
            }

            if (image != null)
            {
                const float maxLogoWidth = 430.0f;
                EditorGUILayout.Separator();
                float w = EditorGUIUtility.currentViewWidth;
                Rect r = new Rect();
                r.width = Math.Min(w - 40.0f, maxLogoWidth);
                r.height = r.width / 4f;
                Rect r2 = GUILayoutUtility.GetRect(r.width, r.height);
                r.x = ((EditorGUIUtility.currentViewWidth - r.width) * 0.5f) - 4.0f;
                r.y = r2.y;
                GUI.DrawTexture(r, image, ScaleMode.StretchToFill);
                EditorGUILayout.Separator();
            }
        }
    }
}