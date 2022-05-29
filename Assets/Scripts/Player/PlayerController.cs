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

    public int [] level=new int[6];
    public int [] hp=new int[6];
    public int [] id=new int[6];

    public int [] images=new int[6];

    public PokemonParty pokemonParty;


    public void setPokemonParty(PokemonParty _pokemonParty){
        pokemonParty=_pokemonParty;
    }
    public PokemonParty getPokemonParty(){
        Debug.Log("number of pokemons");
        pokemonParty.number();
        return pokemonParty;
    }
    public void setImages (int [] newImages) {
        images=newImages;
   }
   public int[] getPokemonImages(){
       return images;
   }



    public void setID (int [] newID) {
        id=newID;
   }
   public int[] getIds(){
       return id;
   }

   public void setHP (int [] newHP) {
        hp=newHP;
   }
   public int[] getHP(){
       return hp;
   }

   public void setLevel (int [] newLevel) {
        level=newLevel;
   }
   public int[] getPokemonsLevel(){
       return level;
   }



    public void setWallet (string newWallet) {
        wallet=newWallet;
   }

    public string getWallet () {
        return wallet;
   }

   

        
    public float[] getPosition()
    {
        float[] position = new float[2];
        position[0] = transform.position.x;
        position[1] = transform.position.y;
        // Debug.Log(position[0]);
        // Debug.Log(position[1]);

        return position;
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
        if(Physics2D.OverlapCircle(targetPos,0.3f,solidObjectsLayer)!=null)
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
