using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections.Generic;

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


    public InputField friendIdInput;
    public Button addFriendButton;
    public Button removeFriendButton;

    List<FriendInfo> _friends = null;
    public Text friendsListText;

    void Start()
    {
        friendsListText = GetComponent<Text>();
        addFriendButton.onClick.AddListener(() => AddFriend(friendIdInput.text));
        removeFriendButton.onClick.AddListener(() => RemoveFriend(friendIdInput.text));
        GetFriends();
    }

    void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        }, result => {
            _friends = result.Friends;
            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);
    }

    void DisplayFriends(List<FriendInfo> friendsCache)
    {
        string display = "";
        friendsCache.ForEach(f => display += f.FriendPlayFabId + "\n");
        friendsListText.text = display;
    }

    void DisplayPlayFabError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    void DisplayError(string error)
    {
        Debug.LogError(error);
    }

    void AddFriend(string friendId)
    {
        PlayFabClientAPI.AddFriend(new AddFriendRequest
        {
            FriendPlayFabId = friendId
        }, result => {
            GetFriends();
        }, DisplayPlayFabError);
    }

    void RemoveFriend(string friendId)
    {
        var friendToRemove = _friends.Find(f => f.FriendPlayFabId == friendId);
        if (friendToRemove != null)
        {
            PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
            {
                FriendPlayFabId = friendToRemove.FriendPlayFabId
            }, result => {
                GetFriends();
            }, DisplayPlayFabError);
        }
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