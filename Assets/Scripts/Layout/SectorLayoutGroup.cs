using System;
using UnityEngine;
using UnityEngine.UI;

namespace Layout
{
    public class SectorLayoutGroup  : LayoutGroup
    {
        [SerializeField] public double radius = 1000;
        
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
            for (var i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i) as RectTransform;
	
                sumChildWidth += child.rect.width;
            }

            var rect = transform as RectTransform;
            var sectorRadian = Math.Min(rect.rect.width, sumChildWidth) / radius;

            var centerPosition = transform.position + new Vector3(0, (float) -radius, 0);
            
            var addedChildWidth = 0.0;
            for (var i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i) as RectTransform;
	
                var currentRadian = sectorRadian * ((addedChildWidth + child.rect.width * 0.5) / sumChildWidth - 0.5);
                child.anchoredPosition = new Vector2((float) (Math.Sin(currentRadian) * radius), (float) ((Math.Cos(currentRadian) - 1) * radius));
	
                var rotaion = (centerPosition - child.position).normalized;
                child.transform.rotation = Quaternion.FromToRotation(Vector3.down, rotaion);
	
                addedChildWidth += child.rect.width;
            }
        }
    }
}
