using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    private const int MAX_DECK_SIZE = 4;

    public List<Piece> playerDeck;
    public List<Piece> playerCollection;

    public Transform[] deckParent;
    public Transform collectionParent;

    private void Start()
    {
        GameObject cardObject = null;
        List<GameObject> cardObjects = new List<GameObject>(4);
        foreach(var card in playerCollection)
        {
            cardObject = Instantiate(card.pieceCardImage, collectionParent);
            cardObjects.Add(cardObject);

            Button btn = cardObject.AddComponent<Button>();

            btn.onClick.AddListener(() => MoveCard(card,cardObject));
        }

        LoadDeckFromPlayerPrefs(cardObjects);
    }
    //Move card from collection to deck and reverse
    public void MoveCard(Piece card, GameObject cardObject)
    {
        if(cardObject.transform.parent != collectionParent)
        {
            playerDeck.Remove(card);
            playerCollection.Add(card);

            cardObject.transform.SetParent(collectionParent);
        }
        else
        {
            if (playerDeck.Count < MAX_DECK_SIZE)
            {
                playerDeck.Add(card);
                playerCollection.Remove(card);

                cardObject.transform.SetParent(deckParent[0]);

                SaveDeckToPlayerPrefs();
            }
        }

        
    }

    private void SaveDeckToPlayerPrefs()
    {
        PlayerPrefs.SetString("Deck", string.Join(",", playerDeck.Select(c => c.cardId).ToArray()));
        PlayerPrefs.Save();
    }

    private void LoadDeckFromPlayerPrefs(List<GameObject> cardObjects)
    {
        if (PlayerPrefs.HasKey("Deck"))
        {
            string[] cardIDs = PlayerPrefs.GetString("Deck").Split(',');
            //foreach (var cardID in cardIDs)
            //{
            //    Piece card = playerCollection.FirstOrDefault(c => c.cardId == cardID);

            //    if(card != null)
            //    {
            //        playerDeck.Add(card);
            //        playerCollection.Remove(card);

            //        Transform cardTransform = card.gameObject.transform;
            //        cardTransform.SetParent(deckParent[0]);
            //    }
            //}

        }
    }
}
