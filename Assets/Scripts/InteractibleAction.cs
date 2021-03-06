﻿using Assets.Services;
using HoloToolkit;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InteractibleAction performs custom actions when you gaze at the holograms.
/// </summary>
public class InteractibleAction : MonoBehaviour
{
    [Tooltip("Drag the Tagalong prefab asset you want to display.")]
    public GameObject ObjectToTagAlong;

    private string _text;
    private BilboardTextFormatterService _bilboardTextParserService;
    private StickyNote _stickyNote;
    private TextMesh _textMesh;

    TouchScreenKeyboard keyboard;

    void Start()
    {
        _bilboardTextParserService = new BilboardTextFormatterService();
        _stickyNote = GetComponent<StickyNote>();
    }

    void Update()
    {
        if (IsKeyboardClosed())
        {
            var wasCanceled = keyboard.wasCanceled;
            var keyboardText = keyboard.text;

            keyboard = null;
            if (wasCanceled)
                return;

            SetStickyNoteText(keyboardText);

            UpdateBilboardText();
        }
    }

    public void PerformTagAlong()
    {
        if (ObjectToTagAlong == null)
            return;

        var tagAlong = GameObject.FindGameObjectWithTag("TagAlong") ?? CreateTagAlongObject();
        _textMesh = tagAlong.GetComponent<TextMesh>();
        UpdateBilboardText();

        OpenKeyboard();
    }

    private GameObject CreateTagAlongObject()
    {
        GameObject item = Instantiate(ObjectToTagAlong);

        item.SetActive(true);
        item.AddComponent<Billboard>();
        item.AddComponent<SimpleTagalong>();

        return item;
    }

    private void SetStickyNoteText(string text)
    {
        _text = text;
        _stickyNote.Content = text;
    }

    private void UpdateBilboardText()
    {
        if (_textMesh != null && _text != null)
            _textMesh.text = _bilboardTextParserService.Format(_text);
    }

    private bool IsKeyboardClosed()
    {
        return keyboard != null && keyboard.active == false && keyboard.done == true;
    }

    private void OpenKeyboard()
    {
        keyboard = new TouchScreenKeyboard(_text, TouchScreenKeyboardType.Default, false, false, false, false, "Edit content");
    }

}