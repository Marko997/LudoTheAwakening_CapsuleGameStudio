using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Security.Principal;
using static UnityEngine.Rendering.DebugUI;
using PlayFab.AuthenticationModels;
using UnityEditor.PackageManager;

public class AddFriends : MonoBehaviour
{
    //void DisplayFriends(List<FriendInfo> friendsCache) { friendsCache.ForEach(f => Debug.Log(f.FriendPlayFabId)); }
    void DisplayPlayFabError(PlayFabError error) { 
        Debug.Log(error.GenerateErrorReport());

        //Text errorText = GameObject.Find("errorText").GetComponent<Text>();
        errorText.text = error.ErrorMessage;
    }

    void DisplayError(string error) {
        Debug.LogError(error);

        //Text errorText = GameObject.Find("errorText").GetComponent<Text>();
        errorText.text = error;
    }

    public GameObject listingPrefab;

    [SerializeField]
    Transform friendScrollView;
    List<FriendInfo> myFriends;

    GameObject listing;

    public Text errorText;
    void DisplayFriends(List<FriendInfo> friendsCache)
    {
        foreach (FriendInfo f in friendsCache)
        {
            bool isFound = false;
            if (myFriends != null)
            {
                foreach (FriendInfo g in myFriends)
                {
                    if (f.FriendPlayFabId == g.FriendPlayFabId)
                        isFound = true;
                }
            }

            if (isFound == false)
            {
                listing = Instantiate(listingPrefab, friendScrollView);
                ListingPrefab tempListing = listing.GetComponent<ListingPrefab>();
                //Debug.Log(tempListing.playerNameText);
                //Debug.Log(f.TitleDisplayName);
                tempListing.playerNameText.text = f.TitleDisplayName;
            }
        }
        myFriends = friendsCache;
    }

    IEnumerator WaitForFriend()
    {
        yield return new WaitForSeconds(1);
        GetFriends();
    }
    public void RunWaitFunction()
    {
        StartCoroutine(WaitForFriend());
    }
    List<FriendInfo> _friends = null;
    public void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false
        }, result =>
        {
            _friends = result.Friends;
            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);
    }

    enum FriendIdType { PlayFabId/*, Username, Email, DisplayName*/ };
    void AddFriend(FriendIdType idType, string friendId)
    {
        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            //case FriendIdType.Username:
            //    request.FriendUsername = friendId;
            //    break;
            //case FriendIdType.Email:
            //    request.FriendEmail = friendId;
            //    break;
            //case FriendIdType.DisplayName:
            //    request.FriendTitleDisplayName = friendId;
            //    break;
        }
        // Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, result =>
        {
            Debug.Log("Friend added successfully!");
            errorText.text = "Sucess added friend";
        }, DisplayPlayFabError);
    }

    string friendSearch;
    [SerializeField]
    GameObject friendPanel;
    public void InputFriendID(string idIn)
    {
        friendSearch = idIn;
    }
    public void SubmitFriendRequest()
    {
        AddFriend(FriendIdType.PlayFabId, friendSearch);
    }

    public void OpenCloseFriends()
    {
        friendPanel.SetActive(!friendPanel.activeInHierarchy);
    }

    public void DeleteFriendRequest()
    {
        DeleteFriend(FriendIdType.PlayFabId, friendSearch);
    }
    void DeleteFriend(FriendIdType idType, string friendId)
    {
        var request = new RemoveFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
               request.FriendPlayFabId = friendId;
                break;
            //case FriendIdType.Username:
            //    //request.FriendUsername = friendId;
            //    break;
            //case FriendIdType.Email:
            //    //request.FriendEmail = friendId;
            //    break;
            //case FriendIdType.DisplayName:
            //    //request.FriendTitleDisplayName = friendId;
            //    break;
        }
        //Execute request and update friends when we are done
        PlayFabClientAPI.RemoveFriend(request, result =>
        {
            Debug.Log("Friend deleted successfully!");
            errorText.text = "Sucess delete friend";
        }, DisplayPlayFabError);

        Destroy(listing);
    }
}