using UnityEngine;
using System.Collections;
using YottaIO.Tools;

public class LoadScene : StateMachineBehaviour {

    public YottaIO.Tools.IntroController IntroController;
    public bool Male, Female;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    //  OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (IntroController == null)
            IntroController = GameObject.FindGameObjectWithTag("MenuController").GetComponent<IntroController>();
        if (Male)
            IntroController.MaleSelected();
        if (!Male)
            IntroController.FemaleSelected();
        //  Transitions.VRTransition.ChangeScene(1, "Main", Selected.selected);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
