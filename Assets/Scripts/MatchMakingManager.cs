using PlayFab;
using PlayFab.MultiplayerModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchMakingManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject leaveQueueButton;
    [SerializeField] private TMP_Text queueStatusText;

    private string ticketId;
    private Coroutine pollTicketCoroutine;

    private const string QueueName = "DefaultQueue";

    public void StartMatchmaking()
    {
        playButton.SetActive(false);
        queueStatusText.text = "Submitting Ticket";
        queueStatusText.gameObject.SetActive(true);

        var matchmakingRequest = new CreateMatchmakingTicketRequest {
            Creator = new MatchmakingPlayer
            {
                Entity = new EntityKey
                {
                    Id = PlayFabUserLogin.EntityId,
                    Type = "title_player_account"
                },
                Attributes = new MatchmakingPlayerAttributes
                {
                    DataObject = new { }
                }
            },
            GiveUpAfterSeconds = 120,
            //CHANGE FOR OTHER QUEUES
            QueueName = QueueName
        };

        PlayFabMultiplayerAPI.CreateMatchmakingTicket( matchmakingRequest, OnMatchmakingTicketSuccess, OnMatchmakingTicketError);
    }

    private void OnMatchmakingTicketSuccess(CreateMatchmakingTicketResult result)
    {
        ticketId = result.TicketId;
        pollTicketCoroutine = StartCoroutine(PollTicket(result.TicketId));

        leaveQueueButton.SetActive(true);
        queueStatusText.text = "Ticket created";
    }

    private void OnMatchmakingTicketError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private IEnumerator PollTicket (string ticketId)
    {
        while (true)
        {
            var getMatchmakingTicketRequest = new GetMatchmakingTicketRequest
            {
                TicketId = ticketId,
                QueueName = QueueName
            };
            Debug.Log("coururu");
            PlayFabMultiplayerAPI.GetMatchmakingTicket(getMatchmakingTicketRequest,OnGetMatchmakingTicketSuccess,OnGetMatchmakingTicketError);
            yield return new WaitForSeconds(6);
        }
    }
    private void OnGetMatchmakingTicketSuccess(GetMatchmakingTicketResult result)
    {
        queueStatusText.text = $"Status: {result.Status}";

        switch (result.Status)
        {
            case "Matched":
                StopCoroutine(pollTicketCoroutine);
                StartMatch(result.MatchId);
                Debug.Log(result.MatchId);
                break;

            case "Canceled":
                break;
        }
    }
    private void OnGetMatchmakingTicketError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private void StartMatch(string matchId)
    {
        queueStatusText.text = $"Starting match";

        var getMatchRequest = new GetMatchRequest {
            MatchId = matchId,
            QueueName = QueueName  
        };

        PlayFabMultiplayerAPI.GetMatch(getMatchRequest,OnGetMatchSuccess,OnGetMatchError);
    }

    private void OnGetMatchSuccess(GetMatchResult result)
    {
        queueStatusText.text = $"{result.Members[0].Entity.Id} vs {result.Members[1].Entity.Id}";
    }
    private void OnGetMatchError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }




}
