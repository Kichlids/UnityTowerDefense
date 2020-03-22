using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using System.Linq;

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

    public GameObject buildingPanel;
    public Image buildingImage;
    public TextMeshProUGUI damageCountTxt;
    public TextMeshProUGUI sellCostTxt;
    public TMP_Dropdown targetDropdown;

    public Button nextWaveBtn;

    #endregion

    public Tower lastSelectedTower = null;

    private const float RAY_DIST = 1000f;

    private void Start()
    {
        // Get neccessary components
        player = GetComponent<Player>();
        store = GetComponent<Store>();
        waveManager = GetComponent<WaveManager>();

        targetDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        InitializeBuildingPanel();
    }

    private void Update()
    {
        UpdatePlayerInfo();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Select an item
            if (store.item == ItemSelect.clear)
            {
                DisplayBuildingPanel();
            }
            // Sell an item
            else if (store.item == ItemSelect.sell)
            {
                store.SellBuilding();
            }
            // Purchase an item
            else
            {
                store.PurchaseBuilding(store.item);
            }
        }

        if (waveManager.waveInProgress)
            nextWaveBtn.gameObject.SetActive(false);
        else
            nextWaveBtn.gameObject.SetActive(true);
    }

    // Update player information
    private void UpdatePlayerInfo()
    {
        livesTxt.text = "Lives: " + player.GetLives();
        goldTxt.text = "Gold: " + player.GetGold();
        waveTxt.text = "Wave: " + waveManager.GetWaveIndex();
    }

    private void DisplayBuildingPanel()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, RAY_DIST))
        {
            if (hit.collider.gameObject.tag == "Node")
            {
                Node node = hit.collider.gameObject.GetComponent<Node>();

                GameObject nodeBuildingGameObject = null;
                Building buildingComponent = null;
                Tower towerComponent = null;

                if ((nodeBuildingGameObject = node.GetBuilding()) != null)
                {
                    if ((buildingComponent = nodeBuildingGameObject.GetComponent<Building>()) != null)
                    {
                        damageCountTxt.text = buildingComponent.damageDone.ToString();
                        sellCostTxt.text = buildingComponent.sell.ToString();

                        if ((towerComponent = nodeBuildingGameObject.GetComponent<Tower>()) != null)
                        {
                            lastSelectedTower = towerComponent;

                            int buildingPrio = SelectTargetEnum(towerComponent.criteria);
                            targetDropdown.value = buildingPrio;
                        }
                    }
                    else
                    {
                        damageCountTxt.text = "";
                        sellCostTxt.text = "";
                    }
                }
                else
                {
                    lastSelectedTower = null;
                }
            }
        }
    }

    private void InitializeBuildingPanel()
    {
        targetDropdown.ClearOptions();
        
        List<string> targetTypes = Enum.GetNames(typeof(Targeting)).ToList<string>();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < targetTypes.Count; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = targetTypes[i];
            options.Add(option);
        }

        targetDropdown.AddOptions(options);
    }

    private int SelectTargetEnum(Targeting targeting)
    {
        List<Targeting> targetList = Enum.GetValues(typeof(Targeting)).Cast<Targeting>().ToList();
        for (int i = 0; i < targetList.Count; i++)
        {
            if (targeting == targetList[i])
            {
                return i;
            }
        }

        return 0;
    }

    public void OnDropdownValueChanged(int prio)
    {
        print("changed to " + prio);
        if (lastSelectedTower != null)
        {
            List<Targeting> targetList = Enum.GetValues(typeof(Targeting)).Cast<Targeting>().ToList();

            lastSelectedTower.criteria = targetList[prio];
            print("New criteria: " + lastSelectedTower.criteria);
        }
    }


    #region Call when a button is activated

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