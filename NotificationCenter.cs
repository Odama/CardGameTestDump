using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Handler = System.Action<System.Object, System.Object>;


using SenderTable = System.Collections.Generic.Dictionary<System.Object, System.Collections.Generic.List<System.Action<System.Object, System.Object>>>;

namespace Notifications
{
	public class NotificationCenter
	{
		private Dictionary<string, SenderTable> notifications = new Dictionary<string, SenderTable>();
		private HashSet<List<Handler>> invokers = new HashSet<List<Handler>>();
		
		public readonly static NotificationCenter instance = new NotificationCenter();
		private NotificationCenter() {}
		
		public void AddObserver (Handler handler, string notificationName)
		{
			AddObserver(handler, notificationName, null);
		}
		
		public void AddObserver (Handler handler, string notificationName, System.Object sender)
		{
			if (handler == null)
			{
				Debug.LogError("null event handler, " + notificationName);
				return;
			}
			
			if (string.IsNullOrEmpty(notificationName))
			{
				Debug.LogError("unnamed notification");
				return;
			}
			
			if (!notifications.ContainsKey(notificationName))
				notifications.Add(notificationName, new SenderTable());
			
			SenderTable subTable = notifications[notificationName];
			
			System.Object key = (sender != null) ? sender : this;
			
			if (!subTable.ContainsKey(key))
				subTable.Add(key, new List<Handler>());
			
			List<Handler> list = subTable[key];
			if (!list.Contains(handler))
			{
				if (invokers.Contains(list))
					subTable[key] = list = new List<Handler>(list);
				
				list.Add( handler );
			}
		}
		
		public void RemoveObserver (Handler handler, string notificationName)
		{
			RemoveObserver(handler, notificationName, null);
		}
		
		public void RemoveObserver (Handler handler, string notificationName, System.Object sender)
		{
			if (handler == null)
			{
				Debug.LogError("null event handler, " + notificationName);
				return;
			}
			
			if (string.IsNullOrEmpty(notificationName))
			{
				Debug.LogError("missing notification name.");
				return;
			}
			
			if (!notifications.ContainsKey(notificationName))
				return;
			
			SenderTable subTable = notifications[notificationName];
			System.Object key = (sender != null) ? sender : this;
			
			if (!subTable.ContainsKey(key))
				return;
			
			List<Handler> handlers = subTable[key];
			int i = handlers.IndexOf(handler);

			if (i != -1)
			{
				if (invokers.Contains(handlers))
					subTable[key] = handlers = new List<Handler>(handlers);
				// double check this
				list.RemoveAt(i);
			}
		}
		
		public void Clean ()
		{
			string[] notKeys = new string[notifications.Keys.Count];
			notifications.Keys.CopyTo(notKeys, 0);
			
			for (int i = notKeys.Length - 1; i >= 0; --i)
			{
				string notificationName = notKeys[i];
				SenderTable senderTable = notifications[notificationName];
				
				object[] senKeys = new object[senderTable.Keys.Count];
				senderTable.Keys.CopyTo(senKeys, 0);
				
				for (int j = senKeys.Length - 1; j >= 0; --j)
				{
					object sender = senKeys[j];
					List<Handler> handlers = senderTable[sender];
					if (handlers.Count == 0)
						senderTable.Remove(sender);
				}
				
				if (senderTable.Count == 0)
					notifications.Remove(notificationName);
			}
		}
		
		public void PostNotification (string notificationName)
		{
			PostNotification(notificationName, null);
		}
		
		public void PostNotification (string notificationName, System.Object sender)
		{
			PostNotification(notificationName, sender, null);
		}
		
		public void PostNotification (string notificationName, System.Object sender, System.Object e)
		{
			if (string.IsNullOrEmpty(notificationName))
			{
				Debug.LogError("Missing notification name.");
				return;
			}
			
			if (!notifications.ContainsKey(notificationName))
				return;
			
			SenderTable subTable = notifications[notificationName];
			if (sender != null && subTable.ContainsKey(sender))
			{
				List<Handler> handlers = subTable[sender];
				invokers.Add(handlers);
				for (int i = 0; i < handlers.Count; ++i)
					handlers[i](sender, e);
				invokers.Remove(handlers);
			}
			
			if (subTable.ContainsKey(this))
			{
				List<Handler> handlers = subTable[this];
				invokers.Add(handlers);
				for (int i = 0; i < handlers.Count; ++i)
					handlers[i](sender, e);
				invokers.Remove(handlers);
			}
		}
	}
}