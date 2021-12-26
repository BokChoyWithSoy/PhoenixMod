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

        internal static void RegisterProjectiles()
        {
            // only separating into separate methods for my sanity
            Createvase();
            Createknife();
            Createphone();
            Createbottle();
            Createservbot();

            AddProjectile(vasePrefab);
            AddProjectile(knifePrefab);
            AddProjectile(phonePrefab);
            AddProjectile(servbotPrefab);
            AddProjectile(bottlePrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
        }

        private static void Createvase()
        {
            vasePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = vasePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 5f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController bombController = vasePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("VaseGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("VaseGhost");
            bombController.startSound = "";
        }

        private static void Createknife()
        {
            knifePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = knifePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 5f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController bombController = knifePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("KnifeGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("KnifeGhost");
            bombController.startSound = "";
        }

        private static void Createphone()
        {
            phonePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = phonePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 5f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController bombController = phonePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("PhoneGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("PhoneGhost");
            bombController.startSound = "";
        }

        private static void Createbottle()
        {
            bottlePrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bottlePrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 5f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController bombController = bottlePrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("BottleGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("BottleGhost");
            bombController.startSound = "";
        }

        private static void Createservbot()
        {
            servbotPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = servbotPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 5f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController bombController = servbotPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("ServbotGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("ServbotGhost");
            bombController.startSound = "";
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

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
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