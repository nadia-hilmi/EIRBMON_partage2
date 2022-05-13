using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;

    public event Action<int> onMenuSelected;
    public event Action onBack;
    List<Text> menuItems;

    int selector = 0;

    private void Awake()
    {
        menuItems = menu.GetComponentsInChildren<Text>().ToList();
    }
    public void OpenMenu()
    {
        menu.SetActive(true);
        UpdateItemSelection(); 

    }
    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        int prevSelector = selector;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            --selector;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            ++selector;

        selector = Mathf.Clamp(selector, 0, menuItems.Count-1);

        if (prevSelector != selector)
            UpdateItemSelection(); 

        if (Input.GetKeyDown(KeyCode.Return))
        {
            onMenuSelected?.Invoke(selector);
            CloseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onBack?.Invoke();
            CloseMenu();
        }

    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (i == selector)
                menuItems[i].color = Color.blue;
            else
                menuItems[i].color = Color.black;
                
        }
    }
}
