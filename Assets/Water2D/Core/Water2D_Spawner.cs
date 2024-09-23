namespace Water2D {
	using UnityEngine;
    using UnityEngine.Events;
   	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine.UI;
	//using Apptouch;

#if UNITY_EDITOR
    using UnityEditor;
#endif


    public struct microSpawn{
		public Vector3 pos;
		public int amount;
		public Vector2 initVel;

		public microSpawn(Vector3 pos, int amount, Vector2 initVel)
		{
			this.pos = pos;
			this.amount = amount;
			this.initVel = initVel;
		}
	}

    [ExecuteInEditMode]
    [SelectionBase]
    public class Water2D_Spawner : MonoBehaviour
    {
        public enum EmissionType {
            ParticleSystem,
            FillerCollider
        }

        public enum FillerColliderType
        {
            Box,
            Circle,
            Polygon
        }

        public Water2D_Spawner instance;

        public enum EnumTypes
        {
            Regular,
            Refracting,
            Toon
        }

        void Awake()
        {
            if (instance == null)
                instance = this;


        }

        public int HashID = -1;

        public EnumTypes Water2DType = EnumTypes.Regular;
        public EmissionType Water2DEmissionType = EmissionType.ParticleSystem;

        public FillerColliderType Water2DFillerType = FillerColliderType.Box;

        public bool FillerColliderMasked = false;


        public string Water2DRenderType = "";

        public string Water2DVersion = "1.2";


        public GameObject DropObject;
        public GameObject[] WaterDropsObjects;


        public string ParticlesTag = "Metaball_liquid";

        public bool PersistentFluid = false;

        public float size = .45f;

        public bool ScaleDown = false;

        //[Range (0f,100f)]
        public float LifeTime = 5f;

        //[Range (0f,.3f)]
        public float DelayBetweenParticles = 0.05f;

        //[Range(0f, 2f)]
        public float TrailStartSize = .4f;

        //[Range(0f, 2f)]
        public float TrailEndSize = .4f;

        //[Range(0f, 2f)]
        public float TrailDelay = .1f;

        public Material WaterMaterial;

        public bool StyleByID;

        public int Sorting;

        public int ColorScheme = 1;

        public Color FillColor = new Color(0f, 112 / 255f, 1f);

        public Color StrokeColor = new Color(4 / 255f, 156 / 255f, 1f);

        public Color _lastStrokeColor = new Color(4 / 255f, 156 / 255f, 1f);

        public bool Blending = false;

        public bool _lastBlending = false;

        public float AlphaCutOff = .2f;


        public float AlphaStroke = .2f;

        public Color TintColor = new Color(0f, 112 / 255f, 1f);

        public float Intensity = .5f;

        public float LensMag = 1.2f;

        public float Distortion = .5f;

        public float DistortionSpeed = .5f;

        public bool GlowEffect = false;

        public Color GlowColor = new Color(1f,1f,1f,.4f);

        public float GlowSize = 1.5f;

        public int GlowSortingOrder = -1;

        [SerializeField] bool _lastGlowEnabledValue = false;
       
        public Vector2 initSpeed = new Vector2(1f, -1.8f);

        public float Speed = 20f;

        public PhysicsMaterial2D PhysicMat;

        public float ColliderSize = 1.5f;

        public float LinearDrag = 0f;

        public float AngularDrag = 0f;

        public float GravityScale = 1f;

        public bool FreezeRotation = false;

		public Vector2 SpeedLimiterX = new Vector2(-300, 300);

        public Vector2 SpeedLimiterY = new Vector2(-300, 300);

        public bool SimulateOnAwake = true;
        public bool SimulateInEditor = false;

        public bool SimulateInPlayMode = false;

        public int DropCount = 100;

        public int _lastDropCount = 100;

        public bool Loop = true;

        public int DropsUsed;

        public bool DynamicChanges = true;

        public Water2DEvents OnValidateShapeFill;

        public Collider2D[] ShapeFillCollider2D;

        public int ShapeFillCollider2DCount = 3;

        public float ShapeFillAccuracy = 1f;

        public Water2DEvents OnCollisionEnterList;

        public Water2DEvents OnSpawnerAboutStart;

        public Water2DEvents OnSpawnerAboutEnd;

        private Transform pointWater = null;
        private SpriteRenderer sprWaterCircle = null;


        public Water2DEvents OnSpawnerEmitingParticle;

        private void Start()
        {
            pointWater = transform.Find("Circle");
            sprWaterCircle = pointWater.GetComponent<SpriteRenderer>();

            IsPlayIng = true;
            StartCoroutine(StartEnumerator());
            //Register
            SpawnersManager.ChangeSpawnerValues(instance);

            var color = Color.HSVToRGB(UnityEngine.Random.Range(0.0f, 1.0f), 1.0f, 1.0f);
            OnUpdateColor(color);
        }

        public float GetHSVColor()
        {
            float h = 0.0f, s = 0.0f, v = 0.0f;
            Color.RGBToHSV(FillColor, out h, out s, out v);
            return h;
        }


        public void OnUpdateColor(Color co)
        {
            FillColor = co;
            sprWaterCircle.color = co;
        }

        private IEnumerator StartEnumerator()
        {


           yield return new WaitForSeconds(.25f);
            if (Application.isPlaying && SimulateOnAwake && Water2DEmissionType == EmissionType.ParticleSystem)
            {
                Restore();
                yield return new WaitForEndOfFrame();
                Spawn();
            }
            else {
                yield return new WaitForEndOfFrame();
                StartCoroutine(UpdateQuietParticleProperties());
                ColliderFiller cf = GetComponent<ColliderFiller>();
                if(cf != null) cf.Fill ();

            }
           
           

            yield return null;

        }

        void RunSpawner()
		{
            instance.Spawn();

        }
       
        void StopSpawner()
        {
            instance.Restore();

        }


        public bool IsPlayIng { get; set; }

        public int AllBallsCount{ get; private set;}
		public bool IsSpawning{ get; private set;}

        public bool isRefractingMaterial = false;

        int usableDropsCount;
		int DefaultCount;


		bool _breakLoop = false;

		GameObject _parent;
        string _parentNameID = "Water2DParticlesID_";
       


        public void SetupParticles()
        {
            if (_parent == null && WaterDropsObjects != null)
            {
                if (WaterDropsObjects.Length > 0 && WaterDropsObjects[0] != null)
                    _parent = WaterDropsObjects[0].transform.parent.gameObject;
            }
            if(_parent != null){
               
                DestroyImmediate(_parent);

            }
            _parent = new GameObject(_parentNameID + gameObject.GetInstanceID());
            _parent.transform.hideFlags = HideFlags.HideInHierarchy;

            WaterDropsObjects = new GameObject[DropCount];

            for (int i = 0; i < WaterDropsObjects.Length; i++)
            {
                WaterDropsObjects[i] = Instantiate(DropObject, gameObject.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
                WaterDropsObjects[i].GetComponent<MetaballParticleClass>().Active = false;
                WaterDropsObjects[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                WaterDropsObjects[i].transform.SetParent(_parent.transform);
                WaterDropsObjects[i].transform.localScale = new Vector3(size, size, 1f);
                Debug.Log("setScale4: " + size);
                WaterDropsObjects[i].layer = WaterDropsObjects[0].layer;
                WaterDropsObjects[i].tag = ParticlesTag;

                // Create glow
                if (GlowEffect )
                {
                    _lastGlowEnabledValue = GlowEffect;

                    GameObject o = new GameObject("_glow");
                    o.transform.SetParent(WaterDropsObjects[i].transform);
                    o.transform.localPosition = new Vector3(0, 0, -1f);
                    SpriteRenderer sr = o.AddComponent<SpriteRenderer>();
                    sr.sprite = WaterDropsObjects[i].GetComponent<SpriteRenderer>().sprite;
                    sr.color = GlowColor;
                    sr.sortingOrder = GlowSortingOrder;
                    o.transform.localScale = Vector3.one * GlowSize;

                }
                else {
                    Transform t = WaterDropsObjects[i].transform.Find("_glow");
                    if (t != null)
                        DestroyImmediate(t);
                }
                

                //Set tex color for scheme selection
                Color ColorTex = Color.white;

                if (ColorScheme == 1)
                    ColorTex = new Color(1f,0f,0f);
                if (ColorScheme == 2)
                    ColorTex = new Color(0f, 1f, 0f);
                if (ColorScheme == 3)
                    ColorTex = new Color(0f, 0f, 1f);
                /* if (ColorScheme == 4)
                     ColorTex = new Color(1f, 1f, 0f);
                 if (ColorScheme == 5)
                     ColorTex = new Color(0f, 1f, 1f);
                 if (ColorScheme == 6)
                     ColorTex = new Color(1f, 1f, 1f);


                 if(Water2DType == EnumTypes.Regular || Water2DType == EnumTypes.Refracting) 
                     ColorTex = FillColor;

                 */
                WaterDropsObjects[i].GetComponent<SpriteRenderer>().color = FillColor;
                 WaterDropsObjects[i].GetComponent<TrailRenderer>().startColor = FillColor;
                 WaterDropsObjects[i].GetComponent<TrailRenderer>().endColor = FillColor;
                 
                WaterDropsObjects[i].GetComponent<MetaballParticleClass>().BlendingColor = Blending;

                TrailRenderer tr = WaterDropsObjects[i].GetComponent<TrailRenderer>();
                if (TrailStartSize <= 0f) {
                    tr.enabled = false;
                }
                else {
                    tr.enabled = true;
                    tr.startWidth = TrailStartSize;
                    tr.endWidth = TrailEndSize;
                    tr.time = TrailDelay;
                }

                WaterDropsObjects[i].GetComponent<MetaballParticleClass>().SpawnerParent = this;
            }

           

            AllBallsCount = WaterDropsObjects.Length;

            if(Water2DEmissionType == EmissionType.ParticleSystem)
            {
                DropsUsed *= 0;
                _spawnedDrops *= 0;
            }



            RestoreCheckingFillShape(); // restore events

        }



#if UNITY_EDITOR

        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
            auxTime = Time.realtimeSinceStartup;
            auxTime += DelayBetweenParticles;
                       
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        float auxTime;
        float NextTick;
        private void OnEditorUpdate()
        {
            if (Application.isPlaying)
                return;


            if (DropObject == null)
                return;

            if (WaterDropsObjects == null || WaterDropsObjects.Length < 1)
            {
                SetupParticles();
            }

            if (WaterDropsObjects[0] == null)
            {
                SetupParticles();
            }

            if (EditorApplication.timeSinceStartup >= NextTick)
            {
               
                //tick  
                NextTick = (float)EditorApplication.timeSinceStartup + DelayBetweenParticles;
                loop_editor(gameObject.transform.position, initSpeed);

                /*
                if (Selection.activeGameObject == gameObject)
                {

                    //print("change color");
                    //CHANGE COLOR
                   
                    //SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                   
                }
                */

            }

            CallShapeFillValidationUpdate();
            

        }
#endif
        private void Update()
        {
            CallShapeFillValidationUpdate();
        }

        void CallShapeFillValidationUpdate()
        { 
        //Check ShapeFill events
            if(OnValidateShapeFill?.GetPersistentEventCount() > 0 && ShapeFillCollider2D != null)
            {
               StartCheckingFillShape();
            }
        }

        public void Spawn(){

			Spawn (DefaultCount);
		}

		public void Spawn(int count){
			
            if (DelayBetweenParticles == 0f)
            {
               // DropsUsed *= 0;
                SpawnAll();
            }
            else {
             //   DropsUsed *= 0;
                StartCoroutine(loop(gameObject.transform.position, initSpeed, count));
            }
			
		}

        public void SpawnAll() {
            SpawnAllParticles(gameObject.transform.position, initSpeed, DefaultCount);
        }

		public void Spawn(int count, Vector3 pos){
		
			StartCoroutine (loop(pos, initSpeed, count));
		}

		public void Spawn(int count, Vector3 pos, Vector2 InitVelocity, float delay = 0f){
			
			StartCoroutine (loop(pos, InitVelocity, count, delay));
		}

		
        public void StopSpawning()
        {
            _breakLoop = true;
            IsSpawning = false;
        }

		public void Restore()
		{

			IsSpawning = false;
			_breakLoop = true;
           // DropsUsed *= 0;

			


			for (int i = 0; i < WaterDropsObjects.Length; i++) {

                if (WaterDropsObjects[i] != null) {
                    if (WaterDropsObjects[i].GetComponent<MetaballParticleClass>().Active == true)
                    {
                        WaterDropsObjects[i].GetComponent<MetaballParticleClass>().Active = false;
                    }
                    WaterDropsObjects[i].GetComponent<MetaballParticleClass>().witinTarget = false;
                }

                		
			}




			//gameObject.transform.localEulerAngles = Vector3.zero;
			//initSpeed = new Vector2 (0, -2f);

			DefaultCount = AllBallsCount;
			usableDropsCount = DefaultCount;
			//Dynamic = false;
		}

        int _spawnedDrops = 0;
        Color _lastFillColor;
        void loop_editor(Vector3 _pos, Vector2 _initSpeed, int count = -1, float delay = 0f, bool waitBetweenDropSpawn = true)
        {
            if (Application.isPlaying)
                return;

            if (Water2DEmissionType == EmissionType.FillerCollider)
            {
                //Debug.LogError("You're trying spawn particles in a Filler type. You should create a water spawner instead");
                return;
            }

            if (!SimulateInEditor)
                return;

            if (WaterDropsObjects == null || WaterDropsObjects.Length < 1) {
                SetupParticles();
                return;
            }


            for (int i = 0; i < WaterDropsObjects.Length; i++)
            {
                if (WaterDropsObjects[i] == null)
                    return;

                MetaballParticleClass MetaBall = WaterDropsObjects[i].GetComponent<MetaballParticleClass>();

                //CHANGE COLOR
               
                    if (MetaBall.Active == false)
                        MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
               

                if (MetaBall.Active == true)
                    continue;

                _canInvokeAttheEnd = true;

                if (LifeTime <= 0) {
                    MetaBall.LifeTime = -1f;
                }
                else {
                    MetaBall.LifeTime = LifeTime;
                }
               
                WaterDropsObjects[i].transform.position = pointWater.transform.position + new Vector3(0, -0.15f, 0);
                MetaBall.Active = true;
                MetaBall.witinTarget = false;

                if (_initSpeed == Vector2.zero)
                    _initSpeed = initSpeed;

                if (true)
                {
                    _initSpeed = initSpeed;
                    MetaBall.transform.localScale = new Vector3(size, size, 1f);
                    Debug.Log("setScale1: " + size);

                    TrailRenderer tr = WaterDropsObjects[i].GetComponent<TrailRenderer>();
                    if (TrailStartSize <= 0f)
                    {
                        tr.enabled = false;
                    }
                    else
                    {
                        tr.enabled = true;
                        tr.startWidth = TrailStartSize;
                        tr.endWidth = TrailEndSize;
                        tr.time = TrailDelay;
                    }

                    MetaBall.Velocity_Limiter_X = SpeedLimiterX;
                    MetaBall.Velocity_Limiter_Y = SpeedLimiterY;

                    Rigidbody2D rb = MetaBall.GetComponent<Rigidbody2D>();
                    rb.sharedMaterial = PhysicMat;
                    rb.drag = LinearDrag;
                    rb.angularDrag = AngularDrag;
                    rb.gravityScale = GravityScale;

                    if (FreezeRotation)
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                    MetaBall.GetComponent<CircleCollider2D>().sharedMaterial = PhysicMat;
                    MetaBall.GetComponent<CircleCollider2D>().radius = ColliderSize;

                    MetaBall.ScaleDown = ScaleDown;

                    //CHANGE COLOR IN REALTIME
              
                    if (MetaBall.Active == false)
                       MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                    SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                   

                    // CHANGE GLOW IN REALTIME
                    if (GlowEffect && MetaBall.glowSP != null)
                    {
                        if (MetaBall.glowSP.color != GlowColor)
                            MetaBall.glowSP.color = GlowColor;

                        if (MetaBall.glowSP.sortingOrder != GlowSortingOrder)
                            MetaBall.glowSP.sortingOrder = GlowSortingOrder;

                        if (MetaBall.glowSP.transform.localScale.x != GlowSize)
                            MetaBall.glowSP.transform.localScale = Vector3.one * GlowSize;
                    }
                }

                Vector2 dir = transform.up;
                MetaBall.GetComponent<Rigidbody2D>().velocity = -dir * Speed;


                DropsUsed++;
                _spawnedDrops++;

                //Invoke event
                InvokeOnSpawnerEmittinEachParticle(gameObject, MetaBall.gameObject);

                if (_spawnedDrops >= DropCount)
                {

                    if(!Loop)
                        SimulateInEditor = false;

                    //Invoke event End
                    if (_canInvokeAttheEnd)
                    {
                        InvokeOnSpawnerEnd(gameObject);
                        _canInvokeAttheEnd = false;
                    }


                    _spawnedDrops *= 0;
                }


                return;
            }
        }

        bool _canInvokeAttheEnd = true;
        IEnumerator loop(Vector3 _pos, Vector2 _initSpeed, int count = -1, float delay = 0f, bool waitBetweenDropSpawn = true){

            if (IsSpawning)
            {
                LogicGame.Instance.txtLog.text = "DCMMMMMMMMMMMM_IsSpawning = false";
                Debug.Log("cmmmm_IsSpawning");
                yield break;
            }

            if (Water2DEmissionType == EmissionType.FillerCollider)
            { Debug.LogError("You're trying spawn particles in a Filler type. You should create a water spawner instead"); yield break; }

            Physics2D.autoSimulation = true;

            IsSpawning = true;

            yield return new WaitForSeconds (delay);

			_breakLoop = false;

			
			//int auxCount = 0;

            //Invoke event Start
            InvokeOnSpawnerStart(gameObject);

            while (true) {

                if (!IsPlayIng)
                {
                    Debug.Log("cmmmm_IsSpawning" + IsPlayIng);

                    yield break;
                }

                if (WaterDropsObjects == null || WaterDropsObjects[0] == null)
                {
                    SetupParticles();
                }

                for (int i = 0; i < WaterDropsObjects.Length; i++) {

					if (_breakLoop || !IsPlayIng)
                    {
                        Debug.Log("cmmmm__breakLoop-!IsPlayIng___" + _breakLoop + "/" + "!IsPlayIng" + !IsPlayIng);

                        LogicGame.Instance.txtLog.text = "cmmmm__breakLoop-!IsPlayIng___" + _breakLoop + "/" + "!IsPlayIng" + !IsPlayIng;

                        yield break;
                    }

                    MetaballParticleClass MetaBall = WaterDropsObjects [i].GetComponent<MetaballParticleClass> ();

					if (MetaBall.Active == true)
						continue;

                    _canInvokeAttheEnd = true; 

                    if (LifeTime <= 0)
                    {
                        MetaBall.LifeTime = -1f;
                    }
                    else
                    {
                        MetaBall.LifeTime = LifeTime;
                    }

                    WaterDropsObjects [i].transform.position = pointWater.transform.position + new Vector3(0, -0.15f, 0);
					

					if (_initSpeed == Vector2.zero)
						_initSpeed = initSpeed;

					if (DynamicChanges) {
						_initSpeed = initSpeed;
						MetaBall.transform.localScale = new Vector3 (size, size, 1f);
                        Debug.Log("setScale2: " + size);

                        //CHANGE COLOR
                            
                         _lastFillColor = FillColor;
                         if (MetaBall.Active == false)
                            MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                        SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                       

                        TrailRenderer tr = WaterDropsObjects[i].GetComponent<TrailRenderer>();
                        if (TrailStartSize <= 0f)
                        {
                            tr.enabled = false;
                        }
                        else
                        {
                            tr.enabled = true;
                            tr.startWidth = TrailStartSize;
                            tr.endWidth = TrailEndSize;
                            tr.time = TrailDelay;
                        }


                        MetaBall.Velocity_Limiter_X = SpeedLimiterX;
                        MetaBall.Velocity_Limiter_Y = SpeedLimiterY;

                        Rigidbody2D rb = MetaBall.GetComponent<Rigidbody2D>();
                        rb.sharedMaterial = PhysicMat;
                        rb.drag = LinearDrag;
                        rb.angularDrag = AngularDrag;
                        rb.gravityScale = GravityScale;

                        if (FreezeRotation)
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                        MetaBall.GetComponent<CircleCollider2D>().sharedMaterial = PhysicMat;
                        MetaBall.GetComponent<CircleCollider2D>().radius = ColliderSize;
                        MetaBall.ScaleDown = ScaleDown;


                    }

                    MetaBall.Active = true;
                    MetaBall.witinTarget = false; 


                    //WaterDropsObjects [i].GetComponent<Rigidbody2D> ().velocity = _initSpeed;
                    Vector2 dir = transform.up;
                    MetaBall.GetComponent<Rigidbody2D>().velocity = -dir * Speed;

                    DropsUsed++;
                    _spawnedDrops++;




                    //print(Time.fixedDeltaTime / DelayBetweenParticles);
                    if (waitBetweenDropSpawn) {
                        if (DelayBetweenParticles < 0.001f)
                            yield return null;
                        else
                            yield return new WaitForSeconds(DelayBetweenParticles);

                    }
						

                    //Invoke event
                    InvokeOnSpawnerEmittinEachParticle(gameObject, MetaBall.gameObject);

                }
				yield return new WaitForEndOfFrame ();

                if (_spawnedDrops >= DropCount)
                {
                    //Invoke event End
                    if (_canInvokeAttheEnd)
                    {
                        InvokeOnSpawnerEnd(gameObject);
                        _canInvokeAttheEnd = false;
                    }

                    _spawnedDrops *= 0;

                    if (!Loop)
                        yield break;
                }


			}
		}

        /// <summary>
        /// Spawn all particles together at the same time
        /// </summary>
        /// <param name="_pos"></param>
        /// <param name="_initSpeed"></param>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        void SpawnAllParticles(Vector3 _pos, Vector2 _initSpeed, int count = -1, float delay = 0f)
        {
            LogicGame.Instance.txtLog.text = "cmmmm__breakLoop-SpawnAllParticles";
            Debug.Log("cmmmm__breakLoop-SpawnAllParticles");

            IsSpawning = true;

            int auxCount = 0;
            // while (true)
            //{

            if (WaterDropsObjects == null || WaterDropsObjects[0] == null)
            {
                SetupParticles();
            }

            for (int i = 0; i < WaterDropsObjects.Length; i++)
                {
                    MetaballParticleClass MetaBall = WaterDropsObjects[i].GetComponent<MetaballParticleClass>();

                    if (MetaBall.Active == true)
                        continue;

                    MetaBall.LifeTime = LifeTime;
                    WaterDropsObjects[i].transform.position = pointWater.transform.position + new Vector3(0, -0.15f, 0);
                    MetaBall.Active = true;
                    MetaBall.witinTarget = false;

                    if (_initSpeed == Vector2.zero)
                        _initSpeed = initSpeed;

                    if (DynamicChanges)
                    {
                        _initSpeed = initSpeed;
                        MetaBall.transform.localScale = new Vector3(size, size, 1f);
                        Debug.Log("setScale3: " + size);

                    //CHANGE COLOR

                    //_lastFillColor = FillColor;
                    if (MetaBall.Active == false)
                       MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                        SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);
                   

                }

                WaterDropsObjects[i].GetComponent<Rigidbody2D>().velocity = _initSpeed;


                    // Count limiter
                    if (count > -1)
                    {
                        auxCount++;
                        if (auxCount >= count && !Loop)
                        {
                            break;
                        }
                    }
                }
               
                //alreadySpawned = true;

             
           // }
        }

        public void InvokeOnShapeFill(GameObject obj, GameObject results)
        {
            OnValidateShapeFill?.Invoke(obj, results);
        }

        public void InvokeOnCollisionEnter2D(GameObject obj, GameObject other)
        {
            OnCollisionEnterList.Invoke(obj, other);

        }

        public void InvokeOnSpawnerStart(GameObject obj)
        {
            if(OnSpawnerAboutStart != null)
                OnSpawnerAboutStart.Invoke(obj, null);

        }

        public void InvokeOnSpawnerEnd(GameObject obj)
        {
            OnSpawnerAboutEnd.Invoke(obj, null);

        }

        public void InvokeOnSpawnerEmittinEachParticle(GameObject obj, GameObject obj2)
        {
            OnSpawnerEmitingParticle.Invoke(obj, obj2);

        }



        Camera fresnelCamera;
        int _lastSorting;
        public void SetRegularWaterparams(Color fill, Color fresnel, float alphaCutoff, float multiplier)
        {


            /*
            WaterMaterial.SetFloat("_constant", multiplier);
            WaterMaterial.SetFloat("_botmcut", alphaCutoff);
           
            */

            SpawnersManager.ChangeSpawnerValues(instance);

            // I do use Stroke as Fresnel
            // Fresnel is a shared property since I've use camera to reach that effect
            if(_lastStrokeColor != StrokeColor) {
                _lastStrokeColor = StrokeColor;
              // SpawnersManager.SetFresnelColor(StrokeColor);
            }

            if (_lastSorting != Sorting)
            {
                _lastSorting = Sorting;
                SpawnersManager.SetSorting(Sorting);
            }





        }
        /*
        public void SetToonWaterparams(Color fill, Color stroke, float alphaCutoff, float alphaStroke)
		{
            
            WaterMaterial.SetColor ("_Color", fill);
            WaterMaterial.SetColor ("_StrokeColor", stroke);
            WaterMaterial.SetFloat("_Cutoff", alphaCutoff);
            WaterMaterial.SetFloat("__Cutoff", alphaCutoff);
            WaterMaterial.SetFloat("_Stroke", alphaStroke);
            


           // SpawnersManager.SetColorScheme(ColorScheme, WaterMaterial, fill,stroke, alphaCutoff, alphaStroke);

            if (_lastSorting != Sorting)
            {
                _lastSorting = Sorting;
                SpawnersManager.SetSorting(Sorting);
            }
           

            SpawnersManager.ChangeSpawnerValues(instance);

            // I do use Stroke as Fresnel
            // Fresnel is a shared property since I've use camera to reach that effect
            if (_lastStrokeColor != StrokeColor)
            {
                _lastStrokeColor = StrokeColor;
                // SpawnersManager.SetFresnelColor(StrokeColor);
            }

            if (_lastSorting != Sorting)
            {
                _lastSorting = Sorting;
                SpawnersManager.SetSorting(Sorting);
            }

        }

        public void SetRefractingWaterparams(float intensity, float mag, float speed)
        {
            
            //WaterMaterial.SetColor("_TintColor", tint);
            WaterMaterial.SetFloat("_AmountOfTintColor", intensity);
            WaterMaterial.SetFloat("_Mag", mag*10f);
            WaterMaterial.SetFloat("_Speed", speed*10f);

            // I do use Stroke as Fresnel
            // Fresnel is a shared property since I've use camera to reach that effect
            if (_lastStrokeColor != StrokeColor)
            {
                _lastStrokeColor = StrokeColor;
               // SpawnersManager.SetFresnelColor(StrokeColor);
            }

            if (_lastSorting != Sorting)
            {
                _lastSorting = Sorting;
                SpawnersManager.SetSorting(Sorting);
            }

        }
        */
        IEnumerator UpdateQuietParticleProperties()
        {
            while (true)
            {
                if (WaterDropsObjects == null || WaterDropsObjects[0] == null)
                {
                    SetupParticles();
                }    

                for (int i = 0; i < WaterDropsObjects.Length; i++)
                {
                    MetaballParticleClass MetaBall = WaterDropsObjects[i].GetComponent<MetaballParticleClass>();

                    //CHANGE COLOR
                   
                        _lastFillColor = FillColor;
                        if (MetaBall.Active == false)
                            MetaBall.gameObject.GetComponent<SpriteRenderer>().color = FillColor;

                        SetRegularWaterparams(FillColor, StrokeColor, AlphaCutOff, AlphaStroke);

                    MetaBall.Velocity_Limiter_X = SpeedLimiterX;
                    MetaBall.Velocity_Limiter_Y = SpeedLimiterY;

                    Rigidbody2D rb = MetaBall.GetComponent<Rigidbody2D>();
                    rb.sharedMaterial = PhysicMat;
                    rb.drag = LinearDrag;
                    rb.angularDrag = AngularDrag;
                    rb.gravityScale = GravityScale;

                    if (FreezeRotation)
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                    MetaBall.GetComponent<CircleCollider2D>().sharedMaterial = PhysicMat;
                    MetaBall.GetComponent<CircleCollider2D>().radius = ColliderSize;



                }
                yield return null;
            }
        }


        public Material GetCurrentMaterial()
        {
           
            return WaterMaterial;

        }

        void StartCheckingFillShape()
        { 
            if(!_checkOnFillRunning && !_checkOnFillComplete)
            {
                print("StartCheckingFillShape");
                StartCoroutine(CheckOnFill(ShapeFillCollider2D, ShapeFillAccuracy));
            }
        }

        public void RestoreCheckingFillShape()
        {

            StartCoroutine(_restoreCheckingFillShapeEnum());
            
        }
        IEnumerator _restoreCheckingFillShapeEnum()
        {
            yield return new WaitForSeconds(.2f);
            _breakCheckOnFill = true;
            _checkOnFillComplete = false;

            if(ShapeFillCollider2DCount != ShapeFillCollider2D.Length)
            ShapeFillCollider2D = new Collider2D[ShapeFillCollider2DCount];
        }


        bool _checkOnFillRunning = false;
        bool _breakCheckOnFill = false;
        bool _checkOnFillComplete = false;
        IEnumerator CheckOnFill(Collider2D []shapeCollider, float accuracy = .8f)
        {

           
            _checkOnFillRunning = true;

            ContactFilter2D cf = new ContactFilter2D();
            cf.useTriggers = true;
            cf.SetLayerMask(Physics2D.GetLayerCollisionMask(DropObject.layer));
            cf.useLayerMask = true;

            Collider2D[] allOverlappingColliders = new Collider2D[DropCount];

           
            int result = 0;
            
            while (true)
            {
                if(_breakCheckOnFill)
                {
                    _checkOnFillRunning = false;
                    _breakCheckOnFill = false;
                    yield break;

                }

                

                yield return new WaitForFixedUpdate();

                for (int i = 0; i < shapeCollider.Length; i++)
                {
                   // Debug.Log(shapeCollider[i]);

                    if (shapeCollider[i] == null) continue;

                   

                    result = shapeCollider[i].OverlapCollider(cf, allOverlappingColliders);

                    

                    bool _trigged = false;

                    if (Water2DEmissionType == EmissionType.FillerCollider)
                    {
                        _trigged = (result >= DropsUsed * accuracy);
                    }
                    else
                    {
                        _trigged = (result >= DropCount * accuracy);
                    }


                    if (_trigged)
                    {

                        InvokeOnShapeFill(instance.gameObject, shapeCollider[i].gameObject);
                        //Debug.Log("Fill Event sucessful Complete! : droplives:" + DropsUsed + "   // " + ((int)(DropsUsed * accuracy)).ToString() + "within the target");
                        _checkOnFillComplete = true;
                        _breakCheckOnFill = true;
                    }
                }
                

                

            }
        }


        private void OnDestroy()
        {

            SpawnersManager.DeleteSpawnerValues(instance);
            DestroyImmediate(instance._parent);
            //StartCoroutine(_destroyItself());
        }
        
    }

}