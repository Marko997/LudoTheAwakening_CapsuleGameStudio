using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class AddFriends : MonoBehaviour
{
    //public InputField playerIdInputField;
    //public Button addFriendButton;
    //public Text friendListText;

    //private List<string> friends = new List<string>();

    //private void Start()
    //{
    //    addFriendButton.onClick.AddListener(AddFriendToList);

    //    // Uèitaj spisak prijatelja iz PlayerPrefs-a//lokalno
    //    int friendCount = PlayerPrefs.GetInt("friendCount", 0);
    //    for (int i = 0; i < friendCount; i++)
    //    {
    //        friends.Add(PlayerPrefs.GetString("friend" + i, ""));
    //    }

    //    // Prikaži spisak prijatelja na ekranu
    //    foreach (string friend in friends)
    //    {
    //        friendListText.text += friend + "\n";
    //    }
    //}

    //private void AddFriendToList()
    //{
    //    string playerId = playerIdInputField.text;

    //    PlayFabClientAPI.AddFriend(new AddFriendRequest()
    //    {
    //        FriendPlayFabId = playerId
    //    }, result =>
    //    {
    //        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
    //        {
    //            PlayFabId = playerId
    //        }, profileResult =>
    //        {
    //            // Dodaj ime prijatelja na listu prijatelja
    //            friends.Add(profileResult.PlayerProfile.DisplayName);

    //            // Saèuvaj spisak prijatelja u PlayerPrefs-u
    //            PlayerPrefs.SetInt("friendCount", friends.Count);
    //            for (int i = 0; i < friends.Count; i++)
    //            {
    //                PlayerPrefs.SetString("friend" + i, friends[i]);
    //            }

    //            // Prikaži spisak prijatelja na ekranu
    //            friendListText.text = "";
    //            foreach (string friend in friends)
    //            {
    //                friendListText.text += friend + "\n";
    //            }
    //        }, error =>
    //        {
    //            Debug.LogError(error.GenerateErrorReport());
    //        });
    //    }, error =>
    //    {
    //        Debug.LogError(error.GenerateErrorReport());
    //    });
    //}







    ////////////    private List<string> friends = new List<string>();
    ////////////    public InputField playerIdInput;
    ////////////    public GameObject friendListItemPrefab;
    ////////////    public Transform friendListParent;

    ////////////    private void Start()
    ////////////{
    ////////////    // Load friends list from PlayFab
    ////////////    PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(), result =>
    ////////////    {
    ////////////        friends = result.FriendIds;
    ////////////        UpdateFriendListDisplay();
    ////////////    }, error =>
    ////////////    {
    ////////////        Debug.LogError(error.GenerateErrorReport());
    ////////////    });
    ////////////}

    ////////////public void AddFriend()
    ////////////{
    ////////////    // Add a new friend using the player ID from the input field
    ////////////    PlayFabClientAPI.AddFriend(new AddFriendRequest()
    ////////////    {
    ////////////        FriendPlayFabId = playerIdInput.text
    ////////////    }, result =>
    ////////////    {
    ////////////        friends.Add(result.FriendPlayFabId);
    ////////////        UpdateFriendListDisplay();
    ////////////    }, error =>
    ////////////    {
    ////////////        Debug.LogError(error.GenerateErrorReport());
    ////////////    });
    ////////////}

    ////////////public void RemoveFriend(string friendId)
    ////////////{
    ////////////    // Remove a friend by their PlayFab ID
    ////////////    PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest()
    ////////////    {
    ////////////        FriendPlayFabId = friendId
    ////////////    }, result =>
    ////////////    {
    ////////////        friends.Remove(friendId);
    ////////////        UpdateFriendListDisplay();
    ////////////    }, error =>
    ////////////    {
    ////////////        Debug.LogError(error.GenerateErrorReport());
    ////////////    });
    ////////////}

    ////////////private void UpdateFriendListDisplay()
    ////////////{
    ////////////    // Clear the current friend list display
    ////////////    foreach (Transform child in friendListParent)
    ////////////    {
    ////////////        Destroy(child.gameObject);
    ////////////    }

    ////////////    // Display the updated friend list using prefab and button
    ////////////    foreach (string friendId in friends)
    ////////////    {
    ////////////        GameObject friendListItem = Instantiate(friendListItemPrefab, friendListParent);
    ////////////        Text friendIdText = friendListItem.GetComponentInChildren<Text>();
    ////////////        Button removeFriendButton = friendListItem.GetComponentInChildren<Button>();

    ////////////        friendIdText.text = friendId;
    ////////////        removeFriendButton.onClick.AddListener(() => RemoveFriend(friendId));
    ////////////    }
    ////////////}


    //////void DisplayFriends(List<FriendInfo> friendsCache) { friendsCache.ForEach(f => Debug.Log(f.FriendPlayFabId)); }
    //////void DisplayPlayFabError(PlayFabError error) { Debug.Log(error.GenerateErrorReport()); }
    //////void DisplayError(string error) { Debug.LogError(error); }

    //////List<FriendInfo> _friends = null;

    //////public void GetFriends()
    //////{
    //////    PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
    //////    {
    //////        IncludeSteamFriends = false,
    //////        IncludeFacebookFriends = false,
    //////        XboxToken = null
    //////    }, result =>
    //////    {
    //////        _friends = result.Friends;
    //////        DisplayFriends(_friends); // triggers your UI
    //////    }, DisplayPlayFabError);
    //////}


    //////enum FriendIdType { PlayFabId, Username, Email, DisplayName };

    //////void AddFriend(FriendIdType idType, string friendId)
    //////{
    //////    var request = new AddFriendRequest();
    //////    switch (idType)
    //////    {
    //////        case FriendIdType.PlayFabId:
    //////            request.FriendPlayFabId = friendId;
    //////            break;
    //////        case FriendIdType.Username:
    //////            request.FriendUsername = friendId;
    //////            break;
    //////        case FriendIdType.Email:
    //////            request.FriendEmail = friendId;
    //////            break;
    //////        case FriendIdType.DisplayName:
    //////            request.FriendTitleDisplayName = friendId;
    //////            break;
    //////    }
    //////    // Execute request and update friends when we are done
    //////    PlayFabClientAPI.AddFriend(request, result =>
    //////    {
    //////        Debug.Log("Friend added successfully!");
    //////    }, DisplayPlayFabError);
    //////}

    //////void RemoveFriend(FriendInfo friendInfo)
    //////{
    //////    PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
    //////    {
    //////        FriendPlayFabId = friendInfo.FriendPlayFabId
    //////    }, result =>
    //////    {
    //////        _friends.Remove(friendInfo);
    //////    }, DisplayPlayFabError);
    //////}
    //void DisplayFriends(List<FriendInfo> friendsCache)
    //{
    //    friendsCache.ForEach(f => Debug.Log(f.FriendPlayFabId));
    //}

    //void DisplayPlayFabError(PlayFabError error)
    //{
    //    Debug.Log(error.GenerateErrorReport());
    //}

    //List<FriendInfo> _friends = null;
    //string _friendIdInput = "";

    //public void GetFriends()
    //{
    //    PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
    //    {
    //        IncludeSteamFriends = false,
    //        IncludeFacebookFriends = false,
    //        XboxToken = null
    //    }, result =>
    //    {
    //        _friends = result.Friends;
    //        DisplayFriends(_friends);
    //    }, DisplayPlayFabError);
    //}

    //enum FriendIdType { PlayFabId, Username, Email, DisplayName };

    //public void AddFriend()
    //{
    //    var request = new AddFriendRequest
    //    {
    //        FriendPlayFabId = _friendIdInput
    //    };

    //    PlayFabClientAPI.AddFriend(request, result =>
    //    {
    //        Debug.Log("Friend added successfully!");
    //        GetFriends();
    //    }, DisplayPlayFabError);
    //}

    //public void RemoveFriend()
    //{
    //    PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
    //    {
    //        FriendPlayFabId = _friendIdInput
    //    }, result =>
    //    {
    //        Debug.Log("Friend removed successfully!");
    //        GetFriends();
    //    }, DisplayPlayFabError);
    //}

    //private void OnGUI()
    //{
    //    _friendIdInput = GUILayout.TextField(_friendIdInput);

    //    if (GUILayout.Button("Get Friends"))
    //    {
    //        GetFriends();
    //    }

    //    if (GUILayout.Button("Add Friend"))
    //    {
    //        AddFriend();
    //    }

    //    if (GUILayout.Button("Remove Friend"))
    //    {
    //        RemoveFriend();
    //    }
    //}


    public GameObject listingPrefab { get; private set; }

    public Text playerNameText;
   // void DisplayFriends(List<FriendInfo> friendsCache) { friendsCache.ForEach(f => Debug.Log(f.FriendPlayFabId)); }
    void DisplayPlayFabError(PlayFabError error) { Debug.Log(error.GenerateErrorReport()); }
    void DisplayError(string error) { Debug.LogError(error); }

    List<FriendInfo> _friends = null;

    [SerializeField] Transform friendScrollView;
    List<FriendInfo> myFriends;
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
            GameObject listing = Instantiate(listingPrefab, friendScrollView);///listingPrefab///???
                FriendInf tempListing = listing.GetComponent<FriendInf>();
            tempListing.playerNameText.text = f.TitleDisplayName;
            }
        }


        myFriends = friendsCache;
        //PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        //{
        //    IncludeSteamFriends = false,
        //    IncludeFacebookFriends = false,
        //    XboxToken = null
        //}, result => {
        //    _friends = result.Friends;
        //    DisplayFriends(_friends); // triggers your UI
        //}, DisplayPlayFabError);
    }

    IEnumerator WaitForFriend()
    {
        yield return new WaitForSeconds(2);
        GetFriends();
    }



    void GetFriends()
    {

        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        }, result =>
        {
            _friends = result.Friends;
            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);

    }

    public void RunwaitFunction()
    {
        StartCoroutine(WaitForFriend());
    }

    enum FriendIdType { PlayFabId, Username, Email, DisplayName };

    void AddFriend(FriendIdType idType, string friendId)
    {
        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;
                break;
        }
        // Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, result => {
            Debug.Log("Friend added successfully!");
        }, DisplayPlayFabError);
    }

    // unlike AddFriend, RemoveFriend only takes a PlayFab ID
    // you can get this from the FriendInfo object under FriendPlayFabId
    void RemoveFriend(FriendInfo friendInfo)
    {
        PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
        {
            FriendPlayFabId = friendInfo.FriendPlayFabId
        }, result => {
            _friends.Remove(friendInfo);
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
}



















//////////private List<string> friends = new List<string>();
//////////public InputField playerIdInput;
//////////public Text friendListText;

//////////private void Start()
//////////{
//////////    // Load friends list from PlayFab
//////////    PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(), result =>
//////////    {
//////////        friends = result.FriendIds;
//////////        UpdateFriendListDisplay();
//////////    }, error =>
//////////    {
//////////        Debug.LogError(error.GenerateErrorReport());
//////////    });
//////////}

//////////public void AddFriend()
//////////{
//////////    // Add a new friend using the player ID from the input field
//////////    PlayFabClientAPI.AddFriend(new AddFriendRequest()
//////////    {
//////////        FriendPlayFabId = playerIdInput.text
//////////    }, result =>
//////////    {
//////////        friends.Add(result.FriendPlayFabId);
//////////        UpdateFriendListDisplay();
//////////    }, error =>
//////////    {
//////////        Debug.LogError(error.GenerateErrorReport());
//////////    });
//////////}

//////////public void RemoveFriend(string friendId)
//////////{
//////////    // Remove a friend by their PlayFab ID
//////////    PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest()
//////////    {
//////////        FriendPlayFabId = friendId
//////////    }, result =>
//////////    {
//////////        friends.Remove(friendId);
//////////        UpdateFriendListDisplay();
//////////    }, error =>
//////////    {
//////////        Debug.LogError(error.GenerateErrorReport());
//////////    });
//////////}

//////////private void UpdateFriendListDisplay()
//////////{
//////////    // Display the updated friend list in the text component
//////////    friendListText.text = string.Join("\n", friends.ToArray());
//////////}