﻿using Live2D.Cubism.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AMG
{

    public class AMGShortcutController : MonoBehaviour
    {
        private Button ShortcutItem;
        private VerticalLayoutGroup ShortcutGroup;

        public void setVerticalLayoutGroup(VerticalLayoutGroup ShortcutGroup)
        {
            this.ShortcutGroup = ShortcutGroup;
        }

        public void setExampleButton(Button ShortcutItem)
        {
            this.ShortcutItem = ShortcutItem;
        }

        public void refreshVerticalLayoutGroup()
        {
            for (int i = 0; i < ShortcutGroup.gameObject.transform.childCount; i++)
            {
                UnityEngine.Object.Destroy(ShortcutGroup.gameObject.transform.GetChild(i).gameObject);
            }
        }

        public void SetVerticalLayoutGroup(CubismModel model)
        {
            var valueToAnimation = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Dictionary<string, ShortcutClass>> kvp in Globle.KeyboardHotkeyDict)
            {
                foreach (KeyValuePair<string, ShortcutClass> kkvp in kvp.Value)
                {
                    if (kkvp.Value.Model == model && !valueToAnimation.ContainsKey(kkvp.Value.AnimationClip))
                    {
                        Debug.Log("Found " + kkvp.Key);
                        valueToAnimation.Add(kkvp.Value.AnimationClip, kkvp.Value.KeyPressed);
                    }
                }
            }
            foreach (string name in model.GetComponent<AMGModelController>().animationClips)
            {
                if (valueToAnimation.ContainsKey(name))
                {
                    this.AddVerticalLayoutGroupItem(name, model, valueToAnimation[name]);
                }
                else
                {
                    this.AddVerticalLayoutGroupItem(name, model);
                }
            }
        }

        public void AddVerticalLayoutGroupItem(string name, CubismModel model, string key = "")
        {
            Button citem = Instantiate<Button>(ShortcutItem);
            citem.name = name;
            citem.gameObject.SetActive(true);
            citem.transform.SetParent(ShortcutGroup.gameObject.transform, false);
            var controller = citem.GetComponent<AMGShortcutItemController>();
            var modelcontroller = model.GetComponent<AMGModelController>();
            controller.ItemShortcut.text = key;
            controller.ItemAction.text = name;
            controller.AnimationClip = name;
            controller.cubismModel = model;
            controller.modelController = modelcontroller;
            controller.InitShortcutItem();
        }
    }
}