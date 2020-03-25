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


    public static UIManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        targetDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        InitializeBuildingPanel();
    }

    private void Update()
    {
        UpdatePlayerInfo();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Select an item
            if (Store._instance.item == ItemSelect.clear)
            {
                DisplayBuildingPanel();
            }
            // Sell an item
            else if (Store._instance.item == ItemSelect.sell)
            {
                Store._instance.SellBuilding();
            }
            // Purchase an item
            else
            {
                Store._instance.PurchaseBuilding(Store._instance.item);
            }
        }

        if (WaveManager._instance.waveInProgress)
            nextWaveBtn.gameObject.SetActive(false);
        else
            nextWaveBtn.gameObject.SetActive(true);
    }

    // Update player information
    private void UpdatePlayerInfo()
    {
        livesTxt.text = "Lives: " + Player._instance.GetLives();
        goldTxt.text = "Gold: " + Player._instance.GetGold();
        waveTxt.text = "Wave: " + WaveManager._instance.GetWaveIndex();
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

                if ((nodeBuildingGameObject = node.GetBuildingObject()) != null)
                {
                    if ((buildingComponent = nodeBuildingGameObject.GetComponent<Building>()) != null)
                    {
                        damageCountTxt.text = buildingComponent.damageDone.ToString();
                        sellCostTxt.text = buildingComponent.sell.ToString();
                        buildingImage.sprite = buildingComponent.sprite;

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
        if (!WaveManager._instance.waveInProgress)
            WaveManager._instance.nextWaveReady = true;
    }

    // Set item selected to none
    public void OnCancelBtn()
    {
        Store._instance.item = ItemSelect.clear;
    }

    // Set item selected to barrier
    public void OnBarrierBtn()
    {
        Store._instance.item = ItemSelect.barrier;
    }

    public void OnTower1Btn()
    {
        Store._instance.item = ItemSelect.tower1;
    }

    // Sell item option
    public void OnSellBtn()
    {
        Store._instance.item = ItemSelect.sell;
    }

    #endregion
}