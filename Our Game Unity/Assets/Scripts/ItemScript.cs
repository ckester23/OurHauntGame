using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    // List to store the items the player or NPC is holding
    private List<GameObject> heldItems = new List<GameObject>();

    // Index to keep track of the currently selected item
    private int selectedItemIndex = 0;

    // Update is called once per frame
    void Update()
    {
        // Scroll the items when the scroll wheel is moved
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelInput != 0)
        {
            // Calculate the new selected item index
            selectedItemIndex += (int)Mathf.Sign(scrollWheelInput);

            // Ensure the index wraps around within the heldItems list
            if (selectedItemIndex < 0)
            {
                selectedItemIndex = heldItems.Count - 1;
            }
            else if (selectedItemIndex >= heldItems.Count)
            {
                selectedItemIndex = 0;
            }

            // Handle item selection and deselection
            SelectItem(selectedItemIndex);
        }
    }

    public void Collect()
    {
        // You can customize the collection logic here.
        // For example, you might want to play a sound, add to the player's inventory, or perform any other action.

        // In this example, we'll simply deactivate the item GameObject when it's collected.
        gameObject.SetActive(false);
    }


    // Method to add an item to the heldItems list
    public void AddItem(GameObject item)
    {
        if (heldItems.Count < 3)
        {
            heldItems.Add(item);
            // Automatically select the first added item
            if (heldItems.Count == 1)
            {
                SelectItem(0);
            }
        }
        else
        {
            // Handle inventory full scenario
            Debug.Log("Inventory is full. Cannot add more items.");
        }
    }

    // Method to remove an item from the heldItems list
    public void RemoveItem(GameObject item)
    {
        if (heldItems.Contains(item))
        {
            heldItems.Remove(item);

            // Deselect the removed item if it was selected
            if (selectedItemIndex >= heldItems.Count)
            {
                selectedItemIndex = heldItems.Count - 1;
                SelectItem(selectedItemIndex);
            }
        }
    }

    // Method to select an item
    private void SelectItem(int index)
    {
        // Deselect all items
        foreach (GameObject item in heldItems)
        {
            item.SetActive(false);
        }

        // Select the item at the specified index
        if (index >= 0 && index < heldItems.Count)
        {
            heldItems[index].SetActive(true);
        }
    }
}
