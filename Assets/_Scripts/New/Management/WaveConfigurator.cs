using UnityEngine;
using WaveOne.StartPoints.StartPointPickers;
using WaveOne.StartPoints;

namespace WaveOne
{
#pragma warning disable 0649
    public class WaveConfigurator : MonoBehaviour
    {
        [SerializeField] private StartPointEnum.StartPointType startPointType;
        [SerializeField] private StartPointPickerEnum.StartPointPickerType startPointPickerType;

        private StartPointEnum.StartPointType prevStartPointType;
        private StartPointPickerEnum.StartPointPickerType prevStartPointPickerType;

        private IStartPoint startPointScript;
        private Component currentStartPoint;
        private Component currentStartPointPicker;

        public void AddComponents()
        {
            if (prevStartPointType != startPointType ||
                prevStartPointPickerType != startPointPickerType ||
                currentStartPoint == null || currentStartPointPicker == null)
            {
                if (currentStartPointPicker != null)
                    DestroyImmediate(currentStartPointPicker);

                if (prevStartPointType != startPointType && currentStartPoint != null)
                    DestroyImmediate(currentStartPoint);

                switch (startPointType)
                {
                    case StartPointEnum.StartPointType.Transform:
                        switch (startPointPickerType)
                        {
                            case StartPointPickerEnum.StartPointPickerType.InOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(InOrderTransform));
                                break;
                            case StartPointPickerEnum.StartPointPickerType.ReverseOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(ReverseOrderTransform));
                                break;
                        }
                        break;
                    case StartPointEnum.StartPointType.Sphere:
                        switch (startPointPickerType)
                        {
                            case StartPointPickerEnum.StartPointPickerType.InOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(InOrderSphere));
                                break;
                            case StartPointPickerEnum.StartPointPickerType.ReverseOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(ReverseOrderSphere));
                                break;
                        }
                        break;
                }

                startPointScript = GetComponent<IStartPoint>();
                currentStartPoint = startPointScript as Component;

                prevStartPointType = startPointType;
                prevStartPointPickerType = startPointPickerType;
            }
        }
    }
}