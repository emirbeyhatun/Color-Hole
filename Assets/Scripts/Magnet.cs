using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(SphereCollider))]
public class Magnet : MonoBehaviour
{
	#region Singleton class: Magnet

	public static Magnet Instance;

	public BoxCollider boxCollider;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}

		boxCollider = GetComponent<BoxCollider>();
	}

	#endregion

	[SerializeField] float magnetForce;
	private bool enableMagnet = true;


	//to store objects inside magnetic field
	List<Rigidbody> affectedRigidbodies = new List<Rigidbody> ();
	Transform magnet;

	void Start ()
	{
		magnet = transform;
		affectedRigidbodies.Clear ();
	}

	void FixedUpdate ()
	{
		if(enableMagnet == false)
		return;

		if (!Game.isGameover && Game.isMoving) {
			foreach (Rigidbody rb in affectedRigidbodies) {
				rb.AddForce ((magnet.position - rb.position) * magnetForce * Time.fixedDeltaTime);
			}
		}
	}

	//Object enters Magnetic field
	void OnTriggerEnter (Collider other)
	{
		if(enableMagnet == false)
		return;
		string tag = other.tag;

		if (!Game.isGameover && (tag.Equals ("Obstacle") || tag.Equals ("Object"))) {
			AddToMagnetField (other.attachedRigidbody);
		}
	}

	//Object exits Magnetic field
	void OnTriggerExit (Collider other)
	{
		if(enableMagnet == false)
		return;
		string tag = other.tag;

		if (!Game.isGameover && (tag.Equals ("Obstacle") || tag.Equals ("Object"))) {
			RemoveFromMagnetField (other.attachedRigidbody);
		}
	}

	public void AddToMagnetField (Rigidbody rb)
	{
		affectedRigidbodies.Add (rb);
	}

	public void RemoveFromMagnetField (Rigidbody rb)
	{
		affectedRigidbodies.Remove (rb);
	}

	public void DisableHole()
	{
		boxCollider.enabled = true;
		enableMagnet = false;
		affectedRigidbodies.Clear ();
	}

	public void EnableHole()
	{
		boxCollider.enabled = false;
		enableMagnet = true;
	}
}
