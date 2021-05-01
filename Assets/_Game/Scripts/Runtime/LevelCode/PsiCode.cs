namespace StuckInALoop
{
    using Spineless;
    using Spineless.Extensions.Components;
    using UnityEngine;
  using UnityEngine.Events;


  [CreateAssetMenu]
    public class PsiCode : ScriptableObject
    {
        public string PhaseName;
        public TriAttack FireAttack;

        public UnityEvent OnInitialize;
        public UnityEvent OnExit;

        public LevelLoop Code;

        private AttackSpawner _attackSpawner;
        private GameObject _psi;
        private PsiEvent _psiEvent;

        private TriAttack spawnedFireAttack;
        private int summonCounter;


        public PsiEvent PsiEvent
        {
            get
            {
                if (this._psiEvent == null)
                    this._psiEvent = FindObjectOfType<PsiEvent>();
                return this._psiEvent;
            }
        }


        public GameObject Psi
        {
            get
            {
                if (this._psi == null)
                    this._psi = GameObject.Find("Enemy-PsiBoss");
                return this._psi;
            }
        }


        public AttackSpawner AttackSpawner
        {
            get
            {
                if (this._attackSpawner == null)
                    this._attackSpawner = this.Psi.GetComponent<AttackSpawner>();
                return this._attackSpawner;
            }
        }


        public void Initialize()
        {
            if (this.OnInitialize != null)
                this.OnInitialize.Invoke();
        }


        public void Exit()
        {
            if (this.OnExit != null)
                this.OnExit.Invoke();
        }


        public void ShockAndAwe()
        {
            foreach (GameObject background in this.PsiEvent.Backgrounds)
            {
                background.SetActive(true);
            }
            
            Camera mainCamera = Camera.main;
            Transform cameraTransform = mainCamera.transform;
            cameraTransform.position = new Vector3(
                6.5f,
                1f,
                -20);
            Coroutines.ShakeTransform(
                transform: cameraTransform,
                duration: 4f,
                magnitude: 0.25f);
        }


        public void SummonOutsider()
        {
            this.summonCounter++;
            if (this.summonCounter >= this.PsiEvent.SummonLocations.Length)
                this.summonCounter = 0;
            GameObject spawnLocation = this.PsiEvent.SummonLocations[this.summonCounter];
            SpriteRenderer circleSprite = spawnLocation.GetComponent<SpriteRenderer>();
            circleSprite.color = Color.clear;
            this.PsiEvent.SummonLocations[this.summonCounter].SetActive(true);
            Coroutines.EaseSpriteColor(
                target: circleSprite,
                color: Color.red,
                duration: 2f,
                callback: () =>
                    {
                        int randomIndex = Random.Range(0, this.PsiEvent.Enemies.Length);
                        Instantiate(
                            this.PsiEvent.Enemies[randomIndex],
                            circleSprite.transform.position,
                            Quaternion.identity);
                        spawnLocation.SetActive(false);
                    });
        }


        public void SummonFire()
        {
            RemoveFire();
            this.spawnedFireAttack = Instantiate(this.FireAttack);
            this.spawnedFireAttack.transform.position = new Vector3(
                this.Psi.transform.position.x,
                -0.4f,
                0f);
        }


        public void RemoveFire()
        {
            if (this.spawnedFireAttack != null)
                Destroy(this.spawnedFireAttack.gameObject);
        }


        public void SpeedUpBlaze(LevelVariable[] variables)
        {
            float factor = variables[0].AsFloat();
            
            float currentSpeed = this.spawnedFireAttack.CurrentRotationSpeed;
            this.spawnedFireAttack.SetRotationSpeed(currentSpeed + factor);
        }


        public void ChooseYourWeapon()
        {
            int weaponIndex = 0;
            foreach (GameObject location in this.PsiEvent.WeaponPickupLocations)
            {
                if (location.transform.childCount > 0)
                {
                    foreach (Pickup child in location.GetComponentsInChildren<Pickup>())
                    {
                        Destroy(child.gameObject);
                    }
                }

                Pickup spawnedWeapon = Instantiate(
                    original: this.PsiEvent.WeaponPickups[weaponIndex], 
                    parent: location.transform, 
                    worldPositionStays: true);
                spawnedWeapon.transform.localPosition = Vector3.zero;
                weaponIndex++;
            }
        }


        public void MimicPlayer()
        {
            AttackSpawner existingAttackSpawner = this.AttackSpawner;
            AttackSpawner playerAttackSpawner = GameManager.Player.GetComponent<AttackSpawner>();

            if (existingAttackSpawner.AttackToSpawn.name == playerAttackSpawner.AttackToSpawn.name)
                return;

            //string playerVariableValue = this.PsiEvent.GameLevel.GetVariable("player").DataValue;
            //this.PsiEvent.GameLevel.SetVariable("response", playerVariableValue);
            
            bool shouldEnable = true;
            if (existingAttackSpawner != null)
                shouldEnable = existingAttackSpawner.enabled;

            AttackSpawner createdAttackSpawner = playerAttackSpawner.CopyComponentTo(this.Psi);
            createdAttackSpawner.enabled = shouldEnable;
        }


        public void AttackPlayer()
        {
            //this.AttackSpawner.AutoAttack = true;
            Attack spawnedAttack = this.AttackSpawner.SpawnAttack(GameManager.Player.transform.position);
            if (spawnedAttack != null)
            {
                spawnedAttack.transform.localScale = 
                    this.AttackSpawner.AttackToSpawn.transform.localScale * 2;
                AutoDestroy autoDestroy = spawnedAttack.GetComponent<AutoDestroy>();
                if (autoDestroy != null)
                {
                    autoDestroy.TimeToLive = 
                        this.AttackSpawner.AttackToSpawn.GetComponent<AutoDestroy>().TimeToLive * 2;
                }
            }
        }


        public void RestorePlayerPerception()
        {
            LevelEvents[] levelEvents = FindObjectsOfType<LevelEvents>();
            foreach (LevelEvents levelEvent in levelEvents)
            {
                levelEvent.IgnorePlaything();
            }
        }


        public void ObservePlaything()
        {
            // Intentionally do nothing here.
        }
    }
}