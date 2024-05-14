using PlayFab.ServerModels;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Items
{
    public class PlayFabIventory : MonoBehaviour
    {
        [SerializeField] private Button _getInventoryButton;
        [SerializeField] private Button _grantRandomItemButton;
        [SerializeField] private Button _removeRandomItemButton;
        [SerializeField] private TextMeshProUGUI _inventoryText;


        private string _playerId;
        private const string Catalog = "Main";

        private PlayFabLogin _playFabLogin;
        private PlayFabGetInventory _playFabGetInventory;
        private PlayFabGrantItems _playFabGrantItems;
        private PlayFabRemoveItems _playFabRemoveItems;
        private List<string> _userItems;
        private Dictionary<string, int> _itemCounts;

        private void Start()
        {
            _itemCounts = new Dictionary<string, int>();
            _userItems = new List<string>();
            AddListeners();
            CreatePlayFabServices();
            DoLogin();
        }

        private void CreatePlayFabServices()
        {
            _playFabLogin = new PlayFabLogin();
            _playFabLogin.OnSuccess += playerId => _playerId = playerId;

            _playFabGetInventory = new PlayFabGetInventory();
            _playFabGetInventory.OnSuccess += OnGetInventory;

            _playFabGrantItems = new PlayFabGrantItems();
            _playFabGrantItems.OnSuccess += OnGrantItems;

            _playFabRemoveItems = new PlayFabRemoveItems();
            _playFabRemoveItems.OnSuccess += OnRemoveItems;
        }

        private void UpdateInventoryText()
        {
            /*
            var text = new StringBuilder();

            foreach (var item in _userItems)
            {
                text.AppendLine($"{item}");
            }
            */
            _itemCounts.Clear();

            foreach (var item in _userItems)
            {
                if (_itemCounts.ContainsKey(item))
                {
                    _itemCounts[item]++;
                }
                else
                {
                    _itemCounts[item] = 1;
                }
            }

            var text = new StringBuilder();

            foreach (var item in _itemCounts)
            {
                text.AppendLine($"{item.Key} X {item.Value}");
            }
            _inventoryText.SetText(text.ToString());
        }
        private void OnGetInventory(List<ItemInstance> result)
        {
            _userItems.Clear();
            foreach (var itemInstance in result)
            {
                _userItems.Add(itemInstance.ItemInstanceId);
            }
            UpdateInventoryText();
        }
        private void OnGrantItems(List<GrantedItemInstance> result)
        {
            foreach (var grantedItemInstance in result)
            {
                _userItems.Add(grantedItemInstance.ItemInstanceId);
                _inventoryText.SetText("New Item recibed:\n" + grantedItemInstance.ItemInstanceId);
            }
            //UpdateInventoryText();
        }
        private void OnRemoveItems()
        {
            _inventoryText.SetText("You have removed:\n" + _userItems.ToArray()[0]);
            _userItems.RemoveAt(0);
            //UpdateInventoryText();
        }
        private void DoLogin()
        {
            _playFabLogin.Login();
        }

        private void AddListeners()
        {
            _getInventoryButton.onClick.AddListener(OnGetInventoryButtonPressed);
            _grantRandomItemButton.onClick.AddListener(OnGrantRandomItemButton);
            _removeRandomItemButton.onClick.AddListener(OnRemoveRandomItemButton);
        }
        private void OnGetInventoryButtonPressed()
        {
            _playFabGetInventory.GetInventory(_playerId);
        }
        private void OnGrantRandomItemButton()
        {
            var itemGrants = new List<ItemGrant>
            {
                new ItemGrant
                {
                    PlayFabId = _playerId,
                    ItemId = Random.Range(0, 100) % 2 == 0 ? "Item001" : "Item002",
                    Data = new Dictionary<string, string> {{"Key2", "Value2"}}
                }
            };
            _playFabGrantItems.GrantItems(Catalog, itemGrants);
        }
        private void OnRemoveRandomItemButton()
        {
            var revokeInventoryItems = new List<RevokeInventoryItem>
            {
                new RevokeInventoryItem
                {
                    PlayFabId = _playerId,
                    ItemInstanceId = _userItems[0]
                }
            };
            _playFabRemoveItems.RevokeItems(revokeInventoryItems);
        }
    }
}