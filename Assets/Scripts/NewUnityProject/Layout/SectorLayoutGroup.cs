using System;
using UnityEngine;
using UnityEngine.UI;

namespace NewUnityProject.Layout
{
    public class SectorLayoutGroup  : LayoutGroup
    {
        [SerializeField] public double radius = 1000;
        [SerializeField] public bool reverse = false;
        
        public override void CalculateLayoutInputHorizontal()
        {
        }

        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
            // 親の幅に合わせて角度範囲を計算
            // 子の幅のほうが小さい場合は中央揃え
            var sumChildWidth = 0.0;
            foreach (RectTransform child in transform)
            {
                sumChildWidth += child.rect.width;
            }

            var rect = transform as RectTransform;
            var sectorRadian = Math.Min(rect.rect.width, sumChildWidth) / radius;

            var centerPosition = transform.position + new Vector3(0, (float) -radius, 0);
            
            var addedChildWidth = 0.0;
            foreach (RectTransform child in transform)
            {
                var currentRadian = sectorRadian * ((addedChildWidth + child.rect.width * 0.5) / sumChildWidth - 0.5);
                child.anchoredPosition = new Vector2((float) (Math.Sin(currentRadian) * radius), (float) ((Math.Cos(currentRadian) - 1) * radius));

                var rotate = (centerPosition - child.position).normalized;
                if (reverse)
                {
                    if (rotate.x == 0)
                    {
                        rotate.x = 1 / 512f; // マイナスゼロを表現できないため
                    }
                    child.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(rotate.x * -1, rotate.y, rotate.z));
                }
                else
                {
                    child.transform.rotation = Quaternion.FromToRotation(Vector3.down, rotate);
                }

                addedChildWidth += child.rect.width;
            }
        }
    }
}
