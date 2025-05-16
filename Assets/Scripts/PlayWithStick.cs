using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayWithStick : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform current_toy; // òåêóùàÿ èãðóøêà
    public Transform stick;
    public Transform bone;
    public Transform fish;
    public LineController line_controller;

    private enum DogState { Idle, RunningToStick, PickingStick, ReturningHome }
    private DogState current_state = DogState.Idle;

    public float stopping_distance = 0.1f;
    public Transform teeth_bone;

    private Vector3 start_position;
    private Quaternion start_rotation;
    private bool stick_attached = false;

    void Start()
    {
        CheckItemAvailability();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = stopping_distance;
        start_position = transform.position;
        start_rotation = transform.rotation;
    }

    void Update()
    {
        switch (current_state)
        {
            case DogState.Idle:
                if (line_controller.is_stick_fall && !stick_attached)
                {
                    StartRunningToStick();
                }
                break;

            case DogState.RunningToStick:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    PickUpStick();
                }
                break;

            case DogState.PickingStick:
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f &&
                    animator.GetCurrentAnimatorStateInfo(0).IsName("EatingStart"))
                {
                    ReturnHome();
                }
                break;

            case DogState.ReturningHome:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    FinishReturn();
                    AchievemenetManager.Instance.EarnAchievement("Делу - время");
                }
                break;
        }
    }

    void StartRunningToStick()
    {
        current_state = DogState.RunningToStick;

        Vector3 stop_position = new Vector3(
            current_toy.position.x - 0.5f,
            current_toy.position.y,
            current_toy.position.z - 0.5f
        );


        Vector3 direction = (stop_position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction); // ïîâîðîò ïåðåä íà÷àëîì äâèæåíèÿ

        animator.SetInteger("AnimationID", 4); // àíèìàöèÿ áåãà
        agent.isStopped = false;
        agent.SetDestination(stop_position);
    }

    void PickUpStick()
    {
        current_state = DogState.PickingStick;
        agent.isStopped = true;

        Vector3 stick_direction = (current_toy.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(stick_direction);

        AttachStick();
        animator.Play("EatingStart");
    }

    void AttachStick()
    {
        current_toy.SetParent(teeth_bone);
        current_toy.localPosition = Vector3.zero;
        current_toy.localRotation = Quaternion.Euler(0f, 0f, 90f);
        stick_attached = true;
    }

    void ReturnHome()
    {
        current_state = DogState.ReturningHome;

        Vector3 homeDirection = (start_position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(homeDirection);

        agent.isStopped = false;
        animator.SetInteger("AnimationID", 4); // àíèìàöèÿ áåãà
        agent.SetDestination(start_position);
    }

    void FinishReturn()
    {
        current_state = DogState.Idle;
        agent.isStopped = true;


        StartCoroutine(CompleteReturnSequence()); // çàâåðøàåì âñå àíèìàöèè è ïîâîðîòû
    }

    IEnumerator CompleteReturnSequence()
    {

        yield return StartCoroutine(SmoothRotateToInitial()); // çàâåðøåíèå ïîâîðîòà â èñõîäíîå ïîëîæåíèå

        stick_attached = false;

        animator.SetInteger("AnimationID", 0); // àíèìàöèÿ äûõàíèÿ


        line_controller.ResetThrow(); // ñáðàñûâàåì ôëàãè òðàåêòîðèè áðîñêà
    }

    IEnumerator SmoothRotateToInitial()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Quaternion startRot = transform.rotation;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, start_rotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = start_rotation;

        yield return new WaitForSeconds(0.2f);
    }

    public void ResetDog()
    {
        if (current_state != DogState.Idle)
        {
            current_state = DogState.Idle;
            agent.isStopped = true;
            stick_attached = false;
            animator.SetInteger("AnimationID", 0);
            transform.rotation = start_rotation;
        }
    }

    void CheckItemAvailability()
    {
        int object_1 = PlayerPrefs.GetInt("Item_Stick_Selected", 0);
        int object_2 = PlayerPrefs.GetInt("Item_Bone_Selected", 0);
        int object_3 = PlayerPrefs.GetInt("Item_Fish_Selected", 0);

        if (object_1 == 1)
        {
            current_toy = stick;
        }
        else if (object_2 == 1)
        {
            current_toy = bone;
        }
        else if (object_3 == 1)
        {
            current_toy = fish;
        }
    }
}