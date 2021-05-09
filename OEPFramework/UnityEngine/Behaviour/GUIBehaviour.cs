using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Basement.OEPFramework.UnityEngine.Behaviour
{
    public abstract class GUIBehaviour : ControlLoopBehaviour
    {
        public GameObject gameObject { get; private set; }
        public RectTransform rectTransform { get; private set; }
        
        protected string prefabPath;
        private RectTransform _parent;
        private readonly bool _external;
        private Dictionary<string, GameObject> _map;
        private string _startWith;
        private GameObject _template;

        protected GUIBehaviour(GameObject go)
        {
            _external = true;
            gameObject = go;
            rectTransform = gameObject.GetComponent<RectTransform>();
        }

        protected GUIBehaviour(string prefabPath, RectTransform parent)
        {
            _parent = parent;
            this.prefabPath = prefabPath;
        }
        protected GUIBehaviour(GameObject template, RectTransform parent)
        {
            _parent = parent;
            _template = template;
        }

        public void ResetRectTransform(bool resetLocalScale = false, bool resetSizeDelta = false, bool resetLocalRotation = false)
        {
            gameObject.SetActive(false);
            rectTransform.anchoredPosition = Vector3.zero;
            rectTransform.localPosition = Vector3.zero;

            if (resetLocalRotation)
                rectTransform.localRotation = Quaternion.Euler(Vector3.zero);

            if (resetLocalScale)
                rectTransform.localScale = Vector3.one;

            if (resetSizeDelta)
                rectTransform.sizeDelta = Vector3.zero;

            gameObject.SetActive(true);
        }

        public virtual void Create()
        {
            if (initialized) return;

            if (!_external)
            {
                gameObject = Object.Instantiate(_template ? _template : Resources.Load<GameObject>(prefabPath));
                _template = null;
                rectTransform = gameObject.GetComponent<RectTransform>();

                if (_parent != null)
                {
                    gameObject.SetActive(false);
                    gameObject.transform.SetParent(_parent, false);
                    gameObject.SetActive(true);
                }
            }
            else
                rectTransform = gameObject.GetComponent<RectTransform>();

            Initialize();
        }

        public void CreateElementsMap(string prefix = "_")
        {
            _startWith = prefix;
            _map = new Dictionary<string, GameObject>();
            InnerCreateMap(gameObject);
        }
        
        public bool HasElement(string elementName)
        {
            return _map.ContainsKey(elementName);
        }
        
        public bool TryGetElement(string elementName, out GameObject element)
        {
            return _map.TryGetValue(elementName, out element);
        }
        
        public bool TryGetElementComponent<T>(string elementName, out T component) where T : Component
        {
            if (_map.TryGetValue(elementName, out var element))
            {
                component = element.GetComponent<T>();
                return true;
            }
            component = null;
            return false;
        }

        public GameObject GetElement(string elementName)
        {
            return _map[elementName];
        }

        public T GetElementComponent<T>(string elementName) where T : Component
        {
            return _map[elementName].GetComponent<T>();
        }

        public RectTransform GetRectTransform(string name)
        {
            return _map[name].GetComponent<RectTransform>();
        }

        private void InnerCreateMap(GameObject go)
        {
            int count = go.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform t = go.transform.GetChild(i);

                if (t.name.StartsWith(_startWith))
                    _map.Add(t.name, t.gameObject);

                if (t.childCount > 0)
                    InnerCreateMap(t.gameObject);
            }
        }

        public void SetParent(RectTransform transform)
        {
            if (_external)
                throw new InvalidOperationException();

            _parent = transform;

            rectTransform.gameObject.SetActive(false);
            rectTransform.parent = _parent;
            rectTransform.gameObject.SetActive(true);
        }

        public virtual void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
                gameObject.SetActive(active);
        }

        protected override void OnUninitialize()
        {
            if (!_external)
                Object.Destroy(gameObject);
			base.OnUninitialize();
        }
    }
}
