using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace PhoenixWright.Modules
{
    internal static class Projectiles
    {
        internal static GameObject vasePrefab;
        internal static GameObject knifePrefab;
        internal static GameObject phonePrefab;
        internal static GameObject bottlePrefab;
        internal static GameObject servbotPrefab;
        internal static GameObject armPrefab;
        private static Vector3 scaleUp = new Vector3(2f, 2f, 2f);

        internal static void RegisterProjectiles()
        {
            // only separating into separate methods for my sanity
            Createvase();
            Createknife();
            Createphone();
            Createbottle();
            Createservbot();
            CreateArm();

            AddProjectile(vasePrefab);
            AddProjectile(knifePrefab);
            AddProjectile(phonePrefab);
            AddProjectile(servbotPrefab);
            AddProjectile(bottlePrefab);
            AddProjectile(armPrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
        }

        private static void Createvase()
        {
            vasePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "VaseProjectile");
            vasePrefab.transform.localScale = scaleUp;

            ProjectileImpactExplosion vaseAOE = vasePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(vaseAOE);

            vaseAOE.blastRadius = 5f;
            vaseAOE.destroyOnEnemy = true;
            vaseAOE.lifetime = 12f;
            vaseAOE.timerAfterImpact = true;
            vaseAOE.lifetimeAfterImpact = 0f;

            ProjectileController vaseController = vasePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("VaseGhost") != null) vaseController.ghostPrefab = CreateGhostPrefab("VaseGhost");
            vaseController.startSound = "";
        }

        private static void Createknife()
        {
            knifePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "KnifeProjectile");
            knifePrefab.transform.localScale = scaleUp;

            ProjectileImpactExplosion bombImpactExplosion = knifePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 5f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;
            bombImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s | DamageType.BleedOnHit;


            ProjectileController bombController = knifePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("KnifeGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("KnifeGhost");
            bombController.startSound = "";
        }

        private static void Createphone()
        {
            phonePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "PhoneProjectile");
            phonePrefab.transform.localScale = scaleUp;

            ProjectileImpactExplosion bombImpactExplosion = phonePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 5f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;
            bombImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s | DamageType.Shock5s;

            ProjectileController bombController = phonePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("PhoneGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("PhoneGhost");
            bombController.startSound = "";
        }

        private static void Createbottle()
        {
            bottlePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "BottleProjectile");
            bottlePrefab.transform.localScale = scaleUp;

            ProjectileImpactExplosion bombImpactExplosion = bottlePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 5f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController bombController = bottlePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("BottleGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("BottleGhost");
            bombController.startSound = "";
        }

        private static void Createservbot()
        {
            servbotPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "ServobotProjectile");
            servbotPrefab.transform.localScale = scaleUp;

            ProjectileImpactExplosion bombImpactExplosion = servbotPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 5f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController bombController = servbotPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("ServbotGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("ServbotGhost");
            bombController.startSound = "";
        }

        private static void CreateArm()
        {
            armPrefab = CloneProjectilePrefab("magefirebolt", "ArmProjectile");
            armPrefab.transform.localScale = new Vector3(4, 4, 4);

            ProjectileImpactExplosion armAOE = armPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(armAOE);

            armAOE.blastRadius = 10f;
            armAOE.destroyOnEnemy = true;
            armAOE.lifetime = 12f;
            armAOE.timerAfterImpact = true;
            armAOE.lifetimeAfterImpact = 0f;
            armAOE.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s | DamageType.BypassArmor;

            ProjectileController armController = armPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("ArmGhost") != null) armController.ghostPrefab = CreateGhostPrefab("ArmGhost");
            armController.startSound = "";
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}