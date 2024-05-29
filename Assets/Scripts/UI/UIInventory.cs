using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots; // ���Ե鿡 ���� ����

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    void Start()    // �ʱ�ȭ�ǰ� â ����
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle; // inventory ��������Ʈ�� Toggle �޼��� ����
        CharacterManager.Instance.Player.addItem += AddItem;    // addItem ��������Ʈ�� AddItem �޼��� ����

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];     // ���� �ǳ��� �ڽ� ������Ʈ ����

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }

    void Update()
    {

    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()    // �κ��丮 â �¿���
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;    // �÷��̾ �Ĺ��� ������ ����

        // ���Կ� �̹� �������� ���� �� 
        if (data.canStack)    // �������� �ߺ� �����ϴٸ�
        {
            ItemSlot slot = GetItemStack(data);    // �ߺ� ������ ���� ��������
            if (slot != null)    // ���Կ� �������� �̹� �ִٸ�
            {
                slot.quantity++;    // ���� 1�� �߰�
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;    // �ʱ�ȭ
                return;
            }
        }

        // �� ���� ��������
        ItemSlot emptySlot = GetEmptySlot();

        // ����ִ� ������ �ִٸ�
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        // ����ִ� ������ ���ٸ�
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)   // �����Ͱ� �� �ִٸ�
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)    // ������ �ߺ��� ������ ����
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == data && slots[i].quantity < data.maxStackAmount)    // ���Կ� �ִ� �����۰� �Ĺ��� �������� ����, ���Կ� �ִ� ������ ���� �ߺ� ������ �ִ� ������ ���ٸ�
            {
                return slots[i];    // �ش� ���� ��ȯ
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()    // �� ����
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)    // ���Կ� ������ ������ ���ٸ�
            {
                return slots[i];    // �ش� ���� ��ȯ
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));    // �ı��ƴ� ������ ����
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }
}
