using UnityEngine;
using SemihOrhan.WaveOne.StartPoints.StartPointPickers;
using SemihOrhan.WaveOne.StartPoints;
using SemihOrhan.WaveOne.Spawners;
using SemihOrhan.WaveOne.EndPoints;

namespace SemihOrhan.WaveOne
{
#pragma warning disable 0649
    public class WaveConfigurator : MonoBehaviour
    {
        [Header("Start point")]
        [SerializeField] private StartPointEnum.StartPointType startPointType;
        [SerializeField] private StartPointPickerEnum.StartPointPickerType startPointPickerType;

        [Header("Spawner")]
        [SerializeField] private SpawnerEnum.SpawnerType spawnerType;

        [Header("End point")]
        [SerializeField] private EndPointsEnum.EndPointsType endPointsType;

        [SerializeField, HideInInspector] private StartPointEnum.StartPointType prevStartPointType;
        [SerializeField, HideInInspector] private StartPointPickerEnum.StartPointPickerType prevStartPointPickerType;
        [SerializeField, HideInInspector] private SpawnerEnum.SpawnerType prevSpawnerType;
        [SerializeField, HideInInspector] private EndPointsEnum.EndPointsType prevEndPointsType;

        [SerializeField, HideInInspector] private Component currentStartPoint;
        [SerializeField, HideInInspector] private Component currentStartPointPicker;
        [SerializeField, HideInInspector] private Component currentSpawner;
        [SerializeField, HideInInspector] private Component currentEndPoint;

        public IStartPoint StartPointScript { get; private set; }
        public ISpawner SpawnerScript { get; private set; }

        private void Start()
        {
            StartPointScript = GetComponent<IStartPoint>();
            SpawnerScript = GetComponent<ISpawner>();
        }

        public void AddStartPointComponents()
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
                    case StartPointEnum.StartPointType.Box:
                        switch (startPointPickerType)
                        {
                            case StartPointPickerEnum.StartPointPickerType.InOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(InOrderBox));
                                break;
                            case StartPointPickerEnum.StartPointPickerType.ReverseOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(ReverseOrderBox));
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

                currentStartPoint = GetComponent<IStartPoint>() as Component;

                prevStartPointType = startPointType;
                prevStartPointPickerType = startPointPickerType;
            }
        }

        public void AddSpawnerComponents()
        {
            if (prevSpawnerType != spawnerType ||
                currentSpawner == null)
            {
                if (currentSpawner != null)
                    DestroyImmediate(currentSpawner);

                switch (spawnerType)
                {
                    case SpawnerEnum.SpawnerType.Progressive:
                        gameObject.AddComponent(typeof(Progressive));
                        break;
                }

                currentSpawner = GetComponent<ISpawner>() as Component;

                prevSpawnerType = spawnerType;
            }
        }

        public void AddEndPointComponents()
        {
            if (prevEndPointsType != endPointsType ||
                currentEndPoint == null)
            {
                if (currentEndPoint != null)
                    DestroyImmediate(currentEndPoint);

                switch (endPointsType)
                {
                    case EndPointsEnum.EndPointsType.Disabled:
                        break;
                    case EndPointsEnum.EndPointsType.Enabled:
                        gameObject.AddComponent(typeof(EndPoint));
                        break;
                }

                currentEndPoint = GetComponent<EndPoint>() as Component;

                prevEndPointsType = endPointsType;
            }
        }

        public void RemoveAllComponents()
        {
            DestroyImmediate(currentStartPointPicker);
            DestroyImmediate(currentStartPoint);
            DestroyImmediate(currentSpawner);
            DestroyImmediate(currentEndPoint);
        }
    }
}