﻿//-----------------------------------------------
using UnityEngine;
using System.Collections;
//-----------------------------------------------
public class Enemy_ToughGuy : Enemy 
{
	//-----------------------------------------------
	//Sound to play on destroy
	public AudioClip DestroyAudio = null;
	
	//Audio Source for sound playback
	private AudioSource SFX = null;
	
	//------------------------------------------------
	protected override void Start()
	{
		//Called super start method
		base.Start();
		
		//Find sound object in scene
		GameObject SoundsObject = GameObject.FindGameObjectWithTag("sounds");
		
		//If no sound object, then exit
		if(SoundsObject == null) return;
		
		//Get audio source component for sfx
		SFX = SoundsObject.GetComponent<AudioSource>();
	}
	//------------------------------------------------
	//Event called when damaged by an attack
	public void Damage(int Damage = 0)
	{
		//Reduce health
		Health -= Damage;
		
		//Play damage animation
		gameObject.SendMessage("PlayColorAnimation",0,SendMessageOptions.DontRequireReceiver);
		
		//Check if dead
		if(Health <= 0)
		{
			//Send enemy destroyed notification
			GameManager.Notifications.PostNotification(this, "EnemyDestroyed");
			
			//Play collection sound, if audio source is available
			if(SFX){SFX.PlayOneShot(DestroyAudio, 1.0f);}
			
			//Remove object from scene
			DestroyImmediate(gameObject);
		}
	}
	//------------------------------------------------
	//Handle patrol state
	public void Patrol()
	{
        AttackAnimator.HideAllSprites();
		
	    StartAnimator(PatrolAnimator);
	}
	//------------------------------------------------
	//Handle Chase State
	public void Chase()
	{
		//Same animations as patrol
		Patrol();
	}
	//------------------------------------------------
	//Entered Attack State
	public void Attack()
	{
        PatrolAnimator.HideAllSprites();
		
	    StartAnimator(AttackAnimator);
	}
	//------------------------------------------------
	//Strike - called each time the enemy makes a strike against the player (deal damage)
	public void Strike()
	{
		//Damage player
		PlayerController.gameObject.SendMessage("ApplyDamage", AttackDamage, SendMessageOptions.DontRequireReceiver);
	}
	//------------------------------------------------
}
