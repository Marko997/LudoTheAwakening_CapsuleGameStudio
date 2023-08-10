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

    public List<Piece> tutorialPlayerDeck;
    public List<GameObject> tutorialPlayerCollection;

    //public Transform[] deckParent;
    public DeckParentTransformItem[] deckParent;
    public Transform collectionParent;

    public Button closeButton;
    public GameObject errorPanel;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("Deck");
        GameObject cardObject = null;
        List<GameObject> cardObjects = new List<GameObject>(4);

        //Close button setup for not letting player to save less than 4 pawns in deck
        closeButton.GetComponent<ScreenSwitcher>().menuButton.onClick.RemoveListener(closeButton.GetComponent<ScreenSwitcher>().OnButtonClicked);
        closeButton.onClick.AddListener(OnDeckCloseButtonClicked);

        if(PlayerPrefs.GetString("TUTORIAL") == "")
        {
            //if(TutorialManager.Instance.currentTutorial.Order == 3)//check for tutorial number 4 but its still 3 when script is initialized
            //{
            //    Button btn = tutorialPlayerCollection[0].gameObject.GetComponent<Button>();
            //    btn.onClick.AddListener(() => MoveCard(tutorialPlayerCollection[0].gameObject.AddComponent<Piece>(), tutorialPlayerCollection[0].gameObject, btn));
            //    return;
            //}
            for (int i = 0; i < tutorialPlayerCollection.Count; i++)
            {
                Button btn1 = tutorialPlayerCollection[i].gameObject.GetComponent<Button>();

                var copyCard = playerCollection[i];
                var copyCardObject = tutorialPlayerCollection[i].gameObject;

                btn1.onClick.AddListener(() => MoveCard(copyCard, copyCardObject, btn1));
            }
        }
        else
        {
            foreach (var tutCard in tutorialPlayerCollection)
            {
                Destroy(tutCard);
            }
            foreach (var card in playerCollection)
            {
                cardObject = Instantiate(card.pieceCardImage, collectionParent);
                cardObjects.Add(cardObject);

                Button btn = cardObject.AddComponent<Button>();
                var copyCard = card;
                var copyCardObject = cardObject;
                btn.onClick.AddListener(() => MoveCard(copyCard, copyCardObject));
            }

            string[] deckArray = PlayerPrefs.GetString("Deck").Split(',');
            if (deckArray.Length == 4)
            {
                LoadDeckFromPlayerPrefs(cardObjects);
            }
        }
    }

    public void OnDeckCloseButtonClicked()
    {
        if (playerDeck.Count < MAX_DECK_SIZE)
        {
            closeButton.GetComponent<ScreenSwitcher>().menuButton.onClick.RemoveListener(closeButton.GetComponent<ScreenSwitcher>().OnButtonClicked);
            errorPanel.SetActive(true);
            StartCoroutine(CloseErrorPanel());
        }
        else
        {
            StopCoroutine(CloseErrorPanel());
            closeButton.GetComponent<ScreenSwitcher>().OnButtonClicked();
        }
    }

    public IEnumerator CloseErrorPanel()
    {
        yield return new WaitForSeconds(2);
        errorPanel.SetActive(false);
    }
    //Move card from collection to deck and reverse
    public void MoveCard(Piece card, GameObject cardObject, Button btn=null)
    {
        SoundManager.PlayOneSound(SoundManager.Sound.ButtonClick);
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
                    cardObject.GetComponent<RectTransform>().sizeDelta = new Vector2(450f,650f);
                    cardObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f,1f);
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
            cardObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);

            int index = playerDeck.IndexOf(card);
            playerDeck.RemoveAt(index);
            playerCollection.Add(card);

        }


        if (PlayerPrefs.GetString("TUTORIAL") == "")
        {
            RemoveTutorialOnClick(btn);
            //Destroy(card);
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
                GameObject cardObject = cardObjects.FirstOrDefault(x => x.name == cardID.value + "_Image(Clone)");

                if (card != null)
                {
                    MoveCard(card, cardObject);
                }
            }
        }
    }

    private void RemoveTutorialOnClick(Button btn)
    {
        btn.onClick.RemoveAllListeners();
    }
}
