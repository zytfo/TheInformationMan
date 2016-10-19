using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// This component implements a selector that allows the player to target and use a usable 
	/// object. 
	/// 
	/// To mark an object usable, add the Usable component and a collider to it. The object's
	/// layer should be in the layer mask specified on the Selector component.
	/// 
	/// The selector can be configured to target items under the mouse cursor or the middle of
	/// the screen. When a usable object is targeted, the selector displays a targeting reticle
	/// and information about the object. If the target is in range, the inRange reticle 
	/// texture is displayed; otherwise the outOfRange texture is displayed.
	/// 
	/// If the player presses the use button (which defaults to spacebar and Fire2), the targeted
	/// object will receive an "OnUse" message.
	/// 
	/// You can hook into SelectedUsableObject and DeselectedUsableObject to get notifications
	/// when the current target has changed.
	/// </summary>
	[AddComponentMenu("Dialogue System/Actor/Player/Selector")]
	public class Selector : MonoBehaviour {
		
		/// <summary>
		/// This class defines the textures and size of the targeting reticle.
		/// </summary>
		[System.Serializable]
		public class Reticle {
			public Texture2D inRange;
			public Texture2D outOfRange;
			public float width = 64f;
			public float height = 64f;
		}
		
		/// <summary>
		/// Specifies how to target: center of screen or under the mouse cursor.
		/// </summary>
		public enum SelectAt { CenterOfScreen, MousePosition, CustomPosition };
		
		/// <summary>
		/// Specifies whether to compute range from the targeted object (distance to the camera
		/// or distance to the selector's game object).
		/// </summary>
		public enum DistanceFrom { Camera, GameObject };

		/// <summary>
		/// Specifies whether to do 2D or 3D raycasts.
		/// </summary>
		public enum Dimension { In2D, In3D };
		
		/// <summary>
		/// The default layermask is just the Default layer.
		/// </summary>
		private static LayerMask DefaultLayer = 1;
		
		/// <summary>
		/// The layer mask to use when targeting objects. Objects on others layers are ignored.
		/// </summary>
		public LayerMask layerMask = DefaultLayer;
		
		/// <summary>
		/// How to target (center of screen or under mouse cursor). Default is center of screen.
		/// </summary>
		public SelectAt selectAt = SelectAt.CenterOfScreen;
		
		/// <summary>
		/// How to compute range to targeted object. Default is from the camera.
		/// </summary>
		public DistanceFrom distanceFrom = DistanceFrom.Camera;
		
		/// <summary>
		/// The max selection distance. The selector won't target objects farther than this.
		/// </summary>
		public float maxSelectionDistance = 30f;

		/// <summary>
		/// Specifies whether to run 2D or 3D raycasts.
		/// </summary>
		public Dimension runRaycasts = Dimension.In3D;
		
		/// <summary>
		/// Set <c>true</c> to check all objects within the raycast range for usables.
		/// If <c>false</c>, the check stops on the first hit, even if it's not a usable.
		/// This prevents selection through walls.
		/// </summary>
		public bool raycastAll = false;
		
		/// <summary>
		/// If <c>true</c>, uses a default OnGUI to display a selection message and
		/// targeting reticle.
		/// </summary>
		public bool useDefaultGUI = true;
		
		/// <summary>
		/// The GUI skin to use for the target's information (name and use message).
		/// </summary>
		public GUISkin guiSkin;
		
		/// <summary>
		/// The name of the GUI style in the skin.
		/// </summary>
		public string guiStyleName = "label";
		
		/// <summary>
		/// The text alignment.
		/// </summary>
		public TextAnchor alignment = TextAnchor.UpperCenter;
		
		/// <summary>
		/// The text style for the text.
		/// </summary>
		public TextStyle textStyle = TextStyle.Shadow;
		
		/// <summary>
		/// The color of the text style's outline or shadow.
		/// </summary>
		public Color textStyleColor = Color.black;
		
		/// <summary>
		/// The color of the information labels when the target is in range.
		/// </summary>
		public Color inRangeColor = Color.yellow;
		
		/// <summary>
		/// The color of the information labels when the target is out of range.
		/// </summary>
		public Color outOfRangeColor = Color.gray;
		
		/// <summary>
		/// The reticle images.
		/// </summary>
		public Reticle reticle;
		
		/// <summary>
		/// The key that sends an OnUse message.
		/// </summary>
		public KeyCode useKey = KeyCode.Space;
		
		/// <summary>
		/// The button that sends an OnUse message.
		/// </summary>
		public string useButton = "Fire2";
		
		/// <summary>
		/// The default use message. This can be overridden in the target's Usable component.
		/// </summary>
		public string defaultUseMessage = "(spacebar to interact)";
		
		/// <summary>
		/// If ticked, the OnUse message is broadcast to the usable object's children.
		/// </summary>
		public bool broadcastToChildren = true;
		
		/// <summary>
		/// The actor transform to send with OnUse. Defaults to this transform.
		/// </summary>
		public Transform actorTransform = null;
		
		/// <summary>
		/// If <c>true</c>, draws gizmos.
		/// </summary>
		public bool debug = false;
		
		/// <summary>
		/// Gets or sets the custom position used when the selectAt is set to SelectAt.CustomPosition.
		/// You can use, for example, to slide around a targeting icon onscreen using a gamepad.
		/// </summary>
		/// <value>
		/// The custom position.
		/// </value>
		public Vector3 CustomPosition { get; set; }
		
		/// <summary>
		/// Gets the current selection.
		/// </summary>
		/// <value>The selection.</value>
		public Usable CurrentUsable { get { return usable; } }
		
		/// <summary>
		/// Gets the distance from the current usable.
		/// </summary>
		/// <value>The current distance.</value>
		public float CurrentDistance { get { return distance; } }
		
		/// <summary>
		/// Gets the GUI style.
		/// </summary>
		/// <value>The GUI style.</value>
		public GUIStyle GuiStyle { get { SetGuiStyle(); return guiStyle; } }
		
		/// <summary>
		/// Occurs when the selector has targeted a usable object.
		/// </summary>
		public event SelectedUsableObjectDelegate SelectedUsableObject = null;
		
		/// <summary>
		/// Occurs when the selector has untargeted a usable object.
		/// </summary>
		public event DeselectedUsableObjectDelegate DeselectedUsableObject = null;
		
		private GameObject selection = null;
		private Usable usable = null;
		private string heading = string.Empty;
		private string useMessage = string.Empty;
		private float distance = 0;
		private GUIStyle guiStyle = null;
		
		private Ray lastRay = new Ray();
		private RaycastHit lastHit = new RaycastHit();
		private RaycastHit[] lastHits = new RaycastHit[0];
		
		/// <summary>
		/// Runs a raycast to see what's under the selection point. Updates the selection and
		/// calls the selection delegates if the selection has changed. If the player hits the
		/// use button, sends an OnUse message to the selection.
		/// </summary>
		void Update() {
			// Exit if disabled or paused:
			if (!enabled || (Time.timeScale <= 0)) return;
			
			// Exit if there's no camera:
			if (UnityEngine.Camera.main == null) return;

			// Raycast 2D or 3D:
			switch (runRaycasts) {
			case Dimension.In2D: 
				Run2DRaycast();
				break;
			default:
			case Dimension.In3D: 
				Run3DRaycast();
				break;
			}

			// If the player presses the use key/button, send the OnUse message:
			if (IsUseButtonDown() && (usable != null) && (distance <= usable.maxUseDistance)) {
				var fromTransform = (actorTransform != null) ? actorTransform : this.transform;
				if (broadcastToChildren) {
					usable.gameObject.BroadcastMessage("OnUse", fromTransform, SendMessageOptions.DontRequireReceiver);
				} else {
					usable.gameObject.SendMessage("OnUse", fromTransform, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		private void Run2DRaycast() {
			if (raycastAll) {
				
				// Run Physics2D.RaycastAll:
				RaycastHit2D[] hits;
				hits = Physics2D.RaycastAll(UnityEngine.Camera.main.ScreenToWorldPoint(GetSelectionPoint()), Vector2.zero, maxSelectionDistance, layerMask);
				bool foundUsable = false;
				foreach (var hit in hits) {
					float hitDistance = (distanceFrom == DistanceFrom.Camera) ? 0 : Vector3.Distance(gameObject.transform.position, hit.collider.transform.position);
					if (selection == hit.collider.gameObject) {
						foundUsable = true;
						distance = hitDistance;
						break;
					} else {
						Usable hitUsable = hit.collider.gameObject.GetComponent<Usable>();
						if (hitUsable != null) {
							foundUsable = true;
							distance = hitDistance;
							usable = hitUsable;
							selection = hit.collider.gameObject;
							if (SelectedUsableObject != null) SelectedUsableObject(usable);
							break;
						}
					}
				}
				if (!foundUsable) {
					DeselectTarget();
				}
				
			} else {
				
				// Cast a ray and see what we hit:
				RaycastHit2D hit;
				hit = Physics2D.Raycast(UnityEngine.Camera.main.ScreenToWorldPoint(GetSelectionPoint()), Vector2.zero, maxSelectionDistance, layerMask);
				if (hit.collider != null) {
					distance = (distanceFrom == DistanceFrom.Camera) ? 0 : Vector3.Distance(gameObject.transform.position, hit.collider.transform.position);
					if (selection != hit.collider.gameObject) {
						Usable hitUsable = hit.collider.gameObject.GetComponent<Usable>();
						if (hitUsable != null) {
							usable = hitUsable;
							selection = hit.collider.gameObject;
							heading = string.Empty;
							useMessage = string.Empty;
							if (SelectedUsableObject != null) SelectedUsableObject(usable);
						} else {
							DeselectTarget();
						}
					}
				} else {
					DeselectTarget();
				}
			}
		}

		private void Run3DRaycast() {
			Ray ray = UnityEngine.Camera.main.ScreenPointToRay(GetSelectionPoint());
			lastRay = ray;
			
			if (raycastAll) {
				
				// Run RaycastAll:
				RaycastHit[] hits;
				hits = Physics.RaycastAll(ray, maxSelectionDistance, layerMask);
				lastRay = ray;
				lastHits = hits;
				bool foundUsable = false;
				foreach (var hit in hits) {
					float hitDistance = (distanceFrom == DistanceFrom.Camera) ? hit.distance : Vector3.Distance(gameObject.transform.position, hit.collider.transform.position);
					if (selection == hit.collider.gameObject) {
						foundUsable = true;
						distance = hitDistance;
						break;
					} else {
						Usable hitUsable = hit.collider.gameObject.GetComponent<Usable>();
						if (hitUsable != null) {
							foundUsable = true;
							distance = hitDistance;
							usable = hitUsable;
							selection = hit.collider.gameObject;
							if (SelectedUsableObject != null) SelectedUsableObject(usable);
							break;
						}
					}
				}
				if (!foundUsable) {
					DeselectTarget();
				}
				
			} else {
				
				// Cast a ray and see what we hit:
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, maxSelectionDistance, layerMask)) {
					distance = (distanceFrom == DistanceFrom.Camera) ? hit.distance : Vector3.Distance(gameObject.transform.position, hit.collider.transform.position);
					if (selection != hit.collider.gameObject) {
						Usable hitUsable = hit.collider.gameObject.GetComponent<Usable>();
						if (hitUsable != null) {
							usable = hitUsable;
							selection = hit.collider.gameObject;
							heading = string.Empty;
							useMessage = string.Empty;
							if (SelectedUsableObject != null) SelectedUsableObject(usable);
						} else {
							DeselectTarget();
						}
					}
				} else {
					DeselectTarget();
				}
				lastHit = hit;
			}
		}
		
		private void DeselectTarget() {
			if ((usable != null) && (DeselectedUsableObject != null)) DeselectedUsableObject(usable);
			usable = null;
			selection = null;
			heading = string.Empty;
			useMessage = string.Empty;
		}
		
		private bool IsUseButtonDown() {
			return ((useKey != KeyCode.None) && Input.GetKeyDown(useKey))
				|| (!string.IsNullOrEmpty(useButton)  && Input.GetButtonUp(useButton));
		}
		
		private Vector3 GetSelectionPoint() {
			switch (selectAt) {
			case SelectAt.MousePosition: 
				return Input.mousePosition;
			case SelectAt.CustomPosition:
				return CustomPosition;
			default:
			case SelectAt.CenterOfScreen:
				return new Vector3(Screen.width / 2, Screen.height / 2);
			}
		}
		
		/// <summary>
		/// If useDefaultGUI is <c>true</c> and a usable object has been targeted, this method
		/// draws a selection message and targeting reticle.
		/// </summary>
		public virtual void OnGUI() {
			if (useDefaultGUI) {
				SetGuiStyle();
				Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
				if (usable != null) {
					bool inUseRange = (distance <= usable.maxUseDistance);
					guiStyle.normal.textColor = inUseRange ? inRangeColor : outOfRangeColor;
					if (string.IsNullOrEmpty(heading)) {
						heading = usable.GetName();
						useMessage = string.IsNullOrEmpty(usable.overrideUseMessage) ? defaultUseMessage : usable.overrideUseMessage;
					}
					UnityGUITools.DrawText(screenRect, heading, guiStyle, textStyle, textStyleColor);
					UnityGUITools.DrawText(new Rect(0, guiStyle.CalcSize(new GUIContent("Ay")).y, Screen.width, Screen.height), useMessage, guiStyle, textStyle, textStyleColor);
					Texture2D reticleTexture = inUseRange ? reticle.inRange : reticle.outOfRange;
					if (reticleTexture != null) GUI.Label(new Rect(0.5f * (Screen.width - reticle.width), 0.5f * (Screen.height - reticle.height), reticle.width, reticle.height), reticleTexture);
				}
			}
		}
		
		protected void SetGuiStyle() {
			GUI.skin = UnityGUITools.GetValidGUISkin(guiSkin);
			if (guiStyle == null) {
				guiStyle = new GUIStyle(GUI.skin.FindStyle(guiStyleName) ?? GUI.skin.label);
				guiStyle.alignment = alignment;
			}
		}
		
		/// <summary>
		/// Draws raycast result gizmos.
		/// </summary>
		public virtual void OnDrawGizmos() {
			if (!debug) return;
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(lastRay.origin, lastRay.origin + lastRay.direction * maxSelectionDistance);
			if (raycastAll) {
				foreach (var hit in lastHits) {
					bool isUsable = (hit.collider.GetComponent<Usable>() != null);
					Gizmos.color = isUsable ? Color.green : Color.red;
					Gizmos.DrawWireSphere(hit.point, 0.2f);
				}
			} else {
				if (lastHit.collider != null) {
					bool isUsable = (lastHit.collider.GetComponent<Usable>() != null);
					Gizmos.color = isUsable ? Color.green : Color.red;
					Gizmos.DrawWireSphere(lastHit.point, 0.2f);
				}
			}
		}
		
	}
	
}
