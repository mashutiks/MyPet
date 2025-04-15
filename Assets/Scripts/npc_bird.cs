//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class BirdFSM : MonoBehaviour
//{
//    public Animator animator;
//    public float detectionRadius = 3f;
//    public float flyHeight = 3f;
//    public float flyDistance = 5f;
//    public float flySpeed = 5f;
//    public float landTime = 2f;

//    private enum State { Idle, FlyingAway, Landing }
//    private State currentState = State.Idle;

//    private Vector3 startPos;
//    private Vector3 targetPos;
//    private Transform player;
//    private float flyTimer;

//    void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player").transform;
//        startPos = transform.position;
//        animator.SetBool("flying", false);
//        animator.SetBool("landing", false);
//    }

//    void Update()
//    {
//        switch (currentState)
//        {
//            case State.Idle:
//                if (Vector3.Distance(player.position, transform.position) < detectionRadius)
//                {
//                    FlyAway();
//                }
//                break;

//            case State.FlyingAway:
//                flyTimer += Time.deltaTime;
//                float t1 = flyTimer / landTime;
//                transform.position = Parabola(startPos, targetPos, flyHeight, t1);

//                if (Vector3.Distance(transform.position, targetPos) < 0.1f)
//                {
//                    animator.SetBool("landing", true);
//                    currentState = State.Landing;
//                    StartCoroutine(LandRoutine());
//                }
//                break;

//            case State.Landing:
//                flyTimer += Time.deltaTime;
//                float t2 = flyTimer / landTime;
//                transform.position = Parabola(startPos, targetPos, flyHeight, t2);

//                if (t2 >= 1f)
//                {
//                    animator.SetBool("flying", false);
//                    animator.SetBool("landing", false);

//                    transform.position = targetPos;
//                    startPos = targetPos;
//                    currentState = State.Idle;
//                }
//                break;
//        }
//    }

//    void FlyAway()
//    {
//        animator.SetBool("flying", true);
//        currentState = State.FlyingAway;
//        flyTimer = 0f;

//        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
//        targetPos = transform.position + randomDir * flyDistance;
//        startPos = transform.position; // обновляем начало полёта
//                                      
//        Vector3 direction = (targetPos - startPos).normalized;

//        // Повернуть птицу лицом к направлению полёта
//        if (direction != Vector3.zero)
//            transform.rotation = Quaternion.LookRotation(direction);

//        // Определить угол между направлением взгляда и направлением движения
//        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

//        // Включить нужную анимацию по углу
//        if (Mathf.Abs(angle) < 20f)
//        {
//            animator.SetTrigger("flyStraight");
//        }
//        else if (angle < 0)
//        {
//            animator.SetTrigger("flyLeft");
//        }
//        else
//        {
//            animator.SetTrigger("flyRight");
//        }
//    }

//    IEnumerator LandRoutine()
//    {
//        yield return new WaitForSeconds(landTime);

//        animator.SetBool("flying", false);
//        animator.SetBool("landing", false);

//        Vector3 groundPos = targetPos;
//        groundPos.y = startPos.y;
//        transform.position = groundPos;

//        startPos = groundPos;
//        currentState = State.Idle;
//    }

//    Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
//    {
//        float parabolicT = t * 2 - 1;
//        Vector3 travelDirection = Vector3.Lerp(start, end, t);
//        float arc = height * (1 - parabolicT * parabolicT);
//        return new Vector3(travelDirection.x, travelDirection.y + arc, travelDirection.z);
//    }
//}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_bird : MonoBehaviour
{
    public Animator animator;
    public float detectionRadius = 3f;
    public float flyHeight = 3f;
    public float flyDistance = 5f;
    public float flySpeed = 5f;
    public float landTime = 2f;
    private Vector3 startPos;
    private Vector3 targetPos;
    private Transform player;
    private float flyTimer;
    public enum State { Idle, FlyingAway, Landing }

    public State currentState { get; private set; } = State.Idle;

    public abstract class BTNode
    {
        public abstract bool Execute();
    }

    public class Selector : BTNode
    {
        private List<BTNode> children;
        public Selector(List<BTNode> children) 
        { 
            this.children = children; 
        }
        public override bool Execute()
        {
            foreach (var child in children)
            {
                if (child.Execute())
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Sequence : BTNode
    {
        private List<BTNode> children;
        public Sequence(List<BTNode> children) 
        { 
            this.children = children; 
        }
        public override bool Execute()
        {
            foreach (var child in children)
            {
                if (!child.Execute())
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class CheckPlayerInRange : BTNode
    {
        private npc_bird bird;
        private Transform player;
        private float radius;

        public CheckPlayerInRange(npc_bird bird, Transform player, float radius)
        {
            this.bird = bird; this.player = player; this.radius = radius;
        }

        public override bool Execute()
        {
            return bird.currentState == npc_bird.State.Idle &&
                   Vector3.Distance(player.position, bird.transform.position) < radius;
        }
    }


    public class StartFlyAway : BTNode
    {
        private npc_bird bird;
        public StartFlyAway(npc_bird bird) { this.bird = bird; }
        public override bool Execute()
        {
            bird.FlyAway(); return true;
        }
    }

    public class ContinueFlying : BTNode
    {
        private npc_bird bird;
        public ContinueFlying(npc_bird bird) { this.bird = bird; }
        public override bool Execute()
        {
            return bird.ContinueFlyingLogic();
        }
    }

    public class LandBird : BTNode
    {
        private npc_bird bird;
        public LandBird(npc_bird bird) { this.bird = bird; }
        public override bool Execute()
        {
            return bird.ContinueLandingLogic();
        }
    }

    private BTNode root;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;

        animator.SetBool("flying", false);
        animator.SetBool("landing", false);

        root = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode>
            {
                new CheckPlayerInRange(this, player, detectionRadius),
                new StartFlyAway(this)
            }),
            new ContinueFlying(this),
            new LandBird(this)
        });
    }

    void Update()
    {
        root.Execute();
    }

    public void FlyAway()
    {
        if (currentState != State.Idle) return;

        animator.SetBool("flying", true);
        currentState = State.FlyingAway;
        flyTimer = 0f;

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        targetPos = transform.position + randomDir * flyDistance;
        targetPos.y = transform.position.y;
        startPos = transform.position;

        Vector3 direction = (targetPos - startPos).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        if (Mathf.Abs(angle) < 20f)
        {
            animator.SetTrigger("flyStraight");
        }
        else if (angle < 0)
        {
            animator.SetTrigger("flyLeft");
        }
        else
        {
            animator.SetTrigger("flyRight");
        }
    }

    public bool ContinueFlyingLogic()
    {
        if (currentState != State.FlyingAway) return false;

        flyTimer += Time.deltaTime;
        float t1 = flyTimer / landTime;
        transform.position = Parabola(startPos, targetPos, flyHeight, t1);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            animator.SetBool("landing", true);
            currentState = State.Landing;
            StartCoroutine(LandRoutine());
        }
        return true;
    }

    public bool ContinueLandingLogic()
    {
        if (currentState != State.Landing) return false;

        flyTimer += Time.deltaTime;
        float t2 = flyTimer / landTime;
        transform.position = Parabola(startPos, targetPos, flyHeight, t2);

        if (t2 >= 1f)
        {
            animator.SetBool("flying", false);
            animator.SetBool("landing", false);
            transform.position = targetPos;
            startPos = targetPos;
            currentState = State.Idle;
        }
        return true;
    }

    IEnumerator LandRoutine()
    {
        yield return new WaitForSeconds(landTime);
        animator.SetBool("flying", false);
        animator.SetBool("landing", false);

        Vector3 groundPos = targetPos;
        groundPos.y = startPos.y;
        transform.position = groundPos;

        startPos = groundPos;
        currentState = State.Idle;
    }

    Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        Vector3 travelDirection = Vector3.Lerp(start, end, t);
        float arc = height * (1 - parabolicT * parabolicT);
        return new Vector3(travelDirection.x, travelDirection.y + arc, travelDirection.z);
    }
}
