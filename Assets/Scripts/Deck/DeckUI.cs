using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    private const int MAX_DECK_SIZE = 4;

    public List<Piece> playerDeck;
    public List<Piece> playerCollection;

    //public Transform[] deckParent;
    public DeckParentTransformItem[] deckParent;
    public Transform collectionParent;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("Deck");
        GameObject cardObject = null;
        List<GameObject> cardObjects = new List<GameObject>(4);

        foreach(var card in playerCollection)
        {
            cardObject = Instantiate(card.pieceCardImage, collectionParent);
            cardObjects.Add(cardObject);

            Button btn = cardObject.AddComponent<Button>();
            var copyCard = card;
            var copyCardObject = cardObject;
            btn.onClick.AddListener(() => MoveCard(copyCard, copyCardObject));
        }

        string[] deckArray = PlayerPrefs.GetString("Deck").Split(',');
        if(deckArray.Length == 4)
        {
            LoadDeckFromPlayerPrefs(cardObjects);
        }
        
    }
    //Move card from collection to deck and reverse
    public void MoveCard(Piece card, GameObject cardObject)
    {
        if (cardObject.transform.parent == collectionParent) //Adds card to deck, removes from collection
        {
            playerDeck.Add(card);
            playerCollection.Remove(card);

            for (int i = 0; i < deckParent.Length; i++)
            {
                if (!deckParent[i].isOccupied)
                {
                    //Debug.Log(cardObject);
                    cardObject.transform.SetParent(deckParent[i].transform);
                    cardObject.transform.position = deckParent[i].transform.position;
                    deckParent[i].isOccupied = true;
                    break;
                }

            }
            SaveDeckToPlayerPrefs();  
        }
        else //Removes card from deck, adds card to collection
        {
            cardObject.GetComponentInParent<DeckParentTransformItem>().isOccupied = false;
            cardObject.transform.SetParent(collectionParent);

            int index = playerDeck.IndexOf(card);
            playerDeck.RemoveAt(index);
            playerCollection.Add(card);

        }
    }

    private void SaveDeckToPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("Deck");

        PlayerPrefs.SetString("Deck", string.Join(",", playerDeck.Select(c => c.pieceName).ToArray()));
        PlayerPrefs.Save();
    }

    private void LoadDeckFromPlayerPrefs(List<GameObject> cardObjects)
    {
        if (PlayerPrefs.HasKey("Deck"))
        {
            string[] cardIDs = PlayerPrefs.GetString("Deck").Split(',');

            foreach (var cardID in cardIDs.Select((value, i) => new { i, value }))
            {
                Piece card = playerCollection.FirstOrDefault(c => c.pieceName == cardID.value);
                GameObject cardObject = cardObjects.FirstOrDefault(x => x.name == cardID.value+"_Image(Clone)");

                if (card != null)
                {
                    MoveCard(card, cardObject);
                }
            }
        }
    }
}
