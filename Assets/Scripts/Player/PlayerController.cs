using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.Networking;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    public event Action OnEncountered; //to know when a battle begins
    public bool isMoving;
    private Vector2 input;

    private Animator animator;

    public string wallet;

    public void setWallet (string newWallet) {
        wallet=newWallet;
   }

    public string getWallet () {
        return wallet;
   }
   

    public void Start(){
        
    }
    private void Awake()
    {
        animator=GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        
        if(!isMoving)
        {
            input.x=Input.GetAxis("Horizontal");
            input.y=Input.GetAxis("Vertical");

            //remove diagonal moves
            if(input.x!=0)input.y=0;

            if(input!=Vector2.zero){

                animator.SetFloat("moveX",input.x);
                animator.SetFloat("moveY",input.y);

                var targetPos=transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

               
                if(IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
                
            }
        }
        animator.SetBool("isMoving",isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving=true;
        while((targetPos-transform.position).sqrMagnitude>Mathf.Epsilon)
        {
            transform.position=Vector3.MoveTowards(transform.position,targetPos,moveSpeed*Time.deltaTime);
            yield return null;
        }
        transform.position=targetPos;
        isMoving=false;

        CheckForEncounters();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos,0.1f,solidObjectsLayer)!=null)
        {
            return false;
        }
        return true;
    }

    private void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position,0.2f,grassLayer)!=null)

            if(UnityEngine.Random.Range(1,101)<=10)
            {
                animator.SetBool("isMoving",false);
                OnEncountered();
            }
    }
}
