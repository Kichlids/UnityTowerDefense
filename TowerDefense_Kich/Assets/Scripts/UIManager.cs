using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 *  Class handles most interactions between UI elements and user
 *  as well as update necessary texts
 */

public class UIManager : MonoBehaviour
{
    private Player player;
    private Store store;
    private WaveManager waveManager;

    #region UI objects

    public TextMeshProUGUI livesTxt;
    public TextMeshProUGUI goldTxt;
    public TextMeshProUGUI waveTxt;

    public Button nextWaveBtn;

    #endregion

    private ItemSelect item;

    private void Start()
    {
        // Get neccessary components
        player = GetComponent<Player>();
        store = GetComponent<Store>();
        waveManager = GetComponent<WaveManager>();
    }

    private void Update()
    {
        UpdateTexts();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (store.item == ItemSelect.barrier)
            {
                item = ItemSelect.barrier;
                store.PurchaseBuilding(item);
            }
            else if (store.item == ItemSelect.tower1)
            {
                item = ItemSelect.tower1;
                store.PurchaseBuilding(item);
            }
            if (store.item == ItemSelect.sell)
            {
                store.SellBuilding();
            }
        }


        if (waveManager.waveInProgress)
            nextWaveBtn.gameObject.SetActive(false);
        else
            nextWaveBtn.gameObject.SetActive(true);
    }

    // Get game information and update every frame
    private void UpdateTexts()
    {
        livesTxt.text = "Lives: " + player.GetLives();
        goldTxt.text = "Gold: " + player.GetGold();
        waveTxt.text = "Wave: " + waveManager.GetWaveIndex();
    }

    #region Calls when a button is activated

    // Move to next wave when ready
    public void OnNextWaveBtn()
    {
        if (!waveManager.waveInProgress)
            waveManager.nextWaveReady = true;
    }

    // Set item selected to none
    public void OnCancelBtn()
    {
        store.item = ItemSelect.clear;
    }

    // Set item selected to barrier
    public void OnBarrierBtn()
    {
        store.item = ItemSelect.barrier;
    }

    public void OnTower1Btn()
    {
        store.item = ItemSelect.tower1;
    }

    // Sell item option
    public void OnSellBtn()
    {
        store.item = ItemSelect.sell;
    }

    #endregion
}
