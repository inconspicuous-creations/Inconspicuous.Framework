using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class FastObservableMonoBehaviour : TypedMonoBehaviour {
		FastSubject<Unit> awake;

		/// <summary>Awake is called when the script instance is being loaded.</summary>
		public override void Awake() {
			if(awake != null) awake.OnNext(Unit.Default);
		}

		/// <summary>Awake is called when the script instance is being loaded.</summary>
		public IObservable<Unit> AwakeAsObservable() {
			return awake ?? (awake = new FastSubject<Unit>());
		}

		FastSubject<Unit> fixedUpdate;

		/// <summary>This function is called every fixed framerate frame, if the MonoBehaviour is enabled.</summary>
		public override void FixedUpdate() {
			if(fixedUpdate != null) fixedUpdate.OnNext(Unit.Default);
		}

		/// <summary>This function is called every fixed framerate frame, if the MonoBehaviour is enabled.</summary>
		public IObservable<Unit> FixedUpdateAsObservable() {
			return fixedUpdate ?? (fixedUpdate = new FastSubject<Unit>());
		}

		FastSubject<Unit> lateUpdate;

		/// <summary>LateUpdate is called every frame, if the Behaviour is enabled.</summary>
		public override void LateUpdate() {
			if(lateUpdate != null) lateUpdate.OnNext(Unit.Default);
		}

		/// <summary>LateUpdate is called every frame, if the Behaviour is enabled.</summary>
		public IObservable<Unit> LateUpdateAsObservable() {
			return lateUpdate ?? (lateUpdate = new FastSubject<Unit>());
		}

		FastSubject<int> onAnimatorIK;

		/// <summary>Callback for setting up animation IK (inverse kinematics).</summary>
		public override void OnAnimatorIK(int layerIndex) {
			if(onAnimatorIK != null) onAnimatorIK.OnNext(layerIndex);
		}

		/// <summary>Callback for setting up animation IK (inverse kinematics).</summary>
		public IObservable<int> OnAnimatorIKAsObservable() {
			return onAnimatorIK ?? (onAnimatorIK = new FastSubject<int>());
		}

		FastSubject<Unit> onAnimatorMove;

		/// <summary>Callback for processing animation movements for modifying root motion.</summary>
		public override void OnAnimatorMove() {
			if(onAnimatorMove != null) onAnimatorMove.OnNext(Unit.Default);
		}

		/// <summary>Callback for processing animation movements for modifying root motion.</summary>
		public IObservable<Unit> OnAnimatorMoveAsObservable() {
			return onAnimatorMove ?? (onAnimatorMove = new FastSubject<Unit>());
		}

		FastSubject<bool> onApplicationFocus;

		/// <summary>Sent to all game objects when the player gets or loses focus.</summary>
		public override void OnApplicationFocus(bool focus) {
			if(onApplicationFocus != null) onApplicationFocus.OnNext(focus);
		}

		/// <summary>Sent to all game objects when the player gets or loses focus.</summary>
		public IObservable<bool> OnApplicationFocusAsObservable() {
			return onApplicationFocus ?? (onApplicationFocus = new FastSubject<bool>());
		}

		FastSubject<bool> onApplicationPause;

		/// <summary>Sent to all game objects when the player pauses.</summary>
		public override void OnApplicationPause(bool pause) {
			if(onApplicationPause != null) onApplicationPause.OnNext(pause);
		}

		/// <summary>Sent to all game objects when the player pauses.</summary>
		public IObservable<bool> OnApplicationPauseAsObservable() {
			return onApplicationPause ?? (onApplicationPause = new FastSubject<bool>());
		}

		FastSubject<Unit> onApplicationQuit;

		/// <summary>Sent to all game objects before the application is quit.</summary>
		public override void OnApplicationQuit() {
			if(onApplicationQuit != null) onApplicationQuit.OnNext(Unit.Default);
		}

		/// <summary>Sent to all game objects before the application is quit.</summary>
		public IObservable<Unit> OnApplicationQuitAsObservable() {
			return onApplicationQuit ?? (onApplicationQuit = new FastSubject<Unit>());
		}

		FastSubject<Tuple<float[], int>> onAudioFilterRead;

		/// <summary>If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain.</summary>
		public override void OnAudioFilterRead(float[] data, int channels) {
			if(onAudioFilterRead != null) onAudioFilterRead.OnNext(Tuple.Create(data, channels));
		}

		/// <summary>If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain.</summary>
		public IObservable<Tuple<float[], int>> OnAudioFilterReadAsObservable() {
			return onAudioFilterRead ?? (onAudioFilterRead = new FastSubject<Tuple<float[], int>>());
		}

		FastSubject<Unit> onBecameInvisible;

		/// <summary>OnBecameInvisible is called when the renderer is no longer visible by any camera.</summary>
		public override void OnBecameInvisible() {
			if(onBecameInvisible != null) onBecameInvisible.OnNext(Unit.Default);
		}

		/// <summary>OnBecameInvisible is called when the renderer is no longer visible by any camera.</summary>
		public IObservable<Unit> OnBecameInvisibleAsObservable() {
			return onBecameInvisible ?? (onBecameInvisible = new FastSubject<Unit>());
		}

		FastSubject<Unit> onBecameVisible;

		/// <summary>OnBecameVisible is called when the renderer became visible by any camera.</summary>
		public override void OnBecameVisible() {
			if(onBecameVisible != null) onBecameVisible.OnNext(Unit.Default);
		}

		/// <summary>OnBecameVisible is called when the renderer became visible by any camera.</summary>
		public IObservable<Unit> OnBecameVisibleAsObservable() {
			return onBecameVisible ?? (onBecameVisible = new FastSubject<Unit>());
		}

		FastSubject<Collision> onCollisionEnter;

		/// <summary>OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.</summary>
		public override void OnCollisionEnter(Collision collision) {
			if(onCollisionEnter != null) onCollisionEnter.OnNext(collision);
		}

		/// <summary>OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.</summary>
		public IObservable<Collision> OnCollisionEnterAsObservable() {
			return onCollisionEnter ?? (onCollisionEnter = new FastSubject<Collision>());
		}

		FastSubject<Collision2D> onCollisionEnter2D;

		/// <summary>Sent when an incoming collider makes contact with this object's collider (2D physics only).</summary>
		public override void OnCollisionEnter2D(Collision2D coll) {
			if(onCollisionEnter2D != null) onCollisionEnter2D.OnNext(coll);
		}

		/// <summary>Sent when an incoming collider makes contact with this object's collider (2D physics only).</summary>
		public IObservable<Collision2D> OnCollisionEnter2DAsObservable() {
			return onCollisionEnter2D ?? (onCollisionEnter2D = new FastSubject<Collision2D>());
		}

		FastSubject<Collision> onCollisionExit;

		/// <summary>OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.</summary>
		public override void OnCollisionExit(Collision collisionInfo) {
			if(onCollisionExit != null) onCollisionExit.OnNext(collisionInfo);
		}

		/// <summary>OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.</summary>
		public IObservable<Collision> OnCollisionExitAsObservable() {
			return onCollisionExit ?? (onCollisionExit = new FastSubject<Collision>());
		}

		FastSubject<Collision2D> onCollisionExit2D;

		/// <summary>Sent when a collider on another object stops touching this object's collider (2D physics only).</summary>
		public override void OnCollisionExit2D(Collision2D coll) {
			if(onCollisionExit2D != null) onCollisionExit2D.OnNext(coll);
		}

		/// <summary>Sent when a collider on another object stops touching this object's collider (2D physics only).</summary>
		public IObservable<Collision2D> OnCollisionExit2DAsObservable() {
			return onCollisionExit2D ?? (onCollisionExit2D = new FastSubject<Collision2D>());
		}

		FastSubject<Collision> onCollisionStay;

		/// <summary>OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.</summary>
		public override void OnCollisionStay(Collision collisionInfo) {
			if(onCollisionStay != null) onCollisionStay.OnNext(collisionInfo);
		}

		/// <summary>OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.</summary>
		public IObservable<Collision> OnCollisionStayAsObservable() {
			return onCollisionStay ?? (onCollisionStay = new FastSubject<Collision>());
		}

		FastSubject<Collision2D> onCollisionStay2D;

		/// <summary>Sent each frame where a collider on another object is touching this object's collider (2D physics only).</summary>
		public override void OnCollisionStay2D(Collision2D coll) {
			if(onCollisionStay2D != null) onCollisionStay2D.OnNext(coll);
		}

		/// <summary>Sent each frame where a collider on another object is touching this object's collider (2D physics only).</summary>
		public IObservable<Collision2D> OnCollisionStay2DAsObservable() {
			return onCollisionStay2D ?? (onCollisionStay2D = new FastSubject<Collision2D>());
		}

		FastSubject<Unit> onConnectedToServer;

		/// <summary>Called on the client when you have successfully connected to a server.</summary>
		public override void OnConnectedToServer() {
			if(onConnectedToServer != null) onConnectedToServer.OnNext(Unit.Default);
		}

		/// <summary>Called on the client when you have successfully connected to a server.</summary>
		public IObservable<Unit> OnConnectedToServerAsObservable() {
			return onConnectedToServer ?? (onConnectedToServer = new FastSubject<Unit>());
		}

		FastSubject<ControllerColliderHit> onControllerColliderHit;

		/// <summary>OnControllerColliderHit is called when the controller hits a collider while performing a Move.</summary>
		public override void OnControllerColliderHit(ControllerColliderHit hit) {
			if(onControllerColliderHit != null) onControllerColliderHit.OnNext(hit);
		}

		/// <summary>OnControllerColliderHit is called when the controller hits a collider while performing a Move.</summary>
		public IObservable<ControllerColliderHit> OnControllerColliderHitAsObservable() {
			return onControllerColliderHit ?? (onControllerColliderHit = new FastSubject<ControllerColliderHit>());
		}

		FastSubject<Unit> onDestroy;

		/// <summary>This function is called when the MonoBehaviour will be destroyed.</summary>
		public override void OnDestroy() {
			if(onDestroy != null) onDestroy.OnNext(Unit.Default);
		}

		/// <summary>This function is called when the MonoBehaviour will be destroyed.</summary>
		public IObservable<Unit> OnDestroyAsObservable() {
			return onDestroy ?? (onDestroy = new FastSubject<Unit>());
		}

		FastSubject<Unit> onDisable;

		/// <summary>This function is called when the behaviour becomes disabled () or inactive.</summary>
		public override void OnDisable() {
			if(onDisable != null) onDisable.OnNext(Unit.Default);
		}

		/// <summary>This function is called when the behaviour becomes disabled () or inactive.</summary>
		public IObservable<Unit> OnDisableAsObservable() {
			return onDisable ?? (onDisable = new FastSubject<Unit>());
		}

		FastSubject<Unit> onDrawGizmos;

		/// <summary>Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.</summary>
		public override void OnDrawGizmos() {
			if(onDrawGizmos != null) onDrawGizmos.OnNext(Unit.Default);
		}

		/// <summary>Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.</summary>
		public IObservable<Unit> OnDrawGizmosAsObservable() {
			return onDrawGizmos ?? (onDrawGizmos = new FastSubject<Unit>());
		}

		FastSubject<Unit> onDrawGizmosSelected;

		/// <summary>Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected.</summary>
		public override void OnDrawGizmosSelected() {
			if(onDrawGizmosSelected != null) onDrawGizmosSelected.OnNext(Unit.Default);
		}

		/// <summary>Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected.</summary>
		public IObservable<Unit> OnDrawGizmosSelectedAsObservable() {
			return onDrawGizmosSelected ?? (onDrawGizmosSelected = new FastSubject<Unit>());
		}

		FastSubject<Unit> onEnable;

		/// <summary>This function is called when the object becomes enabled and active.</summary>
		public override void OnEnable() {
			if(onEnable != null) onEnable.OnNext(Unit.Default);
		}

		/// <summary>This function is called when the object becomes enabled and active.</summary>
		public IObservable<Unit> OnEnableAsObservable() {
			return onEnable ?? (onEnable = new FastSubject<Unit>());
		}

		FastSubject<Unit> onGUI;

		/// <summary>OnGUI is called for rendering and handling GUI events.</summary>
		public override void OnGUI() {
			if(onGUI != null) onGUI.OnNext(Unit.Default);
		}

		/// <summary>OnGUI is called for rendering and handling GUI events.</summary>
		public IObservable<Unit> OnGUIAsObservable() {
			return onGUI ?? (onGUI = new FastSubject<Unit>());
		}

		FastSubject<float> onJointBreak;

		/// <summary>Called when a joint attached to the same game object broke.</summary>
		public override void OnJointBreak(float breakForce) {
			if(onJointBreak != null) onJointBreak.OnNext(breakForce);
		}

		/// <summary>Called when a joint attached to the same game object broke.</summary>
		public IObservable<float> OnJointBreakAsObservable() {
			return onJointBreak ?? (onJointBreak = new FastSubject<float>());
		}

		FastSubject<int> onLevelWasLoaded;

		/// <summary>This function is called after a new level was loaded.</summary>
		public override void OnLevelWasLoaded(int level) {
			if(onLevelWasLoaded != null) onLevelWasLoaded.OnNext(level);
		}

		/// <summary>This function is called after a new level was loaded.</summary>
		public IObservable<int> OnLevelWasLoadedAsObservable() {
			return onLevelWasLoaded ?? (onLevelWasLoaded = new FastSubject<int>());
		}

#if !UNITY_IPHONE

		FastSubject<Unit> onMouseDown;

		/// <summary>OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.</summary>
		public override void OnMouseDown() {
			if(onMouseDown != null) onMouseDown.OnNext(Unit.Default);
		}

		/// <summary>OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.</summary>
		public IObservable<Unit> OnMouseDownAsObservable() {
			return onMouseDown ?? (onMouseDown = new FastSubject<Unit>());
		}

		FastSubject<Unit> onMouseDrag;

		/// <summary>OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.</summary>
		public override void OnMouseDrag() {
			if(onMouseDrag != null) onMouseDrag.OnNext(Unit.Default);
		}

		/// <summary>OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.</summary>
		public IObservable<Unit> OnMouseDragAsObservable() {
			return onMouseDrag ?? (onMouseDrag = new FastSubject<Unit>());
		}

		FastSubject<Unit> onMouseEnter;

		/// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
		public override void OnMouseEnter() {
			if(onMouseEnter != null) onMouseEnter.OnNext(Unit.Default);
		}

		/// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
		public IObservable<Unit> OnMouseEnterAsObservable() {
			return onMouseEnter ?? (onMouseEnter = new FastSubject<Unit>());
		}

		FastSubject<Unit> onMouseExit;

		/// <summary>OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider.</summary>
		public override void OnMouseExit() {
			if(onMouseExit != null) onMouseExit.OnNext(Unit.Default);
		}

		/// <summary>OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider.</summary>
		public IObservable<Unit> OnMouseExitAsObservable() {
			return onMouseExit ?? (onMouseExit = new FastSubject<Unit>());
		}

		FastSubject<Unit> onMouseOver;

		/// <summary>OnMouseOver is called every frame while the mouse is over the GUIElement or Collider.</summary>
		public override void OnMouseOver() {
			if(onMouseOver != null) onMouseOver.OnNext(Unit.Default);
		}

		/// <summary>OnMouseOver is called every frame while the mouse is over the GUIElement or Collider.</summary>
		public IObservable<Unit> OnMouseOverAsObservable() {
			return onMouseOver ?? (onMouseOver = new FastSubject<Unit>());
		}

		FastSubject<Unit> onMouseUp;

		/// <summary>OnMouseUp is called when the user has released the mouse button.</summary>
		public override void OnMouseUp() {
			if(onMouseUp != null) onMouseUp.OnNext(Unit.Default);
		}

		/// <summary>OnMouseUp is called when the user has released the mouse button.</summary>
		public IObservable<Unit> OnMouseUpAsObservable() {
			return onMouseUp ?? (onMouseUp = new FastSubject<Unit>());
		}

		FastSubject<Unit> onMouseUpAsButton;

		/// <summary>OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed.</summary>
		public override void OnMouseUpAsButton() {
			if(onMouseUpAsButton != null) onMouseUpAsButton.OnNext(Unit.Default);
		}

		/// <summary>OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed.</summary>
		public IObservable<Unit> OnMouseUpAsButtonAsObservable() {
			return onMouseUpAsButton ?? (onMouseUpAsButton = new FastSubject<Unit>());
		}

#endif

		FastSubject<GameObject> onParticleCollision;

		/// <summary>OnParticleCollision is called when a particle hits a collider.</summary>
		public override void OnParticleCollision(GameObject other) {
			if(onParticleCollision != null) onParticleCollision.OnNext(other);
		}

		/// <summary>OnParticleCollision is called when a particle hits a collider.</summary>
		public IObservable<GameObject> OnParticleCollisionAsObservable() {
			return onParticleCollision ?? (onParticleCollision = new FastSubject<GameObject>());
		}

		FastSubject<Unit> onPostRender;

		/// <summary>OnPostRender is called after a camera finished rendering the scene.</summary>
		public override void OnPostRender() {
			if(onPostRender != null) onPostRender.OnNext(Unit.Default);
		}

		/// <summary>OnPostRender is called after a camera finished rendering the scene.</summary>
		public IObservable<Unit> OnPostRenderAsObservable() {
			return onPostRender ?? (onPostRender = new FastSubject<Unit>());
		}

		FastSubject<Unit> onPreCull;

		/// <summary>OnPreCull is called before a camera culls the scene.</summary>
		public override void OnPreCull() {
			if(onPreCull != null) onPreCull.OnNext(Unit.Default);
		}

		/// <summary>OnPreCull is called before a camera culls the scene.</summary>
		public IObservable<Unit> OnPreCullAsObservable() {
			return onPreCull ?? (onPreCull = new FastSubject<Unit>());
		}

		FastSubject<Unit> onPreRender;

		/// <summary>OnPreRender is called before a camera starts rendering the scene.</summary>
		public override void OnPreRender() {
			if(onPreRender != null) onPreRender.OnNext(Unit.Default);
		}

		/// <summary>OnPreRender is called before a camera starts rendering the scene.</summary>
		public IObservable<Unit> OnPreRenderAsObservable() {
			return onPreRender ?? (onPreRender = new FastSubject<Unit>());
		}

		FastSubject<Tuple<RenderTexture, RenderTexture>> onRenderImage;

		/// <summary>OnRenderImage is called after all rendering is complete to render image.</summary>
		public override void OnRenderImage(RenderTexture src, RenderTexture dest) {
			if(onRenderImage != null) onRenderImage.OnNext(Tuple.Create(src, dest));
		}

		/// <summary>OnRenderImage is called after all rendering is complete to render image.</summary>
		public IObservable<Tuple<RenderTexture, RenderTexture>> OnRenderImageAsObservable() {
			return onRenderImage ?? (onRenderImage = new FastSubject<Tuple<RenderTexture, RenderTexture>>());
		}

		FastSubject<Unit> onRenderObject;

		/// <summary>OnRenderObject is called after camera has rendered the scene.</summary>
		public override void OnRenderObject() {
			if(onRenderObject != null) onRenderObject.OnNext(Unit.Default);
		}

		/// <summary>OnRenderObject is called after camera has rendered the scene.</summary>
		public IObservable<Unit> OnRenderObjectAsObservable() {
			return onRenderObject ?? (onRenderObject = new FastSubject<Unit>());
		}

		FastSubject<Unit> onServerInitialized;

		/// <summary>Called on the server whenever a Network.InitializeServer was invoked and has completed.</summary>
		public override void OnServerInitialized() {
			if(onServerInitialized != null) onServerInitialized.OnNext(Unit.Default);
		}

		/// <summary>Called on the server whenever a Network.InitializeServer was invoked and has completed.</summary>
		public IObservable<Unit> OnServerInitializedAsObservable() {
			return onServerInitialized ?? (onServerInitialized = new FastSubject<Unit>());
		}

		FastSubject<Collider> onTriggerEnter;

		/// <summary>OnTriggerEnter is called when the Collider other enters the trigger.</summary>
		public override void OnTriggerEnter(Collider other) {
			if(onTriggerEnter != null) onTriggerEnter.OnNext(other);
		}

		/// <summary>OnTriggerEnter is called when the Collider other enters the trigger.</summary>
		public IObservable<Collider> OnTriggerEnterAsObservable() {
			return onTriggerEnter ?? (onTriggerEnter = new FastSubject<Collider>());
		}

		FastSubject<Collider2D> onTriggerEnter2D;

		/// <summary>Sent when another object enters a trigger collider attached to this object (2D physics only).</summary>
		public override void OnTriggerEnter2D(Collider2D other) {
			if(onTriggerEnter2D != null) onTriggerEnter2D.OnNext(other);
		}

		/// <summary>Sent when another object enters a trigger collider attached to this object (2D physics only).</summary>
		public IObservable<Collider2D> OnTriggerEnter2DAsObservable() {
			return onTriggerEnter2D ?? (onTriggerEnter2D = new FastSubject<Collider2D>());
		}

		FastSubject<Collider> onTriggerExit;

		/// <summary>OnTriggerExit is called when the Collider other has stopped touching the trigger.</summary>
		public override void OnTriggerExit(Collider other) {
			if(onTriggerExit != null) onTriggerExit.OnNext(other);
		}

		/// <summary>OnTriggerExit is called when the Collider other has stopped touching the trigger.</summary>
		public IObservable<Collider> OnTriggerExitAsObservable() {
			return onTriggerExit ?? (onTriggerExit = new FastSubject<Collider>());
		}

		FastSubject<Collider2D> onTriggerExit2D;

		/// <summary>Sent when another object leaves a trigger collider attached to this object (2D physics only).</summary>
		public override void OnTriggerExit2D(Collider2D other) {
			if(onTriggerExit2D != null) onTriggerExit2D.OnNext(other);
		}

		/// <summary>Sent when another object leaves a trigger collider attached to this object (2D physics only).</summary>
		public IObservable<Collider2D> OnTriggerExit2DAsObservable() {
			return onTriggerExit2D ?? (onTriggerExit2D = new FastSubject<Collider2D>());
		}

		FastSubject<Collider> onTriggerStay;

		/// <summary>OnTriggerStay is called once per frame for every Collider other that is touching the trigger.</summary>
		public override void OnTriggerStay(Collider other) {
			if(onTriggerStay != null) onTriggerStay.OnNext(other);
		}

		/// <summary>OnTriggerStay is called once per frame for every Collider other that is touching the trigger.</summary>
		public IObservable<Collider> OnTriggerStayAsObservable() {
			return onTriggerStay ?? (onTriggerStay = new FastSubject<Collider>());
		}

		FastSubject<Unit> onTriggerStay2D;

		/// <summary>Sent each frame where another object is within a trigger collider attached to this object (2D physics only).</summary>
		public override void OnTriggerStay2D(Collider2D other) {
			if(onTriggerStay2D != null) onTriggerStay2D.OnNext(Unit.Default);
		}

		/// <summary>Sent each frame where another object is within a trigger collider attached to this object (2D physics only).</summary>
		public IObservable<Unit> OnTriggerStay2DAsObservable() {
			return onTriggerStay2D ?? (onTriggerStay2D = new FastSubject<Unit>());
		}

		FastSubject<Unit> onValidate;

		/// <summary>This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).</summary>
		public override void OnValidate() {
			if(onValidate != null) onValidate.OnNext(Unit.Default);
		}

		/// <summary>This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).</summary>
		public IObservable<Unit> OnValidateAsObservable() {
			return onValidate ?? (onValidate = new FastSubject<Unit>());
		}

		FastSubject<Unit> onWillRenderObject;

		/// <summary>OnWillRenderObject is called once for each camera if the object is visible.</summary>
		public override void OnWillRenderObject() {
			if(onWillRenderObject != null) onWillRenderObject.OnNext(Unit.Default);
		}

		/// <summary>OnWillRenderObject is called once for each camera if the object is visible.</summary>
		public IObservable<Unit> OnWillRenderObjectAsObservable() {
			return onWillRenderObject ?? (onWillRenderObject = new FastSubject<Unit>());
		}

		FastSubject<Unit> reset;

		/// <summary>Reset to default values.</summary>
		public override void Reset() {
			if(reset != null) reset.OnNext(Unit.Default);
		}

		/// <summary>Reset to default values.</summary>
		public IObservable<Unit> ResetAsObservable() {
			return reset ?? (reset = new FastSubject<Unit>());
		}

		FastSubject<Unit> start;

		/// <summary>Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.</summary>
		public override void Start() {
			if(start != null) start.OnNext(Unit.Default);
		}

		/// <summary>Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.</summary>
		public IObservable<Unit> StartAsObservable() {
			return start ?? (start = new FastSubject<Unit>());
		}

		FastSubject<Unit> update;

		/// <summary>Update is called every frame, if the MonoBehaviour is enabled.</summary>
		public override void Update() {
			if(update != null) update.OnNext(Unit.Default);
		}

		/// <summary>Update is called every frame, if the MonoBehaviour is enabled.</summary>
		public IObservable<Unit> UpdateAsObservable() {
			return update ?? (update = new FastSubject<Unit>());
		}

#if !(UNITY_METRO || UNITY_WP8 || UNITY_NACL_CHROME)

		FastSubject<NetworkDisconnection> onDisconnectedFromServer;

		/// <summary>Called on the client when the connection was lost or you disconnected from the server.</summary>
		public override void OnDisconnectedFromServer(NetworkDisconnection info) {
			if(onDisconnectedFromServer != null) onDisconnectedFromServer.OnNext(info);
		}

		/// <summary>Called on the client when the connection was lost or you disconnected from the server.</summary>
		public IObservable<NetworkDisconnection> OnDisconnectedFromServerAsObservable() {
			return onDisconnectedFromServer ?? (onDisconnectedFromServer = new FastSubject<NetworkDisconnection>());
		}

		FastSubject<NetworkConnectionError> onFailedToConnect;

		/// <summary>Called on the client when a connection attempt fails for some reason.</summary>
		public override void OnFailedToConnect(NetworkConnectionError error) {
			if(onFailedToConnect != null) onFailedToConnect.OnNext(error);
		}

		/// <summary>Called on the client when a connection attempt fails for some reason.</summary>
		public IObservable<NetworkConnectionError> OnFailedToConnectAsObservable() {
			return onFailedToConnect ?? (onFailedToConnect = new FastSubject<NetworkConnectionError>());
		}

		FastSubject<NetworkConnectionError> onFailedToConnectToMasterServer;

		/// <summary>Called on clients or servers when there is a problem connecting to the MasterServer.</summary>
		public override void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
			if(onFailedToConnectToMasterServer != null) onFailedToConnectToMasterServer.OnNext(info);
		}

		/// <summary>Called on clients or servers when there is a problem connecting to the MasterServer.</summary>
		public IObservable<NetworkConnectionError> OnFailedToConnectToMasterServerAsObservable() {
			return onFailedToConnectToMasterServer ?? (onFailedToConnectToMasterServer = new FastSubject<NetworkConnectionError>());
		}

		FastSubject<MasterServerEvent> onMasterServerEvent;

		/// <summary>Called on clients or servers when reporting events from the MasterServer.</summary>
		public override void OnMasterServerEvent(MasterServerEvent msEvent) {
			if(onMasterServerEvent != null) onMasterServerEvent.OnNext(msEvent);
		}

		/// <summary>Called on clients or servers when reporting events from the MasterServer.</summary>
		public IObservable<MasterServerEvent> OnMasterServerEventAsObservable() {
			return onMasterServerEvent ?? (onMasterServerEvent = new FastSubject<MasterServerEvent>());
		}

		FastSubject<NetworkMessageInfo> onNetworkInstantiate;

		/// <summary>Called on objects which have been network instantiated with Network.Instantiate.</summary>
		public override void OnNetworkInstantiate(NetworkMessageInfo info) {
			if(onNetworkInstantiate != null) onNetworkInstantiate.OnNext(info);
		}

		/// <summary>Called on objects which have been network instantiated with Network.Instantiate.</summary>
		public IObservable<NetworkMessageInfo> OnNetworkInstantiateAsObservable() {
			return onNetworkInstantiate ?? (onNetworkInstantiate = new FastSubject<NetworkMessageInfo>());
		}

		FastSubject<NetworkPlayer> onPlayerConnected;

		/// <summary>Called on the server whenever a new player has successfully connected.</summary>
		public override void OnPlayerConnected(NetworkPlayer player) {
			if(onPlayerConnected != null) onPlayerConnected.OnNext(player);
		}

		/// <summary>Called on the server whenever a new player has successfully connected.</summary>
		public IObservable<NetworkPlayer> OnPlayerConnectedAsObservable() {
			return onPlayerConnected ?? (onPlayerConnected = new FastSubject<NetworkPlayer>());
		}

		FastSubject<NetworkPlayer> onPlayerDisconnected;

		/// <summary>Called on the server whenever a player disconnected from the server.</summary>
		public override void OnPlayerDisconnected(NetworkPlayer player) {
			if(onPlayerDisconnected != null) onPlayerDisconnected.OnNext(player);
		}

		/// <summary>Called on the server whenever a player disconnected from the server.</summary>
		public IObservable<NetworkPlayer> OnPlayerDisconnectedAsObservable() {
			return onPlayerDisconnected ?? (onPlayerDisconnected = new FastSubject<NetworkPlayer>());
		}

		FastSubject<Tuple<BitStream, NetworkMessageInfo>> onSerializeNetworkView;

		/// <summary>Used to customize synchronization of variables in a script watched by a network view.</summary>
		public override void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
			if(onSerializeNetworkView != null) onSerializeNetworkView.OnNext(Tuple.Create(stream, info));
		}

		/// <summary>Used to customize synchronization of variables in a script watched by a network view.</summary>
		public IObservable<Tuple<BitStream, NetworkMessageInfo>> OnSerializeNetworkViewAsObservable() {
			return onSerializeNetworkView ?? (onSerializeNetworkView = new FastSubject<Tuple<BitStream, NetworkMessageInfo>>());
		}

#endif
	}
}

// above code is generated from template

/*
var template = @"FastSubject<Unit> {0};

/// <summary>{1}</summary>
public override void {2}()
{{
	if ({0} != null) {0}.OnNext(Unit.Default);
}}

/// <summary>{1}</summary>
public IObservable<Unit> {2}AsObservable()
{{
	return {0} ?? ({0} = new FastSubject<Unit>());
}}";
*/
