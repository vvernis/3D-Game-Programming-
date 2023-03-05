using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {
	HealthManager healthManager;
	bool isDestroyed = false;
	public GameObject deadScreen;
	public LevelSystem levelsys;

	void Start() {
		healthManager = GetComponent<HealthManager>();
		deadScreen = GameObject.Find("UI/InGameUI/DeadScreen");
		levelsys = GetComponent<LevelSystem>();
	}

	void Update() {
		if(healthManager.IsDead && !isDestroyed) {
			isDestroyed = true;

			StartCoroutine(ShowDeadScreen());

			MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>();

			foreach(MonoBehaviour script in scripts) {
				// Disable all weapons
				if(script is WeaponBase) {
					DisableWeapon((WeaponBase)script);
				}
				// Deactivate player controls
				else if(script is FirstPersonController) {
					DisableController((FirstPersonController)script);
				}
			}
		}
	}

	void DisableWeapon(WeaponBase weapon) {
		weapon.IsEnabled = false;
	}

	void DisableController(FirstPersonController controller) {
		controller.enabled = false;
	}

	IEnumerator ShowDeadScreen() {
		deadScreen.SetActive(true);

		Image image = deadScreen.GetComponent<Image>();
		Color origColor = image.color;
		Text deathText = GameObject.Find("UI/InGameUI/DeadScreen/DeathText").GetComponent<Text>();
		if(levelsys.GetLevel()>0)
		{
			deathText.text = "Current Level: "+levelsys.GetLevel();
		}
	
		for(float alpha = 0.0f; alpha <= 1.1f; alpha += 0.1f) {
			image.color = new Color(origColor.r, origColor.g, origColor.b, alpha);
			yield return new WaitForSeconds(0.1f);
		}

		yield break;
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(hit.gameObject.tag == "BulletCase") {
			Physics.IgnoreCollision(GetComponent<Collider>(), hit.gameObject.GetComponent<Collider>());
		}
	}
}
