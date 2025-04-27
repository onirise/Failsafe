using UnityEngine;
using UnityEngine.UIElements;

public class PlayerVisibility : MonoBehaviour
{
    [Header("Player Visibility")]
    [SerializeField] private Transform[] playerPoints;

    [Range(0, 100)]
    [SerializeField] private int playerVisScore;
    [SerializeField] private string playerStatus;
    [SerializeField] private float lowModifire = 1.5f;
    [SerializeField] private float mediumModifire = 2f;
    [SerializeField] private float highModifire = 3f;
    [Header("Detection Lights")]
    [SerializeField] private float checkRadius = 5f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;


}
