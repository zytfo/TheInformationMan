using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// Adds and removes extra dialogue databases. This script only adds one
	/// database per frame, so it may take several frames to add a list of 
	/// databases.
	/// </summary>
	[AddComponentMenu("Dialogue System/Miscellaneous/Extra Databases")]
	public class ExtraDatabases : MonoBehaviour {

		/// <summary>
		/// Add the databases when this trigger occurs.
		/// </summary>
		public DialogueTriggerEvent addTrigger = DialogueTriggerEvent.OnStart;

		/// <summary>
		/// Remove the databases when this trigger occurs.
		/// </summary>
		public DialogueTriggerEvent removeTrigger = DialogueTriggerEvent.None;

		/// <summary>
		/// The databases to add/remove.
		/// </summary>
		public DialogueDatabase[] databases = new DialogueDatabase[0];

		/// <summary>
		/// The condition that must be true for the trigger to fire.
		/// </summary>
		public Condition condition = new Condition();

		/// <summary>
		/// As soon as one event (add or remove) is occurred, destroy this component.
		/// </summary>
		public bool once = false;

		private bool trying = false;

		private void TryAddDatabases(Transform interactor, bool immediate) {
			if (!trying) {
				trying = true;
				try {
					if ((condition == null) || condition.IsTrue(interactor)) {
						AddDatabases(immediate);
						if (once) Destroy(this);
					}
				} finally {
					trying = false;
				}
			}
		}
		
		public void AddDatabases(bool immediate) {
			if (immediate) {
				AddDatabasesImmediate();
			} else if (gameObject.activeInHierarchy && enabled) {
				StartCoroutine(AddDatabasesCoroutine());
			}
		}

		private void AddDatabasesImmediate() {
			foreach (var database in databases) {
				AddDatabase(database);
			}
		}

		private IEnumerator AddDatabasesCoroutine() {
			foreach (var database in databases) {
				AddDatabase(database);
				yield return null;
			}
		}
		
		private void AddDatabase(DialogueDatabase database) {
			if (database != null) {
				if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Adding database {1}", new object[] { DialogueDebug.Prefix, database.name }), this);
				DialogueManager.AddDatabase(database);
			}
		}

		private void TryRemoveDatabases(Transform interactor, bool immediate) {
			if (!trying) {
				trying = true;
				try {
					if ((condition == null) || condition.IsTrue(interactor)) {
						RemoveDatabases(immediate);
						if (once) Destroy(this);
					}
				} finally {
					trying = false;
				}
			}
		}
		
		public void RemoveDatabases(bool immediate) {
			if (immediate) {
				RemoveDatabasesImmediate();
			} else if (gameObject.activeInHierarchy && enabled) {
				StartCoroutine(RemoveDatabasesCoroutine());
			}
		}

		private void RemoveDatabasesImmediate() {
			foreach (var database in databases) {
				RemoveDatabase(database);
			}
		}

		private IEnumerator RemoveDatabasesCoroutine() {
			foreach (var database in databases) {
				RemoveDatabase(database);
				yield return null;
			}
		}

		private void RemoveDatabase(DialogueDatabase database) {
			if (database != null) {
				if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Removing database {1}", new object[] { DialogueDebug.Prefix, database.name }), this);
				DialogueManager.RemoveDatabase(database);
			}
		}
		
		public IEnumerator Start() {
			yield return null;
			if (addTrigger == DialogueTriggerEvent.OnStart) TryAddDatabases(null, false);
			if (removeTrigger == DialogueTriggerEvent.OnStart) TryRemoveDatabases(null, false);
		}

		public void OnEnable() {
			if (addTrigger == DialogueTriggerEvent.OnEnable) TryAddDatabases(null, false);
			if (removeTrigger == DialogueTriggerEvent.OnEnable) TryRemoveDatabases(null, false);
		}
		
		public void OnDisable() {
			if (addTrigger == DialogueTriggerEvent.OnDisable) TryAddDatabases(null, true);
			if (removeTrigger == DialogueTriggerEvent.OnDisable) TryRemoveDatabases(null, true);
		}
		
		public void OnDestroy() {
			if (addTrigger == DialogueTriggerEvent.OnDestroy) TryAddDatabases(null, true);
			if (removeTrigger == DialogueTriggerEvent.OnDestroy) TryRemoveDatabases(null, true);
		}
		
		public void OnUse(Transform actor) {
			if (!enabled) return;
			if (addTrigger == DialogueTriggerEvent.OnUse) TryAddDatabases(actor, false);
			if (removeTrigger == DialogueTriggerEvent.OnUse) TryRemoveDatabases(actor, false);
		}
		
		public void OnUse(string message) {
			if (!enabled) return;
			if (addTrigger == DialogueTriggerEvent.OnUse) TryAddDatabases(null, false);
			if (removeTrigger == DialogueTriggerEvent.OnUse) TryRemoveDatabases(null, false);
		}
		
		public void OnUse() {
			if (!enabled) return;
			if (addTrigger == DialogueTriggerEvent.OnUse) TryAddDatabases(null, false);
			if (removeTrigger == DialogueTriggerEvent.OnUse) TryRemoveDatabases(null, false);
		}
		
		public void OnTriggerEnter(Collider other) {
			if (!enabled) return;
			if (addTrigger == DialogueTriggerEvent.OnTriggerEnter) TryAddDatabases(other.transform, false);
			if (removeTrigger == DialogueTriggerEvent.OnTriggerEnter) TryRemoveDatabases(other.transform, false);
		}
		
		public void OnTriggerEnter2D(Collider2D other) {
			if (!enabled) return;
			if (addTrigger == DialogueTriggerEvent.OnTriggerEnter) TryAddDatabases(other.transform, false);
			if (removeTrigger == DialogueTriggerEvent.OnTriggerEnter) TryRemoveDatabases(other.transform, false);
		}
		
		public void OnTriggerExit(Collider other) {
			if (!enabled) return;
			if (addTrigger == DialogueTriggerEvent.OnTriggerExit) TryAddDatabases(other.transform, false);
			if (removeTrigger == DialogueTriggerEvent.OnTriggerExit) TryRemoveDatabases(other.transform, false);
		}
		
		public void OnTriggerExit2D(Collider2D other) {
			if (!enabled) return;
			if (addTrigger == DialogueTriggerEvent.OnTriggerExit) TryAddDatabases(other.transform, false);
			if (removeTrigger == DialogueTriggerEvent.OnTriggerExit) TryRemoveDatabases(other.transform, false);
		}
		
	}

}