using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that determines the behavior before a race. This one does a flyover, a wipe, then a tween to look at the player's gremlin.
/// </summary>
public class RaceStart : MonoBehaviour
{
    /// <summary>
    /// The RaceManager's racingCamera.
    /// </summary>
    RacingCamera racingCamera;
    /// <summary>
    /// The RaceManager that called this start.
    /// </summary>
    RaceManager manager;

    /// <summary>
    /// The camera offset when it's viewing the starting line.
    /// </summary>
    [Tooltip("The camera offset when it's viewing the starting line.")]
    public Vector3 startingLineOffset = new Vector3(-20, 3, 0);
    /// <summary>
    /// The camera's offset when it's doing a flyover.
    /// </summary>
    [Tooltip("The camera's offset when it's doing a flyover.")]
    public Vector3 flyoverOffset = new Vector3(2, 6, 0);
    /// <summary>
    /// The camera's offset when it's viewing a race. The value of the z-axis is completely ignored (So we can view all the racers).
    /// </summary>
    [Tooltip("The camera's offset when it's viewing a race.The value of the z-axis is completely ignored(So we can view all the racers).")]
    public Vector3 actualRaceOffset = new Vector3(-10, 6, 0);

    public virtual void RaceStartSetup(RaceManager raceManager) {
        manager = raceManager;
        racingCamera = raceManager.racingCamera;
        racingCamera.cameraOffset = flyoverOffset;
        racingCamera.SetFlyover(manager.racetracks[Mathf.RoundToInt(manager.racetracks.Count / 2)].GetComponent<TrackManager>(), 1, true, FlyoverDone);
    }

    //Could I have made all these callbacks easier to do than just making a function for each one? Sure. But whatever, it works.

    void FlyoverDone()
    {
        //Set up cool racer lineup effect:
        GetComponentInChildren<Camera>().transform.position = manager.racetracks[0].GetComponent<TrackManager>().RacingGremlin.transform.position + startingLineOffset;
        racingCamera.SetWipe(GetComponentInChildren<Camera>(), manager.ActiveUI, new Vector3(-Screen.width, Screen.height / 2), new Vector3(Screen.width / 2, Screen.height / 2), 1.0f, BeginActualLineup);
    }

    void BeginActualLineup()
    {
        racingCamera.transform.position = manager.racetracks[0].GetComponent<TrackManager>().RacingGremlin.transform.position + startingLineOffset;
        racingCamera.SetTween(manager.racetracks[manager.racetracks.Count - 1].GetComponent<TrackManager>().RacingGremlin.transform.position + startingLineOffset, racingCamera.transform.rotation, 2.0f, LookAtGremlin);
    }

    void LookAtGremlin() {
        Transform gremlinTransform = manager.racetracks[manager.gremlinPlayerIndex].GetComponent<TrackManager>().RacingGremlin.transform;
        racingCamera.SetTween(racingCamera.transform.position, Quaternion.LookRotation(gremlinTransform.position - racingCamera.transform.position), 1.0f, CameraReady);
    }

    void CameraReady()
    {
        Transform gremlinTransform = manager.racetracks[manager.gremlinPlayerIndex].GetComponent<TrackManager>().RacingGremlin.transform;
        racingCamera.cameraOffset = new Vector3(actualRaceOffset.x, actualRaceOffset.y, racingCamera.transform.position.z - gremlinTransform.position.z + actualRaceOffset.z);
        Vector3 targetPos = gremlinTransform.position + racingCamera.cameraOffset;
        racingCamera.SetTween(targetPos, Quaternion.LookRotation(gremlinTransform.position - targetPos, Vector3.up), 2.0f, StartCountdown);
    }

    float countdownSeconds;
    public GameObject countdownTextPrefab;
    GameObject countdownText;

    void StartCountdown() {
        countdownSeconds = 3;
        countdownText = Instantiate(countdownTextPrefab, manager.ActiveUI.transform);
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown() {
        while (countdownSeconds > 0)
        {
            countdownText.GetComponentInChildren<UnityEngine.UI.Text>().text = countdownSeconds.ToString();
            countdownSeconds -= 1;
            yield return new WaitForSeconds(1);
        }
        countdownText.GetComponentInChildren<UnityEngine.UI.Text>().text = "GO!";
        LockGremlinAndStart();
        yield return new WaitForSeconds(1); //Leave the "GO!" up for a little bit.
        Destroy(countdownText);
    }

    void LockGremlinAndStart() {
        racingCamera.SetGremlinFocus(manager.racetracks[manager.gremlinPlayerIndex].GetComponent<TrackManager>().RacingGremlin, false);
        manager.StartTracks();
    }
}
