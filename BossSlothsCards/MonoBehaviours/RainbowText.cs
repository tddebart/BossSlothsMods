using System;
using BossSlothsCards;
using TMPro;
using UnityEngine;
using BossSlothsCards.Extensions;

namespace BossSlothsCards.MonoBehaviours
{
    public class RainbowText : MonoBehaviour
    {

        private TextMeshProUGUI textMesh;

        private Mesh mesh;

        private Vector3[] vertices;

        public Gradient rainbow = new Gradient();

        void Start()
        {
            rainbow.colorKeys = new[]
            {
                new GradientColorKey(Color.red, 0.1428f),
                new GradientColorKey(new Color(1, 0.5f, 0), 0.1428f * 2),
                new GradientColorKey(Color.yellow, 0.1428f * 3),
                new GradientColorKey(Color.green, 0.1428f * 4),
                new GradientColorKey(Color.blue, 0.1428f * 5),
                new GradientColorKey(new Color(0.29f, 0f, 0.51f), 0.1428f * 6),
                new GradientColorKey(new Color(0.58f, 0f, 0.83f), 0.1428f * 6)
            };

            textMesh = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;

            Color[] colors = mesh.colors;

            for (int w = 0; w < textMesh.textInfo.wordCount; w++)
            {
                var word = textMesh.textInfo.wordInfo[w].GetWord();

                if (word.Contains("random", StringComparison.OrdinalIgnoreCase) || word.Contains("randomly", StringComparison.OrdinalIgnoreCase))
                {
                    int wordIndex = textMesh.textInfo.wordInfo[w].firstCharacterIndex;

                    for (int i = 0; i < textMesh.textInfo.wordInfo[w].characterCount; i++)
                    {
                        TMP_CharacterInfo c = textMesh.textInfo.characterInfo[wordIndex + i];

                        int index = c.vertexIndex;

                        colors[index] =     rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index]    .x * 0.001f, 1f));
                        colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x * 0.001f, 1f));
                        colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x * 0.001f, 1f));
                        colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x * 0.001f, 1f));

                        vertices[index]     += (Vector3) Wobble(Time.time + i + 1);
                        vertices[index + 1] += (Vector3) Wobble(Time.time + i + 2);
                        vertices[index + 2] += (Vector3) Wobble(Time.time + i + 3);
                        vertices[index + 3] += (Vector3) Wobble(Time.time + i + 4);
                    }
                }
            }

            mesh.vertices = vertices;
            mesh.colors = colors;
            textMesh.canvasRenderer.SetMesh(mesh);
        }

        Vector2 Wobble(float time)
        {
            return new Vector2(Mathf.Sin(time * 3.3f * 3), Mathf.Cos(time * 2.5f * 3));
        }
    }
}