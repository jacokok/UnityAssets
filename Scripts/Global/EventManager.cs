using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;


namespace Kok
{
    public static class EventManager
    {
        private static Dictionary<int, List<Action>> _events = new Dictionary<int, List<Action>>();

        public static void Register(int eventType, Action handler)
        {
            List<Action> list = null;
            if (!_events.TryGetValue(eventType, out list))
            {
                list = new List<Action>();
                _events.Add(eventType, list);
            }

            if (!list.Contains(handler))
                _events[eventType].Add(handler);
        }

        public static void Unregister(int eventType, Action handler)
        {
            List<Action> list = null;
            if (_events.TryGetValue(eventType, out list))
            {
                if (list.Contains(handler))
                    list.Remove(handler);
            }
        }

        public static void Invoke(int eventType)
        {
            List<Action> list = null;
            if (_events.TryGetValue(eventType, out list))
            {
                for (var i = list.Count - 1; i >= 0; i--)
                    list[i]();
            }
        }

        public static void ClearEvents(int eventType)
        {
            if (_events.ContainsKey(eventType))
                _events.Remove(eventType);
        }

        public static void ClearEvents()
        {
            _events.Clear();
        }

        public static void LogListeners(int eventType)
        {
            if (_events.ContainsKey(eventType) == false || _events[eventType].Count == 0)
            {
                Debug.Log("eventType: " + eventType + " has no observers");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("eventType: " + eventType + " has " + _events[eventType].Count + " Observers");

            for (var i = 0; i < _events[eventType].Count; i++)
            {
                Action action = _events[eventType][i];
                sb.AppendLine("Target: " + action.Target + "\t Method: " + action.Method);
            }

            Debug.Log(sb.ToString());
        }
    }

    public static class EventManager<T>
    {
        private static Dictionary<int, List<Action<T>>> _events = new Dictionary<int, List<Action<T>>>();

        public static void Register(int eventType, Action<T> handler)
        {
            List<Action<T>> list = null;
            if (!_events.TryGetValue(eventType, out list))
            {
                list = new List<Action<T>>();
                _events.Add(eventType, list);
            }

            if (!list.Contains(handler))
                _events[eventType].Add(handler);
        }

        public static void Unregister(int eventType, Action<T> handler)
        {
            List<Action<T>> list = null;
            if (_events.TryGetValue(eventType, out list))
            {
                if (list.Contains(handler))
                    list.Remove(handler);
            }
        }


        public static void Invoke(int eventType, T param)
        {
            List<Action<T>> list = null;
            if (_events.TryGetValue(eventType, out list))
            {
                for (var i = list.Count - 1; i >= 0; i--)
                    list[i](param);
            }
        }


        public static void ClearEvents(int eventType)
        {
            if (_events.ContainsKey(eventType))
                _events.Remove(eventType);
        }


        public static void ClearEvents()
        {
            _events.Clear();
        }


        public static void LogListeners(int eventType)
        {
            if (_events.ContainsKey(eventType) == false || _events[eventType].Count == 0)
            {
                Debug.Log("eventType: " + eventType + " has no observers");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("eventType: " + eventType + " has " + _events[eventType].Count + " Observers");

            for (var i = 0; i < _events[eventType].Count; i++)
            {
                var action = _events[eventType][i];
                sb.AppendLine("Target: " + action.Target + "\t Method: " + action.Method);
            }

            Debug.Log(sb.ToString());
        }
    }


    public static class EventManager<T, U>
    {
        private static Dictionary<int, List<Action<T, U>>> _events = new Dictionary<int, List<Action<T, U>>>();

        public static void Register(int eventType, Action<T, U> handler)
        {
            List<Action<T, U>> list = null;
            if (!_events.TryGetValue(eventType, out list))
            {
                list = new List<Action<T, U>>();
                _events.Add(eventType, list);
            }

            if (!list.Contains(handler))
                _events[eventType].Add(handler);
        }


        public static void Unregister(int eventType, Action<T, U> handler)
        {
            List<Action<T, U>> list = null;
            if (_events.TryGetValue(eventType, out list))
            {
                if (list.Contains(handler))
                    list.Remove(handler);
            }
        }


        public static void Invoke(int eventType, T firstParam, U secondParam)
        {
            List<Action<T, U>> list = null;
            if (_events.TryGetValue(eventType, out list))
            {
                for (var i = list.Count - 1; i >= 0; i--)
                    list[i](firstParam, secondParam);
            }
        }


        public static void ClearEvents(int eventType)
        {
            if (_events.ContainsKey(eventType))
                _events.Remove(eventType);
        }


        public static void ClearEvents()
        {
            _events.Clear();
        }


        public static void LogListeners(int eventType)
        {
            if (_events.ContainsKey(eventType) == false || _events[eventType].Count == 0)
            {
                Debug.Log("eventType: " + eventType + " has no observers");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("eventType: " + eventType + " has " + _events[eventType].Count + " Observers");

            for (var i = 0; i < _events[eventType].Count; i++)
            {
                Action<T, U> action = _events[eventType][i];
                sb.AppendLine("Target: " + action.Target + "\t Method: " + action.Method);
            }

            Debug.Log(sb.ToString());
        }
    }
}